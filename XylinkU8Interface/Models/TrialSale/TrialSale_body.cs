using System;
using System.Collections.Generic;
using System.Web;

namespace XylinkU8Interface.Models.TrialSale
{
    public class TrialSale_body
    {
        public string req_id { get; set; }//明细唯一标识
        public string ori_req_id { get; set; }//原借用单明细唯一标识

        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称
        public string unit_name { get; set; }//单位-单位
        public decimal iquantity { get; set; }//母件数量-数量
        public decimal iquoteprice { get; set; }//标准报价-报价
        public decimal itaxunitprice { get; set; }//折后单价-含税单价
        public decimal isum { get; set; }//折后金额-价税合计
        public string cmemo { get; set; }//备注-备注
        public string cwhname { get; set; }//仓库名称-仓库名称
        public decimal taxrate { get; set; }//税率
        public List<TrialSale_body_detail> detail { get; set; }
    }
}