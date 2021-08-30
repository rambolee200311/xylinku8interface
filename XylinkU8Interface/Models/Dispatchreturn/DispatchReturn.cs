using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Dispatchreturn
{
    public class DispatchReturn
    {
        public string companycode { get; set; }//帐套号
        public Dispatchreturn_head head { get; set; }
        public List<Dispatchreturn_body> body { get; set; }
    }
}