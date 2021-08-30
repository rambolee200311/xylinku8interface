using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XylinkU8Interface.Models.Person
{
    public class Persons
    {
        public string companycode { get; set; }//帐套号
        public List<Person> person { get; set; }//人员档案
    }
}