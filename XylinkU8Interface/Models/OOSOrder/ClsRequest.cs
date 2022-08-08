using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSOrder
{
    public class ClsRequest
    {
        public string companycode { get; set; }//帐套号
        public ClsRequestHead head { get; set; }
        public List<ClsRequestBody> body { get; set; }
    }
}