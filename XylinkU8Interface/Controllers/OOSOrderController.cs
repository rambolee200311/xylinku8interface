using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.OOSOrder;

namespace XylinkU8Interface.Controllers
{
    public class OOSOrderController : ApiController
    {
        // GET api/oosorder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/oosorder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/oosorder
        //public void Post([FromBody]string value)
        //{
        //}
        public ClsResponse Post([FromBody]ClsRequest req)
        {
            LogHelper.WriteLog(typeof(OOSOrderController), JsonHelper.ToJson(req));
            ClsResponse rep=null;
            ClsResponse rep1 = null;
            if (req.head.category != "试⽤业务SN的调换")
            {
                 rep= OOSOrderEntity.postResquest(req);
            }
            else
            {
                req.head.category = "试⽤业务SN的调换-CRM入库";
                rep = OOSOrderEntity.postResquest(req);
                if (rep.recode == "0")
                {
                    req.head.category = "试⽤业务SN的调换-CRM出库";
                    rep1= OOSOrderEntity.postResquest(req);
                    if (rep1.recode == "0")
                    {
                        rep.u8code += "," + rep1.u8code;
                    }
                    else
                    {
                        rep = rep1;
                    }
                }
            }
            LogHelper.WriteLog(typeof(OOSOrderController), JsonHelper.ToJson(rep));
            return rep;
        }
        // PUT api/oosorder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/oosorder/5
        public void Delete(int id)
        {
        }
    }
}
