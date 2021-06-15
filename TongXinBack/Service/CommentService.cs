using ReturnParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface CommentService
    {
        Task< Result> CreateCommentAsync(string postId, string username, string commentInfo);
        void DeleteComment(string postId, Comment comment);
        void CreateNullList(string postId);

        void CreateUserCommentNullList(string username);

        Result Get(string postId, int pageNum, int pageSize);


        Result GetUserComment(string username, int pageNum, int pageSize);

    }
}
