using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Service;
using TongXinBack.Model;
using Microsoft.AspNetCore.Authorization;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [Route("CreatePost")]
        [HttpPost]
        public ActionResult<Object> CreatePost(string username,Post post)
        {
            _postService.create(username,post);

            return StatusCode(200, post);
        }

        [Route("getPost")]
        [HttpGet]
        public ActionResult<Object> getPost(string postId)
        {
            Post result = _postService.getPost(postId);
            if (result == null)
            {
                return StatusCode(400, "未找到");
            }
            else
            {
                return StatusCode(200, result);
            }
        }

        [Route("getUserPost")]
        [HttpGet]
        public ActionResult<Object> getUserPost(string userId)
        {
            var result = _postService.GetUserPost(userId);

            return StatusCode(200, result);
        }

        [Route("getAllUserPost")]
        [HttpGet]
        public ActionResult<Object> getAllUserPost()
        {
            var result = _postService.GetAllUserPostCapped();

            return StatusCode(200, result);
        }
    }
}
