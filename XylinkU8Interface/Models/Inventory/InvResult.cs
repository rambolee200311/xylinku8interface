using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Inventory
{
    public class InvResult
    {
        public int totalNumber { get; set; }
        public List<Inventory> products { get; set; }
    }
}