using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.RevokeOOSOrder
{
    public class ClsRequest
    {
        public string companycode { get; set; }//帐套号
        public List<ClsRequestCode> codes { get; set; }

    }
}