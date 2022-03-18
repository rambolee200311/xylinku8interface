using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.SNInfo;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    public class SNInfoController : ApiController
    {
        // GET api/sninfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/sninfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sninfo
        public SNInfo Post([FromBody]SNInfoQuery value)
        {
            SNInfo result = SNInfoEntity.GetResult(value);
            return result;
        }

        // PUT api/sninfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sninfo/5
        public void Delete(int id)
        {
        }
    }
}
