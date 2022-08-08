using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSLogisticsInfo
{
    public class ClsInfo
    {
        public string companycode { get; set; }//帐套号       
        public List<ClsInfoData> datas { get; set; }
    }
}