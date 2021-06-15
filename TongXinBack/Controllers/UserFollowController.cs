using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TongXinBack.Service;
using TongXinBack.Model;
using TongXinBack.DTO;
using Microsoft.AspNetCore.Authorization;
using ReturnParam;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class UserFollowController : ControllerBase
    {
        private readonly FollowService _followService;

        public UserFollowController(FollowService followService)
        {
            this._followService = followService;

        }

        [Route("Follow1")]
        [HttpPost]
        public ActionResult<Object> Follow1(UserFollowParam userFollowParam)
        {
            Result result= _followService.follow(userFollowParam.u1,userFollowParam.u2);
            return StatusCode(200, result);
        }
        [Route("Follow")]
        [HttpPost]
        public ActionResult<Object> Follow( [FromForm]string username1, [FromForm] string username2, [FromForm] string nickname1, [FromForm] string nickname2)
        {
            Result result = _followService.follow(username1,username2,nickname1,nickname2);
            return StatusCode(200, result);
        }
        [Route("UnFollow")]
        [HttpPost]
        public ActionResult<Object> Delete([FromForm] string uid1, [FromForm] string uid2)
        {

            Result result = _followService.unfollow(uid1, uid2);
            return StatusCode(200, result);
        }

        [Route("getOneFollowing")]
        [HttpGet]
        public ActionResult<Object> getOneFollowing(string username)
        {
            var result = _followService.GetFollowing(username);
            
            return StatusCode(200, result);
        }
        [Route("getOneFollowed")]
        [HttpGet]
        public ActionResult<Object> getOneFollowed(string username)
        {
            var result = _followService.GetFollowed(username);

            return StatusCode(200, result);
        }
        [Route("isFollow")]
        [HttpPost]
        public ActionResult<Object> isFollow([FromForm]string username,[FromForm]string username2)//1是否关注2
        {
            var result = _followService.isFollow(username,username2);

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