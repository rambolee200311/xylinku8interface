using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.STInfo
{
    /// <summary>
    /// CRM订单号查询各种金额查询条件参数
    /// 20210829
    /// </summary>
    public class STInfoQuery
    {
        public string companycode { get; set; }
        public List<STInfoQueryCode> codes { get; set; }
    }
}