using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 * 20230827
 * 查询全部待归还的借出借⽤单
 */
namespace XylinkU8Interface.Models.BorrowLedger
{
    public class OutMain
    {
        /*
         {
            "companycode": "995", // 账套号
            "total": 99, // 总记录数
            "pages": 10, // 总⻚数
            "datas": [
             {
                "u8RowId": "001", // U8借出借⽤单-产品⼦件⾏ID
                "u8Code": "JCJY202307280021", // U8借出借⽤单-单号
                "u8Date": "2023-07-01", // U8借出借⽤单-单据⽇期
                "u8ExtCode": "SY202307280002", // U8借出借⽤单-外部订单号（CRM试⽤单号，可能为空）
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
             ]
          }
         */
        public string companycode { get; set; }// 账套号
        public int total { get; set; }//总记录数
        public int pages { get; set; }//总⻚数
        public List<OutData> datas { get; set; }
    }
}