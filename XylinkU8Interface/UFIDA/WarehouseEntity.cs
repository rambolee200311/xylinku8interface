using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Models.Warehouse;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.UFIDA
{
    public class WarehouseEntity
    {
        public static Warehouse getWarehouse(WarehouseQuery wq)
        {
            Warehouse wh = new Warehouse();
            wh.result = new List<WarehouseResult>();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(wq.companycode);
            string strSql = "select cWhCode whcode,cWhName whname,cWhAddress address,cWhPhone phone,cWhPerson,b.cPsn_Name personname from Warehouse a left join hr_hi_person b on a.cwhperson=b.cPsn_Num";
            DataTable dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
            foreach(DataRow dr in dtResult.Rows)
            {
                WarehouseResult whResult = new WarehouseResult();
                whResult.whcode = dr["whcode"].ToString();
                whResult.whname = dr["whname"].ToString();
                whResult.address = dr["address"].ToString();
                whResult.phone = dr["phone"].ToString();
                whResult.personname = dr["personname"].ToString();
                wh.result.Add(whResult);
            }
            return wh;
        }
    }
}