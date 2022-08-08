using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSU8ExtInfo;
namespace XylinkU8Interface.Controllers
{
        /*
        * 2022-08-08
        * 根据U8单据号或CRM单据号，查询该订单对应U8出库的产品信息（批量）
        * lijianqiang
        */
    public class OOSU8ExtInfoController : ApiController
    {
        // GET api/oosu8extinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oosu8extinfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oosu8extinfo
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSU8ExtInfoController), JsonHelper.ToJson(query));
            ClsInfo infor =OOSU8ExtInfoEntity.getInfo(query);
            LogHelper.WriteLog(typeof(OOSU8ExtInfoController), JsonHelper.ToJson(infor));
            return infor;
        }
        // PUT api/oosu8extinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oosu8extinfo/5
        public void Delete(int id)
        {
        }
    }
}
