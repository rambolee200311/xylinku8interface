using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8toOAInterface.Models.Warehouse
{
    public class Uf_ckda
    {
        /*
            表名：uf_ckda
            字段：
            仓库编码 ckbm 
            仓库名称 ckmc 
            部门名称 bmmc 
            仓库地址 ckdz 
            电话 dh 
            负责人 fzr 
            计价方式 jjfs  
         */
        public string ckbm { get; set; }
        public string ckmc { get; set; }
        //public Nullable<int> bmmc { get; set; }
        public string bmmc { get; set; }
        public string ckdz { get; set; }
        public string dh { get; set; }
        //public Nullable<int> fzr { get; set; }
        public string fzr { get; set; }
        public string jjfs { get; set; }
    }
}
