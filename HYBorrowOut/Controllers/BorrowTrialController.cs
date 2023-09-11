using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HYBorrowOut.Models.BorrowTrial;
using HYBorrowOut.Helper;
using HYBorrowOut.UFIDA;
namespace HYBorrowOut.Controllers
{
    public class BorrowTrialController : ApiController
    {
        // GET api/borrowtrial
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/borrowtrial/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowtrial
        public OutMain Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowTrialController), JsonHelper.ToJson(inMain));
            OutMain outMain = BorrowTrialEntity.Set_Borrow_Out(inMain);
            LogHelper.WriteLog(typeof(BorrowTrialController), JsonHelper.ToJson(outMain));
            return outMain;
        }

        // PUT api/borrowtrial/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowtrial/5
        public void Delete(int id)
        {
        }
    }
}
