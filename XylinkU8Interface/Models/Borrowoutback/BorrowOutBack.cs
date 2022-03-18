using System;
using System.Collections.Generic;
using System.Web;

namespace XylinkU8Interface.Models.Borrowoutback
{
    public class BorrowOutBack
    {
        public string companycode { get; set; }//帐套号
        public BorrowOutBack_head head { get; set; }
        public List<BorrowOutBack_body> body { get; set; }
    }
}