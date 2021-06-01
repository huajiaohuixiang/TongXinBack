using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Model
{
    public class FollowUserInfo
    {
        public FollowUserInfo(string id, string name)
        {
            this.userid = id;
            this.nackname = name;
        }
        public string userid { get; set; }
        public string nackname { get; set; }
    }
}
