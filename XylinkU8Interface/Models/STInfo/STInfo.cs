using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.STInfo
{
    /// <summary>
    /// CRM订单号查询各种金额结果
    /// 20210829
    /// </summary>
    public class STInfo
    {
        public string companycode { get; set; }
        public List<STInfoData> datas { get; set; }
        public List<STInfoReturnData> returndatas { get; set; }
    }
}