using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用detail
 */
namespace HYBorrowOut.Models.BorrowTrial
{
    /*
    "cinvCode": "62045-00224-001", // 产品编码-存货编码
    "cinvName": "ME45视频会议终端", // 产品名称-存货名称
    "unitName": "台", // 单位-单位
    "iquantity": 2, // 数量-数量
     */
    public class InDetail
    {
        public String cinvCode { get; set; } // 产品编码-存货编码
        public String cinvName { get; set; } // 产品名称-存货名称
        public String unitNam { get; set; } // 单位-单位
        public Decimal iquantity { get; set; }// 数量-数量
        public List<InSncode> sncodes { get; set; }//序列号明细
    }
}