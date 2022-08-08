using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Dispatchreturn
{
    public class Dispatchreturn_body
    {
        public string req_id { get; set; }//明细唯一标识
        public string ori_req_id { get; set; }//原订单明细唯一标识
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称        
        public string unit_name { get; set; }//单位-主计量
        public decimal iquantity { get; set; }//数量-数量
        public decimal iquoteprice { get; set; }//标准报价-报价
        public decimal itaxunitprice { get; set; }//折后单价-含税单价
        public decimal isum { get; set; }//折后金额-价税合计
        public string cwhname { get; set; }//仓库名称-仓库名称
        //public string cord_code { get; set; }//订单号-订单号
        //public string cdsp_code { get; set; }//发货单号-原发货单号"
        public string creason { get; set; }//退货原因-退货原因
        public decimal itaxrate { get; set; }
        //public string requestid { get; set; }
        //public string sn { get; set; }//产品序列号

    }
}