using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HYBorrowOut.Models.Borrowoutback
{
    public class BorrowOutBack_body_detail
    {
        public string cinv_code { get; set; }
        public string cinv_name { get; set; }
        public string unit_name { get; set; }
        public decimal iquantity { get; set; }
        public List<BorrowOutBack_body_detail_sncodes> sncodes { get; set; }
    }
}