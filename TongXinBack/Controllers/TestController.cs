using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [DllImport("C:\\Users\\花椒茴香\\source\\repos\\ATLProject1\\Debug\\ATLProject2.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int MyAdd(int x, int y);


        [Route("TestCLR")]
        [HttpGet]
        public ActionResult<Object> TestCLR(int x)
        {
            return StatusCode(200, MyAdd(2, 3));
            // return StatusCode(200, CppCLR.Calculator.GetCalculator().Calculate(5));
        }
    }
}
