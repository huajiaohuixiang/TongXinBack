using Microsoft.EntityFrameworkCore.Internal;
using ReturnParam;
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

        public Result follow(FollowUserInfo u1, FollowUserInfo u2)
        {
            try
            {
                _followingService.add(u1.userid, u2);
                _followedservice.add(u2.userid, u1);
                return Result.success("关注成功");
            }catch(Exception e)
            {
                return Result.error("关注失败");
            }


        }
        public Result follow(string  uid1, string uid2,string unickname1,string unickname2)
        {
            FollowUserInfo u1 = new FollowUserInfo(uid1,unickname1);
            FollowUserInfo u2 = new FollowUserInfo(uid2, unickname2);


            try
            {
                _followingService.add(u1.userid, u2);
                _followedservice.add(u2.userid, u1);
                return Result.success("关注成功");
            }
            catch (Exception e)
            {
                return Result.error("关注失败");
            }


        }
        public Result GetFollowing(string username)
        {
            try { 
                following oneFollowings = _followingService.Get(username);
                return Result.success(oneFollowings);
            }
            catch (Exception e)
            {
                return Result.error("获取失败");
            }

        }

        public Result unfollow(string userid1, string userid2)
        {
            try
            {
                _followedservice.remove(userid2, userid1);
                _followingService.remove(userid1, userid2);
                return Result.success("取消关注成功");
            }
             catch (Exception e)
            {
                Console.WriteLine(userid1 + "," + userid2);
                Console.WriteLine(e.ToString());
                return Result.error("获取失败");
            }
        }

        public Result GetFollowed(string username)
        {
            try
            {
                followed oneFolloweds=_followedservice.Get(username);
                return Result.success(oneFolloweds);
            }catch(Exception e)
            {
                return Result.error("获取失败");
            }
           
        }

        public Result isFollow(string username, string username2)
        {
            try
            {
                if (_followingService.isFollow(username,username2))
                {
                    return Result.success("1");
                }
                else
                {
                    return Result.success("0");
                }
            }catch(Exception e)
            {
                return Result.success("0");
            }
        }
    }
}
