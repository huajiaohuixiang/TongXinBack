using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    class FollowServiceImpl :  FollowService
    {
        private readonly FollowedService _followedservice;
        private readonly FollowingService _followingService;

        public FollowServiceImpl(FollowedService fs,FollowingService fsing)
        {
            _followedservice = fs;
            _followingService = fsing;
        }

        public void CreateNullList(string username)
        {
            _followedservice.CreateNullList(username);
            _followingService.CreateNullList(username);
        }

        public void follow(FollowUserInfo u1, FollowUserInfo u2)
        {
            _followingService.add(u1.userid, u2);
            _followedservice.add(u2.userid,u1);
        }

        public following GetFollowing(string username)
        {
           return  _followingService.Get(username);
        }

        public void unfollow(string userid1, string userid2)
        {
            _followedservice.remove(userid2, userid1);
            _followingService.remove(userid1, userid2);

        }

        public  followed GetFollowed(string username)
        {
            return _followedservice.Get(username);
        }
    }
}
