using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.BorrowTrial;
using XylinkU8Interface.Models.Result;
/*
    *20230910 
    *lijianqiang
    * 试用、⽣成其他出库单并审核通过
 */
namespace XylinkU8Interface.Controllers
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
        public Result Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowTrialController), JsonHelper.ToJson(inMain));
            Result result = BorrowTrialEntity.addBorrowTrial(inMain);
            LogHelper.WriteLog(typeof(BorrowTrialController), JsonHelper.ToJson(result));
            return result;
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
