using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace U8toOAInterface.Models.dlsddU8WriteSn
{
    public class SN_data//代理商订单U8回写Sn
    {
        public string cpbm { get; set; }//产品编码
        public string cpmc { get; set; }//产品名称
        public string snm { get; set; }//SN码
        public string sfkt { get; set; }//是否开通
        public string kdgs { get; set; }//快递公司
        public string kddh { get; set; }//快递单号
        public string shr { get; set; }//收货人
        public string shrdh { get; set; }//收货人电话
        public string shxxdz { get; set; }//收货详细地址
        public string oaddh { get; set; }//oa订单号
        public string u8ddh { get; set; }//u8订单号

    }
}