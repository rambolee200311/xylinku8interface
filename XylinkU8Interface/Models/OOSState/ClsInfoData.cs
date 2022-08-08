using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSState
{
    public class ClsInfoData
    {
        public string code { get; set; }//U8出库单号
        public string ccode { get; set; }//CRM单据（U8外部订单号）
        public string state { get; set; }//出库单状态，开⽴、已审核、关闭等
    }
}