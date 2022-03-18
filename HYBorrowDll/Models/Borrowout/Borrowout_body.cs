using System;
using System.Collections.Generic;
using System.Web;

namespace HYBorrowDll.Models.Borrowout
{
    public class Borrowout_body
    {
        public string req_id { get; set; }//明细唯一标识
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称
        public string unit_name { get; set; }//单位-单位
        public decimal iquantity { get; set; }//数量-数量
        public string cmemo { get; set; }//备注-备注
        public DateTime back_date { get; set; }//结束时间（主表）-预计归还时间
        public string recv_name { get; set; }//收货联系人-收货人姓名
        public string recv_phone { get; set; }//联系电话-收货人电话
        public string recv_address { get; set; }//收货地址-收货地址
    }
}