using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Customer
{
    public class Cust
    {
        public string code { get; set; }//oa客户编码
        public string name { get; set; }//oa客户名称

        public string typecode { get; set; }//U8客户分类编码 （01 销售客户，02 其他客户）
        public DateTime dcusdevdate { get; set; }//客户发展日期
        public string ccusdefine7 { get; set; }//客户GUID
    }
}