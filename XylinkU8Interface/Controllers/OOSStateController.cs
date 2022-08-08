using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSState;

namespace XylinkU8Interface.Controllers
{
    public class OOSStateController : ApiController
    {
        // GET api/oosstate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oosstate/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oosstate
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSStateController), JsonHelper.ToJson(query));
            ClsInfo infor = new ClsInfo();
            LogHelper.WriteLog(typeof(OOSStateController), JsonHelper.ToJson(infor));
            return infor;
        }
        // PUT api/oosstate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oosstate/5
        public void Delete(int id)
        {
        }
    }
}
