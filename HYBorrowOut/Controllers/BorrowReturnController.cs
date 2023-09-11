using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HYBorrowOut.Models.BorrowReturn;
using HYBorrowOut.Models.Result;
using HYBorrowOut.UFIDA;
using HYBorrowOut.Helper;
/*
 *20230903
 * 归还controller
 */
namespace HYBorrowOut.Controllers
{
    public class BorrowReturnController : ApiController
    {
        // GET api/borrowreturn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/borrowreturn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowreturn
        public OutMain Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowReturnController), JsonHelper.ToJson(inMain));
            OutMain outMain = BorrowReturnEntity.Add_Borrow_Out_Back(inMain);
            LogHelper.WriteLog(typeof(BorrowReturnController), JsonHelper.ToJson(outMain));
            return outMain;
        }

        // PUT api/borrowreturn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowreturn/5
        public void Delete(int id)
        {
        }
    }
}
