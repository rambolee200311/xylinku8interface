using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8toOAInterface.Models.U8WriteFhData
{
    public class SData
    {
        public Operationinfo operationinfo { get; set; }
        public MainTable mainTable { get; set; }
        public List<SDetail> detail1 { get; set; }
    }
}
