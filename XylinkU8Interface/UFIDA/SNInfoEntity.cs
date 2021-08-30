using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
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
            result.datas = new List<SNInfoData>();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(lq.companycode);
            result.companycode = lq.companycode;

            if (m_ologin != null)
            {
                string condition = "";
                foreach (SNInfoQueryCode code in lq.codes)
                {
                    condition += "'" + code.ccode + "',";
                }
                condition = condition.Substring(0, condition.Length - 1);

                string sql =@"select ccode,invcode,invname,reqId,
                        sum(ordnum) ordnum,sum(outnum) outnum,sum(billnum) billnum,
                        sum(ordamt) ordamt,sum(outamt) outamt,sum(billamt) billamt
                        from
                        (select b.cDefine10 ccode,a.cInvCode invcode,c.cInvName invname,d.cbdefine21 reqId,
                        a.iQuantity ordnum,0 outnum,0 billnum,
                        a.iSum ordamt,0 outamt,0 billamt
                        from SO_SODetails a
                        inner join SO_SOMain b on a.ID=b.ID
                        inner join inventory c on a.cInvCode=c.cInvCode
                        inner join SO_SODetails_extradefine d on a.iSOsID=d.iSOsID
                        where a.cChildCode is null
                        and b.cDefine10 in ("+condition+") ";
                        sql+=@"union all
                        select g.cDefine10 ccode,a.cInvCode invcode,c.cInvName invname,f.cbdefine21 reqId,
                        0 ordnum,a.iQuantity outnum,0 billnum,
                        0 ordamt,a.iSum outamt,0 billamt
                        from DispatchLists a
                        inner join DispatchList b on a.DLID=b.DLID
                        inner join inventory c on a.cInvCode=c.cInvCode
                        inner join DispatchLists_extradefine d on a.iDLsID=d.iDLsID
                        inner join SO_SODetails e on a.iSOsID=e.iSOsID
                        inner join SO_SODetails_extradefine f on e.iSOsID=f.iSOsID
                        inner join SO_SOMain g on e.ID=g.ID
                        where a.cChildCode is null
                        and g.cDefine10 in ("+condition+") ";
                        sql += @"union all
                        select g.cDefine10 ccode,a.cInvCode invcode,c.cInvName invname,f.cbdefine21 reqId,
                        0 ordnum,0 outnum,a.iQuantity billnum,
                        0 ordamt,0 outamt,a.iSum billamt
                        from SaleBillVouchs a
                        inner join SaleBillVouch b on a.SBVID=b.SBVID
                        inner join inventory c on a.cInvCode=c.cInvCode
                        inner join SaleBillVouchs_extradefine d on a.AutoID=d.AutoID
                        inner join SO_SODetails e on a.iSOsID=e.iSOsID
                        inner join SO_SODetails_extradefine f on e.iSOsID=f.iSOsID
                        inner join SO_SOMain g on e.ID=g.ID
                        where a.cChildCode is null
                        and g.cDefine10 in ("+condition+")";
                        sql += @") a group by ccode,invcode,invname,reqId";
                        sql = sql.Replace("\r\n", " ");
                DataTable dt = Ufdata.getDatatableFromSql(m_ologin.UfDbName,  sql+ "");
                foreach (DataRow dr in dt.Rows)
                {
                    SNInfoData data = new SNInfoData();
                    data.invcode = dr["invcode"].ToString();
                    data.invname = dr["invname"].ToString();
                    data.ccode = dr["ccode"].ToString();
                    data.reqId = dr["reqId"].ToString();
                    data.ordnum = Convert.ToDecimal(dr["ordnum"]);
                    data.outnum = Convert.ToDecimal(dr["outnum"]);
                    data.billnum = Convert.ToDecimal(dr["billnum"]);
                    data.ordamt = Convert.ToDecimal(dr["ordamt"]);
                    data.outamt = Convert.ToDecimal(dr["outamt"]);
                    data.billamt = Convert.ToDecimal(dr["billamt"]);
                    result.datas.Add(data);
                }


            }
            return result; ;
        }
    }
}