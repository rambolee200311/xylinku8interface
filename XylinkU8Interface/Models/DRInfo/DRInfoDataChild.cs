using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.DRInfo
{
    public class DRInfoDataChild
    {
        public string invcode { get; set; }
        /// <summary>
        /// 产品名称（母件）
        /// </summary>
        public string invname { get; set; }
        /// <summary>
        /// 退货入库数量
        /// </summary>
        public decimal outnum { get; set; }   
    }
}