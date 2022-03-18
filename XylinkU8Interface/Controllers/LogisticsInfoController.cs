using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.LogisticsInfo;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    /// <summary>
    /// 订单出库序列号查询控制器
    /// 20210829
    /// </summary>
    public class LogisticsInfoController : ApiController
    {
        // GET api/logisticsinfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/logisticsinfo/5
        public string Get(int id)
        {
            return "value";
        }
       
        // POST api/logisticsinfo
        public LogisticsInfo Post([FromBody]LogisticQuery value)
        {
            LogisticsInfo result =LogisticsInfoEntity.GetResult(value);

           return result;
        }

        // PUT api/logisticsinfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/logisticsinfo/5
        public void Delete(int id)
        {
        }
    }
}
