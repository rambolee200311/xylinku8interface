using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleBill
{
    public class Salebill_body
    {
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称                
        public decimal iquantity { get; set; }//数量-数量        
        public decimal itaxunitprice { get; set; }//含税单价-含税单价
        public decimal iunitprice { get; set; }//无税单价-无税单价
        public decimal imoney { get; set; }//无税金额-无税金额
        public decimal itax { get; set; }//税额-税额
        public decimal isum { get; set; }//价税合计-价税合计
    }
}