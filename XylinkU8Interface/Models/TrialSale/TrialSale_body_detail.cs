using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.TrialSale
{
    public class TrialSale_body_detail
    {
        public string cinv_code { get; set; }
        public string cinv_name { get; set; }
        public string unit_name { get; set; }
        public decimal iquantity { get; set; }
        public List<TrialSale_body_detail_sncodes> sncodes { get; set; }
    }
}