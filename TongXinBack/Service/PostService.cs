using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface PostService
    {
        Post create(string username,Post post);

        Post delete(string username, string postid);

        Post getPost(string postId);

        UserPost CreateNullList(string username);
        UserFollowsPosts CreateUserFollowedNullList(string username);


        List<Post> GetUserPost(string username);


        List<Post> GetAllUserPostCapped();

        //ToDo  获取用户关注的帖子集合
   //     List<Post> GetUserFollowsCapped();

    }
}
