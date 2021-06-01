using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TongXinBack.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


       
        public string username { get; set; }
        public string nickname { get; set; }
        public string password { get; set; }
        public int following { get; set; }
        public int followed { get; set; }

        public string stu_id { get; set; }

        public DateTime regtime { get; set; }
    }
}
