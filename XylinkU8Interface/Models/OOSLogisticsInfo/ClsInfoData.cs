using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSLogisticsInfo
{
    public class ClsInfoData
    {
        public string code { get; set; }//U8出库单号
        public string ccode { get; set; }//CRM单据（U8外部订单号）
        public string invcode { get; set; }//出库存货编码
        public string invname { get; set; }//出库存货名称
        public decimal num { get; set; }//出库数量
        public List<ClsInfoDataSncode> sncodes { get; set; }//SN码
        public string excomp { get; set; }//寄出快递公司
        public string exnum { get; set; }//寄出快递单号
        public string receiver { get; set; }//收货⼈
        public string recemobi { get; set; }//收货⼈⼿机号
        public string receaddress { get; set; }//收货⼈地址
        public string outTime { get; set; }//出库时间
        public string reqId { get; set; }//产品⾏唯⼀标识

    }
}