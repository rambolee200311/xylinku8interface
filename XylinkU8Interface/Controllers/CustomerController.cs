using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Customer;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;
namespace XylinkU8Interface.Controllers
{
    //1 客户信息 泛微→→U8
    public class CustomerController : ApiController
    {
        // GET api/customer
        public Custs Get()
        {
            //return new string[] { "value1", "value2" };
            Custs mycusts = new Custs();
            mycusts.companycode = "001";

            mycusts.cust = new List<Cust>();

            Cust cust1 = new Cust();
            cust1.code = "7667-ade";
            cust1.name = "武汉三大不六孵化器管理有限公司";
            cust1.typecode = "01";
            cust1.dcusdevdate =Convert.ToDateTime( "2020-01-13");
            cust1.ccusdefine7 = Guid.NewGuid().ToString();
            mycusts.cust.Add(cust1);

            Cust cust2 = new Cust();
            cust2.code = "98984-ufi";
            cust2.name = "成都乐育信息技术有限公司";
            cust2.typecode = "01";
            cust2.dcusdevdate = Convert.ToDateTime("2020-01-18");
            cust2.ccusdefine7 = Guid.NewGuid().ToString();
            mycusts.cust.Add(cust2);

            return mycusts;

        }

        // GET api/customer/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/customer
        public Results Post([FromBody]Custs mycusts)
        {
            Results myresults=new Results();
            //myresults.result = new List<Result>();
            //myresults.companycode = mycusts.companycode;
            //string strResult = "";
            //foreach (Cust singlecust in mycusts.cust)
            //{
            //    Result singleresult = new Result();
            //    singleresult.oacode = singlecust.code;
            //    singleresult.u8code = singlecust.code;
            //    singleresult.recode = "0";
            //    singleresult.remsg = strResult;
            //    myresults.result.Add(singleresult);
            //}
            myresults = CustomerEntity.add_cust(mycusts);
            return myresults;
        }

        // PUT api/customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/customer/5
        public void Delete(int id)
        {
        }
    }
}
