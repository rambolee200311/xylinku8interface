using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.DRInfo;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    public class DRInfoController : ApiController
    {
        // GET api/drinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/drinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/drinfo
        public DRInfo Post([FromBody]DRInfoQuery dq)
        {
            DRInfo drInfo = DRInfoEntity.GetResult(dq);
            return drInfo;
        }

        // PUT api/drinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/drinfo/5
        public void Delete(int id)
        {
        }
    }
}
