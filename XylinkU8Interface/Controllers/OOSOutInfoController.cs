using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSOutInfo;
namespace XylinkU8Interface.Controllers
{
        /*
         * 2022-08-08
         * 接口根据U8出库单号查询该出库单全部记录产品信息
         * lijianqiang
         */
    public class OOSOutInfoController : ApiController
    {
        // GET api/oosoutinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oosoutinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oosoutinfo
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSOutInfoController), JsonHelper.ToJson(query));
            ClsInfo infor = OOSOutInfoEntity.getInfo(query);
            LogHelper.WriteLog(typeof(OOSOutInfoController), JsonHelper.ToJson(infor));
            return infor;
        }
        // PUT api/oosoutinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oosoutinfo/5
        public void Delete(int id)
        {
        }
    }
}
