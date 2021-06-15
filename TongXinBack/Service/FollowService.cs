using ReturnParam;
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


        Result follow(FollowUserInfo u1, FollowUserInfo u2);
        Result follow(string uid1, string uid2, string unickname1, string unickname2);

        Result unfollow(string userid1, string userid2);
        void CreateNullList(string username);

        Result GetFollowing(string username);
        Result GetFollowed(string username);

        Result isFollow(string username, string username2);
     }
}
