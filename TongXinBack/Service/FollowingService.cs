using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    /**
     * 该接口维护关注列表
     * 
     **/
    interface FollowingService
    {
        void add(string userid, FollowUserInfo u);              //关注        userid为被关注的  向userid的关注列表添加一项


        void remove(string userid, string uid2);                          //取关
        void CreateNullList(string username);


        //获取一个用户的所有关注
        following Get(string username);
    }
}
