using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.RevokeOOSOrder;
using XylinkU8Interface.Models.Result;

namespace XylinkU8Interface.Controllers
{
    public class RevokeOOSOrderController : ApiController
    {
        // GET api/revokeoosorder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/revokeoosorder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/revokeoosorder
        //public void Post([FromBody]string value)
        //{
        //}
        public ResultDatas Post([FromBody]ClsRequest req)
        {
            LogHelper.WriteLog(typeof(RevokeOOSOrderController), JsonHelper.ToJson(req));
            ResultDatas res = RevokeOOSOrderEntity.revokeOOSOrder(req);
            LogHelper.WriteLog(typeof(RevokeOOSOrderController), JsonHelper.ToJson(res));
            return res;

        }
        // PUT api/revokeoosorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/revokeoosorder/5
        public void Delete(int id)
        {
        }
    }
}
