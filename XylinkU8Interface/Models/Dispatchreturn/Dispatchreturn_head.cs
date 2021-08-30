using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Dispatchreturn
{
    public class Dispatchreturn_head
    {
        public string ccode { get; set; }//单据编号-CRM退货单号       
        public DateTime ddate { get; set; }//申请日期-退货日期   
        public string cust_name { get; set; }//客户名称（订单）-客户简称
        public string dept_name { get; set; }//申请部门（订单）-销售部门
        public string person_name { get; set; }//申请人-业务员
        public string cmemo { get; set; }//备注-备注
        public string cexchan { get; set; }//退换货
    }
}