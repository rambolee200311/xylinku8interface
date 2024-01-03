using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用head
 */
namespace XylinkU8Interface.Models.BorrowTrial
{
    /*
    "head": {
                "ccode": "SY202307260001", // CRM单据编号-U8外部订单号
                "ddate": "2023-07-26", // 单据⽇期
                "cmemo": "备注", // 备注
                "custName": "客户名称", // 客户名称
                "personName": "唐媛媛", // 申请⼈-业务员
                "ctype": "客户" // 客户名称-单位
            }
 
    */
    public class InHead
    {
        public String ccode { get; set; }// CRM单据编号-U8外部订单号
        public String ddate { get; set; }// 单据⽇期
        public String cmemo { get; set; }// 备注
        public String custName { get; set; }// 客户名称
        public String personName { get; set; }// 申请⼈-业务员
        public String ctype { get; set; }//客户

        public String sendType { get; set; }// U8其他出库单-出库类型（2023-12-07 新增字段）
    }
}