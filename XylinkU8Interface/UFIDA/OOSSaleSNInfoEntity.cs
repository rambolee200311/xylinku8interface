using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSSaleSNInfo;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    public class OOSSaleSNInfoEntity
    {
        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;
            
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            string strSql = @"select top 1 c.cCode u8outcode,c.dnverifytime u8outtime,a.cInvCode invcode,d.cInvName invname,a.cInvSN sncode,som.cSOCode u8code,dt.cDLCode u8invcode,som.cDefine10 u8extcode
                            from ST_SNDetail_SaleOut a
                            inner join RdRecords32 b on a.iVouchsID=b.AutoID 
                            inner join RdRecord32 c on b.ID=c.ID 
                            left join DispatchLists dts on b.iDLsID=dts.iDLsID
                            left join DispatchList dt on dts.DLID=dt.DLID
                            left join SO_SODetails sod on dts.iSOsID=sod.iSOsID
                            left join SO_SOMain som on sod.ID=som.ID
                            inner join inventory d on a.cInvCode=d.cInvCode";
            strSql += " where a.cInvSN=?";
            strSql += " order by c.dnverifytime desc";
            

            DataTable dtResult = null;
            infor.datas = new List<ClsInfoData>();
            try
            {
                foreach (ClsQueryCode sncode in query.sncodes)
                {
                    List<Param> myParams = new List<Param>();
                    Param param1 = new Param();
                    param1.paramname = "@sncode";
                    param1.paramtype = OleDbType.VarChar;
                    param1.paramvalue = sncode.sncode.ToString();
                    myParams.Add(param1);
                    LogHelper.WriteLog(typeof(OOSSaleSNInfoEntity), strSql);
                    LogHelper.WriteLog(typeof(OOSSaleSNInfoEntity), JsonHelper.ToJson(myParams));
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                    if (dtResult != null)
                    {

                        foreach (DataRow dr in dtResult.Rows)
                        {
                            ClsInfoData infordata = new ClsInfoData();
                            infordata.invcode = dr["invcode"].ToString();
                            infordata.invname = dr["invname"].ToString();
                            infordata.sncode = dr["sncode"].ToString();
                            infordata.u8Code = dr["u8code"].ToString();
                            infordata.u8ExtCode = dr["u8extcode"].ToString();
                            infordata.u8InvCode = dr["u8invcode"].ToString();
                            infordata.u8OutCode = dr["u8outcode"].ToString();
                            infordata.u8OutTime = Convert.ToDateTime(dr["u8outtime"]).ToShortDateString() + " " + Convert.ToDateTime(dr["u8outtime"]).ToLongTimeString();
                            infor.datas.Add(infordata);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OOSSaleSNInfoEntity), ex);
            }
            return infor;
        }
        
    }
}