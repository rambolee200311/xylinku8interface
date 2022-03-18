using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.TrialSale;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.Helper;
namespace XylinkU8Interface.Controllers
{
    public class TrialSaleOrderController : ApiController
    {
        // GET api/trialsaleorder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/trialsaleorder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/trialsaleorder
        public Result Post([FromBody]TrialSale so)
        {
            LogHelper.WriteLog(typeof(TrialSaleOrderController), JsonHelper.ToJson(so));
            Result re= TrialSaleOrderEntity.add_SO(so);
            LogHelper.WriteLog(typeof(TrialSaleOrderController), JsonHelper.ToJson(re));
            return re;

        }

        // PUT api/trialsaleorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/trialsaleorder/5
        public void Delete(int id)
        {
        }
    }
}
