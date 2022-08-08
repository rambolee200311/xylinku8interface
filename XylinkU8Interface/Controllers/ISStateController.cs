using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.ISState;

namespace XylinkU8Interface.Controllers
{
    /*
     * 2022-08-08
     * 根据SN查询该SN的在库状态
     * lijianqiang
     */
    public class ISStateController : ApiController
    {
        // GET api/isstate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/isstate/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/isstate
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(ISStateController), JsonHelper.ToJson(query));
            ClsInfo infor = ISStateEntity.getInfo(query);
            LogHelper.WriteLog(typeof(ISStateController), JsonHelper.ToJson(infor));
            return infor;
        }
        // PUT api/isstate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/isstate/5
        public void Delete(int id)
        {
        }
    }
}
