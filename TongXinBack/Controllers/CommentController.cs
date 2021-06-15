using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Service;
using TongXinBack.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReturnParam;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;
        
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }


        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<Object>> Create([FromForm] string postId, [FromForm] string username, [FromForm] string commentInfo)
        {
           Task<Result> result=  _commentService.CreateCommentAsync(postId, username,commentInfo);
           await  result;
            return StatusCode(200, result.Result);
        }


        [Route("getPostComment")]
        [HttpGet]
        public ActionResult<Object> getPostComment(string postId,int pageNum,int pageSize)
        {
           var result=_commentService.Get(postId,pageNum,pageSize);
            return StatusCode(200, result);
        }

        [Route("getUserComment")]
        [HttpPost]
        public ActionResult<Object> getUserComment([FromForm]string username, [FromForm] int pageNum, [FromForm] int pageSize)
        {
            var result = _commentService.GetUserComment(username,  pageNum,  pageSize);
            return StatusCode(200, result);
        }
    }
}
