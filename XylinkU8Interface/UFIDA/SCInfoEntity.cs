using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.SCInfo;
using XylinkU8Interface.Models.Result;
using System.Data;
using System.Data.OleDb;

namespace XylinkU8Interface.UFIDA
{
    public class SCInfoEntity
    {
        public static SCInfo getSCInfo(SCInfoQuery scInfoQuery)
        {
            SCInfo scInfo = new SCInfo();
            scInfo.companycode = scInfoQuery.companycode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(scInfoQuery.companycode);
            scInfo.datas = new List<SCInfoDatas>();
            string strSql = "select c.cCode ccode,a.cInvCode invcode,d.cInvName invname,case when b.iQuantity>0 then 1 else -1 end outnum from ST_SNDetail_SaleOut a"
                            + " inner join RdRecords32 b on a.iVouchsID=b.AutoID inner join RdRecord32 c on b.ID=c.ID inner join inventory d on a.cInvCode=d.cInvCode"
                            + " where  a.cInvSN=?";
            foreach (SCInfoQueryCode diqCode in scInfoQuery.sncodes)
            {
                SCInfoDatas scinfoDatas = new SCInfoDatas();
                scinfoDatas.detail = new List<SCInfoDetail>();
                scinfoDatas.sncode = diqCode.sncode;
                List<Param> myParams = new List<Param>();
                Param param = new Param();
                param.paramname = "@sncode";
                param.paramtype = OleDbType.VarChar;
                param.paramvalue = diqCode.sncode;
                myParams.Add(param);
                DataTable dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);                
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        SCInfoDetail dinfoData = new SCInfoDetail();
                        dinfoData.ccode = dr["ccode"].ToString();                       
                        dinfoData.invcode = dr["invcode"].ToString();
                        dinfoData.invname = dr["invname"].ToString();
                        dinfoData.outnum = Convert.ToDecimal(dr["outnum"]);
                        scinfoDatas.detail.Add(dinfoData);
                    }
                    scInfo.datas.Add(scinfoDatas);
               
            }
            return scInfo;
        }
    }
}