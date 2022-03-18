using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DispatchReturnBack
{
    public class DispatchReturnBack_head
    {
        public string ccode { get; set; }//单据编号-外部借出还回单号
        public DateTime ddate { get; set; }//单据日期
        public string cexchan { get; set; }//退换货(退货/换货)
        public string cust_name { get; set; }//客户名称-单位
        public string dept_name { get; set; }//申请部门-部门名称
        public string person_name { get; set; }//申请人-业务员姓名
        public string cmemo { get; set; }//备注-备注
        
    }
}