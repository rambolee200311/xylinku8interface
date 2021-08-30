using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Person
{
    public class Person
    {
        public string code { get; set; }//oa人员编码
        public string name { get; set; }//oa人员名称
        public string rsex { get; set; }//oa人员性别
        public string depname { get; set; }//U8部门名称
        
    }
}