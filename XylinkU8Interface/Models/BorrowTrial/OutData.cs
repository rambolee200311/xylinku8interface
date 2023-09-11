using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用返回消息
 */
namespace XylinkU8Interface.Models.BorrowTrial
{
    public class OutData
    {
        /*
            "oacode": "ZJSQGH202307260001001", // CRM单据编号
            "u8code": "ZJSQGH202307260001001112505" // U8借出归还单
            "u8rdcode": "QTRK202307260002", // U8其他⼊库单
            "recode": "0", // 结果编码
            "remsg": "xxx" // 结果描述 
         */
            public string oacode{get;set;} // CRM单据编号
            public string u8code{get;set;} // U8借出归还单
            public string u8rdcode{get;set;}  // U8其他⼊库单
            public string recode{get;set;} // 结果编码
            public string remsg { get; set; }  // 结果描述 
    }
}