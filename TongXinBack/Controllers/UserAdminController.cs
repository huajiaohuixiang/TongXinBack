using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TongXinBack.DTO;
using TongXinBack.Service;
using TongXinBack.Model;
using ReturnParam;
using Microsoft.AspNetCore.Authorization;
using IdentityCode;
namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IdentityCodeService _identityCodeService;

        public UserAdminController(UserService userService,IdentityCodeService identityCodeService)
        {
            this._userService = userService;
            _identityCodeService = identityCodeService;
        }

        [Route("Register")]
        [HttpPost]
        public ActionResult<Object> Register([FromForm] UserRegisterParam userParam)
        {
            Console.WriteLine("111");
                return StatusCode(200, _userService.register(userParam));        
        }

        [Route("GetCode")]
        [HttpPost]
        public ActionResult<Object> GetCode([FromForm] GetCodeParam getCodeParam)
        {
            Console.WriteLine("222");
            try
            {
                _identityCodeService.sendIdCode(getCodeParam.username, getCodeParam.stuid + "@tongji.edu.cn");
                return StatusCode(200, Result.success());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(200, Result.error("发送验证码失败，请骚后再试"));
            }
            
        }



        [Route("Login")]
        [HttpPost]
        public ActionResult<Object> Login([FromForm] UserLoginParam userLoginParam)
        {
            if (userLoginParam == null)
            {
                return StatusCode(200, Result.paramIsNull());
            }
            Result code = _userService.login(userLoginParam);            
            return StatusCode(200,code);           
        }


        [Route("getUser")]
        [HttpGet]
        [Authorize]
        public ActionResult<Object> getUser(string username)
        {
            return StatusCode(200, _userService.getByUsername(username));
            
        }


        [Route("getAllUser")]
        [HttpGet]
        [Authorize]
        public ActionResult<Object> getAllUser()
        {
           return  StatusCode(200, _userService.getAllUser());
        }

        [Route("UpdateUserAvatar")]
        [HttpPost]
        public ActionResult<Object> UpdateUserAvatar([FromForm] string username, [FromForm] string avatar)
        {
            var result = _userService.updataAvatar(username,avatar);

            return StatusCode(200, result);
        }

        [Route("getUserAvatar")]
        [HttpPost]
        public ActionResult<Object> getUserAvatar([FromForm] string username)
        {
            var result = _userService.getAvatar(username);

            return StatusCode(200, result);
        }
    }
}
