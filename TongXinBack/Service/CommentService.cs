using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.Service
{
    public interface CommentService
    {
        void CreateComment(string postId, Comment comment);


        void DeleteComment(string postId, Comment comment);
        void CreateNullList(string postId);

        void CreateUserCommentNullList(string username);

        PostComments Get(string postId);


        List<Comment> GetUserComment(string username);

    }
}
