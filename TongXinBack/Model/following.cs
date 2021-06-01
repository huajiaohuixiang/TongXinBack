using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Model
{
    
    public class following
    {
        public following(string id)
        {
            this.userid = id;

        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string userid { get; set; }


        //
        public List<FollowUserInfo> followings { get; set; }


    }
}
