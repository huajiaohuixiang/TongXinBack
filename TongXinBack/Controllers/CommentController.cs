using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Service;
using TongXinBack.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;
        
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }


        [Route("/Comment/Create")]
        [HttpPost]
        public ActionResult<Object> Create(string postId,Comment comment)
        {
            _commentService.CreateComment(postId, comment);

            return StatusCode(200, "评论成功");
        }


        [Route("/Comment/getPostComment")]
        [HttpGet]
        public ActionResult<Object> getPostComment(string postId)
        {
           var result=_commentService.Get(postId);
            return StatusCode(200, result);
        }

        [Route("/Comment/getUserComment")]
        [HttpGet]
        public ActionResult<Object> getUserComment(string username)
        {
            var result = _commentService.GetUserComment(username);
            return StatusCode(200, result);
        }
    }
}
