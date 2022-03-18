using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.SNInfo
{
    /// <summary>
    /// CRM订单号查询各种金额结果数据
    /// 20210829
    /// </summary>
    public class SNInfoData
    {
        /// <summary>
        /// CRM订单号
        /// </summary>
        public string ccode { get; set; }
        public string u8code { get; set; }
        /// <summary>
        /// 母件产品编码
        /// </summary>
        public string invcode { get; set; }
        /// <summary>
        /// 母件产品名称
        /// </summary>
        public string invname { get; set; }
        /// <summary>
        /// 订货数量
        /// </summary>
        public decimal ordnum { get; set; }
        /// <summary>
        /// 发货数量
        /// </summary>
        //public decimal outnum { get; set; }
        ///// <summary>
        ///// 开票数量
        ///// </summary>
        //public decimal billnum { get; set; }
        ///// <summary>
        ///// 订货金额
        ///// </summary>
        public decimal ordamt { get; set; }
        ///// <summary>
        ///// 发货金额
        ///// </summary>
        //public decimal outamt { get; set; }
        ///// <summary>
        ///// 开票金额
        ///// </summary>
        //public decimal billamt { get; set; }
        /// <summary>
        /// 母件唯一标识
        /// </summary>
        public string req_id { get; set; }
        public List<SNInfoDLDetail> dldetail { get; set; }
        public List<SNInfoBLDetail> bldetail { get; set; }
    }
}