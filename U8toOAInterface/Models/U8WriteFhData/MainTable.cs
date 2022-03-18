using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace U8toOAInterface.Models.U8WriteFhData
{
    public class MainTable
    {
        public Nullable<int> id { get; set; }//id
        public string fwddbm { get; set; }//泛微订单编码
        public string u8ddbm { get; set; }//u8订单编码
        public string cpbm { get; set; }//产品编码
        public string cpmc { get; set; }//产品名称
        public decimal wkpsl { get; set; }//未开票数量
        public decimal wkpje { get; set; }//未开票金额
        public string khmc { get; set; }//客户名称
        public string u8fhdbm { get; set; }//u8发货单编码
        public int danwe { get; set; }//单位
        public decimal jshj { get; set; }//价税合计
        public decimal hsdj { get; set; }//含税单价
        public decimal fhsl { get; set; }//发货数量

        public decimal ddsl { get; set; }//订单数量
    }
}
