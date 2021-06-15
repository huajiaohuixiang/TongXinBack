using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TongXinBack.Config;
using TongXinBack.DTO;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    class FollowingServiceImpl : FollowingService
    {
        private IMongoCollection<following> _followings;
        private readonly IMongoCollection<User> _users;

        public FollowingServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _followings = database.GetCollection<following>("followings");
            _users = database.GetCollection<User>("Users");

        }


        public void add(string userid, FollowUserInfo u)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$push", new BsonDocument("followings", u.ToBsonDocument()));//new BsonValue(post)
            _followings.UpdateOne(filter, update);
            _users.UpdateOne<User>(user => user.username == userid, new BsonDocument("$inc", new BsonDocument("following", 1)));
        
        }

        public void remove(string userid, string uid2)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$pull", new BsonDocument("followings", new BsonDocument("userid", uid2)));//删除
            _followings.UpdateOne(filter, update);
            _users.UpdateOne<User>(user => user.username == userid, new BsonDocument("$inc", new BsonDocument("following", -1)));

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

        public bool isFollow(string username, string username2)
        {
            following oneFollowing = _followings.Find<following>(ff => ff.userid == username).FirstOrDefault();
            foreach(FollowUserInfo fu in oneFollowing.followings)
            {
                if (fu.userid.Equals( username2))
                {
                    return true;
                }
            }
            return false;

        }
    }
}
