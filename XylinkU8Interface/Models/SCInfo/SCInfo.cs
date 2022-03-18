using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SCInfo
{
    public class SCInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string companycode { get; set; }
        
        public List<SCInfoDatas> datas { get; set; }
    }
}