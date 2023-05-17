using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.SaleOrder;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.Controllers.SaleOrder
{
    //3 直销订单 泛微←→U8
    public class ResellController : ApiController
    {
        // GET api/saleorder
        public Saleorder Get()
        {
            //return new string[] { "value1", "value2" };
            Saleorder so = new Saleorder();
            so.head = new Saleorder_head();
            so.body = new List<Saleorder_body>();

            so.head.ccode = "SO9889873834";
            so.head.cust_name = "保定奥维百特科技有限公司";
            so.head.ddate = Convert.ToDateTime("2020-08-09");
            so.head.dept_name = "营销部";
            so.head.person_name = "张磊";
            so.head.cmemo = "钻石伙伴三年续约奖励";

            Saleorder_body body1 = new Saleorder_body();
            body1.cinv_code = "62020-00701-001";
            body1.cinv_name = "小鱼易连ME20套装";
            body1.iquantity = 1;
            body1.iquoteprice = 10000;
            body1.itaxunitprice = 8500;
            body1.isum = 8500;
            body1.cprv_name = "北京";
            body1.ccty_name = "北京市";
            body1.cdis_name = "朝阳区";
            body1.cfst_rcv_name = "茅静";
            body1.cfst_rcv_phone = "18611175355";
            body1.cfst_rcv_address = "科技街73号";
            body1.cord_name = "李岚若";
            body1.cord_phone = "13646687562";
            so.body.Add(body1);

            Saleorder_body body2 = new Saleorder_body();
            body2.cinv_code = "00000-00005-001";
            body2.cinv_name = "小鱼在家通用版礼盒";
            body2.iquantity = 1;
            body2.iquoteprice = 12000;
            body2.itaxunitprice = 10000;
            body2.isum = 10000;
            body2.cprv_name = "北京";
            body2.ccty_name = "北京市";
            body2.cdis_name = "丰台区";
            body2.cfst_rcv_name = "林应军";
            body2.cfst_rcv_phone = "15918727984";
            body2.cfst_rcv_address = "五四路9号宁德大厦10楼1009";
            body2.cord_name = "王烁";
            body2.cord_phone = "15338606661";
            so.body.Add(body2);
            return so;
        }

        // GET api/saleorder/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/saleorder
        public Result Post([FromBody]Saleorder so)
        {
            LogHelper.WriteLog(typeof(SaleOrderController), JsonHelper.ToJson(so));
            Result re = new Result();
            //re.oacode = so.head.ccode;
            //re.u8code = "SY201803777099";
            //re.recode = "0";
            //re.remsg = "";
            re = SaleOrderEntity.Add_SO(so);
            LogHelper.WriteLog(typeof(SaleOrderController), JsonHelper.ToJson(re));
            return re;
        }

        // PUT api/saleorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/saleorder/5
        public void Delete(int id)
        {
        }
    }
}
