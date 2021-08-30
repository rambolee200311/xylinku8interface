using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Result
{
    public class Results
    {
        public string companycode { get; set; }//帐套号
        public List<Result> result { get; set; }//返回结果
    }
}