using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Customer
{
    public class Custs
    {
        public string companycode { get; set; }//帐套号
        public List<Cust> cust { get; set; }//客户档案
    }
}