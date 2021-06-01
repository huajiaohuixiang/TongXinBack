using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TongXinBack.Service;
using TongXinBack.Model;
using TongXinBack.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFollowController : ControllerBase
    {
        private readonly FollowService _followService;

        public UserFollowController(FollowService followService)
        {
            this._followService = followService;

        }

        [Route("/Follow")]
        [HttpPost]
        public ActionResult<Object> Follow(UserFollowParam userFollowParam)
        {
            _followService.follow(userFollowParam.u1,userFollowParam.u2);
            return StatusCode(200, "关注成功");
        }

        [Route("/UnFollow")]
        [HttpPost]
        public ActionResult<Object> Delete(string uid1,string uid2)
        {

            _followService.unfollow(uid1, uid2);
            return StatusCode(200, "取消关注成功");
        }

        [Route("/getOneFollowing")]
        [HttpGet]
        public ActionResult<Object> getOneFollowing(string username)
        {
            var result = _followService.GetFollowing(username);
            
            return StatusCode(200, result);
        }
        [Route("/getOneFollowed")]
        [HttpGet]
        public ActionResult<Object> getOneFollowed(string username)
        {
            var result = _followService.GetFollowed(username);

            return StatusCode(200, result);
        }


    }
}


/**
 * 测试Json
 * {
 "u1": {
   "userid": "huajiaohuixiang",
   "nackname": "花椒茴香"
  },
"u2": {
    "userid": "rhy",
    "nackname": "憨憨"
}
}
**/