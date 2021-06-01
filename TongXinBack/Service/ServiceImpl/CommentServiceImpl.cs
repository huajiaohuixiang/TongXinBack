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
    class CommentServiceImpl : CommentService
    {
        private readonly IMongoCollection<PostComments> _postComments;
        private readonly IMongoCollection<UserComments> _UserComments;
        public CommentServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _postComments = database.GetCollection<PostComments>("PostComments");
            _UserComments = database.GetCollection<UserComments>("UserComments");
        }
        public void CreateComment(string postId, Comment comment)
        {
            //向帖子中添加评论
            comment._id = ObjectId.GenerateNewId().ToString();
            var filter = new BsonDocument("PostId", postId);
            var update = new BsonDocument("$push", new BsonDocument("comments", comment.ToBsonDocument()));//new BsonValue(post)
            _postComments.UpdateOne(filter, update);
            //向用户的Comment库中添加
            var usercmfilter = new BsonDocument("username", comment.username);
            var usercmupdate = new BsonDocument("$push", new BsonDocument("comments", comment.ToBsonDocument()));//new BsonValue(post)
            _UserComments.UpdateOne(usercmfilter, usercmupdate);
            
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

        public PostComments Get(string postId) => 
            _postComments.Find<PostComments>(postcomment => postcomment.PostId == postId).FirstOrDefault();

        public List<Comment> GetUserComment(string username)
        {
            List<Comment> result= _UserComments.Find<UserComments>(um => um.username == username).FirstOrDefault().comments;
            result.Reverse();
            return result;
        }
    }
}
