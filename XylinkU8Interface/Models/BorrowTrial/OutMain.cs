using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用返回消息
 */
namespace XylinkU8Interface.Models.BorrowTrial
{
    public class OutMain
    {
        public string companycode { get; set; }
        public List<OutData> dataList { get; set; }
    }
}