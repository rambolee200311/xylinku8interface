using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.DispatchReturnBack;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.Controllers
{
    public class DispatchReturnBackController : ApiController
    {
        // GET api/dispatchreturnback
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dispatchreturnback/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dispatchreturnback
        public Result Post([FromBody]DispatchReturnBack dprb)
        {
            LogHelper.WriteLog(typeof(DispatchReturnBackController), JsonHelper.ToJson(dprb));
            Result re = new Result();
            re = SaleOutEntity.Add_so(dprb,"red");
            LogHelper.WriteLog(typeof(DispatchReturnBackController), JsonHelper.ToJson(re));
            return re;
        }

        // PUT api/dispatchreturnback/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dispatchreturnback/5
        public void Delete(int id)
        {
        }
    }
}
