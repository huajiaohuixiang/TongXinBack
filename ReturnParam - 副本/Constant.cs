using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnParam
{
    public  class Constant 
    {

        public   static  String SUCCESS_CODE = "00000";
    public  static   String SUCCESS_MSG = "操作成功";

    public static   String ERROR_CODE = "00001";
    public static   String ERROR_MSG = "操作失败";

    public static   String ERROR_CODE_DEBUG = "00002";

    public static   String ERROR_CODE_PARAM_NULL = "00100";
    public static   String ERROR_MSG_PARAM_NULL = "参数为空";

    public static   String ERROR_CODE_ID_NULL = "00101";
    public static   String ERROR_MSG_ID_NULL = "id为空";

    public static   String ERROR_CODE_OBJ_NULL = "00102";
    public static   String ERROR_MSG_OBJ_NULL = "该id对应的对象不存在";

    public static   String ERROR_CODE_PAGE_NULL = "00103";
    public static   String ERROR_MSG_PAGE_NULL = "分页参数为空";

    public static   String DEFAULT_USER_ID = "bef95afda1cf4d42a5e232b5be131f66";

}
}
