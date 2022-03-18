using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.LogisticsInfo;
using XylinkU8Interface.Models.Result;
using System.Data;
namespace XylinkU8Interface.UFIDA
{
    /// <summary>
    /// 订单出库序列号查询业务实现
    /// 20210829
    /// </summary>
    public class LogisticsInfoEntity
    {
        public static LogisticsInfo GetResult(LogisticQuery lq)
        {
            LogHelper.WriteLog(typeof(LogisticsInfoEntity), lq.ToJson());
            LogisticsInfo result=new LogisticsInfo();
            result.datas = new List<LogisticsInfoData>();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(lq.companycode);
            result.companycode = lq.companycode;

            if (m_ologin != null)
            {

                string sql = @"select a.cInvCode invcode,g.cInvName invname,a.cDefine22 excomp,a.cDefine23 exnum,
                            b.cbdefine4 receiver,b.cbdefine5 recrmobi,
                            replace(isnull(b.cbdefine6,''),'/','')+replace(isnull(b.cbdefine7,''),'/','')+replace(isnull(b.cbdefine8,''),'/','')+replace(isnull(b.cbdefine9,''),'/','') recraddress,
                            b.cbdefine21 reqId,f.cInvSN sncode,case when isnull(f.cInvSN,'')='' then a.iQuantity else case when a.iQuantity>0 then 1 else -1 end end num,
                            e.cSOCode u8code ,e.cDefine10 ccode 
                            from rdrecords32 a 
                            inner join rdrecords32_extradefine b on a.AutoID=b.AutoID 
                            inner join DispatchLists c on a.iDLsID=c.iDLsID 
                            inner join SO_SODetails d on c.iSOsID=d.iSOsID 
                            inner join SO_SOMain e on d.ID=e.ID 
                            left join ST_SNDetail_SaleOut f on a.AutoID=f.iVouchsID 
                            inner join Inventory g on a.cInvCode=g.cInvCode";
                string condition = "";
                foreach(LogisticQueryCode code in lq.codes)
                {
                    condition += "'" + code.ccode + "',";
                }
                condition = condition.Substring(0, condition.Length - 1);
                DataTable dt = Ufdata.getDatatableFromSql(m_ologin.UfDbName, sql + " where e.cDefine10 in (" + condition + ") order by e.ID");
                foreach(DataRow dr in dt.Rows)
                {
                    LogisticsInfoData data = new LogisticsInfoData();
                    data.ccode = dr["ccode"].ToString();
                    data.u8code = dr["u8code"].ToString();
                    data.invcode = dr["invcode"].ToString();
                    data.invname = dr["invname"].ToString();
                    data.excomp = dr["excomp"].ToString();
                    data.exnum = dr["exnum"].ToString();
                    data.receiver = dr["receiver"].ToString();
                    data.recrmobi = dr["recrmobi"].ToString();
                    data.recraddress = dr["recraddress"].ToString();
                    data.reqId = dr["reqId"].ToString();
                    data.num = Convert.ToDecimal(dr["num"]);
                    data.sncode = dr["sncode"].ToString();
                    result.datas.Add(data);
                }
                
                
            }
            return result; ;
        }
    }
}