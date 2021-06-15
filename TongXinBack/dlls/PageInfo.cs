using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.dlls
{
    public class PageInfo<E>
    {

        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<E> list { get; set; }
        public int size { get; set; }
    }
}
