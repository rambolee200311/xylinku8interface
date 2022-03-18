using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Inventory;
namespace XylinkU8Interface.Controllers
{
    public class InventoryController : ApiController
    {
        // GET api/inventory
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public InvResult Get(string companycode,int currentPage,int size)
        {
            InvResult ir=null;
            try
            {                
                if ((string.IsNullOrEmpty(companycode))||(currentPage==null)||(currentPage<0)||(size==null)||(size<0))
                {
                    return ir;
                }
                LogHelper.WriteLog(typeof(InventoryController), "companycode:" + companycode + ",currentPage:" + currentPage.ToString() + ",size:" + size.ToString());
                ir = InventoryEntity.GetInvResult(companycode, currentPage, size);
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(typeof(InventoryController), ex.Message);
            }
            return ir;
        }
        // GET api/inventory/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/inventory
        public void Post([FromBody]string value)
        {
        }

        // PUT api/inventory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/inventory/5
        public void Delete(int id)
        {
        }
    }
}
