using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TongXinBack.Model;
namespace TongXinBack.DTO
{
    public class CommentPost
    {
        public Post post { get; set; }
        public Comment comment { get; set; }
    }
}
