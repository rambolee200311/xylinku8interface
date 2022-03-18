using System;
using System.Collections.Generic;
using System.Web;

namespace HYBorrowDll.Models.Borrowout
{
    public class Borrowout_head
    {
        public string ccode { get; set; }//单据编号-外部借出借用单号        
        public DateTime ddate { get; set; }//单据日期-单据日期
        public string ctype { get; set; }//单位类型-单位类型（部门，客户）
        public string cust_name { get; set; }//客户名称
        public string dept_name { get; set; }//申请部门-部门名称
        public string person_name { get; set; }//发起人-业务员姓名
        public string cmemo { get; set; }//备注-备注
        public string recv_name { get; set; }//收货联系人-收货人姓名
        public string recv_phone { get; set; }//联系电话-收货人电话
        public string recv_address { get; set; }//收货地址-收货地址
    }
}