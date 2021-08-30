using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleBill
{
    public class Salebill_head
    {
        public string ccode { get; set; }//发票号-单据编号      
        public DateTime ddate { get; set; }//开票日期-开票日期
        public string cord_code { get; set; }//订单号-订单号（U8
        public string cust_name { get; set; }//客户名称-客户简称
        public string dept_name { get; set; }//申请部门-部门名称
        public string person_name { get; set; }//申请人-业务员
        public string cust_address { get; set; }//客户地址-客户地址
        public string cust_bank { get; set; }//开户银行-开户银行
        public string cust_account { get; set; }//银行账号-账号
        public decimal tax_rate { get; set; }//税率-税率
        public string cmemo { get; set; }//备注-备注
        
    }
}