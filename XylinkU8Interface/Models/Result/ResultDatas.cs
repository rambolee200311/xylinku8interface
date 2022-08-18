using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Result
{
    public class ResultDatas
    {
        public string companycode { get; set; }//帐套号
        public List<Result> datas { get; set; }//返回结果
    }
}