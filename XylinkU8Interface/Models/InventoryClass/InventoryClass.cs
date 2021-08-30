using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.InventoryClass
{
    public class InventoryClass
    {
         public string categoryName{get;set;}
         public string categoryCode { get; set; }
         public List<InventoryClass> children { get; set; }
    }
}