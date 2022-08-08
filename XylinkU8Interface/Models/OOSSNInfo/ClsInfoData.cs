using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSSNInfo
{
    public class ClsInfoData
    {
        public string sncode { get; set; }//SN码
        public List<ClsInfoDataDetatil> detail { get; set; }
    }
}