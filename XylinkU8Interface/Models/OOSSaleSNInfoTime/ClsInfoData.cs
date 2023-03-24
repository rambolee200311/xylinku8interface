using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSSaleSNInfoTime
{
    public class ClsInfoData
    {
        public string sncode { get; set; }//SN码
        public string invcode { get; set; }//U8存货编码
        public string invname { get; set; }//U8存货名称
        public string u8OutTime { get; set; }//U8出库时间
        public string u8OutCode { get; set; }//U8出库单号
        public string u8InvCode { get; set; }//U8发货单
        public string u8Code { get; set; }//U8销售订单号
        public string u8ExtCode { get; set; }//CRM单据（U8外部订单号）
        public decimal num { get; set; }//出库数量
        public string excomp { get; set; }//寄出快递公司
        public string exnum { get; set; }//寄出快递单号
        public string receiver { get; set; }//收货⼈
        public string recemobi { get; set; }//收货⼈⼿机号
        public string receaddress { get; set; }//收货⼈地址
        public string reqId { get; set; }//产品⾏唯⼀标识
    }
}