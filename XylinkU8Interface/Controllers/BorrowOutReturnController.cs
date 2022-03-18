using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Borrowoutreturn;
using XylinkU8Interface.Models.Result;

namespace XylinkU8Interface.Controllers
{
    //5 产品归还 泛微→→U8
    public class BorrowOutReturnController : ApiController
    {
        // GET api/borrowoutreturn
        public Borrowoutreturn Get()
        {
            //return new string[] { "value1", "value2" };
            Borrowoutreturn bout = new Borrowoutreturn();
            bout.body = new List<Borrowoutreturn_body>();
            bout.head = new Borrowoutreturn_head();
            bout.head.ccode = "SY20200426020";
            bout.head.oriccode = "SY20180322001";
            bout.head.ddate = Convert.ToDateTime("2020-08-02");
            bout.head.ctype = "客户";
            bout.head.cust_name = "杭州楷知科技有限公司";
            bout.head.dept_name = "营销部";
            bout.head.person_name = "张磊";
            bout.head.cmemo = "abcdefghgijkdf";
            bout.head.recv_name = "许文辉";
            bout.head.recv_type = "借出还回入库";
            

            Borrowoutreturn_body body1 = new Borrowoutreturn_body();
            body1.cinv_code = "62020-00701-001";
            body1.cinv_name = "小鱼易连ME20套装";
            body1.iquantity = 1;
            body1.unit_name = "台";
            body1.cwhname = "富士康仓";
            bout.body.Add(body1);

            Borrowoutreturn_body body2 = new Borrowoutreturn_body();
            body2.cinv_code = "70201-00126-001";
            body2.cinv_name = "大屏投放器";
            body2.iquantity = 2;
            body2.unit_name = "件";
            body2.cwhname = "小鱼北京仓";
            bout.body.Add(body2);

            return bout;
        }

        // GET api/borrowoutreturn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowoutreturn
        public Result Post([FromBody]Borrowoutreturn bout)
        {
            Result re = new Result();
            re.oacode = bout.head.ccode;
            re.u8code = "SY201803abc099";
            re.recode = "0";
            re.remsg = "";
            return re;
        }

        // PUT api/borrowoutreturn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowoutreturn/5
        public void Delete(int id)
        {
        }
    }
}
