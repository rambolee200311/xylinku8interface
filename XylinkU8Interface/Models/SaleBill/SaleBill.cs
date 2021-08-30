using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleBill
{
    public class SaleBill
    {
        public string companycode { get; set; }//帐套号
        public Salebill_head head { get; set; }
        public List<Salebill_body> body { get; set; }
    }
}