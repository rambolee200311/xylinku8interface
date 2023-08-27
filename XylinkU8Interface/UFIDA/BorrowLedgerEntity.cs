using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using XylinkU8Interface.Models.BorrowLedger;
using XylinkU8Interface.Helper;
namespace XylinkU8Interface.UFIDA
{
    public class BorrowLedgerEntity
    {
        public static OutMain getBorrowLedgerEntity(InMain inMain)
        {
            OutMain outMain = new OutMain();
            outMain.datas = new List<OutData>();
            DataTable dtPaged = null;
            string dDate = "2099-12-31";
            if (String.IsNullOrEmpty(inMain.ctype))
            {
                 outMain.companycode=inMain.companycode;
                 outMain.pages = 0;
                 outMain.total = 0;
                 LogHelper.WriteLog(typeof(BorrowLedgerEntity), "cType为空！");
                 return outMain;
            }

            try
            {

                //登录u8
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity();
                if(m_ologin==null){
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), inMain.companycode+"账套登录失败！");
                    return outMain;
                }

                //借用借出表
                DataTable tempTable = newTempTable();
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = @"select a.ID,a.cType,a.cCODE,a.bObjectCode,a.ddate,a.cmemo,a.cMaker,a.cHandler,a.dVeriDate,a.iStatus,a.cdefine12,
                                    b.AutoID,b.cinvcode,c.cinvname,b.iquantity,d.cbdefine21
                                    from HY_DZ_BorrowOut a 
                                    inner join HY_DZ_BorrowOuts b on a.ID=b.ID
                                    inner join inventory c on b.cinvcode=c.cinvcode
                                    left join HY_DZ_BorrowOuts_extradefine d on b.AutoID=d.AutoID where 1=1";

                string whereSql="";

                //ctype
                whereSql += " and a.cType=?";
                Param param1 = new Param();
                param1.paramname = "@ctype";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = inMain.ctype.ToString();
                myParams.Add(param1);

                // 是否是⾸次同步，⾸次同步: true；后续定期同步: false
                if (!inMain.firstSync)
                {

                    //// 上线时间，⾸次同步可不传（⾮必填）
                    if (!string.IsNullOrEmpty(inMain.onlineTime))
                    {
                        dDate = inMain.onlineTime;
                    }
                    whereSql += " and a.ddate<=?";
                    Param param2 = new Param();
                    param2.paramname = "@ctype";
                    param2.paramtype = OleDbType.VarChar;                    
                    param2.paramvalue = dDate;
                    myParams.Add(param2);
                    //// U8借出借⽤单-单号（查询参数，⾮必填）
                    if (inMain.u8Codes != null)
                    {
                        if (inMain.u8Codes.Count > 0)
                        {
                            whereSql += " and (1=0 ";
                            foreach (InU8Code inU8Code in inMain.u8Codes)
                            {
                                whereSql += " or a.cCODE='" + inU8Code.u8Code + "'";
                            }
                            whereSql += ")";
                        }
                    }

                    //// U8借出借⽤单-⾏ID（查询参数，⾮必填）
                    if (inMain.u8RowIds != null)
                    {
                        if (inMain.u8RowIds.Count > 0)
                        {
                            whereSql += " and (1=0 ";
                            foreach (InU8RowID inU8Rowid in inMain.u8RowIds)
                            {
                                whereSql += " or b.AutoID='" + inU8Rowid.u8RowId + "'";
                            }
                            whereSql += ")";
                        }
                    }
                }


                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(m_ologin.UfDbName, selectSql + whereSql, myParams);

