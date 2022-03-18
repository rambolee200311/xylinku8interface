using System;
using System.Collections.Generic;
using System.Web;

namespace HYBorrowDll.Models.Borrowoutback
{
    public class BorrowOutBack_body
    {
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称
        public string unit_name { get; set; }//单位-单位
        public decimal iquantity { get; set; }//归还数量-本次归还数量
        public string cmemo { get; set; }//备注-备注
        public string cwhname { get; set; }//仓库名称-仓库名称
    }
}