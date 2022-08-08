using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSSaleSNInfoTime
{
    public class ClsInfo
    {
        public string companycode { get; set; }//帐套号
        public int total { get; set; }//总记录数
        public int pages { get; set; }//总页数
        public List<ClsInfoData> datas { get; set; }
    }
}