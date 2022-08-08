using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSOutInfo
{
    public class ClsInfoData
    {
        public string code { get; set; }//U8出库单号
        public List<ClsInfoDataDetatil> detail { get; set; }
    }
}