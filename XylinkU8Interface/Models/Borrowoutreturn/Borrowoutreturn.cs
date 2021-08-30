using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Borrowoutreturn
{
    public class Borrowoutreturn
    {
        public string companycode { get; set; }//帐套号
        public Borrowoutreturn_head head { get; set; }
        public List<Borrowoutreturn_body> body { get; set; }
    }
}