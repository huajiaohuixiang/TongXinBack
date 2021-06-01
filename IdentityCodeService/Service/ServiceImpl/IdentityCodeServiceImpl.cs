
using RedisTemplet;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCode
{
    public class IdentityCodeServiceImpl : IdentityCodeService
    {
        private readonly SendMailService _sendMailService;
        private readonly  IDatabase _redis;
        public   IdentityCodeServiceImpl(SendMailService sendMailService, RedisHelper redisHelper)
        {
            _sendMailService = sendMailService;
            _redis = redisHelper.GetDatabase();

        }

        public string generatorCode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            return random.Next(0, 999999).ToString();          
        }

        public void sendIdCode(string username, string mailaddress)
        {
            string code = generatorCode();
            _sendMailService.sendIdentityCode(username, mailaddress,code);
            TimeSpan ts = new TimeSpan(0, 5, 0);
            _redis.StringSet("RegisterCode:"+username, code, expiry: ts);
		}

        public bool verifyCode(string username, string code)
        {
            string rediscode =_redis.StringGet("RegisterCode:"+username);
            if (rediscode == null || !rediscode.Equals(code))
            {
                return false;
            }
            return true;
        }
    }
}
