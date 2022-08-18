using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSLogisticsInfo;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    public class OOSLogisticsInfoEntity
    {

        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;

            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            
            infor.datas = new List<ClsInfoData>();
            try
            {
                foreach (ClsQueryCode code in query.codes)
                {
                    string strSql = @"select a.AutoID,a.cInvCode invcode,g.cInvName invname,a.cDefine22 excomp,a.cDefine23 exnum,
                                e.cbdefine4 receiver,e.cbdefine5 recrmobi,
                                replace(isnull(e.cbdefine6,''),'/','')+replace(isnull(e.cbdefine7,''),'/','')+replace(isnull(e.cbdefine8,''),'/','')+replace(isnull(e.cbdefine9,''),'/','') recraddress,
                                isnull(b.cbdefine21,'')+isnull(e.cbdefine21,'') reqId,a.iQuantity num,c.cDefine12,c.dnverifytime u8outtime 
                                from rdrecords09 a 
                                inner join rdrecords09_extradefine b on a.AutoID=b.AutoID 
                                inner join rdrecord09 c on a.ID=c.ID
                                left join HY_DZ_BorrowOuts d on d.AutoID=a.idebitchildids
                                left join HY_DZ_BorrowOuts_extradefine e on e.AutoID=d.AutoID                
                                inner join Inventory g on a.cInvCode=g.cInvCode
                                where c.ccode=?";
                    List<Param> myParams = new List<Param>();
                    Param param1 = new Param();
                    param1.paramname = "@code";
                    param1.paramtype = OleDbType.VarChar;
                    param1.paramvalue = code.code.ToString();
                    myParams.Add(param1);
                    LogHelper.WriteLog(typeof(OOSLogisticsInfoEntity), strSql);
                    LogHelper.WriteLog(typeof(OOSLogisticsInfoEntity), JsonHelper.ToJson(myParams));
                    DataTable dtResult = null;
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);

                    

                    if (dtResult != null)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            ClsInfoData infordata = new ClsInfoData();
                            infordata.code = code.code;
                            infordata.ccode = dr["cDefine12"].ToString();
                            infordata.invcode = dr["invcode"].ToString();
                            infordata.invname = dr["invname"].ToString();
                            infordata.num =Convert.ToDecimal(dr["num"]);
                            infordata.excomp = dr["excomp"].ToString();
                            infordata.exnum = dr["exnum"].ToString();
                            infordata.receiver = dr["receiver"].ToString();
                            infordata.recemobi = dr["recrmobi"].ToString();
                            infordata.receaddress = dr["recraddress"].ToString();
                            infordata.outTime = dr["u8outtime"].ToString();
                            infordata.reqId = dr["reqId"].ToString();

                            //sncode
                            string autoId = dr["AutoID"].ToString();
                            strSql = "select cInvSN from ST_SNDetail_OtherOut where iVouchsID=" + autoId;
                            DataTable dtSncode = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                            LogHelper.WriteLog(typeof(OOSLogisticsInfoEntity), strSql);
                            infordata.sncodes = new List<ClsInfoDataSncode>();
                            if (dtSncode != null)
                            {
                                foreach (DataRow drSn in dtSncode.Rows)
                                {
                                    ClsInfoDataSncode sncode = new ClsInfoDataSncode();
                                    sncode.sncode = drSn["cInvSN"].ToString();
                                    infordata.sncodes.Add(sncode);
                                }
                            }

                            infor.datas.Add(infordata);
                        }
                    }
                    
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OOSLogisticsInfoEntity), ex);
            }
            return infor;
        }
        
    }
}