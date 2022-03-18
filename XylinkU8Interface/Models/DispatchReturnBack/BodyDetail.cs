using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DispatchReturnBack
{
    public class BodyDetail
    {
        public string cinv_code { get; set; }//产品编码-存货编码
        public string cinv_name { get; set; }//产品名称-存货名称
        public string unit_name { get; set; }//单位-单位
        public decimal iquantity { get; set; }//归还数量-本次归还数量
        public List<SNinfo> sncodes { get; set; }
    }
}