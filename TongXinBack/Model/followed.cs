using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Model
{

    public class followed
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public followed(string id)
        {
            this.userid = id;

        }
        public string userid { get; set; }



        public List<FollowUserInfo> followeds { get; set; }
    }
}
