using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using ReturnParam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        [Route("uploadAvatar")]
        [HttpPost]
        public ActionResult<Object> PostAvatar(string username,[FromForm] IFormCollection formCollection)
        {
            //TODO 这里username并没有进去
            List<string> result = new List<string>();

            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    ///var/www/html
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    String name = username + "/avatar" + file.FileName;

                    String filename = "/var/www/html/"+ name;
                   
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    Console.WriteLine(username);
                    Console.WriteLine(name);
                    Console.WriteLine(filename);
                    result.Add("http://120.79.157.21/" + name);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return StatusCode(200, Result.error("fail"));
            }
            Console.WriteLine(result.ToString());
            result.ForEach(Console.WriteLine);
            return StatusCode(200, Result.success(result));


        }

        [Route("uploadImage")]
        [HttpPost]
        public ActionResult<Object> PostImage( string username, [FromForm] IFormCollection formCollection)
        {
            List<string> result = new List<string>();

            try
            {
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    ///var/www/html
                    StreamReader reader = new StreamReader(file.OpenReadStream());
                    String content = reader.ReadToEnd();
                    String name = username + "/" + file.FileName;

                    String filename = "/var/www/html/" + name;

                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                    string directory = "/var/www/html/" + username + "/";
                    if (System.IO.Directory.Exists(directory)) 
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        // 复制文件
                        file.CopyTo(fs);
                        // 清空缓冲区数据
                        fs.Flush();
                    }
                    Console.WriteLine(username);
                    Console.WriteLine(name);
                    Console.WriteLine(filename);
                    result.Add("http://120.79.157.21/" + name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return StatusCode(200, Result.error("fail"));
            }
            //Console.WriteLine(result.ToString());
            //result.ForEach(Console.WriteLine);
            //Console.WriteLine(result.Count());
            return StatusCode(200, Result.success(result));


        }
    }
}
