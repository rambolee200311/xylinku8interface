using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DRInfo
{
    /// <summary>
    /// 订单出库序列号查询结果
    /// 20210829
    /// </summary>
    public class DRInfo
    {
        public string companycode { get; set; }
        public List<DRInfoData> datas { get; set; }
    }
}