using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.SCInfo;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    public class SCInfoController : ApiController
    {
        // GET api/scinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/scinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/scinfo
        public SCInfo Post([FromBody]SCInfoQuery scInfoQuery)
        {
            SCInfo scInfo = SCInfoEntity.getSCInfo(scInfoQuery);
            return scInfo;
        }

        // PUT api/scinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/scinfo/5
        public void Delete(int id)
        {
        }
    }
}
