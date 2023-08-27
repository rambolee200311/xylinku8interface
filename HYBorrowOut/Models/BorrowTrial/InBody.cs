using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用body
 */
namespace HYBorrowOut.Models.BorrowTrial
{
    /*
        "reqId": "9047034573724324", // 产品明细唯⼀标识
        "cinvCode": "62045-00224-001", // 产品编码-存货编码
        "cinvName": "ME45视频会议终端", // 产品名称-存货名称
        "unitName": "台", // 单位-单位
        "iquantity": 2, // 数量-数量
        "cmemo": "备注", // 备注
        "backDate": "2023-09-01", // 结束时间（主表）-预计归还时间
        "recvName": "赵婷婷", // 收货联系⼈-收货⼈姓名
        "recvPhone": "15765674545", // 联系电话-收货⼈电话
        "recvAddress": "北京市/北京市/东城区三⼭五园艺术中⼼", // 收货地址-收货地址
         */
    public class InBody
    {
        public String reqId { get; set; }// 产品明细唯⼀标识
        public String cinvCode { get; set; }// 产品编码-存货编码
        public String cinvName { get; set; }// 产品名称-存货名称
        public String unitName { get; set; }// 单位-单位
        public Decimal iquantity { get; set; }// 数量-数量
        public String cmemo { get; set; }// 备注
        public String backDate { get; set; } // 结束时间（主表）-预计归还时间
        public String recvName { get; set; } // 收货联系⼈-收货⼈姓名
        public String recvPhone { get; set; }// 联系电话-收货⼈电话
        public String recvAddress { get; set; } // 收货地址-收货地址
        public List<InDetail> detail { get; set; }//子件明细
    }
}