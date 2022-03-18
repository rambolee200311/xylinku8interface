using System;
using System.Collections.Generic;
using System.Web;

namespace XylinkU8Interface.Models.Borrowoutback
{
    public class BorrowOutBack_body
    {
        public string req_id { get; set; }//明细唯一标识
        public string ori_req_id { get; set; }//原借用单明细唯一标识

        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称
        public string unit_name { get; set; }//单位-单位
        public decimal iquantity { get; set; }//归还数量-本次归还数量
        public string cmemo { get; set; }//备注-备注
        public string cwhname { get; set; }//仓库名称-仓库名称
        public List<BorrowOutBack_body_detail> detail { get; set; }
    }
}