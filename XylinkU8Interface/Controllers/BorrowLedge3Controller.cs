using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.BorrowLedger;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    public class BorrowLedge3Controller : ApiController
    {
        // GET api/borrowledge3
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/borrowledge3/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowledge3
        public OutMain Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowLedge3Controller), JsonHelper.ToJson(inMain));
            OutMain infor = BorrowLedgerEntity.getBorrowLedger3Entity(inMain);
            LogHelper.WriteLog(typeof(BorrowLedge3Controller), JsonHelper.ToJson(infor));
            return infor;
        }

        // PUT api/borrowledge3/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowledge3/5
        public void Delete(int id)
        {
        }
    }
}
