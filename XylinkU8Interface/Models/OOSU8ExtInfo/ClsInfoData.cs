using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSU8ExtInfo
{
    public class ClsInfoData
    {
        public string u8Code { get; set; }//U8出库单号
        public string u8ExtCode { get; set; }//CRM单据（U8外部订单号）
        public List<ClsInfoDataDetatil> detail { get; set; }
    }
}