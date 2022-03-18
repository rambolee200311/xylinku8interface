using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.STInfo
{
    /// <summary>
    /// CRM订单号查询各种金额结果数据
    /// 20210829
    /// </summary>
    public class STInfoData
    {
        /// <summary>
        /// CRM订单号
        /// </summary>
        public string ccode { get; set; }
        /// <summary>
        /// 子件产品编码
        /// </summary>
        public string invcode { get; set; }
        /// <summary>
        /// 子件产品名称
        /// </summary>
        public string invname { get; set; }
        /// <summary>
        /// 订货数量
        /// </summary>
        public decimal num { get; set; }
        /// <summary>
        /// 发货数量
        /// </summary>        
        public string req_id { get; set; }
        public string u8code { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string excomp { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string exnum { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string receiver { get; set; }
        /// <summary>
        /// 收货人手机号码
        /// </summary>
        public string recrmobi { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string recraddress { get; set; }
        public List<STInfoData_sncodes> sncodes { get; set; }
    }
}