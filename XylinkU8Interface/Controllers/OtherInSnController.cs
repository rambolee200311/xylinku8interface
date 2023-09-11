using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.Models.Borrowoutback;
using XylinkU8Interface.UFIDA;
namespace XylinkU8Interface.Controllers
{
    public class OtherInSnController : ApiController
    {
        // GET api/otherinsn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/otherinsn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/otherinsn
        public Result Post([FromBody]BorrowOutBack bob)
        {
            Result re = STSNEntity.add_otherinSTSN(bob);
            return re;
        }       
        // PUT api/otherinsn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/otherinsn/5
        public void Delete(int id)
        {
        }
    }
}
