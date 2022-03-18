using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.Warehouse;

namespace XylinkU8Interface.Controllers
{
    public class WarehouseController : ApiController
    {
        // GET api/warehouse
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/warehouse/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/warehouse
        public Warehouse Post([FromBody]WarehouseQuery wq)
        {
            Warehouse wh = WarehouseEntity.getWarehouse(wq);
            return wh;
        }

        // PUT api/warehouse/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/warehouse/5
        public void Delete(int id)
        {
        }
    }
}
