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
    /*
         *20230924
         *首次同步无上线时间、非首次同步有上线时间，返回待归还的借出和归还数据 
         */
    public class BorrowLedge1Controller : ApiController
    {
        // GET api/borrowledge
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/borrowledge/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/borrowledge
        public OutMain Post([FromBody]InMain inMain)
        {
            LogHelper.WriteLog(typeof(BorrowLedge1Controller), JsonHelper.ToJson(inMain));
            OutMain infor = BorrowLedgerEntity.getBorrowLedger1Entity(inMain);
            LogHelper.WriteLog(typeof(BorrowLedge1Controller), JsonHelper.ToJson(infor));
            return infor;
        }

        // PUT api/borrowledge/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/borrowledge/5
        public void Delete(int id)
        {
        }
    }
}
