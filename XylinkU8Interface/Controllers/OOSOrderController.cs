using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSOrder;

namespace XylinkU8Interface.Controllers
{
    public class OOSOrderController : ApiController
    {
        // GET api/oosorder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oosorder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oosorder
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsResponse Post([FromBody]ClsRequest req)
        {
            LogHelper.WriteLog(typeof(OOSOrderController), JsonHelper.ToJson(req));
            ClsResponse rep = new ClsResponse();
            LogHelper.WriteLog(typeof(OOSOrderController), JsonHelper.ToJson(rep));
            return rep;
        }
        // PUT api/oosorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oosorder/5
        public void Delete(int id)
        {
        }
    }
}
