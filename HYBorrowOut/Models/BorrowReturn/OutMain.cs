using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 归还返回消息
 */
namespace HYBorrowOut.Models.BorrowReturn
{
    public class OutMain
    {
        public string companycode { get; set; }
        public List<OutData> dataList { get; set; }
    }
}