using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
using TongXinBack.Config;
using MongoDB.Driver;
using MongoDB.Bson;

namespace TongXinBack.Service
{
    public class PostServiceImpl : PostService
    {
        private readonly IMongoCollection<UserPost> _userPosts;
        private readonly IMongoCollection<Post> _AllUserPosts;
        private readonly IMongoCollection<UserFollowsPosts> _userFollowsPosts;
        private readonly FollowedService _followedService;
        private readonly CommentService _commentService;
        public PostServiceImpl(UserAdminDatabaseSettings settings, CommentService commentService, FollowedService followedService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _AllUserPosts = database.GetCollection<Post>("AllPostsCapped");
            _userPosts = database.GetCollection<UserPost>("UserPosts");
            _userFollowsPosts = database.GetCollection<UserFollowsPosts>("UserFollowsPostsCapped");
            _commentService = commentService;
            _followedService = followedService;
        }

        public Post create(string username, Post post)
        {
            //不要在这里判断是否已经有了，应该在创建新用户的时候，直接给他创建好post仓库，review库，likes库，到时候直接追加

            post.Id = ObjectId.GenerateNewId().ToString();
            post.likes = 0;
            post.comments = 0;

            //向所有集合中插入post
            _AllUserPosts.InsertOne(post);

            //向所有其粉丝推送
            List<FollowUserInfo> followeds = _followedService.Get(username).followeds;
            foreach(var user in followeds){
                var followfilter = new BsonDocument("username", user.userid);
                var followupdate = new BsonDocument("$push", new BsonDocument("userFollowedsPostList", post.Id));
                _userFollowsPosts.UpdateOne(followfilter, followupdate);
            }

            //添加到用户帖子中
            var filter = new BsonDocument("username",username);
            var update = new BsonDocument("$push",new BsonDocument("userPostList", post.ToBsonDocument()));//new BsonValue(post)
            _userPosts.UpdateOne(filter, update);


            //同时应该在这里给帖子创建好评论库，like库
            //TODO  还没有创建like库
            _commentService.CreateNullList(post.Id);
            return post;
        }

        public Post delete(string username, string postid)
        {
            throw new NotImplementedException();
        }


        public UserPost Get(string username) =>
            _userPosts.Find<UserPost>(userpost => userpost.username == username).FirstOrDefault();


        public List<Post> GetUserPost(string username)
        {
            List<Post> result= _userPosts.Find<UserPost>(userpost => userpost.username == username).FirstOrDefault().userPostList;
            result.Reverse();
            return result;
        }


        public UserPost CreateNullList(string username)
        {
            var nowUser = new UserPost();
            nowUser.username = username;
            nowUser.userPostList = new List<Post>();
            _userPosts.InsertOne(nowUser);
            return nowUser;
        }

        public UserFollowsPosts CreateUserFollowedNullList(string username)
        {
            var nowUser = new UserFollowsPosts();
            nowUser.username = username;
            nowUser.userFollowedsPostList = new List<string>();
            _userFollowsPosts.InsertOne(nowUser);
            return nowUser;
        }

        public Post getPost(string postId)
        {
           UserPost result=_userPosts.Find(x => x.userPostList.Any(p=>p.Id==postId)).FirstOrDefault();
            Console.WriteLine(result.ToJson());
            if (result.userPostList.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.userPostList.First();
            }
        }

        public List<Post> GetAllUserPostCapped()
        {
            return _AllUserPosts.AsQueryable().OrderByDescending(p=>p.Id).ToList<Post>();
                //Find<Post>(post => true).SortByDescending<Post>(Post=>Post.Id).ToList<Post>();
        }
    }
}
