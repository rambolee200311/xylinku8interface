using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.ISState;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    /*
     * 2022-08-08
     * 根据SN查询该SN的在库状态
     * lijianqiang
     */
    public class ISStateEntity
    {

        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;

            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            string strSql = @"select cInvCode,cInvSN,iSNState,iSNOperateCount from [dbo].[ST_SNState] ";
            strSql += " where a.cInvSN=?";


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
                    LogHelper.WriteLog(typeof(ISStateEntity), strSql);
                    LogHelper.WriteLog(typeof(ISStateEntity),JsonHelper.ToJson(myParams));
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                    if (dtResult != null)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            ClsInfoData infordata = new ClsInfoData();
                            infordata.sncode = dr["cInvSN"].ToString();
                            infordata.inStock = false;
                            if (dr["iSNState"].ToString() == "2")
                            {
                                infordata.inStock = true;
                            }
                            infor.datas.Add(infordata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(ISStateEntity), ex);
            }
            return infor;
        }
        
    }
}