using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
using TongXinBack.Config;
using MongoDB.Driver;
using MongoDB.Bson;
using ReturnParam;
using TongXinBack.dlls;

namespace TongXinBack.Service
{
    public class PostServiceImpl : PostService
    {
        private readonly IMongoCollection<UserPost> _userPosts;
        private readonly IMongoCollection<Post> _AllUserPosts;
        private readonly IMongoCollection<UserFollowsPosts> _userFollowsPosts;
        private readonly FollowedService _followedService;
        private readonly CommentService _commentService;
        private readonly IMongoCollection<User> _users;
        public PostServiceImpl(UserAdminDatabaseSettings settings, CommentService commentService, FollowedService followedService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _AllUserPosts = database.GetCollection<Post>("AllPostsCapped");
            _userPosts = database.GetCollection<UserPost>("UserPosts");
            _userFollowsPosts = database.GetCollection<UserFollowsPosts>("UserFollowsPostsCapped");
            _users = database.GetCollection<User>("Users");
            _commentService = commentService;
            _followedService = followedService;
        }

        public async Task<Result> createAsync( string username,string content,List<string> imageList)
        {
            //Console.WriteLine(username);
            //Console.WriteLine(content);
            //Console.WriteLine(imageList.Count());
            //imageList.ForEach(Console.WriteLine);
            Post post = new Post();
            //不要在这里判断是否已经有了，应该在创建新用户的时候，直接给他创建好post仓库，review库，likes库，到时候直接追加、
            post.Id = ObjectId.GenerateNewId().ToString();
            post.username = username;
            post.content = content;
            post.photoesList = imageList;
            //ToDo 在这里查询器nickname并复制
            post.likes = 0;
            post.comments = 0;
            post.views = 0;
            post.updatedTime= DateTime.Now;
            post.createdTime = DateTime.Now;
            
            post.likesList = new List<string>();
            post.viewList = new List<string>();
            try
            {
                User finduser = _users.Find<User>(u => u.username == username).FirstOrDefault();
                post.nickname = finduser.nickname;
                post.avatar = finduser.avatar ;
                
                //向所有集合中插入post
                _AllUserPosts.InsertOne(post);
                List<Task> tasks = new List<Task>();
                //向所有其粉丝推送
                List<FollowUserInfo> followeds = _followedService.Get(post.username).followeds;
                foreach (var user in followeds)
                {
                    var followfilter = new BsonDocument("username", user.userid);
                    var followupdate = new BsonDocument("$push", new BsonDocument("userFollowedsPostList", post.Id));
                    Task task= _userFollowsPosts.UpdateOneAsync(followfilter, followupdate);
                    tasks.Add(task);
                }

                //添加到用户帖子中
                var filter = new BsonDocument("username", post.username);
                var update = new BsonDocument("$push", new BsonDocument("userPostList", post.ToBsonDocument()));//new BsonValue(post)
                Task usertask= _userPosts.UpdateOneAsync(filter, update);
                tasks.Add(usertask);

                //同时应该在这里给帖子创建好评论库
                 _commentService.CreateNullList(post.Id);

                foreach(Task temp  in tasks){
                    await temp;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("fail");
            }


            return Result.success(post) ;
        }

        public Post delete(string username, string postid)
        {
            throw new NotImplementedException();
        }


        public UserPost Get(string username) =>
            _userPosts.Find<UserPost>(userpost => userpost.username == username).FirstOrDefault();


        public Result GetUserPost(string username, int pageNum, int pageSize)
        {
            PageInfo<Post> page = new PageInfo<Post>();
            page.pageNum = pageNum;
            page.pageSize = pageSize;
            try
            {
                
                List<Post> result = _userPosts.Find<UserPost>(userpost => userpost.username == username).FirstOrDefault().userPostList;
                page.total = result.Count();
                int start = (pageNum - 1) * pageSize;
                page.list = result.GetRange(start, Math.Min(pageSize, result.Count() - start));
                page.size = page.list.Count();

                result.Reverse();
                return Result.success(page);
            }
            catch (NullReferenceException e)
            {
                page.list = new List<Post>();
                return Result.success(page);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("查询失败");
            }

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

        public Result GetAllUserPostCappedPage(int pageNum,int pageSize)
        {
            try
            {
                PageInfo<Post> page = new PageInfo<Post>();
                page.pageNum = pageNum;
                page.pageSize = pageSize;
                page.total = GetAllUserPostCapped().Count();
                page.list = _AllUserPosts.Find<Post>(post => true).SortByDescending(p => p.Id).Skip((pageNum - 1) * pageSize).Limit(pageSize).ToList<Post>();
                page.size = page.list.Count();
                return Result.success(page);
            } catch (Exception e)
            {
                return Result.error("查询失败");
            }
            //_AllUserPosts.AsQueryable().OrderByDescending(p => p.Id).Skip((pageNum-1)*pageSize).ToList<Post>());
            //Find<Post>(post => true).SortByDescending<Post>(Post=>Post.Id).ToList<Post>();
        }

        public async Task<Result> addViewAsync(string postId, string username)
        {
          //  int views;
            try
            {

                // UserPost userPost = _userPosts.Find<UserPost>(x => x.userPostList.Any(p => p.Id == postId)).FirstOrDefault();
                //UserPost userPost = _userPosts.Find(new BsonDocument("userPostList",new BsonDocument("$elemMatch", new BsonDocument("_Id",postId)))).FirstOrDefault();
                //views = userPost.userPostList.First<Post>().views;
                //if (userPost.userPostList.Any(p => p.viewList.Any(u => u == username))){
                //    return Result.error("该用户已经浏览过啦！");
                //}
                var filter = new BsonDocument("userPostList._Id", postId);
                var update = new BsonDocument("$inc", new BsonDocument("userPostList.$.views",1));//new BsonValue(post)
                Task uptask1= _userPosts.UpdateOneAsync<UserPost>(x=>x.userPostList.Any<Post>(p=>p.Id==postId), update);
                Task uptask2=_AllUserPosts.UpdateOneAsync<Post>(post => post.Id == postId, new BsonDocument("$inc", new BsonDocument("views",  1)));
                Task uptask3 = _userPosts.UpdateOneAsync<UserPost>(x => x.userPostList.Any<Post>(p => p.Id == postId), new BsonDocument("$push", new BsonDocument("userPostList.$.viewList",  username)));
                Task uptask4 = _AllUserPosts.UpdateOneAsync<Post>(post => post.Id == postId, new BsonDocument("$push", new BsonDocument("viewList", username)));
                // _userFollowsPosts.UpdateMany(filter, update);
                await uptask1;
                await uptask2;
                await uptask3;
                await uptask4;
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("增加浏览量失败！");
            } 
            return Result.success( "success");
        }
    }
}
