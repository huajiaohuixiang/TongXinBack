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
    class FollowedServiceImpl : FollowedService
    {
        private readonly IMongoCollection<followed> _followeds;
        private readonly IMongoCollection<User> _users;

        public FollowedServiceImpl(UserAdminDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var  database = client.GetDatabase(settings.DatabaseName);
            _followeds = database.GetCollection<followed>("followeds");
            _users = database.GetCollection<User>("Users");

        }

        public void add(string userid, FollowUserInfo u)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$push", new BsonDocument("followeds", u.ToBsonDocument()));//new BsonValue(post)
            _followeds.UpdateOne(filter, update);
            _users.UpdateOne<User>(user => user.username == userid, new BsonDocument("$inc", new BsonDocument("followed", 1)));

        }

        public void remove(string userid, string uid2)
        {
            var filter = new BsonDocument("userid", userid);
            var update = new BsonDocument("$pull", new BsonDocument("followeds", new BsonDocument("userid", uid2)));//删除
            _followeds.UpdateOne(filter, update);
            _users.UpdateOne<User>(user => user.username == userid, new BsonDocument("$inc", new BsonDocument("followed", -1)));

        }

        public void CreateNullList(string username)
        {
            var newFollowed = new followed(username);
            newFollowed.followeds = new List<FollowUserInfo>();
            _followeds.InsertOne(newFollowed);
        }

        public followed Get(string username) => _followeds.Find<followed>(foed => foed.userid == username).FirstOrDefault();
    }
}
