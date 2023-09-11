using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 借用main
 */
namespace HYBorrowOut.Models.BorrowReturn
{
    public class InMain
    {
        public string companycode { get; set; }
        public InHead head { get; set; }
        public List<InBody> body { get; set; }
    }
}