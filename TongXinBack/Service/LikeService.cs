
using ReturnParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Service
{
    public interface LikeService
    {
        void createNewForUser(string username);

        Task<Result> userLikeAsync(string username, string postId);

        Task< Result> userUnLikeAsync(string username, string postId);

        bool isUserLike(string username, string postId);

        Result getUserLike(string username, int pageNum, int pageSize);
    }
}
