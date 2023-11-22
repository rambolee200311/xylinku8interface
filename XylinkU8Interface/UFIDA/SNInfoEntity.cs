using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using XylinkU8Interface.Models.SNInfo;
using XylinkU8Interface.Helper;

namespace XylinkU8Interface.UFIDA
{
    public class SNInfoEntity
    {
        public static SNInfo GetResult(SNInfoQuery lq)
        {
            LogHelper.WriteLog(typeof(SNInfoEntity), lq.ToJson());
            SNInfo result = new SNInfo();
            try { 
            result.datas = new List<SNInfoData>();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(lq.companycode);
            result.companycode = lq.companycode;
            string strSql = "";
            DataTable dtOrder = null;
            DataTable dtDL = null;
            DataTable dtBL = null;
            result.datas = new List<SNInfoData>();
            if (m_ologin != null)
            {
                foreach (SNInfoQueryCode code in lq.codes)
                {
                    strSql = @"select b.cDefine10 ccode,b.csocode u8code,a.cInvCode invcode,c.cInvName invname,d.cbdefine21 req_id,a.iQuantity ordnum,a.isum,a.iSOsID
                            from SO_SOMain b
                            inner join SO_SODetails a on a.ID=b.ID
                            inner join inventory c on c.cInvCode=a.cInvCode
                            inner join SO_SODetails_extradefine d on a.iSOsID=d.iSOsID
                            where b.cDefine10=?";
                    List<Param> myParams = new List<Param>();
                    Param param = new Param();
                    param.paramname = "@cDefine10";
                    param.paramtype = OleDbType.VarChar;
                    param.paramvalue = code.ccode;
                    myParams.Add(param);
                    dtOrder = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                    if (dtOrder != null)
                    {
                        foreach (DataRow dr in dtOrder.Rows)
                        {
                            SNInfoData data = new SNInfoData();
                            data.bldetail = new List<SNInfoBLDetail>();
                            data.dldetail = new List<SNInfoDLDetail>();
                            data.invcode = dr["invcode"].ToString();
                            data.invname = dr["invname"].ToString();
                            data.u8code = dr["u8code"].ToString();
                            data.ordnum = Convert.ToDecimal(dr["ordnum"]);
                            data.ordamt = Convert.ToDecimal(dr["isum"]);
                            data.ccode = dr["ccode"].ToString();
                            data.req_id = dr["req_id"].ToString();
                            //dldetail
                            //20231120 增加销售出库单已审核条件

                            strSql = @"select a.cInvCode invcode,c.cInvName invname,isnull(a.iQuantity,0) outnum,isnull(a.isum,0) isum,b.cDLCode u8code
                                    from DispatchLists a
                                    inner join DispatchList b on a.DLID=b.DLID
                                    inner join inventory c on a.cInvCode=c.cInvCode
                                    left join RdRecords09 e on a.iDLsID=e.iDLsID
									left join RdRecord09 f on e.ID=f.ID    
                                    where isnull(f.dVeriDate,'1900-01-01')!='1900-01-01' and a.iSOsID=?";
                            List<Param> yourParams = new List<Param>();
                            Param param1 = new Param();
                            param1.paramname = "@iSOsID";
                            param1.paramtype = OleDbType.VarChar;
                            param1.paramvalue = dr["iSOsID"].ToString();
                            yourParams.Add(param1);
                            dtDL = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, yourParams);
                            if (dtDL != null)
                            {
                                foreach (DataRow dr1 in dtDL.Rows)
                                {
                                    SNInfoDLDetail dlDetail = new SNInfoDLDetail();
                                    dlDetail.invcode = dr1["invcode"].ToString();
                                    dlDetail.invname = dr1["invname"].ToString();
                                    dlDetail.u8code = dr1["u8code"].ToString();
                                    dlDetail.outnum = Convert.ToDecimal(dr1["outnum"]);
                                    dlDetail.outamt = Convert.ToDecimal(dr1["isum"]);
                                    data.dldetail.Add(dlDetail);
                                }
                            }

                            //bldetail
                           // strSql = @"select  a.cInvCode invcode,c.cInvName invname,isnull(a.iQuantity,0) billnum,isnull(a.isum,0) isum,b.cSBVCode u8code
                                    //    from SaleBillVouchs a
                                    //    inner join SaleBillVouch b on a.SBVID=b.SBVID
                                    //    inner join inventory c on a.cInvCode=c.cInvCode
                                    //where a.iSOsID=?";
                            strSql = @"select a.iSOsID,b.cBDefine21,a.cInvCode invcode,c.cInvName invname,isnull(b.cbdefine26,0) billnum,isnull(b.cbdefine27,0) isum,'' u8code
                                    from SO_SODetails a
                                    inner join SO_SODetails_extradefine b on a.iSOsID=b.iSOsID
                                    inner join Inventory c on a.cInvCode=c.cInvCode 
                                    where a.iSOsID=?";
                            dtBL = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, yourParams);
                            if (dtBL != null)
                            {
                                foreach (DataRow dr1 in dtBL.Rows)
                                {
                                    SNInfoBLDetail blDetail = new SNInfoBLDetail();
                                    blDetail.invcode = dr1["invcode"].ToString();
                                    blDetail.invname = dr1["invname"].ToString();
                                    blDetail.u8code = dr1["u8code"].ToString();
                                    blDetail.billnum = Convert.ToDecimal(dr1["billnum"]);
                                    blDetail.billamt = Convert.ToDecimal(dr1["isum"]);
                                    data.bldetail.Add(blDetail);
                                }
                            }
                            result.datas.Add(data);
                        }
                    }

                }
            }
            }
            catch(Exception e){
                LogHelper.WriteLog(typeof(SNInfoEntity), e.Message);
                }
                
            
            return result;
        }
    }
}