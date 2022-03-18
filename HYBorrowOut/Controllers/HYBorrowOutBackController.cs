using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HYBorrowOut.Models.Borrowoutback;
using HYBorrowOut.Models.Result;
using HYBorrowOut.UFIDA;

namespace HYBorrowOut.Controllers
{
    public class HYBorrowOutBackController : ApiController
    {
        // GET api/hyborrowoutback
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/hyborrowoutback/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/hyborrowoutback
        public Result Post([FromBody]BorrowOutBack value)
        {
            HYBorrowOutBackEntity hobe = new HYBorrowOutBackEntity();
            Result re = new Result();
            re = hobe.Add_Borrow_Out_Back(value);
            return re;
        }

        // PUT api/hyborrowoutback/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/hyborrowoutback/5
        public void Delete(int id)
        {
        }
    }
}
