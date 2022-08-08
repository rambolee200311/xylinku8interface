using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.OOSOrder
{
    public class ClsRequestHead
    {
        public string ccode { get; set; }//CRM订单号（U8存U8外部订单号）
        public string ddate { get; set; }//单据⽇期
        public string custName { get; set; }//客户名称
        public string personName { get; set; }//操作员，固定为 system
        public string cmemo { get; set; }//备注
        public string category { get; set; }//固定为售后换货出库-CRM⼊库，CRM出库,试⽤业务SN的调换

    }
}