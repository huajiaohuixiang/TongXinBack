using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TongXinBack.Config;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    class FollowingServiceImpl : FollowingService
    {
        private IMongoCollection<following> _followings;
        
        public FollowingServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _followings = database.GetCollection<following>("followings");
        }


        public void add(string userid, FollowUserInfo u)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$push", new BsonDocument("followings", u.ToBsonDocument()));//new BsonValue(post)
            _followings.UpdateOne(filter, update);

        }

        public void remove(string userid, string uid2)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$pull", new BsonDocument("followings", new BsonDocument("userid", uid2)));//删除
            _followings.UpdateOne(filter, update);
        }

        public void CreateNullList(string username)
        {
            var newFollowing = new following(username);
            //
            newFollowing.followings = new List<FollowUserInfo>();
            _followings.InsertOne(newFollowing);
        }

        public following Get(string username) =>
            _followings.Find<following>(foing => foing.userid == username).FirstOrDefault();
    }
}