                //写入临时表
                foreach (DataRow drResult in dtResult.Rows)
                {
                    DataRow drTemp = tempTable.NewRow();
                    Decimal applyBorrowNum = 0;
                    Decimal borrowNum = 0;
                    Decimal returnNum = 0;

                    drTemp["u8RowId"] = drResult["AutoID"].ToString();// U8借出借⽤单-产品⼦件⾏ID
                    drTemp["u8Code"] = drResult["cCODE"].ToString();// U8借出借⽤单-单号
                    drTemp["u8Date"] = drResult["ddate"].ToString(); // U8借出借⽤单-单据⽇期
                    drTemp["u8ExtCode"] = drResult["cdefine12"].ToString();// U8借出借⽤单-外部订单号（CRM试⽤单号，可能为空）
                    drTemp["invcode"] = drResult["cinvcode"].ToString();// U8借出借⽤单-⾏⼦件产品编码
                    drTemp["invname"] = drResult["cinvname"].ToString();// U8借出借⽤单-⾏⼦件产品名称

                    applyBorrowNum=Convert.ToDecimal(drResult["iquantity"]);
                    borrowNum = getBorrowNum(drResult["AutoID"].ToString(),dDate, m_ologin.UfDbName);
                    returnNum = getReturnNum(drResult["AutoID"].ToString(),dDate, m_ologin.UfDbName);

                    drTemp["applyBorrowNum"] = applyBorrowNum;// U8借出借⽤单-⾏⼦件产品申请借⽤数量
                    drTemp["borrowNum"] = borrowNum;// U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
                    drTemp["returnNum"] = returnNum;// U8借出借⽤单-⾏⼦件产品归还数量（归还数量）
                    drTemp["reqId"] = drResult["cbdefine21"].ToString();// 产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）
                    tempTable.Rows.Add(drTemp);
                }

                //对临时数据表分页
                if (tempTable != null)
                {
                    outMain.total = tempTable.Rows.Count;
                    outMain.pages = (tempTable.Rows.Count + inMain.size - 1) / inMain.size;
                    dtPaged = Ufdata.getPagedTable(dtResult, inMain.current, inMain.size);
                }

                //组装data对象
                if (dtPaged != null)
                {
                    foreach (DataRow drPaged in dtPaged.Rows)
                    {
                        OutData outData = new OutData();
                        outData.borrowSncodes = new List<OutSnCode>();
                        outData.returnSncodes = new List<OutSnCode>();

                        outData.u8RowId = drPaged["u8RowId"].ToString();
                        outData.u8Code = drPaged["u8Code"].ToString();
                        outData.u8Date = drPaged["u8Date"].ToString();
                        outData.u8ExtCode = drPaged["u8ExtCode"].ToString();
                        outData.invcode = drPaged["invcode"].ToString();
                        outData.invname = drPaged["invname"].ToString();
                        outData.reqId = drPaged["reqId"].ToString();

                        outData.applyBorrowNum = Convert.ToDecimal(drPaged["applyBorrowNum"]);
                        outData.borrowNum = Convert.ToDecimal(drPaged["borrowNum"]);
                        outData.returnNum = Convert.ToDecimal(drPaged["returnNum"]);

                        outData.borrowSncodes = getBorrowSncodes(drPaged["u8RowId"].ToString(),m_ologin.UfDbName);
                        outData.returnSncodes = getReturnSncodes(drPaged["u8RowId"].ToString(), m_ologin.UfDbName);

                        outMain.datas.Add(outData);
                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), ex);
                outMain.companycode = inMain.companycode;
                outMain.pages = 0;
                outMain.total = 0;
                return outMain;
            }

