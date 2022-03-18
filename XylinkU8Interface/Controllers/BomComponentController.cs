using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using XylinkU8Interface;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Bom;

namespace XylinkU8Interface.Controllers
{
    public class BomComponentController : ApiController
    {
        // GET api/bomcomponent
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public List<BomComponent> Get(string companycode,string parentProductCode)
        {
            LogHelper.WriteLog(typeof(BomComponentController), "companycode:" + companycode+",parentProductCode:"+parentProductCode);
            return BomComponentEntity.GetBomComponent(companycode, parentProductCode);
        }
        // GET api/bomcomponent/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/bomcomponent
        public void Post([FromBody]string value)
        {
        }

        // PUT api/bomcomponent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/bomcomponent/5
        public void Delete(int id)
        {
        }
    }
}
