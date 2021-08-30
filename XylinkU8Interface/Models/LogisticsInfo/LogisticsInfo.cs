using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.LogisticsInfo
{
    /// <summary>
    /// 订单出库序列号查询结果
    /// 20210829
    /// </summary>
    public class LogisticsInfo
    {
        public string companycode { get; set; }
        public List<LogisticsInfoData> datas { get; set; }
    }
}