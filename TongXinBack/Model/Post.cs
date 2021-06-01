using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TongXinBack.Model
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string username { get; set; }

        public string nackname { get; set; }

        public DateTime publishTime { get; set; }

        public string content { get; set; }

        public int likes { get; set; }

        public int comments { get; set; }

    }
}
