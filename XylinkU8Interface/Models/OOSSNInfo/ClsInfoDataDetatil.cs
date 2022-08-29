using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSSNInfo
{
    public class ClsInfoDataDetatil
    {
        public string rowId { get; set; }//⾏ID，U8数据库主键值
        public string invcode { get; set; }//U8存货编码
        public string invname { get; set; }//U8存货名称
        public string u8OutTime { get; set; }//U8出库时间
        public string businessType { get; set; }//业务类型（销售出库、其他出库等）
        public string u8OutCode { get; set; }//U8出库单号
        public string u8InvCode { get; set; }//U8发货单号
        public string u8Code { get; set; }//U8销售订单号
        public string u8ExtCode { get; set; }//CRM单据（U8外部订单号）
        public string custName { get; set; }//客户名称
        public string reqid { get; set; }
    }
}