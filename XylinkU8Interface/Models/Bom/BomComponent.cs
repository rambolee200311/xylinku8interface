using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Bom
{
    public class BomComponent
    {
        public string serialNum{get;set;}//     序号         
        public string productCode{get;set;}//  物料编码       
        public string productName{get;set;}//  物料名称       
        public string model{get;set;}//       规格         
        public decimal amount {get;set;}//      数量         
        public string remark {get;set;}//    备注信息       
    }
}