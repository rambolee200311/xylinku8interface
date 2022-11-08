using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.DRInfo;
using XylinkU8Interface.Models.Result;
using System.Data;
using System.Data.OleDb;
namespace XylinkU8Interface.UFIDA
{
    /*
     * 接口根据CRM销售订单的req_id，查询母件产品编码，母件产品名称、母件已入库数量、入库单号
     */
    public class DRInfoEntity
    {
        public static DRInfo GetResult(DRInfoQuery lq)
        {
            DRInfo drInfo = new DRInfo();
            drInfo.companycode = lq.companycode;
            try
            {
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(lq.companycode);
                drInfo.datas = new List<DRInfoData>();
                //            string strSql = @"select a.cDefine10 ccode,a.cDLCode  u8code,b.cInvCode invcode,d.cInvName invname ,b.fOutQuantity outnum,c.cbdefine21 req_id,b.cParentCode,b.cChildCode
                //                            ,b.iquotedprice,b.itaxunitprice,b.isum
                //                            from  DispatchList a inner join DispatchLists b on a.DLID=b.DLID
                //                            inner join DispatchLists_extradefine c on b.iDLsID=c.iDLsID
                //                            inner join inventory d on b.cInvCode=d.cInvCode
                //                            where b.cChildCode is null and a.bReturnFlag=1 and c.cbdefine21=?";
                string strSql = @"select a.cDefine10 ccode,a.cDLCode  u8code,b.cInvCode invcode,d.cInvName invname ,b.fOutQuantity outnum,c.cbdefine21 req_id,b.cParentCode,b.cChildCode
                                ,b.iquotedprice,b.itaxunitprice,b.isum
                                from  DispatchList a inner join DispatchLists b on a.DLID=b.DLID
                                inner join SO_SODetails_extradefine c on b.iSOsID=c.iSOsID
                                inner join inventory d on b.cInvCode=d.cInvCode
                                where b.cChildCode is null and a.bReturnFlag=1 and c.cbdefine21=?";
                foreach (DRInfoQueryCode diqCode in lq.reqids)
                {
                    strSql = @"select a.cDefine10 ccode,a.cDLCode  u8code,b.cInvCode invcode,d.cInvName invname ,isnull(b.fOutQuantity,0) outnum,c.cbdefine21 req_id,b.cParentCode,b.cChildCode
                                ,isnull(b.iquotedprice,0) iquotedprice,isnull(b.itaxunitprice,0) itaxunitprice,isnull(b.isum,0) isum
                                from  DispatchList a inner join DispatchLists b on a.DLID=b.DLID
                                inner join SO_SODetails_extradefine c on b.iSOsID=c.iSOsID
                                inner join inventory d on b.cInvCode=d.cInvCode
                                where b.cChildCode is null and a.bReturnFlag=1 and c.cbdefine21=?";
                    List<Param> myParams = new List<Param>();
                    Param param = new Param();
                    param.paramname = "@cbdefine21";
                    param.paramtype = OleDbType.VarChar;
                    param.paramvalue = diqCode.reqid;
                    myParams.Add(param);
                    DataTable dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, myParams);
                    LogHelper.WriteLog(typeof(DRInfoEntity), strSql);
                    LogHelper.WriteLog(typeof(DRInfoEntity), JsonHelper.ToJson(myParams));
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        DRInfoData dinfoData = new DRInfoData();
                        //dinfoData.ccode = dr["ccode"].ToString();
                        dinfoData.u8code = dr["u8code"].ToString();
                        dinfoData.invcode = dr["invcode"].ToString();
                        dinfoData.invname = dr["invname"].ToString();
                        dinfoData.outnum = Convert.ToDecimal(dr["outnum"]);
                        dinfoData.iquoteprice = Convert.ToDecimal(dr["iquotedprice"]);
                        dinfoData.itaxunitprice = Convert.ToDecimal(dr["itaxunitprice"]);
                        dinfoData.isum = Convert.ToDecimal(dr["isum"]);
                        dinfoData.req_id = dr["req_id"].ToString();


                        //strSql = "select a.cDefine10 ccode,a.cDLCode  u8code,b.cInvCode invcode,d.cInvName invname ,b.fOutQuantity outnum,c.cbdefine21 req_id,b.cParentCode,b.cChildCode"
                        //        + " from  DispatchList a inner join DispatchLists b on a.DLID=b.DLID 
                        //        inner join DispatchLists_extradefine c on b.iDLsID=c.iDLsID"
                        //        + " inner join inventory d on b.cInvCode=d.cInvCode"
                        //        + " where a.bReturnFlag=1 and  b.cChildCode=?";
                        strSql = @"select a.cDefine10 ccode,a.cDLCode  u8code,b.cInvCode invcode,d.cInvName invname ,isnull(b.fOutQuantity,0) outnum,c.cbdefine21 req_id,b.cParentCode,b.cChildCode
                                ,isnull(b.iquotedprice,0) iquotedprice,isnull(b.itaxunitprice,0) itaxunitprice,isnull(b.isum,0) isum
                                from  DispatchList a inner join DispatchLists b on a.DLID=b.DLID
                                inner join SO_SODetails_extradefine c on b.iSOsID=c.iSOsID
                                inner join inventory d on b.cInvCode=d.cInvCode
                                where b.cParentCode is null and a.bReturnFlag=1 and b.cChildCode=? and  c.cbdefine21=?";
                        List<Param> yourParams = new List<Param>();
                        //yourParams = new List<Param>();
                        Param param1 = new Param();
                        param1.paramname = "@cChildCode";
                        param1.paramtype = OleDbType.VarChar;
                        param1.paramvalue = dr["cParentCode"].ToString();
                        yourParams.Add(param1);
                        Param param2 = new Param();
                        param2.paramname = "@cbdefine21";
                        param2.paramtype = OleDbType.VarChar;
                        param2.paramvalue = diqCode.reqid;
                        yourParams.Add(param2);
                        DataTable dtChild = null;
                        dtChild = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql, yourParams);
                        LogHelper.WriteLog(typeof(DRInfoEntity), strSql);
                        LogHelper.WriteLog(typeof(DRInfoEntity), JsonHelper.ToJson(yourParams));
                        dinfoData.children = new List<DRInfoDataChild>();
                        foreach (DataRow drChild in dtChild.Rows)
                        {
                            DRInfoDataChild drInfoDataChild = new DRInfoDataChild();
                            drInfoDataChild.invcode = drChild["invcode"].ToString();
                            drInfoDataChild.invname = drChild["invname"].ToString();
                            drInfoDataChild.outnum = Convert.ToDecimal(drChild["outnum"]);
                            dinfoData.children.Add(drInfoDataChild);
                        }

                        drInfo.datas.Add(dinfoData);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(DRInfoEntity), ex.Message);
            }
            return drInfo;

        }
    }
}