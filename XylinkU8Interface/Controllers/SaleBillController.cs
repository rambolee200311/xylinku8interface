using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.SaleBill;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    //10 开票申请 泛微→→U8
    public class SaleBillController : ApiController
    {
        // GET api/salebill
        public SaleBill Get()
        {
            //return new string[] { "value1", "value2" };
            SaleBill sbill = new SaleBill();
            sbill.head = new Salebill_head();
            sbill.body = new List<Salebill_body>();
            sbill.head.ccode = "SO9889873834";
            sbill.head.cust_name = "保定奥维百特科技有限公司";
            sbill.head.ddate = Convert.ToDateTime("2020-08-09");
            sbill.head.dept_name = "营销部";
            sbill.head.person_name = "张磊";
            sbill.head.cord_code = "SO68768968";
            sbill.head.tax_rate = 0.13m;
            sbill.head.cust_bank = "中国工商银行北京平谷支行";
            sbill.head.cust_address = "天目西路99号";
            sbill.head.cust_account = "77987490720397029";
            sbill.head.cmemo = "钻石伙伴三年续约奖励";


            Salebill_body body1 = new Salebill_body();
            body1.cinv_code = "62020-00701-001";
            body1.cinv_name = "小鱼易连ME20套装";
            body1.iquantity = 1;
            body1.iunitprice = 10000;
            body1.itaxunitprice = 11300;
            body1.imoney = 10000;
            body1.itax = 1300;
            body1.isum = 11300;
            body1.isum = 8500;
            sbill.body.Add(body1);

            return sbill;
        }

        // GET api/salebill/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/salebill
        public Result Post([FromBody]SaleBill sbill)
        {
            Result re = new Result();
            //re.oacode = sbill.head.ccode;
            //re.u8code = "SB978903745";
            //re.recode = "0";
            //re.remsg = "";
            re = SaleBillVouchEntity.Add_SO(sbill);
            return re;
        }

        // PUT api/salebill/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/salebill/5
        public void Delete(int id)
        {
        }
    }
}
