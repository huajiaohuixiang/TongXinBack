using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Service;
using TongXinBack.Model;
using Microsoft.AspNetCore.Authorization;
using ReturnParam;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly LikeService _likeService;

        public PostController(PostService postService,LikeService likeService)
        {
            _postService = postService;
            _likeService = likeService;
        }


        [Route("CreatePost")]
        [HttpPost]
        public async Task<ActionResult<object>> CreatePostAsync([FromForm] string username, [FromForm] string content,  List<string> imageList)
        {
            Task< Result> result= _postService.createAsync(username,content,imageList);
            await result;
            return StatusCode(200, result.Result);
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
        [HttpPost]
        public ActionResult<Object> getUserPost([FromForm] string username,[FromForm]  int pageNum, [FromForm] int pageSize)
        {
            var result = _postService.GetUserPost(username,pageNum,pageSize);

            return StatusCode(200, result);
        }

        [Route("getAllUserPost")]
        [HttpGet]
        public ActionResult<Object> getAllUserPost()
        {
            var result = _postService.GetAllUserPostCapped();

            return StatusCode(200, result);
        }


        [Route("getAllUserPostPage")]
        [HttpGet]
        public ActionResult<Object> getAllUserPostPage(int pageNum,int pageSize)
        {
            var result = _postService.GetAllUserPostCappedPage(pageNum,pageSize);

            return StatusCode(200, result);
        }

        [Route("addView")]
        [HttpPost]
        public async  Task<ActionResult<Object>> adView([FromForm] string postId, [FromForm] string username)
        {
            Task<Result> result = _postService.addViewAsync(postId,username);
            await result;
            return StatusCode(200, result.Result);
        }

        [Route("UserAddLike")]
        [HttpPost]
        public async Task< ActionResult<Object>> UserAddLike([FromForm] string postId, [FromForm] string username)
        {
            Task<Result> result = _likeService.userLikeAsync(username, postId);
            await result;
            return StatusCode(200, result.Result);
        }
        [Route("UserUnLike")]
        [HttpPost]
        public async Task< ActionResult<Object>> UserUnLike([FromForm] string postId, [FromForm] string username)
        {
            Task<Result> result = _likeService.userUnLikeAsync(username, postId);
            await result;
            return StatusCode(200, result.Result);
        }

        [Route("IsUserLike")]
        [HttpPost]
        public ActionResult<Object> IsUserLike([FromForm] string postId, [FromForm] string username)
        {
            var result = _likeService.isUserLike(username, postId);

            return StatusCode(200, result);
        }

        [Route("GetUserLike")]
        [HttpPost]
        public ActionResult<Object> GetUserLike([FromForm] string username, [FromForm] int pageNum, [FromForm] int pageSize)
        {
            var result = _likeService.getUserLike(username,pageNum,pageSize);

            return StatusCode(200, result);
        }


    }
}
