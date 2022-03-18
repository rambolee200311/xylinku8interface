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
    public class SNInfoDLDetail
    {
        /// <summary>
        /// CRM订单号
        /// </summary>
        
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
        public decimal outnum { get; set; }       
        ///// <summary>
        ///// 订货金额
        ///// </summary>
        public decimal outamt { get; set; }
        
        
    }
}