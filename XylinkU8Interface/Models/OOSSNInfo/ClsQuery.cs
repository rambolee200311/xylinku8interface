using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSSNInfo
{
    public class ClsQuery
    {
        public string companycode { get; set; }//帐套号
        public string startTime { get; set; }//SN出库时间（查询开始时间）
        public string endTime { get; set; }//SN出库时间（查询结束时间）
        public List<ClsQueryCode> sncodes { get; set; }
    }
}