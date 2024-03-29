﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 * 20230827
 * 查询全部待归还的借出借⽤单
 */
namespace XylinkU8Interface.Models.BorrowLedger
{
    public class OutData
    {
        /*
    
        {
            "u8RowId": "001", // U8借出借⽤单-产品⼦件⾏ID
            "u8Code": "JCJY202307280021", // U8借出借⽤单-单号
            "u8Date": "2023-07-01", // U8借出借⽤单-单据⽇期
            "u8ExtCode": "SY202307280002", // U8借出借⽤单-外部订单号（CRM试⽤单
            号，可能为空）
            "invcode": "62045-00224-001", // U8借出借⽤单-⾏⼦件产品编码
            "invname": "ME45视频会议终端", // U8借出借⽤单-⾏⼦件产品名称
            "borrowSncodes": [// U8借出借⽤单-⾏⼦件产品出库SN（是纳⼊序列号管理的才有值）
             {"sncode": "SN1"},{"sncode": "SN2"},{"sncode": "SN3"}],
            "returnSncodes": [// U8借出借⽤单-⾏⼦件产品归还SN（是纳⼊序列号管理的才有值）
              {"sncode": "SN1"},{"sncode": "SN2"},{"sncode": "SN3"}],
            "applyBorrowNum": 10, // U8借出借⽤单-⾏⼦件产品申请借⽤数量
            "borrowNum": 3, // U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
            "returnNum": 2, // U8借出借⽤单-⾏⼦件产品归还数量（归还数量）
            "reqId": "42517026993864704" // 产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）
        }
             
         */
        public string u8RowId { get; set; }// U8借出借⽤单-产品⼦件⾏ID
        public string u8Code { get; set; }// U8借出借⽤单-单号
        public string u8Date { get; set; } // U8借出借⽤单-单据⽇期
        public string u8ExtCode { get; set; } // U8借出借⽤单-外部订单号（CRM试⽤单号，可能为空）
        public string invcode { get; set; } // U8借出借⽤单-⾏⼦件产品编码
        public string invname { get; set; } // U8借出借⽤单-⾏⼦件产品名称
        public List<OutSnCode> borrowSncodes { get; set; }// U8借出借⽤单-⾏⼦件产品出库SN（是纳⼊序列号管理的才有值）
        public List<OutSnCode> returnSncodes { get; set; }// U8借出借⽤单-⾏⼦件产品归还SN（是纳⼊序列号管理的才有值）
        public decimal applyBorrowNum { get; set; } // U8借出借⽤单-⾏⼦件产品申请借⽤数量
        public decimal borrowNum { get; set; } // U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
        public decimal returnNum { get; set; } // U8借出借⽤单-⾏⼦件产品归还数量（归还数量）
        public string reqId { get; set; } // 产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）
        //20231115
        public bool u8Presale { get; set; } // U8预售出库（如果该借出借⽤单对应的其他出库单的出库类别存在“预售机借⽤”，则为true；否则为false
        public string personCode { get; set; } // U8借出借⽤单的业务员编码
        public string personName{ get; set; } // { get; set; } // U8借出借⽤单的业务员名称
        public decimal iQtyCOver { get; set; } // 累计转耗用数量 
        public decimal iQtyBack { get; set; } // 累计归还数量 
        public decimal iQtyCFree { get; set; } // 累计转赠品数量 
        public decimal iQtyCOut { get; set; } // 累计转借出数量 
        public decimal iQtyCSale { get; set; } // 累计转销售数量 


    }
}