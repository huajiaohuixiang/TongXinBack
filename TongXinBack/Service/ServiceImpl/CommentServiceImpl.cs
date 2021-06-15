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
using TongXinBack.DTO;

namespace TongXinBack.Service
{
    class CommentServiceImpl : CommentService
    {
        private readonly IMongoCollection<PostComments> _postComments;
        private readonly IMongoCollection<UserComments> _UserComments;
        private readonly IMongoCollection<UserPost> _UserPosts;
        private readonly IMongoCollection<Post> _AllPosts;
        private readonly IMongoCollection<User> _users;

        public CommentServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _postComments = database.GetCollection<PostComments>("PostComments");
            _UserComments = database.GetCollection<UserComments>("UserComments");
            _UserPosts = database.GetCollection<UserPost>("UserPosts");
            _AllPosts = database.GetCollection<Post>("AllPostsCapped");
            _users = database.GetCollection<User>("Users");


        }
        public async Task< Result> CreateCommentAsync(string postId, string username, string commentInfo)
        {
            Console.WriteLine(postId + "," + username + "," + commentInfo);
            try
            {
                Comment comment = new Comment();
                //向帖子中添加评论
                comment.comment = commentInfo;
                //comment.nickname=_AllPosts.Find<Post>(p => p.Id == postId).FirstOrDefault().nickname;
                //comment.avatar = _AllPosts.Find<Post>(p => p.Id == postId).FirstOrDefault().avatar;
                comment.avatar = _users.Find<User>(user => user.username == username).FirstOrDefault().avatar;
                comment.nickname = _users.Find<User>(user => user.username == username).FirstOrDefault().nickname;
                comment.username = username;
                comment.comTime = DateTime.Now;
                comment.comments = 0;
                comment.likes = 0;
                comment._id = ObjectId.GenerateNewId().ToString();
                var filter = new BsonDocument("PostId", postId);
                var update = new BsonDocument("$push", new BsonDocument("comments", comment.ToBsonDocument()));//new BsonValue(post)
                Task task1= _postComments.UpdateOneAsync(filter, update);
                
                //向用户的Comment库中添加
                var usercmfilter = new BsonDocument("username", comment.username);
                var usercmupdate = new BsonDocument("$push", new BsonDocument("comments", comment.ToBsonDocument()));//new BsonValue(post)
                Task task2= _UserComments.UpdateOneAsync(usercmfilter, usercmupdate);


                //增加allpost和userpost中的comment数new BsonDocument("userPostList._Id", postId)
                Task task3= _UserPosts.UpdateOneAsync<UserPost>(up => up.userPostList.Any<Post>(p => p.Id == postId), new BsonDocument("$inc", new BsonDocument("userPostList.$.comments", 1)));
                Task task4= _AllPosts.UpdateOneAsync<Post>(p => p.Id == postId, new BsonDocument("$inc", new BsonDocument("comments", 1)));
                await task1;
                await task2;
                await task3;
                await task4;
            }
            catch(Exception e){
                Console.WriteLine(e.ToString());
                return Result.error("评论失败");
            }
            return Result.success("评论成功");
            
        }

        public void CreateNullList(string postId)
        {
            PostComments newPM = new PostComments();
            newPM.PostId = postId;
            newPM.comments = new List<Comment>();
            _postComments.InsertOne(newPM);
        }

        public void CreateUserCommentNullList(string username)
        {
            UserComments newUC = new UserComments();
            newUC.username = username;
            newUC.comments = new List<Comment>();
            _UserComments.InsertOne(newUC);
        }

        public void DeleteComment(string postId, Comment comment)
        {
            throw new NotImplementedException();
        }

        public Result Get(string postId, int pageNum, int pageSize)
        {
            PageInfo<Comment> page = new dlls.PageInfo<Comment>();
            var postCom = _postComments.Find<PostComments>(postcomment => postcomment.PostId == postId).FirstOrDefault();
            List<Comment> comments;
            comments = postCom ==null ? new List<Comment>() : postCom.comments;

            page.pageSize = pageSize;
            page.pageNum = pageNum;
            page.total = comments.Count();
            if ((pageNum ) * pageSize > comments.Count())
            {
                pageSize = comments.Count - (pageNum - 1) * pageSize;
            }
            comments = comments.GetRange((pageNum - 1) * pageSize, pageSize);
            page.list = comments;
            page.size = comments.Count();
                        return Result.success(page);
        }
            
         
          

        public Result GetUserComment(string username, int pageNum, int pageSize)
        {
            PageInfo<CommentPost> page = new PageInfo<CommentPost>();
            page.pageNum = pageNum;
            page.pageSize = pageSize;
            try
            {

                List<Comment> result = _UserComments.Find<UserComments>(um => um.username == username).FirstOrDefault().comments;
                result.Reverse();
                page.total = result.Count();
                int start = (pageNum - 1) * pageSize;
                result = result.GetRange(start, Math.Min(pageSize, result.Count() - start));
                page.list =new List<CommentPost>();

                foreach (Comment c in result)
                {
                    CommentPost cp = new CommentPost();
                    cp.comment = c;
                    String postId = _postComments.Find<PostComments>(pc => pc.comments.Any<Comment>(cc => cc._id == c._id)).FirstOrDefault().PostId;
                    cp.post = _AllPosts.Find<Post>(p => p.Id == postId).FirstOrDefault();
                    page.list.Add(cp);
                }
                page.size = page.list.Count();
                return Result.success(page);
            }
            catch (NullReferenceException e)
            {
                page.list = new List<CommentPost>();
                return Result.success(page);
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return Result.error("查询失败");
            }
        }
    }
}
