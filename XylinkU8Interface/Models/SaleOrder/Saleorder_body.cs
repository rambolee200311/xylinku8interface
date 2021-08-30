using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleOrder
{
    public class Saleorder_body
    {
        public string req_id { get; set; }//明细唯一标识
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称        
        public string cemb_name { get; set; }//嵌入式名称-嵌入式名称
        public decimal iquantity { get; set; }//数量-数量
        public decimal iquoteprice { get; set; }//标准报价-报价
        public decimal itaxunitprice { get; set; }//折后单价-含税单价
        public decimal isum { get; set; }//折后金额-价税合计
        public string cprv_name { get; set; }//所在省-所在省
        public string ccty_name { get; set; }//所在市-所在市
        public string cdis_name { get; set; }//所在区-所在区
        public string cfst_rcv_name { get; set; }//收货联系人-第一收货人
        public string cfst_rcv_phone { get; set; }//联系电话-第一收货人手机
        public string cfst_rcv_address { get; set; }//收货地址-收货详细地址
        public string cord_name { get; set; }//收货联系人-订货人
        public string cord_phone { get; set; }//联系电电话-订货人手机

        public decimal taxrate { get; set; }//税率
    }
}