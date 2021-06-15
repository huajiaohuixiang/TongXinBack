using Microsoft.AspNetCore.Mvc;
using ReturnParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.DTO;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface UserService
    {
        Result register(UserRegisterParam newUser);


        Result login(UserLoginParam userLoginParam);


        Result getByUsername(string username);

        Result getAllUser();

        Result updataAvatar(string username,  string avatar);
        Result getAvatar(string username);
    }
}
