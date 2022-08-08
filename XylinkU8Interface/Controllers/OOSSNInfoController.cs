using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSSNInfo;
namespace XylinkU8Interface.Controllers
{
    /*
     * 2022-08-08
     * 根据SN查询关于该SN所有的出库记录，包括销售出库、其他出库（批量）（不包括红字销售出库单）
     * lijianqiang
     */
    public class OOSSNInfoController : ApiController
    {
        // GET api/oossninfo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oossninfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oossninfo
        // POST api/isstate
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsInfo Post([FromBody]ClsQuery query)
        {
            LogHelper.WriteLog(typeof(OOSSNInfoController), JsonHelper.ToJson(query));
            ClsInfo infor = OOSSNInfoEntity.getInfo(query);
            LogHelper.WriteLog(typeof(OOSSNInfoController), JsonHelper.ToJson(infor));
            return infor;
        }

        // PUT api/oossninfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oossninfo/5
        public void Delete(int id)
        {
        }
    }
}
