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

        public string nickname { get; set; }

        public string avatar { get; set; }

        public DateTime publishTime { get; set; }

        public string content { get; set; }

        public int likes { get; set; }

        public int comments { get; set; }

        public int views { get; set; }

        public  List<String> viewList { get; set; }

        //用户名
        public List<String> likesList { set; get; }

        //图片地址
        public List<String> photoesList { get; set; }

        public DateTime createdTime { get; set; }
        public DateTime updatedTime { get; set; }

    }
}
