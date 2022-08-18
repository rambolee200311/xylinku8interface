using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSOrder
{
    public class ClsRequestBody
    {
        public string reqId { get; set; }//明细唯⼀标识
        public string sncode { get; set; }//SN码
        public string invcode { get; set; }//存货编码
        public string invname { get; set; }//存货名称
        public string priceGroup { get; set; }//报价分组
        public string majorUnit { get; set; }//单位
        public decimal iquantity { get; set; }//数量（负数），负数表示⼊库，正数表示出库
        public string warehouse { get; set; }//仓库
        public string ori_reqid { get; set; }//原出库单明细唯一标识
        //public string reqId { get; set; }//明细唯⼀标识
        public string old_sncode { get; set; }//原SN码
        public string new_sncode { get; set; }//新SN码
        public string receiver { get; set; }// 收货⼈
        public string recemobi { get; set; }// 收货⼈⼿机号
        public string receaddress { get; set; }// 收货⼈地址

    }
}