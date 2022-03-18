using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.InventoryClass;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.Controllers
{
    public class InventoryClassController : ApiController
    {
        // GET api/inventoryclass
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public InventoryClass Get(string companycode)
        {
            InventoryClass model = new InventoryClass();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
            if (m_ologin == null)
            { return model; }
            LogHelper.WriteLog(typeof(InventoryClassController), "companycode:" + companycode );
            model.categoryCode = "0";
            model.categoryName = "root";
            model.children = new List<InventoryClass>();
            InventoryClassEntity icc = new InventoryClassEntity();
            model.children = icc.GetChild(companycode,model,m_ologin.UfDbName);
            return model;
        }
        // GET api/inventoryclass/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/inventoryclass
        public void Post([FromBody]string value)
        {
        }

        // PUT api/inventoryclass/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/inventoryclass/5
        public void Delete(int id)
        {
        }
    }
}
