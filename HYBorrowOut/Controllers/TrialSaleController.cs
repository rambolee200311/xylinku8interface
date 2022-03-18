﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HYBorrowOut.Models.TrialSale;
using HYBorrowOut.Models.Result;
using HYBorrowOut.UFIDA;
using HYBorrowOut.Helper;

namespace HYBorrowOut.Controllers
{
    public class TrialSaleController : ApiController
    {
        // GET api/trialsale
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/trialsale/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/trialsale
        public Result Post([FromBody]TrialSale bo)
        {
            LogHelper.WriteLog(typeof(TrialSaleController), JsonHelper.ToJson(bo));
            Result re= TrialSaleEntity.add_TrialSale(bo);
            LogHelper.WriteLog(typeof(TrialSaleController), JsonHelper.ToJson(re));
            return re;
        }

        // PUT api/trialsale/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/trialsale/5
        public void Delete(int id)
        {
        }
    }
}
