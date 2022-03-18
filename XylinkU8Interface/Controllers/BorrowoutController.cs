using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Borrowout;
using XylinkU8Interface.Models.Result;

namespace XylinkU8Interface.Controllers
{
    //4 产品试用  泛微←→U8
    public class BorrowoutController : ApiController    {
        
        // GET api/borrowout
        public Borrowout Get()
        {
            //return new string[] { "value1", "value2" };
            Borrowout bout = new Borrowout();
            bout.companycode = "001";
            bout.body = new List<Borrowout_body>();
            bout.head=new Borrowout_head();
            bout.head.ccode="SY20180322001";
            bout.head.ddate=Convert.ToDateTime("2020-08-02");
            bout.head.ctype="客户";
            bout.head.cust_name="杭州楷知科技有限公司";
            bout.head.dept_name="营销部";
            bout.head.person_name="张磊";
            bout.head.cmemo="abcdefghgijkdf";
            bout.head.recv_name="许文辉";
            bout.head.recv_phone="18600769077";
            bout.head.recv_address="北京市市辖区海淀区北京市海淀区中关村东路8号东升大厦C座206";

            Borrowout_body body1=new Borrowout_body();
            body1.cinv_code="62020-00701-001";
            body1.cinv_name="小鱼易连ME20套装";
            body1.iquantity=1;
            body1.unit_name="台";
            body1.back_date=Convert.ToDateTime("2020-08-09");
            bout.body.Add(body1);

            Borrowout_body body2=new Borrowout_body();
            body2.cinv_code="70201-00126-001";
            body2.cinv_name="大屏投放器";
            body2.iquantity=2;
            body2.unit_name="件";
            body2.back_date=Convert.ToDateTime("2020-08-15");
            bout.body.Add(body2);

            return bout;
        }

        // GET api/borrowout/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowout
        public Result Post([FromBody]Borrowout bout)
        {
            Result re = new Result();
            re.oacode = bout.head.ccode;
            re.u8code = "SY201803abc099";
            re.recode = "0";
            re.remsg = "";
            return re;
        }

        // PUT api/borrowout/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowout/5
        public void Delete(int id)
        {
        }
    }
}
