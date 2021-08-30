using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SaleOrder
{
    public class Saleorder
    {
        public string companycode { get; set; }//帐套号
        public Saleorder_head head { get; set; }
        public List<Saleorder_body> body { get; set; }
    }
}