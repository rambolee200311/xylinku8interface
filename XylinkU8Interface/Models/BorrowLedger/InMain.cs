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
    public class InMain
    {
        /*
         请求体:
        {
        "companycode": "995", // 账套号
        "current": 1, // 当前⻚
        "size": 10, // 每⻚数据量
        "u8RowIds": [// U8借出借⽤单-⾏ID（查询参数，⾮必填）
         {"u8RowId": "001"},{"u8RowId": "002"}]
        "u8Codes": [// U8借出借⽤单-单号（查询参数，⾮必填）
         {"u8Code": "JCJY202307280021"},{"u8Code": "JCJY202307280022"}],
         "firstSync": true, // 是否是⾸次同步，⾸次同步: true；后续定期同步: false
         "onlineTime": "2023-10-10 10:10:10", // 上线时间，⾸次同步可不传（⾮必填）
        "ctype": "客户" // 单位类型（客户/部⻔）
        }
         */
        public string companycode { get; set; }// 账套号
        public int current { get; set; }//当前⻚
        public int size { get; set; }//每⻚数据量
        public bool firstSync { get; set; }// 是否是⾸次同步，⾸次同步: true；后续定期同步: false
        public string onlineTime { get; set; }// 上线时间，⾸次同步可不传（⾮必填）
        public string ctype { get; set; }// 单位类型（客户/部⻔）
        public List<InU8RowID> u8RowIds { get; set; }// U8借出借⽤单-⾏ID（查询参数，⾮必填）
        public List<InU8Code> u8Codes { get; set; }// U8借出借⽤单-单号（查询参数，⾮必填）
    }
}