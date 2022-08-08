using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSSaleSNInfo;
namespace XylinkU8Interface.Controllers
{
    public class OOSSaleSNInfoController : ApiController
    {
        // GET api/oossalesninfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oossalesninfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oossalesninfo
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSSaleSNInfoController), JsonHelper.ToJson(query));
            ClsInfo infor = OOSSaleSNInfoEntity.getInfo(query);
            LogHelper.WriteLog(typeof(OOSSaleSNInfoController), JsonHelper.ToJson(infor));
            return infor;
        }

        // PUT api/oossalesninfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oossalesninfo/5
        public void Delete(int id)
        {
        }
    }
}
