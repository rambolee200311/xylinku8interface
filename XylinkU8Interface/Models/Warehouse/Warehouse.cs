using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Warehouse
{
    public class Warehouse
    {
        public string companycode { get; set; }
        public List<WarehouseResult> result { get; set; }
    }
}