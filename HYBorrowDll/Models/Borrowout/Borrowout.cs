﻿using System;
using System.Collections.Generic;
using System.Web;

namespace HYBorrowDll.Models.Borrowout
{
    public class Borrowout
    {
        public string companycode { get; set; }//帐套号
        public Borrowout_head head { get; set; }
        public List<Borrowout_body> body { get; set; }
    }
}