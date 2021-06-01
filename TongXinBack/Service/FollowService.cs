using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface FollowService
    {
        /**
         * 调用FollowingService和FollowedService
         * 实现关注取关操作
         * */
         

        void follow(FollowUserInfo u1, FollowUserInfo u2);


        void unfollow(string userid1, string userid2);
        void CreateNullList(string username);

        following GetFollowing(string username);
        followed GetFollowed(string username);
    }
}
