using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SNInfo
{
    /// <summary>
    /// CRM订单号查询各种金额结果
    /// 20210829
    /// </summary>
    public class SNInfo
    {
        public string companycode { get; set; }
        public List<SNInfoData> datas { get; set; }
    }
}