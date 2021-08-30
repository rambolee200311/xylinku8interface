using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.LogisticsInfo
{
    /// <summary>
    /// 订单出库序列号查询条件参数
    /// 20210829
    /// </summary>
    public class LogisticQuery
    {
        public string companycode { get; set; }
        public List<LogisticQueryCode> codes { get; set; }
    }
}