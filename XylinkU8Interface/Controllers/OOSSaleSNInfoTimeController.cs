using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSSaleSNInfoTime;
namespace XylinkU8Interface.Controllers
{
    //2022-12 查询某个时间段销售出库的SN CRM←→U8
    public class OOSSaleSNInfoTimeController : ApiController
    {
        // GET api/oossalesninfotime
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oossalesninfotime/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oossalesninfotime
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSSaleSNInfoTimeController), JsonHelper.ToJson(query));
            ClsInfo infor = OOSSaleSNInfoTimeEntity.getInfo(query);
            LogHelper.WriteLog(typeof(OOSSaleSNInfoTimeController), JsonHelper.ToJson(infor));
            return infor;
        }
        // PUT api/oossalesninfotime/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oossalesninfotime/5
        public void Delete(int id)
        {
        }
    }
}
