using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.InventoryClass;

namespace XylinkU8Interface.UFIDA
{
    public class InventoryClassEntity
    {
        List<InventoryClass> listInvClassAll = new List<InventoryClass>();
        InventoryClass invClassAll = new InventoryClass();

        //public InventoryClass BindNew(string companycode, InventoryClass node)
        //{
        //    DataTable dt = GetReader(companycode, node.categoryCode);
        //    InventoryClass n = new InventoryClass();
        //    foreach(DataRow dr in dt.Rows)
        //    {
        //        n.categoryCode=dr["cinvccode"].ToString();
        //        n.categoryName = dr["cinvcname"].ToString();
        //        n.children = GetChild(companycode, n);
        //    }
        //    return n;
        //}

        public List<InventoryClass> GetChild(string companycode,InventoryClass node,string ufdbname)
        {
           
            List<InventoryClass> children = new List<InventoryClass>();
            
            DataTable dt = GetReader(ufdbname, node.categoryCode);
            try
            {
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        InventoryClass n = new InventoryClass();
                        n.categoryCode = dr["cinvccode"].ToString();
                        n.categoryName = dr["cinvcname"].ToString();
                        children.Add(n);
                        DataTable dtc = GetReader(ufdbname, n.categoryCode);
                        if (dtc != null)
                        {
                            n.children = GetChild(companycode, n,ufdbname);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(InventoryClassEntity), ex.Message);
            }
            LogHelper.WriteLog(typeof(InventoryClassEntity), JsonHelper.ToJson(children));
            return children;
        }

        public DataTable GetReader(string ufdbname, string pid)
        {
            DataTable dt=null;
            
            string sql = "";
            if (pid == "0")
            { sql = "select * from InventoryClass where iInvCGrade=1"; }
            else
            {
              sql = "select * from InventoryClass where cInvCCode like '" + pid + "%' and iInvCGrade=(select iInvCGrade from InventoryClass where cInvCCode= '" + pid + "')+1";
            }
            dt = Ufdata.getDatatableFromSql(ufdbname, sql);
            return dt;
        }
    }
}