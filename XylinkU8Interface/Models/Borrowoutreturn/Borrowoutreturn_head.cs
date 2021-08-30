using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Borrowoutreturn
{
    public class Borrowoutreturn_head
    {
        public string ccode { get; set; }//单据编号-外部借出还回单号
        public string oriccode { get; set; }//关联试用单号-外部借出借用单号
        public DateTime ddate { get; set; }//单据日期-归还日期
        public string ctype { get; set; }//单位类型-单位类型（部门，客户）
        public string cust_name { get; set; }//客户名称-单位
        public string dept_name { get; set; }//申请部门-部门名称
        public string person_name { get; set; }//申请人-业务员姓名
        public string cmemo { get; set; }//备注-备注
        public string recv_name { get; set; }//联系人-联系人
        public string recv_type { get; set; }//收发类别名称-入库类别(借出还回入库)
        
    }
}