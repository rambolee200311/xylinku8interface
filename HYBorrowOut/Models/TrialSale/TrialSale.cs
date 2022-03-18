using System;
using System.Collections.Generic;
using System.Web;

namespace HYBorrowOut.Models.TrialSale
{
    public class TrialSale
    {
        public string companycode { get; set; }//帐套号
        public TrialSale_head head { get; set; }
        public List<TrialSale_body> body { get; set; }
    }
}