using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.STInfo;
using XylinkU8Interface.UFIDA;
namespace XylinkU8Interface.Controllers
{
    public class STInfoController : ApiController
    {
        // GET api/stinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/stinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/stinfo
        public STInfo Post([FromBody]STInfoQuery sq)
        {
            return STInfoEntity.get_STInfo(sq);
        }

        // PUT api/stinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/stinfo/5
        public void Delete(int id)
        {
        }
    }
}
