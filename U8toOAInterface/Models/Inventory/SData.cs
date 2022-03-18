using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8toOAInterface.Models.Inventory
{
    public class SData
    {
        public OperationInfo operationinfo { get; set; }
        public MainTable mainTable { get; set; }
        public List<SDetail> detail1 { get; set; }
    }
}
