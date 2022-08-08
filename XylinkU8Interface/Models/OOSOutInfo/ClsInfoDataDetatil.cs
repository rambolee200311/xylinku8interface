using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSOutInfo
{
    public class ClsInfoDataDetatil
    {
        public string rowId { get; set; }//⾏ID，U8数据库主键值
        public string invcode { get; set; }//U8存货编码
        public string invname { get; set; }//U8存货名称
        public string u8OutTime { get; set; }//U8出库时间       
        public string u8OutCode { get; set; }//U8出库单号
        public string u8InvCode { get; set; }//U8发货单号
        public string u8Code { get; set; }//U8销售订单号
        public string u8ExtCode { get; set; }//CRM单据（U8外部订单号）
        public string custName { get; set; }//客户名称
        public decimal num { get; set; }//数量

        public List<ClsInfoDataDetailSncode> sncodes { get; set; }//SN号
    }
}