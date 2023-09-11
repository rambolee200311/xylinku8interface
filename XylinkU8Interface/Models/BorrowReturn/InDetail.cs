using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 归还detail
 */
namespace XylinkU8Interface.Models.BorrowReturn
{
    public class InDetail
    {
        /*
         "cinvCode": "62045-00224-001", // ⼦件产品编码-存货编码
            "cinvName": "ME45视频会议终端", // ⼦件产品名称-存货名称
            "unitName": "台", // 单位-单位
            "iquantity": 2, // 数量-数量 
         */
        public String cinvCode{ get; set; } // ⼦件产品编码-存货编码
        public String cinvName{ get; set; }// ⼦件产品名称-存货名称
        public String unitName{ get; set; }// 单位-单位
        public Decimal iquantity { get; set; }// 数量-数量 
        public List<InSncode> sncodes { get; set; }// 明细序列号
    }
}