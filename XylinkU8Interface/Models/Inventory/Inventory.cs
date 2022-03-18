using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Inventory
{
    public class Inventory
    {
        public string productCategoryCode { get; set; }//  存货分类编码  
        public string productCode { get; set; }//    物料编码    
        public string productName { get; set; }//    物料名称    
        public string embedSoftware { get; set; }// 嵌入式软件名称 
        public string cloudServiceDeadlineUnit { get; set; }//  产品周期单位  
        public decimal standardPrice { get; set; }//    参考售价    
        public string priceGroup { get; set; }//    报价分组    
        public decimal taxRate { get; set; }//      税率      
        public string model { get; set; }//    规格型号    
        public string rememberCode { get; set; }//     助记码     
        public string sampleMachine { get; set; }//    是否样机    
        public string unitGroup { get; set; }//   计量单位组   
        public string majorUnit { get; set; }//   主计量单位   
        public string cloudService { get; set; }//   是否云服务   
        public string suit { get; set; }//    是否套装    
        public string hasU8 { get; set; }//有无子件

        public string remark { get; set; }//产品描述
        public int bvirtual { get; set; }//是否虚拟产品 1:是，0:否 

        public int hasSequence { get; set; }//是否序列号管理  1:是，0:否 
    }
}