using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DispatchReturnBack
{
    public class DispatchReturnBack
    {
        public string companycode { get; set; }//帐套号
        public DispatchReturnBack_head head { get; set; }
        public List<DispatchReturnBack_body> body { get; set; }
    }
}