            return outMain;
        }
        /*
         *创建临时表 
         */
        private static DataTable newTempTable()
        {
            DataTable tempTable = new DataTable();
            tempTable.Columns.Add("u8RowId", Type.GetType("System.String"));// U8借出借⽤单-产品⼦件⾏ID
            tempTable.Columns.Add("u8Code", Type.GetType("System.String"));// U8借出借⽤单-单号
            tempTable.Columns.Add("u8Date", Type.GetType("System.String")); // U8借出借⽤单-单据⽇期
            tempTable.Columns.Add("u8ExtCode", Type.GetType("System.String")); // U8借出借⽤单-外部订单号（CRM试⽤单号，可能为空）
            tempTable.Columns.Add("invcode", Type.GetType("System.String"));// U8借出借⽤单-⾏⼦件产品编码
            tempTable.Columns.Add("invname", Type.GetType("System.String"));// U8借出借⽤单-⾏⼦件产品名称
            //public List<OutSnCode> borrowSncodes { get; set; }// U8借出借⽤单-⾏⼦件产品出库SN（是纳⼊序列号管理的才有值）
            //public List<OutSnCode> returnSncodes { get; set; }// U8借出借⽤单-⾏⼦件产品归还SN（是纳⼊序列号管理的才有值）
            tempTable.Columns.Add("applyBorrowNum", Type.GetType("System.Decimal")); // U8借出借⽤单-⾏⼦件产品申请借⽤数量
            tempTable.Columns.Add("borrowNum", Type.GetType("System.Decimal")); // U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
            tempTable.Columns.Add("returnNum", Type.GetType("System.Decimal"));// U8借出借⽤单-⾏⼦件产品归还数量（归还数量）
            tempTable.Columns.Add("reqId", Type.GetType("System.String")); // 产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）
            return tempTable;
        }



        /*
         * 取得 U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
         */
        private static Decimal getBorrowNum(string autoID, string dDate, string UfDbName)
        {
            Decimal result = 0;
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select a.AutoID,a.iQuantity from RdRecords09 a inner join RdRecord09 b on a.ID=b.ID where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and a.iDebitIDs=?";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);

                whereSql += " and b.dDate<=?";
                Param param2 = new Param();
                param2.paramname = "@dDate";
                param2.paramtype = OleDbType.VarChar;
                param2.paramvalue = dDate;
                myParams.Add(param2);

                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowNum: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                foreach (DataRow drResult in dtResult.Rows)
                {
                    result +=Convert.ToDecimal( drResult["iQuantity"]);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), ex);
                return 0;
            }

            return result;
        }
        /*
         * 取得 U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
         */
        private static Decimal getReturnNum(string autoID, string dDate, string UfDbName)
        {
            Decimal result = 0;
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select a.AutoID,a.iQuantity from RdRecords08 a inner join RdRecord08 b on a.ID=b.ID where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and a.iDebitIDs in (select AutoID from HY_DZ_BorrowOutBacks where UpAutoID=?)";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);

                whereSql += " and b.ddate<=?";
                Param param2 = new Param();
                param2.paramname = "@dDate";
                param2.paramtype = OleDbType.VarChar;
                param2.paramvalue = dDate;
                myParams.Add(param2);

                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getReturnNum: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                foreach (DataRow drResult in dtResult.Rows)
                {
                    result += Convert.ToDecimal(drResult["iQuantity"]);
                }


            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), ex);
                return 0;
            }
            return result;
        }

        /*
         * 取得 U8借出借⽤单-⾏⼦件产品出库SN（是纳⼊序列号管理的才有值
         */
        private static List<OutSnCode> getBorrowSncodes(string autoID, string UfDbName)
        {
            List<OutSnCode> list1 = new List<OutSnCode>();
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select iVouchsID,cInvSN from ST_SNDetail_OtherOut where where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and iVouchsID in (select AutoID from RdRecords09 where iDebitIDs=?)";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);


                LogHelper.WriteLog(typeof(BorrowLedgerEntity),"getBorrowSncodes: " +selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                foreach (DataRow drResult in dtResult.Rows)
                {
                    OutSnCode item1 = new OutSnCode();
                    item1.sncode = drResult["cInvSN"].ToString();
                    list1.Add(item1);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), ex);
                return null;
            }
            return list1;
        }
        /*
         * 取得 U8借出借⽤单-⾏⼦件产品归还SN（是纳⼊序列号管理的才有值）
         */
        private static List<OutSnCode> getReturnSncodes(string autoID, string UfDbName)
        {
            List<OutSnCode> list1 = new List<OutSnCode>();
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select iVouchsID,cInvSN  from ST_SNDetail_OtherIN where where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and iVouchsID in (select AutoID from RdRecords08 where iDebitIDs in (select AutoID from HY_DZ_BorrowOutBacks where UpAutoID=?))";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);


                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getReturnSncodes: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                foreach (DataRow drResult in dtResult.Rows)
                {
                    OutSnCode item1 = new OutSnCode();
                    item1.sncode = drResult["cInvSN"].ToString();
                    list1.Add(item1);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), ex);
                return null;
            }
            return list1;
        }
    }
}