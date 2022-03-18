using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DRInfo
{
    /// <summary>
    /// 订单出库序列号结果数据
    /// 20210829
    /// </summary>
    public class DRInfoData
    {
        /// <summary>
        /// CRM订单号
        /// </summary>
        //public string ccode { get; set; }
        /// <summary>
        /// U8销售出库单编码
        /// </summary>
        public string u8code { get; set; }
        /// <summary>
        /// 产品编码（母件）
        /// </summary>
        public string invcode { get; set; }
        /// <summary>
        /// 产品名称（母件）
        /// </summary>
        public string invname { get; set; }        
        /// <summary>
        /// 退货入库数量
        /// </summary>
        public decimal outnum { get; set; }
        public decimal iquoteprice { get; set; }
        public decimal itaxunitprice { get; set; }
        public decimal isum { get; set; } 
        /// <summary>
        /// 产品行号（req_id）
        /// </summary>
        public string req_id { get; set; }
        public List<DRInfoDataChild> children { get; set; }
    }
}