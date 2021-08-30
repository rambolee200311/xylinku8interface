using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.LogisticsInfo
{
    /// <summary>
    /// 订单出库序列号结果数据
    /// 20210829
    /// </summary>
    public class LogisticsInfoData
    {
        /// <summary>
        /// CRM订单号
        /// </summary>
        public string ccode { get; set; }
        /// <summary>
        /// U8销售订单编码
        /// </summary>
        public string u8code { get; set; }
        /// <summary>
        /// 产品编码（子件）
        /// </summary>
        public string invcode { get; set; }
        /// <summary>
        /// 产品名称（子件）
        /// </summary>
        public string invname { get; set; }
        /// <summary>
        /// 序列号管理返回SN码，非序列号管理返回空
        /// </summary>
        public string sncode { get; set; }
        /// <summary>
        /// 序列号管理返回1，非序列号管理返回出库数量
        /// </summary>
        public decimal num { get; set; }
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
        /// <summary>
        /// 产品行号（req_id）
        /// </summary>
        public string reqId { get; set; }
    }
}