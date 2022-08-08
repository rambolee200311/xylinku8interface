using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.AuditOOSOrder;
using XylinkU8Interface.Models.Result;

namespace XylinkU8Interface.Controllers
{
    public class AuditOOSOrderController : ApiController
    {
        // GET api/auditoosorder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/auditoosorder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/auditoosorder
        //public void Post([FromBody]string value)
        //{
        //}
        public Result Post([FromBody]ClsRequest req)
        {
            LogHelper.WriteLog(typeof(AuditOOSOrderController),JsonHelper.ToJson(req));
            Result res = new Result();
            LogHelper.WriteLog(typeof(AuditOOSOrderController), JsonHelper.ToJson(res));
            return res;

        }
        // PUT api/auditoosorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/auditoosorder/5
        public void Delete(int id)
        {
        }
    }
}
