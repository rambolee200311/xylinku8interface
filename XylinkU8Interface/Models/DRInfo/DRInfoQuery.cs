using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DRInfo
{
    /// <summary>
    /// 订单出库序列号查询条件参数
    /// 20210829
    /// </summary>
    public class DRInfoQuery
    {
        public string companycode { get; set; }
        public List<DRInfoQueryCode> reqids { get; set; }
    }
}