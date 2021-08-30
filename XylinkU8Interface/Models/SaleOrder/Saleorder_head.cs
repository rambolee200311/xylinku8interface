using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleOrder
{
    public class Saleorder_head
    {
        public string ccode { get; set; }//单据编号-外部单号（新）       
        public DateTime ddate { get; set; }//单据日期-订单日期       
        public string cust_name { get; set; }//客户名称-客户简称
        public string dept_name { get; set; }//申请部门-部门名称
        public string person_name { get; set; }//申请人-业务员
        public string cmemo { get; set; }//备注-备注
        public string crcv_name { get; set; }//收货人-收货人
        public string rcv_phone { get; set; }//收货电话-收货电话
        public string crcv_address { get; set; }//退货地址-收货地址
    }
}