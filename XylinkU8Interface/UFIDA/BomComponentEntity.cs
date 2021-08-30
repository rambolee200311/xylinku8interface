using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Bom;

namespace XylinkU8Interface.UFIDA
{
    public class BomComponentEntity
    {
        public static List<BomComponent> GetBomComponent(string companycode, string parentProductCode)
        {
            List<BomComponent> bcs = new List<BomComponent>();
            //BomComponent bc = new BomComponent();
            try
            {
                string bomid = "";
                string remark = "";
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
                if (m_ologin == null)
                {
                    return bcs;
                }
                bomid = Ufdata.getDataReader(m_ologin.UfDbName, "select bomid from [dbo].[bom_parent] a"
                                                            + " inner join bas_part b on a.ParentId=b.PartId"
                                                            + " inner join inventory c on b.InvCode=c.cInvCode"
                                                            + " where a.BomId in (select BomId from bom_bom where Status=3 and AuditStatus=1)"
                                                            + " and c.cinvcode='" + parentProductCode + "'");
                if (!string.IsNullOrEmpty(bomid))
                {
                    remark = Ufdata.getDataReader(m_ologin.UfDbName, "select versiondesc from bom_bom where bomid=" + bomid);
                    DataTable dt = Ufdata.getDatatableFromSql(m_ologin.UfDbName, "select a.BomId,a.sortseq,c.cInvStd,c.cInvCode,c.cInvName,BaseQtyN/BaseQtyD ND,a.Remark from [dbo].[bom_opcomponent] a"
                                                                            + " inner join bas_part b on a.ComponentId=b.PartId"
                                                                            + " inner join inventory c on b.InvCode=c.cInvCode"
                                                                            + " where bomid=" + bomid);
                    foreach (DataRow dr in dt.Rows)
                    {
                        BomComponent bc = new BomComponent();
                        bc.serialNum = dr["sortseq"].ToString();
                        bc.productCode = dr["cInvCode"].ToString();
                        bc.productName = dr["cInvName"].ToString();
                        bc.model = dr["cInvStd"].ToString();
                        bc.amount = Convert.ToDecimal(dr["ND"]);
                        if (!string.IsNullOrEmpty(dr["Remark"].ToString()))
                        {
                            remark = dr["Remark"].ToString();
                        }
                        bc.remark = remark;
                        bcs.Add(bc);
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(typeof(BomComponentEntity), ex.Message);
            }
            LogHelper.WriteLog(typeof(BomComponentEntity), JsonHelper.ToJson(bcs));
            return bcs;
        }
    }
}