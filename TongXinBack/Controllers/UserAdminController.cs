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

        [Route("/Register")]
        [HttpPost]
        public ActionResult<Object> Register(UserRegisterParam userParam)
        {                    
                return StatusCode(200, _userService.register(userParam));        
        }

        [Route("GetCode")]
        [HttpPost]
        public ActionResult<Object> GetCode([FromForm] GetCodeParam getCodeParam)
        {
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
            User result = _userService.getByUsername(username);
            if (result==null)
            {
                return StatusCode(400,"未找到该用户");
            }
            else
            {
                return  StatusCode(200, result);
            }

        }


        [Route("getAllUser")]
        [HttpGet]
        [Authorize]
        public ActionResult<Object> getAllUser()
        {
            List<User> result = _userService.getAllUser();
            if (result == null)
            {
                return StatusCode(400, "BadRequest！！！");
            }
            else
            {
                return StatusCode(200, result);
            }

        }
    }
}
