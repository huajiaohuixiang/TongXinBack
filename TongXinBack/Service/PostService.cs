using ReturnParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface PostService
    {
        //  Result createAsync(string username, string content, List<String> imageList);
        Task<Result> createAsync(string username, string content, List<string> imageList);
        Post delete(string username, string postid);

        Post getPost(string postId);

        UserPost CreateNullList(string username);
        UserFollowsPosts CreateUserFollowedNullList(string username);


        Result GetUserPost(string username, int pageNum, int pageSize);


        List<Post> GetAllUserPostCapped();
         Result GetAllUserPostCappedPage(int pageNum, int pageSize);
        //ToDo  获取用户关注的帖子集合
        //     List<Post> GetUserFollowsCapped();

  //      Result addView(string postId, string username);
        Task<Result> addViewAsync(string postId, string username);

    }
}
