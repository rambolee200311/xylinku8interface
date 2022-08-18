using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSState;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    public class OOSStateEntity
    {

        public static ClsInfo getInfo(ClsQuery query)
        {
            //20220728
            ClsInfo infor = new ClsInfo();
            infor.companycode = query.companycode;

            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(query.companycode);
            string strSql = @"select cHandler,dVeriDate,cdefine12 from rdrecord09 where cCode=?";         


            DataTable dtResult = null;
            infor.datas = new List<ClsInfoData>();
            try
            {
                foreach (ClsQueryCode code in query.codes)
                {
                    List<Param> myParams = new List<Param>();
                    Param param1 = new Param();
                    param1.paramname = "@code";
                    param1.paramtype = OleDbType.VarChar;
                    param1.paramvalue = code.code.ToString();
                    myParams.Add(param1);
                    LogHelper.WriteLog(typeof(OOSStateEntity), strSql);
                    LogHelper.WriteLog(typeof(OOSStateEntity), JsonHelper.ToJson(myParams));
                    dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);

                    ClsInfoData infordata = new ClsInfoData();
                    infordata.code = code.code;

                    if (dtResult != null)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            
                            infordata.ccode = dr["cdefine12"].ToString();

                            if (string.IsNullOrEmpty(dr["cHandler"].ToString()))
                            {
                                infordata.state = "1";// "开⽴";
                            }
                            else
                            {
                                infordata.state = "2";// "已审核";
                            }
                            
                        }
                    }
                    infor.datas.Add(infordata);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(OOSStateEntity), ex);
            }
            return infor;
        }
        
    }
}