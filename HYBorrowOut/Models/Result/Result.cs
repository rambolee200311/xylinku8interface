using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HYBorrowOut.Models.Result
{
    public class Result
    {
        
            public string oacode { get; set; }//oa客户编码
            public string u8code { get; set; }//u8客户编码
            public string recode { get; set; }//结果（0 成功,其他数字 失败）
            public string remsg { get; set; }//消息(空 成功,不为空 失败原因)
        
    }
}