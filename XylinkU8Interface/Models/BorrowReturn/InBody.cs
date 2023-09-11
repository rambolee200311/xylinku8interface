using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 归还body
 */
namespace XylinkU8Interface.Models.BorrowReturn
{
    /*
          "ccode": "ZJSQGH202307260001001", // 订单号
            "oriU8RowId": "001", // 原U8借出借⽤单-产品⼦件⾏ID
            "oriU8Code": "JCJY202307280021", // 原U8借出借⽤单-单号
            "oriReqId": "79473905879037590", // 原产品明细唯⼀标识（可能为空，⽆CRM
            试⽤申请单的情况）
            "cinvCode": "62045-00224-001", // 产品编码-存货编码
            "cinvName": "ME45视频会议终端", // 产品名称-存货名称
            "iquantity": 2, // 数量-数量
            "iquoteprice": "893.1", // 标准报价-报价
            "itaxunitprice": "276", // 折后单价-含税单价
            "isum": "2760", // 折后⾦额-价税合计
            "cmemo": "备注", // 备注
            "cwhname": "⼩⻥仓", // 虚拟仓库名称
            "unitName": "台", // 单位-单位
            "taxrate": "13", // 税率
         */
    public class InBody
    {
        
        public String ccode { get; set; } // 订单号
        public String oriU8RowId { get; set; }// 原U8借出借⽤单-产品⼦件⾏ID
        public String oriU8Code { get; set; } // 原U8借出借⽤单-单号
        public String oriReqId { get; set; } // 原产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）
        public String cinvCode { get; set; } // 产品编码-存货编码
        public String cinvName { get; set; }// 产品名称-存货名称
        public Decimal iquantity { get; set; } //数量-数量
        public Decimal iquoteprice { get; set; } // 标准报价-报价
        public Decimal itaxunitpric { get; set; } // 折后单价-含税单价
        public Decimal isum { get; set; }// 折后⾦额-价税合计
        public String cmemo { get; set; }// 备注
        public String cwhname { get; set; } // 虚拟仓库名称
        public String unitName { get; set; } // 单位-单位
        public Decimal taxrate { get; set; }// 税率
        public List<InDetail> detail { get; set; } // ⼦件明细
    }
}