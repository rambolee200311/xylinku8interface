using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSState
{
    public class ClsQuery
    {
        public string companycode { get; set; }//帐套号

        public List<ClsQueryCode> codes { get; set; }//U8出库单号
    }
}