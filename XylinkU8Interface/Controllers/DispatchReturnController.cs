using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Dispatchreturn;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.Controllers
{
    //7 产品退货 泛微←→U8
    public class DispatchReturnController : ApiController
    {
        // GET api/dispatchreturn
        public DispatchReturn Get()
        {
            //return new string[] { "value1", "value2" };
            DispatchReturn dpr = new DispatchReturn();
            dpr.head = new Dispatchreturn_head();
            dpr.body = new List<Dispatchreturn_body>();

            dpr.head.ccode = "AB9908034853";
            dpr.head.ddate =Convert.ToDateTime( "2020-08-11");
            dpr.head.cust_name = "武汉科情新技术开发有限责任公司";
            dpr.head.dept_name = "营销部";
            dpr.head.person_name = "果天";
            dpr.head.cmemo = "";
            dpr.head.cexchan = "换货";

            Dispatchreturn_body body1 = new Dispatchreturn_body();
            body1.cinv_code = "62020-00701-001";
            body1.cinv_name = "小鱼易连ME20套装";
            body1.iquantity = 1;
            body1.unit_name = "台";
            body1.iquantity = 1;
            body1.iquoteprice = 12000;
            body1.itaxunitprice = 10000;
            body1.isum = 10000;
            //body1.cord_code = "SO09779887";
            //body1.cdsp_code = "DS05027520";
            body1.creason = "产品参数问题";
            dpr.body.Add(body1);

            Dispatchreturn_body body2 = new Dispatchreturn_body();
            body2.cinv_code = "00000-00005-001";
            body2.cinv_name = "小鱼在家通用版礼盒";
            body2.iquantity = 1;
            body2.unit_name = "台";
            body2.iquantity = 1;
            body2.iquoteprice = 10000;
            body2.itaxunitprice = 9000;
            body2.isum = 9000;
            //body2.cord_code = "SO5225245246";
            //body2.cdsp_code = "DS25245245252";
            body2.creason = "软件配置问题";

            dpr.body.Add(body2);

            return dpr;
        }

        // GET api/dispatchreturn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dispatchreturn
        public Result Post([FromBody]DispatchReturn dpr)
        {
            LogHelper.WriteLog(typeof(DispatchReturnController), JsonHelper.ToJson(dpr));
            Result re = new Result();
            //re.oacode = dpr.head.ccode;
            //re.u8code = "DS98798749274";
            //re.recode = "0";
            //re.remsg = "";
            //退货
            re = DispatchReturnEntity.Add_SO(dpr, "red");
            if ((dpr.head.cexchan=="换货")&&(re.recode=="0"))
            {
                //换货
                re = DispatchReturnEntity.Add_SO(dpr, "blue"); 
            }
            LogHelper.WriteLog(typeof(DispatchReturnController), JsonHelper.ToJson(re));
            return re;
        }

        // PUT api/dispatchreturn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dispatchreturn/5
        public void Delete(int id)
        {
        }
    }
}
