using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XylinkU8Interface.Models.Person;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface.Controllers
{
    //11 组织架构 泛微→→U8

    public class PersonController : ApiController
    {
        // GET api/person
        public Persons Get()
        {
            //return new string[] { "value1", "value2" };
            Persons pers = new Persons();
            pers.person = new List<Person>();
            pers.companycode = "001";
            Person per1 = new Person();
            per1.code = "000014";
            per1.name = "李兆泽";
            per1.depname = "供应链部";
            per1.rsex = "男";
            pers.person.Add(per1);

            Person per2 = new Person();
            per2.code = "000021";
            per2.name = "贾翠颖";
            per2.depname = "供应链部";
            per2.rsex = "女";
            pers.person.Add(per2);
            return pers;
        }
        public Result Get(string companycode, string name)
        {
            Result re = new Result();
            re = PersonEntity.GetResult(companycode, name);
            return re;
        }
        // GET api/person/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/person
        public Results Post([FromBody]Persons pers)
        {
            Results myresults = new Results();
            //myresults.result = new List<Result>();
            //myresults.companycode = pers.companycode;
            //string strResult = "";
            //foreach (Person singlecust in pers.person)
            //{
            //    Result singleresult = new Result();
            //    singleresult.oacode = singlecust.code;
            //    singleresult.u8code = singlecust.code;
            //    singleresult.recode = "0";
            //    singleresult.remsg = strResult;
            //    myresults.result.Add(singleresult);
            //}
            myresults = PersonEntity.add_person(pers);
            return myresults;
        }

        // PUT api/person/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/person/5
        public void Delete(int id)
        {
        }
    }
}
