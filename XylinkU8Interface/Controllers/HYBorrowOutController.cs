﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XylinkU8Interface.Helper;
//using HYBorrowOut.Models.Result;
//using HYBorrowOut.Models.Borrowout;


namespace XylinkU8Interface.Controllers
{
    public class HYBorrowOutController : ApiController
    {
        // GET api/hyborrowout
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/hyborrowout/5
        public string Get(int id)
        {
            return "value";
            //HYBorrowOutEntity hboe = new HYBorrowOutEntity();
            //string result = hboe.Set_Borrow_Out();
            //return id.ToString() + "  " + result;
        }

        // POST api/hyborrowout
        public String Post([FromBody]string value)
        {
            //Result re;
            //LogHelper.WriteLog(typeof(HYBorrowOutController), JsonHelper.ToJson(bo));
            //HYBorrowOutEntity hoe = new HYBorrowOutEntity();
            //HYBorrowOutEntity hoe = new HYBorrowOutEntity();
            //LogHelper.WriteLog(typeof(HYBorrowOutController),"1");
            //Result re = hoe.Set_Borrow_Out();// hoe.Add_Borrow_Out(bo);
           // Result re = hoe.Add_Borrow_Out(bo);
            //LogHelper.WriteLog(typeof(HYBorrowOutController), "2");
            //LogHelper.WriteLog(typeof(HYBorrowOutController), JsonHelper.ToJson(re));
            //return re;
            return "";
        }

        // PUT api/hyborrowout/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/hyborrowout/5
        public void Delete(int id)
        {
        }
    }
}
