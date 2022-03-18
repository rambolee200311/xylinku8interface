using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using XylinkU8Interface.Models.STInfo;
using XylinkU8Interface.Helper;
namespace XylinkU8Interface.UFIDA
{
    public class STInfoEntity
    {
        public static STInfo get_STInfo(STInfoQuery sq)
        {
            STInfo stInfo = new STInfo();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(sq.companycode);
            stInfo.companycode = sq.companycode;
            string strSql = "";
            DataTable dtResult = null;
            DataTable dtSN = null;
            stInfo.datas = new List<STInfoData>();
            stInfo.returndatas = new List<STInfoReturnData>();
            foreach (STInfoQueryCode code in sq.codes)
            {  
                #region//datas
                strSql = @"select a.ID,a.AutoID,c.cdefine12 ccode,c.cCODE u8code ,a.cInvCode invcode,e.cInvName invname,a.iQuantity num
                            ,a.cDefine22 excomp,a.cDefine23 exnum,d.cbdefine4 receiver,d.cbdefine5 recrmobi,d.cbdefine9 recraddress,d.cbdefine21 req_id 
                            from rdrecords09 a
                            inner join HY_DZ_BorrowOuts b on a.iDebitIDs=b.AutoID
                            inner join HY_DZ_BorrowOut c on b.id=c.id
                            inner join HY_DZ_BorrowOuts_extradefine d on b.AutoID=d.AutoID 
                            inner join Inventory e on a.cInvCode=e.cInvCode
                            where c.cdefine12=?";
                List<Param> myParams = new List<Param>();
                Param param = new Param();
                param.paramname = "@cbdefine21";
                param.paramtype = OleDbType.VarChar;
                param.paramvalue = code.ccode;
                myParams.Add(param);
                dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                if (dtResult != null)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        STInfoData data = new STInfoData();
                        data.sncodes = new List<STInfoData_sncodes>();
                        data.ccode = dr["ccode"].ToString();
                        data.u8code = dr["u8code"].ToString();
                        data.invcode = dr["invcode"].ToString();
                        data.invname = dr["invname"].ToString();
                        data.invcode = dr["invcode"].ToString();
                        data.num = Convert.ToDecimal(dr["num"]);
                        data.excomp = dr["excomp"].ToString();
                        data.exnum = dr["exnum"].ToString();
                        data.receiver = dr["receiver"].ToString();
                        data.recrmobi = dr["recrmobi"].ToString();
                        data.recraddress = dr["recraddress"].ToString();
                        data.req_id = dr["req_id"].ToString();

                        //otherout sn
                        strSql = @"select cInvSN from ST_SNDetail_OtherOut where iVouchID=? and iVouchsID=?";
                        List<Param> yourParams = new List<Param>();
                        Param param1 = new Param();
                        param1.paramname = "@iVouchID";
                        param1.paramtype = OleDbType.VarChar;
                        param1.paramvalue = dr["ID"].ToString();
                        yourParams.Add(param1);
                        Param param2 = new Param();
                        param2.paramname = "@iVouchsID";
                        param2.paramtype = OleDbType.VarChar;
                        param2.paramvalue = dr["AutoID"].ToString();
                        yourParams.Add(param2);
                        dtSN = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, yourParams);
                        if (dtSN != null)
                        {
                            foreach (DataRow drSN in dtSN.Rows)
                            {
                                STInfoData_sncodes sncode = new STInfoData_sncodes();
                                sncode.sncode = drSN["cInvSN"].ToString();
                                data.sncodes.Add(sncode);
                            }
                        }
                        stInfo.datas.Add(data);
                    }
                }
                #endregion


                #region//retrundatas
                strSql = @"select  a.ID,a.AutoID,a.cInvCode invcode,a.iQuantity num,g.cbdefine21 req_id
                            ,e.cdefine12 ccode,c.cCODE u8code,f.cInvName invname
                            from  rdrecords08 a 
                            inner join HY_DZ_BorrowOutBacks b on a.iDebitIDs=b.AutoID
                            inner join HY_DZ_BorrowOutBack c on b.ID=c.ID
                            inner join HY_DZ_BorrowOuts d on b.UpAutoID=d.AutoID
                            inner join HY_DZ_BorrowOut e on d.id=e.id
                            inner join Inventory f on a.cInvCode=f.cInvCode
                            inner join HY_DZ_BorrowOuts_extradefine g on d.AutoID=g.AutoID 
                            where e.cDefine12=?";

                dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                if (dtResult != null)
                {
                    foreach (DataRow drRetrun in dtResult.Rows)
                    {
                        STInfoReturnData returndata = new STInfoReturnData();
                        returndata.sncodes = new List<STInfoData_sncodes>();
                        returndata.ccode = drRetrun["ccode"].ToString();
                        returndata.u8code = drRetrun["u8code"].ToString();
                        returndata.invcode = drRetrun["invcode"].ToString();
                        returndata.invname = drRetrun["invname"].ToString();
                        returndata.invcode = drRetrun["invcode"].ToString();
                        returndata.num = Convert.ToDecimal(drRetrun["num"]);
                        //data.excomp = dr["excomp"].ToString();
                        //data.exnum = dr["exnum"].ToString();
                        //data.receiver = dr["receiver"].ToString();
                        //data.recrmobi = dr["recrmobi"].ToString();
                        //data.recraddress = dr["recraddress"].ToString();
                        returndata.req_id = drRetrun["req_id"].ToString();

                        //otherout sn
                        strSql = @"select cInvSN from ST_SNDetail_OtherIn where iVouchID=? and iVouchsID=?";
                        List<Param> hisParams = new List<Param>();
                        Param param3 = new Param();
                        param3.paramname = "@iVouchID";
                        param3.paramtype = OleDbType.VarChar;
                        param3.paramvalue = drRetrun["ID"].ToString();
                        hisParams.Add(param3);
                        Param param4 = new Param();
                        param4.paramname = "@iVouchsID";
                        param4.paramtype = OleDbType.VarChar;
                        param4.paramvalue = drRetrun["AutoID"].ToString();
                        hisParams.Add(param4);

                        dtSN = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, hisParams);
                        if (dtSN != null)
                        {
                            foreach (DataRow drSN1 in dtSN.Rows)
                            {
                                STInfoData_sncodes sncode = new STInfoData_sncodes();
                                sncode.sncode = drSN1["cInvSN"].ToString();
                                returndata.sncodes.Add(sncode);
                            }
                        }
                        stInfo.returndatas.Add(returndata);
                    }
                }
                #endregion
            }
            return stInfo;
        }
    }
}