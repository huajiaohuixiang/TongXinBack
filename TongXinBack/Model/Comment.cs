using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Model
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public  string _id { get; set; }


        public string username { get; set; }
        public DateTime comTime { get; set; }
        public string comment { get; set; }
        public int likes { get; set; }
        public int comments { get; set; }
    }
}
/**
{
    "_id": "string",
  "
}

*/