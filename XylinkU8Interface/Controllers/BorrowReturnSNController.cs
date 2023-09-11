using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.Models.BorrowReturn;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
/*
    *20230903 
    *lijianqiang
    * 归还（归还、⽣成其他⼊库单并审核通过）
 */
namespace XylinkU8Interface.Controllers
{
    public class BorrowReturnSNController : ApiController
    {
        // GET api/borrowreturnsn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/borrowreturnsn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowreturnsn
        public Result Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowReturnSNController), JsonHelper.ToJson(inMain));
            Result re = STSNEntity.add_BorrowReturnSN(inMain);
            LogHelper.WriteLog(typeof(BorrowReturnSNController), JsonHelper.ToJson(re));
            return re;
        }

        // PUT api/borrowreturnsn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowreturnsn/5
        public void Delete(int id)
        {
        }
    }
}
