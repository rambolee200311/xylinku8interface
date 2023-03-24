using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSSaleSNInfoTime;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    //2022-12 查询某个时间段销售出库的SN CRM←→U8
    public class OOSSaleSNInfoTimeEntity
    {
        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;
            infor.pages = 0;
            infor.total = 0;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            string strSql = @"select c.cCode u8outcode,c.dnverifytime u8outtime,a.cInvCode invcode,d.cInvName invname,a.cInvSN sncode,som.cSOCode u8code,dt.cDLCode u8invcode,som.cDefine10 u8extcode,
                            b.cDefine22 excomp,b.cDefine23 exnum,e.cbdefine4 receiver,e.cbdefine5 recrmobi,
                            replace(isnull(e.cbdefine6,''),'/','')+replace(isnull(e.cbdefine7,''),'/','')+replace(isnull(e.cbdefine8,''),'/','')+replace(isnull(e.cbdefine9,''),'/','') recraddress,
                            isnull(f.cbdefine21,'') reqId,case when isnull(a.cInvSN,'')!='' and b.iQuantity>0 then 1 else -1 end num 
                            from ST_SNDetail_SaleOut a
                            inner join RdRecords32 b on a.iVouchsID=b.AutoID 
                            inner join RdRecord32 c on b.ID=c.ID 
                            left join DispatchLists dts on b.iDLsID=dts.iDLsID
                            left join DispatchList dt on dts.DLID=dt.DLID
                            left join SO_SODetails sod on dts.iSOsID=sod.iSOsID
                            left join SO_SOMain som on sod.ID=som.ID
                            left join RdRecords32_extradefine e on e.AutoID=b.AutoID
							left join SO_SODetails_extradefine f on f.iSOsID=sod.iSOsID
                            inner join inventory d on a.cInvCode=d.cInvCode";
            strSql += " where c.dnverifytime>=? and c.dnverifytime<?";
            strSql += " order by c.dnverifytime,u8outcode";
            string dDates = "1980-01-01";
            string dDatee = "2049-12-31";
            
            DataTable dtResult = null;
            DataTable dtPaged = null;
            infor.datas = new List<ClsInfoData>();
            try
            {
                if (!string.IsNullOrEmpty(query.startTime))
                {
                    //dDates = Convert.ToDateTime(query.startTime).ToShortDateString();
                    dDates = Convert.ToDateTime(query.startTime).ToLongDateString();
                }
                if (!string.IsNullOrEmpty(query.endTime))
                {
                    //dDatee = Convert.ToDateTime(Convert.ToDateTime(query.endTime).ToShortDateString()).AddDays(1).ToShortDateString();
                    dDates = Convert.ToDateTime(query.endTime).ToLongDateString();
                }


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

                LogHelper.WriteLog(typeof(OOSSaleSNInfoTimeEntity), strSql);
                LogHelper.WriteLog(typeof(OOSSaleSNInfoTimeEntity), JsonHelper.ToJson(myParams));
                dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                if (dtResult != null)
                {
                    infor.total = dtResult.Rows.Count;
                    infor.pages = (dtResult.Rows.Count + query.size - 1) / query.size;
                    dtPaged = getPagedTable(dtResult, query.current, query.size);
                    if (dtPaged != null)
                    {
                        foreach (DataRow dr in dtPaged.Rows)
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

                            infordata.num = Convert.ToDecimal(dr["num"]);
                            infordata.excomp = dr["excomp"].ToString();
                            infordata.exnum = dr["exnum"].ToString();
                            infordata.receiver = dr["receiver"].ToString();
                            infordata.recemobi = dr["recrmobi"].ToString();
                            infordata.receaddress = dr["recraddress"].ToString();                            
                            infordata.reqId = dr["reqId"].ToString();
                            infor.datas.Add(infordata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OOSSaleSNInfoTimeEntity), ex);
            }
            return infor;
        }
        public static DataTable getPagedTable(DataTable dt, int currentPage, int size)
        {
            DataTable dtt;
            if (currentPage == 0)
            { dtt = dt; }
            dtt = dt.Copy();
            dtt.Clear();
            int rowbegin = (currentPage - 1) * size;
            int rowend = currentPage * size;
            if (rowbegin >= dt.Rows.Count)
            { dtt = dt; }
            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }

            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = dtt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                dtt.Rows.Add(newdr);
            }
            return dtt;
        }
    }
}