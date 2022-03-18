using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Data;

namespace XylinkU8Interface.Helper
{
    public class Param
    {
        public string paramname { get; set; }
        public OleDbType paramtype { get; set; }
        public string paramvalue { get; set; }
    }
}