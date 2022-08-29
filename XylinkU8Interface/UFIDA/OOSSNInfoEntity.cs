using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSSNInfo;
using System.Data.OleDb;

namespace XylinkU8Interface.UFIDA
{
    /*
     * 2022-08-08
     * 根据SN查询关于该SN所有的出库记录，包括销售出库、其他出库（批量）（不包括红字销售出库单）
     * lijianqiang
     */
    public class OOSSNInfoEntity
    {
        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;
            
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            string strSql = "";
            string dDates = "1980-01-01";
            string dDatee = "2049-12-31";

            DataTable dtResult = null;
            DataTable dtPaged = null;
            infor.datas = new List<ClsInfoData>();
            try
            {
                if (!string.IsNullOrEmpty(query.startTime))
                {
                    dDates = Convert.ToDateTime(query.startTime).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(query.endTime))
                {
                    dDatee = Convert.ToDateTime(Convert.ToDateTime(query.endTime).ToShortDateString()).AddDays(1).ToShortDateString();
                }              
                
               
                foreach (ClsQueryCode sncode in query.sncodes)
                {

                    List<Param> myParams = new List<Param>();
                    Param param1 = new Param();
                    param1.paramname = "@starttime";
                    param1.paramtype = OleDbType.VarChar;
                    param1.paramvalue = dDates;
                    myParams.Add(param1);
                    Param param2 = new Param();
                    param2.paramname = "@endtime";
                    param2.paramtype = OleDbType.VarChar;
                    param2.paramvalue = dDatee;
                    myParams.Add(param2);
                    Param param3 = new Param();
                    param3.paramname = "@sncode";
                    param3.paramtype = OleDbType.VarChar;
                    param3.paramvalue = sncode.sncode.ToString();
                    myParams.Add(param3);

                    ClsInfoData infordata = new ClsInfoData();
                    infordata.sncode = sncode.sncode.ToString();
                    infordata.detail = new List<ClsInfoDataDetatil>();

                    //销售出库
                    strSql = @"select c.cCode u8outcode,c.dnverifytime u8outtime,a.cInvCode invcode,d.cInvName invname,a.cInvSN sncode,som.cSOCode u8code,dt.cDLCode u8invcode,som.cDefine10 u8extcode,
                                b.irowno rowid,cus.cCusName custname,'销售出库' businesstype,ext.cbdefine21 reqid
                                from ST_SNDetail_SaleOut a
                                inner join RdRecords32 b on a.iVouchsID=b.AutoID 
                                inner join RdRecords32_extradefine ext on b.AutoID=ext.AutoID 
                                inner join RdRecord32 c on b.ID=c.ID 
                                left join DispatchLists dts on b.iDLsID=dts.iDLsID
                                left join DispatchList dt on dts.DLID=dt.DLID
                                left join SO_SODetails sod on dts.iSOsID=sod.iSOsID
                                left join SO_SOMain som on sod.ID=som.ID
                                left join Customer cus on som.cCusCode=cus.cCusCode
                                inner join inventory d on a.cInvCode=d.cInvCode";
                    strSql += " where c.dnverifytime>=? and c.dnverifytime<? and a.cInvSN=? and b.iQuantity>0";
                    strSql += " order by c.dnverifytime,c.cCode";
                    LogHelper.WriteLog(typeof(OOSSNInfoEntity), strSql);
                    LogHelper.WriteLog(typeof(OOSSNInfoEntity), JsonHelper.ToJson(myParams));
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                    
                    if (dtResult != null)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            ClsInfoDataDetatil detail = new ClsInfoDataDetatil();

                            detail.invcode = dr["invcode"].ToString();
                            detail.invname = dr["invname"].ToString();
                            detail.custName = dr["custname"].ToString();
                            detail.businessType = dr["businesstype"].ToString();
                            detail.rowId = dr["rowid"].ToString();
                            detail.u8Code = dr["u8code"].ToString();
                            detail.u8ExtCode = dr["u8extcode"].ToString();
                            detail.u8InvCode = dr["u8invcode"].ToString();
                            detail.u8OutCode = dr["u8outcode"].ToString();
                            detail.reqid = dr["reqid"].ToString();
                            detail.u8OutTime = Convert.ToDateTime(dr["u8outtime"]).ToShortDateString() + " " + Convert.ToDateTime(dr["u8outtime"]).ToLongTimeString();

                            infordata.detail.Add(detail);
                        }

                    }


                    //其他出库
                    strSql = @"select c.cCode u8outcode,c.dnverifytime u8outtime,a.cInvCode invcode,d.cInvName invname,a.cInvSN sncode,bo.cCODE u8code,'' u8invcode,bo.cDefine12 u8extcode,
                                b.irowno rowid,cus.cCusName custname,'其他出库' businesstype,ext.cbdefine21 reqid
                                from ST_SNDetail_OtherOut a
                                inner join RdRecords09 b on a.iVouchsID=b.AutoID 
                                inner join RdRecord09 c on b.ID=c.ID 
                                left join HY_DZ_BorrowOuts bos on bos.AutoID=b.idebitchildids
                                left join HY_DZ_BorrowOuts_extradefine ext on bos.AutoID=ext.AutoID    
                                left join HY_DZ_BorrowOut bo on bo.ID=bos.ID
                                inner join inventory d on a.cInvCode=d.cInvCode
                                left join Customer cus on bo.bObjectCode=cus.cCusCode";
                    strSql += " where c.dnverifytime>=? and c.dnverifytime<? and a.cInvSN=? and b.iQuantity>0";
                    strSql += " order by c.dnverifytime,c.cCode";
                    LogHelper.WriteLog(typeof(OOSSNInfoEntity), strSql);
                    LogHelper.WriteLog(typeof(OOSSNInfoEntity), JsonHelper.ToJson(myParams));
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);

                    if (dtResult != null)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            ClsInfoDataDetatil detail = new ClsInfoDataDetatil();

                            detail.invcode = dr["invcode"].ToString();
                            detail.invname = dr["invname"].ToString();
                            detail.custName = dr["custname"].ToString();
                            detail.businessType = dr["businesstype"].ToString();
                            detail.rowId = dr["rowid"].ToString();
                            detail.u8Code = dr["u8code"].ToString();
                            detail.u8ExtCode = dr["u8extcode"].ToString();
                            detail.u8InvCode = dr["u8invcode"].ToString();
                            detail.u8OutCode = dr["u8outcode"].ToString();
                            detail.u8OutTime = Convert.ToDateTime(dr["u8outtime"]).ToShortDateString() + " " + Convert.ToDateTime(dr["u8outtime"]).ToLongTimeString();
                            detail.reqid = dr["reqid"].ToString();
                            infordata.detail.Add(detail);
                        }

                    }


                    infor.datas.Add(infordata);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OOSSNInfoEntity), ex);
            }
            return infor;
        }
       
    }
}