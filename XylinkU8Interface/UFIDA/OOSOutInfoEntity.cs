using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSOutInfo;
using System.Data.OleDb;

namespace XylinkU8Interface.UFIDA
{
    public class OOSOutInfoEntity
    {
        /*
         * 2022-08-08
         * 接口根据U8出库单号查询该出库单全部记录产品信息
         * lijianqiang
         */

        public static ClsInfo getInfo(ClsQuery query)
            {
                //20220728
                ClsInfo infor = new ClsInfo();
                infor.companycode = query.companycode;
                infor.datas = new List<ClsInfoData>();
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
                string strSql = "";
                string autoId = "";
                DataTable dtResult = null;
                DataTable dtSncode = null;
                
                try
                {      
                    foreach (ClsQueryCode code in query.codes)
                    {

                        List<Param> myParams = new List<Param>();
                       
                        Param param3 = new Param();
                        param3.paramname = "@code";
                        param3.paramtype = OleDbType.VarChar;
                        param3.paramvalue = code.code.ToString();
                        myParams.Add(param3);

                        ClsInfoData infordata = new ClsInfoData();
                        infordata.code = code.code.ToString();
                        infordata.detail = new List<ClsInfoDataDetatil>();

                        //销售出库
                        strSql = @"select c.cCode u8outcode,c.dnverifytime u8outtime,b.cInvCode invcode,d.cInvName invname,som.cSOCode u8code,dt.cDLCode u8invcode,som.cDefine10 u8extcode,
                                    b.irowno rowid,cus.cCusName custname,b.iQuantity num,b.AutoID
                                    from RdRecords32 b
                                    inner join RdRecord32 c on b.ID=c.ID 
                                    left join DispatchLists dts on b.iDLsID=dts.iDLsID
                                    left join DispatchList dt on dts.DLID=dt.DLID
                                    left join SO_SODetails sod on dts.iSOsID=sod.iSOsID
                                    left join SO_SOMain som on sod.ID=som.ID
                                    left join Customer cus on som.cCusCode=cus.cCusCode
                                    inner join inventory d on b.cInvCode=d.cInvCode";
                        strSql += " where c.cCode=?";
                        LogHelper.WriteLog(typeof(OOSOutInfoEntity), strSql);
                        LogHelper.WriteLog(typeof(OOSOutInfoEntity), JsonHelper.ToJson(myParams));
                        dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);

                        if (dtResult != null)
                        {
                            foreach (DataRow dr in dtResult.Rows)
                            {
                                ClsInfoDataDetatil detail = new ClsInfoDataDetatil();

                                detail.invcode = dr["invcode"].ToString();
                                detail.invname = dr["invname"].ToString();
                                detail.custName = dr["custname"].ToString();
                                detail.rowId = dr["rowid"].ToString();
                                detail.u8Code = dr["u8code"].ToString();
                                detail.u8ExtCode = dr["u8extcode"].ToString();
                                detail.u8InvCode = dr["u8invcode"].ToString();
                                detail.u8OutCode = dr["u8outcode"].ToString();
                                detail.num = Convert.ToDecimal(dr["num"]);
                                detail.u8OutTime = Convert.ToDateTime(dr["u8outtime"]).ToShortDateString() + " " + Convert.ToDateTime(dr["u8outtime"]).ToLongTimeString();
                                //sncode
                                autoId = dr["AutoID"].ToString();
                                strSql = "select cInvSN from ST_SNDetail_SaleOut where iVouchsID=" + autoId;
                                dtSncode = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                                LogHelper.WriteLog(typeof(OOSOutInfoEntity), strSql);
                                detail.sncodes = new List<ClsInfoDataDetailSncode>();
                                if (dtSncode!=null)
                                {
                                    foreach (DataRow drSn in dtSncode.Rows)
                                    {
                                        ClsInfoDataDetailSncode sncode = new ClsInfoDataDetailSncode();
                                        sncode.sncode = drSn["cInvSN"].ToString();
                                        detail.sncodes.Add(sncode);
                                    }
                                }


                                infordata.detail.Add(detail);
                            }

                        }


                        //其他出库
                        strSql = @"select c.cCode u8outcode,c.dnverifytime u8outtime,b.cInvCode invcode,d.cInvName invname,bo.cCODE u8code,'' u8invcode,bo.cDefine12 u8extcode,
                                    b.irowno rowid,cus.cCusName custname,b.iQuantity num,b.AutoID
                                    from RdRecords09 b
                                    inner join RdRecord09 c on b.ID=c.ID 
                                    left join HY_DZ_BorrowOuts bos on bos.AutoID=b.idebitchildids
                                    left join HY_DZ_BorrowOut bo on bo.ID=bos.ID
                                    inner join inventory d on b.cInvCode=d.cInvCode
                                    left join Customer cus on bo.bObjectCode=cus.cCusCode";
                        strSql += " where c.cCode=?";
                        LogHelper.WriteLog(typeof(OOSOutInfoEntity), strSql);
                        LogHelper.WriteLog(typeof(OOSOutInfoEntity), JsonHelper.ToJson(myParams));
                        dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);

                        if (dtResult != null)
                        {
                            foreach (DataRow dr in dtResult.Rows)
                            {
                                ClsInfoDataDetatil detail = new ClsInfoDataDetatil();

                                detail.invcode = dr["invcode"].ToString();
                                detail.invname = dr["invname"].ToString();
                                detail.custName = dr["custname"].ToString();
                                detail.rowId = dr["rowid"].ToString();
                                detail.u8Code = dr["u8code"].ToString();
                                detail.u8ExtCode = dr["u8extcode"].ToString();
                                detail.u8InvCode = dr["u8invcode"].ToString();
                                detail.u8OutCode = dr["u8outcode"].ToString();
                                detail.num = Convert.ToDecimal(dr["num"]);
                                detail.u8OutTime = Convert.ToDateTime(dr["u8outtime"]).ToShortDateString() + " " + Convert.ToDateTime(dr["u8outtime"]).ToLongTimeString();

                                //sncode
                                autoId = dr["AutoID"].ToString();
                                strSql = "select cInvSN from ST_SNDetail_OtherOut where iVouchsID=" + autoId;
                                dtSncode = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                                LogHelper.WriteLog(typeof(OOSOutInfoEntity), strSql);
                                detail.sncodes = new List<ClsInfoDataDetailSncode>();
                                if (dtSncode != null)
                                {
                                    foreach (DataRow drSn in dtSncode.Rows)
                                    {
                                        ClsInfoDataDetailSncode sncode = new ClsInfoDataDetailSncode();
                                        sncode.sncode = drSn["cInvSN"].ToString();
                                        detail.sncodes.Add(sncode);
                                    }
                                }

                                infordata.detail.Add(detail);
                            }

                        }


                        infor.datas.Add(infordata);
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(OOSOutInfoEntity), ex);
                }
                return infor;
            }

        }
    
}