using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Model
{
    //该类和UserPost没什么区别只不过是它表示用户关注的人的post列表，是一个固定大小的，方便用户打开时快速获得关注的人的信息。
    public class UserFollowsPosts
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        public string username;


        public List<string> userFollowedsPostList { get; set; }

    }
}
