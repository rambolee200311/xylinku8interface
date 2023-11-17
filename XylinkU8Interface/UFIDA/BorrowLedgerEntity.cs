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
        /*
         *20230924
         *BorrowLedge1 首次同步无上线时间、非首次同步有上线时间，返回待归还的借出和归还数据 
         */
        public static OutMain getBorrowLedger1Entity(InMain inMain)
        {
            OutMain outMain = new OutMain();
            outMain.datas = new List<OutData>();
            DataTable dtPaged = null;
            DataTable dtID = null;
            outMain.companycode = inMain.companycode;
            string selectSql = "";
            string whereSql = "";
            List<ClassID> listClassID = new List<ClassID>();

            try
            {

                //登录u8
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode.ToString());

                if (m_ologin == null)
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), inMain.companycode + "账套登录失败！");
                    return outMain;
                }
                selectSql = "select distinct a.ID from HY_DZ_BorrowOut a inner join  HY_DZ_BorrowOuts b on a.ID=b.ID";

                string innerSql = @" inner join (select ID,AutoID,sum(outqty) outqty,sum(inqty) inqty from
                                (select a.ID,a.AutoID,sum(isnull(c.iquantity,0)) outqty,0 inqty from HY_DZ_BorrowOuts a left join RdRecords09 c on a.AutoID=c.iDebitIDs 
                                where c.id in (select id from RdRecord09 where isnull(dVeriDate,'1900-01-01')!='1900-01-01' )
                                group by a.ID,a.AutoID
                                union all
                                select b.ID,a.UpAutoID AutoID,0 outqty,sum(isnull(c.iquantity,0)) inqty from HY_DZ_BorrowOutBacks a left join  HY_DZ_BorrowOuts b on a.UpAutoID=b.AutoID left join RdRecords08 c on a.AutoID=c.iDebitIDs 
                                where c.id in (select id from RdRecord08 where isnull(dVeriDate,'1900-01-01')!='1900-01-01' )
                                group by b.ID,a.UpAutoID) tsum
                                group by ID,AutoID
                                having sum(outqty)!=sum(inqty)) c on a.ID=c.ID and b.AutoID=C.AutoID and isnull(a.dVeriDate,'1900-01-01')!='1900-01-01'";

                selectSql += innerSql;

                selectSql += " where 1=1";
                //selectSql += " and  isnull(b.iqtyout,0)-isnull(b.iqtyback,0)!=0";
                List<Param> myParams = new List<Param>();
                //ctype
                if (String.IsNullOrEmpty(inMain.ctype))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "cType不能为空！");
                    return outMain;
                }
                whereSql = " and a.cType=?";
                Param param1 = new Param();
                param1.paramname = "@ctype";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = inMain.ctype.ToString();
                myParams.Add(param1);
                //首次同步和上线时间
                if ((inMain.firstSync == null) && (string.IsNullOrEmpty(inMain.onlineTime)))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "首次同步和上线时间不能同时为空！");
                    return outMain;
                }
                //首次同步

                if (!inMain.firstSync)//非首次同步
                {
                    if (inMain.onlineTime == null)
                    {
                        outMain.companycode = inMain.companycode;
                        outMain.pages = 0;
                        outMain.total = 0;
                        LogHelper.WriteLog(typeof(BorrowLedgerEntity), "非首次同步，上线时间不能为空！");
                        return outMain;
                    }
                    whereSql += " and a.ddate<=?";
                    Param param2 = new Param();
                    param2.paramname = "@ddate";
                    param2.paramtype = OleDbType.VarChar;
                    param2.paramvalue = inMain.onlineTime.ToString();
                    myParams.Add(param2);
                }

                selectSql = @"select a.ID,a.cType,a.cCODE,a.bObjectCode,a.ddate,a.cmemo,a.cMaker,a.cHandler,a.dVeriDate,a.iStatus,a.cdefine12,
                                    b.AutoID,b.cinvcode,c.cinvname,b.iquantity,d.cbdefine21,g.cRdName,a.cpersoncode,h.cPersonName
                                    from HY_DZ_BorrowOut a 
                                    inner join HY_DZ_BorrowOuts b on a.ID=b.ID
                                    inner join inventory c on b.cinvcode=c.cinvcode
                                    left join HY_DZ_BorrowOuts_extradefine d on b.AutoID=d.AutoID 
									left join RdRecords09 e on e.iDebitIDs=b.AutoID
									left join RdRecord09 f on e.ID=f.ID
									left join Rd_Style g on f.cRdCode=g.cRdCode
									left join Person h on a.cpersoncode=h.cPersonCode 
                                    where a.ID in(" + selectSql + whereSql + ") and isnull(a.dVeriDate,'1900-01-01')!='1900-01-01'";
                DataTable[] dtResult = new DataTable[1];
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult: " + selectSql);
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult params: " + JsonHelper.ToJson(myParams));
                dtResult[0] = Ufdata.getDatatableFromSql(m_ologin.UfDbName, selectSql, myParams);
                int size = 1;
                if ((inMain.size != null) && (inMain.size != 0))
                {
                    size = inMain.size;
                }
                outMain.total = dtResult[0].Rows.Count;
                outMain.pages = (dtResult[0].Rows.Count + size - 1) / size;

                DataTable dtResultPaged = Ufdata.getPagedTable(dtResult[0], inMain.current, size);
                OutMain outMainRe = getOutMain(m_ologin, dtResultPaged, inMain, outMain);
                return outMainRe;
            }
            catch (Exception ex)
            {
                outMain.companycode = inMain.companycode;
                outMain.pages = 0;
                outMain.total = 0;
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowLedger1Entity exception:" + ex.Message);
                return outMain;
            }

            return outMain;
        }
        /*
         *20230924
         *BorrowLedge2 非首次同步,有同步上线时间，返回上线时间后全部借出和归还数据
         */
        public static OutMain getBorrowLedger2Entity(InMain inMain)
        {
            OutMain outMain = new OutMain();
            outMain.datas = new List<OutData>();
            DataTable dtPaged = null;
            DataTable dtID = null;
            outMain.companycode = inMain.companycode;
            string selectSql = "";
            string whereSql = "";
            List<ClassID> listClassID = new List<ClassID>();

            try
            {

                //登录u8
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode.ToString());

                if (m_ologin == null)
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), inMain.companycode + "账套登录失败！");
                    return outMain;
                }
                selectSql = "select a.ID from HY_DZ_BorrowOut a where 1=1";
                List<Param> myParams = new List<Param>();
                //ctype
                if (String.IsNullOrEmpty(inMain.ctype))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "cType不能为空！");
                    return outMain;
                }
                whereSql = " and a.cType=?";
                Param param1 = new Param();
                param1.paramname = "@ctype";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = inMain.ctype.ToString();
                myParams.Add(param1);

                //非首次同步                
                if (inMain.onlineTime == null)
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "非首次同步，上线时间不能为空！");
                    return outMain;
                }

                whereSql += " and a.ddate>?";
                Param param2 = new Param();
                param2.paramname = "@ddate";
                param2.paramtype = OleDbType.VarChar;
                param2.paramvalue = inMain.onlineTime.ToString();
                myParams.Add(param2);

                //借用借出表

                DataTable[] dtResult = new DataTable[1];
                selectSql = @"select a.ID,a.cType,a.cCODE,a.bObjectCode,a.ddate,a.cmemo,a.cMaker,a.cHandler,a.dVeriDate,a.iStatus,a.cdefine12,
                                b.AutoID,b.cinvcode,c.cinvname,b.iquantity,d.cbdefine21,g.cRdName,a.cpersoncode,h.cPersonName
                                from HY_DZ_BorrowOut a 
                                inner join HY_DZ_BorrowOuts b on a.ID=b.ID
                                inner join inventory c on b.cinvcode=c.cinvcode
                                left join HY_DZ_BorrowOuts_extradefine d on b.AutoID=d.AutoID 
							    left join RdRecords09 e on e.iDebitIDs=b.AutoID
							    left join RdRecord09 f on e.ID=f.ID
							    left join Rd_Style g on f.cRdCode=g.cRdCode
							    left join Person h on a.cpersoncode=h.cPersonCode 
                                where a.ID in (" + selectSql + whereSql + ") and isnull(a.dVeriDate,'1900-01-01')!='1900-01-01'";
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult: " + selectSql);
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult params: " + JsonHelper.ToJson(myParams));
                 dtResult[0] = Ufdata.getDatatableFromSql(m_ologin.UfDbName, selectSql, myParams);
                    
                int size = 1;
                if ((inMain.size != null) && (inMain.size != 0))
                {
                    size = inMain.size;
                }
                outMain.total = dtResult[0].Rows.Count;
                outMain.pages = (dtResult[0].Rows.Count + size - 1) / size;

                DataTable dtResultPaged = Ufdata.getPagedTable(dtResult[0], inMain.current, size);
                OutMain outMainRe = getOutMain(m_ologin, dtResultPaged, inMain, outMain);
                return outMainRe;
            }
            catch (Exception ex)
            {
                outMain.companycode = inMain.companycode;
                outMain.pages = 0;
                outMain.total = 0;
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowLedger2Entity exception:" + ex.Message);
                return outMain;
            }

            return outMain;
        }
        /*
         *20230924
         *BorrowLedge3 单据条件查询，返回符合条件的全部借出和归还数据
         */
        public static OutMain getBorrowLedger3Entity(InMain inMain)
        {
            OutMain outMain = new OutMain();
            outMain.datas = new List<OutData>();
            DataTable dtPaged = null;
            DataTable dtID = null;
            outMain.companycode = inMain.companycode;
            string selectSql = "";
            string whereSql = "";
            List<ClassID> listClassID = new List<ClassID>();

            try
            {

                //登录u8
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode.ToString());

                if (m_ologin == null)
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), inMain.companycode + "账套登录失败！");
                    return outMain;
                }
                selectSql = "select a.ID,b.AutoID from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on a.ID=b.ID where 1=1 and isnull(a.dVeriDate,'1900-01-01')!='1900-01-01'";
                List<Param> myParams = new List<Param>();
                //ctype
                if (String.IsNullOrEmpty(inMain.ctype))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "cType不能为空！");
                    return outMain;
                }
                whereSql = " and a.cType=?";
                Param param1 = new Param();
                param1.paramname = "@ctype";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = inMain.ctype.ToString();
                myParams.Add(param1);

                //非首次同步                
                if ((inMain.u8Codes==null)&&(inMain.u8RowIds==null))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "非首次同步，单据号和⾏ID不能同时为空！");
                    return outMain;
                }
                //// U8借出借⽤单-单号（查询参数，⾮必填）
                whereSql += " and (";
                if (inMain.u8Codes != null)
                {
                    if (inMain.u8Codes.Count > 0)
                    {
                        whereSql += " (1=0 ";
                        foreach (InU8Code inU8Code in inMain.u8Codes)
                        {
                            whereSql += " or a.cCODE='" + inU8Code.u8Code + "'";
                        }
                        whereSql += ") or ";
                    }
                }
                
                //// U8借出借⽤单-⾏ID（查询参数，⾮必填）
                if (inMain.u8RowIds != null)
                {
                    if (inMain.u8RowIds.Count > 0)
                    {
                        whereSql += " (1=0 ";
                        foreach (InU8RowID inU8Rowid in inMain.u8RowIds)
                        {
                            whereSql += " or b.AutoID='" + inU8Rowid.u8RowId + "'";
                        }
                        whereSql += ") or";
                    }
                }
                whereSql += " 1=0 )";
                

                dtID = Ufdata.getDatatableFromSql(m_ologin.UfDbName, selectSql + whereSql, myParams);

                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult: " + selectSql + whereSql);
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getDtResult params: " + JsonHelper.ToJson(myParams));

                //借出借用主表
                if ((dtID == null) || (dtID.Rows.Count <= 0))
                {
                    outMain.companycode = inMain.companycode;
                    outMain.pages = 0;
                    outMain.total = 0;
                    LogHelper.WriteLog(typeof(BorrowLedgerEntity), "返回数据为空！");
                    return outMain;
                }
                else
                {
                    foreach (DataRow drID in dtID.Rows)
                    {
                        //返回全部单据
                        ClassID clsID = new ClassID();
                        clsID.ID = drID["ID"].ToString();
                        clsID.AutoID = drID["AutoID"].ToString();
                        listClassID.Add(clsID);

                    }
                }


                //借用借出表

                DataTable[] dtResult = new DataTable[listClassID.Count];
                int iRow = 0;
                foreach (ClassID clsID in listClassID)
                {
                    selectSql = @"select a.ID,a.cType,a.cCODE,a.bObjectCode,a.ddate,a.cmemo,a.cMaker,a.cHandler,a.dVeriDate,a.iStatus,a.cdefine12,
                                    b.AutoID,b.cinvcode,c.cinvname,b.iquantity,d.cbdefine21,g.cRdName,a.cpersoncode,h.cPersonName
                                    from HY_DZ_BorrowOut a 
                                    inner join HY_DZ_BorrowOuts b on a.ID=b.ID
                                    inner join inventory c on b.cinvcode=c.cinvcode
                                    left join HY_DZ_BorrowOuts_extradefine d on b.AutoID=d.AutoID 
							        left join RdRecords09 e on e.iDebitIDs=b.AutoID
							        left join RdRecord09 f on e.ID=f.ID
							        left join Rd_Style g on f.cRdCode=g.cRdCode
							        left join Person h on a.cpersoncode=h.cPersonCode 
                                    where isnull(a.dVeriDate,'1900-01-01')!='1900-01-01' and a.ID=" + clsID.ID + " and b.AutoID=" + clsID.AutoID;
                    dtResult[iRow] = Ufdata.getDatatableFromSql(m_ologin.UfDbName, selectSql);// + whereSql, myParams);
                    //合并dtResult
                    if (iRow != 0)
                    {
                        if (dtResult[iRow] != null)
                        {
                            if (dtResult[iRow].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtResult[iRow].Rows)
                                {
                                    dtResult[0].ImportRow(dr);
                                }
                            }
                        }
                    }
                    iRow++;
                }
                int size = 1;
                if ((inMain.size != null) && (inMain.size != 0))
                {
                    size = inMain.size;
                }
                outMain.total = dtResult[0].Rows.Count;
                outMain.pages = (dtResult[0].Rows.Count + size - 1) / size;

                DataTable dtResultPaged = Ufdata.getPagedTable(dtResult[0], inMain.current, size);
                OutMain outMainRe = getOutMain(m_ologin, dtResultPaged, inMain, outMain);
                return outMainRe;
            }
            catch (Exception ex)
            {
                outMain.companycode = inMain.companycode;
                outMain.pages = 0;
                outMain.total = 0;
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowLedger2Entity exception:" + ex.Message);
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
            //20231115 add
            tempTable.Columns.Add("u8Presale", Type.GetType("System.Boolean"));// U8预售出库（如果该借出借⽤单对应的其他出库单的出库类别存在“预售机借⽤”，则为true；否则为false
            tempTable.Columns.Add("personCode", Type.GetType("System.String"));// U8借出借⽤单的业务员编码
            tempTable.Columns.Add("personName", Type.GetType("System.String")); // U8借出借⽤单的业务员名称

            return tempTable;
        }

        /*
         * 取得 U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
         */
        private static Decimal getBorrowNum(string autoID, string UfDbName)
        {
            Decimal result = 0;
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select a.AutoID,isnull(a.iquantity,0) iQuantity from RdRecords09 a inner join RdRecord09 b on a.ID=b.ID where 1=1 and isnull(b.dVeriDate,'1900-01-01')!='1900-01-01'";
                string whereSql = "";

                //ctype
                whereSql += " and a.iDebitIDs=?";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);
                /*
                whereSql += " and b.dDate<=?";
                Param param2 = new Param();
                param2.paramname = "@dDate";
                param2.paramtype = OleDbType.VarChar;
                param2.paramvalue = dDate;
                myParams.Add(param2);
                */
                //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowNum: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)
                        {
                            result += Convert.ToDecimal(drResult["iQuantity"]);
                        }
                    }
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
        private static Decimal getReturnNum(string autoID, string UfDbName)
        {
            Decimal result = 0;
            try
            {
                DataTable dtResult = null;
                List<Param> myParams = new List<Param>();
                string selectSql = "select a.AutoID,isnull(a.iquantity,0) iQuantity from RdRecords08 a inner join RdRecord08 b on a.ID=b.ID where 1=1 and isnull(b.dVeriDate,'1900-01-01')!='1900-01-01'";
                string whereSql = "";

                //ctype
                whereSql += " and a.iDebitIDs in (select AutoID from HY_DZ_BorrowOutBacks where UpAutoID=?)";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);
                /*
                whereSql += " and b.ddate<=?";
                Param param2 = new Param();
                param2.paramname = "@dDate";
                param2.paramtype = OleDbType.VarChar;
                param2.paramvalue = dDate;
                myParams.Add(param2);
                */
                //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getReturnNum: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)
                        {
                            result += Convert.ToDecimal(drResult["iQuantity"]);
                        }
                    }
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
                string selectSql = "select iVouchsID,cInvSN from ST_SNDetail_OtherOut where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and iVouchsID in (select AutoID from RdRecords09 where iDebitIDs=?)";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);


                //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getBorrowSncodes: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)
                        {
                            OutSnCode item1 = new OutSnCode();
                            item1.sncode = drResult["cInvSN"].ToString();
                            list1.Add(item1);
                        }
                    }
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
                string selectSql = "select iVouchsID,cInvSN  from ST_SNDetail_OtherIN where 1=1";
                string whereSql = "";

                //ctype
                whereSql += " and iVouchsID in (select AutoID from RdRecords08 where iDebitIDs in (select AutoID from HY_DZ_BorrowOutBacks where UpAutoID=?))";
                Param param1 = new Param();
                param1.paramname = "@autoID";
                param1.paramtype = OleDbType.VarChar;
                param1.paramvalue = autoID.ToString();
                myParams.Add(param1);


                //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getReturnSncodes: " + selectSql + whereSql);
                dtResult = Ufdata.getDatatableFromSql(UfDbName, selectSql + whereSql, myParams);
                if (dtResult != null)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)
                        {
                            OutSnCode item1 = new OutSnCode();
                            item1.sncode = drResult["cInvSN"].ToString();
                            list1.Add(item1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "getReturnSncodes exception:" + ex.Message);
                return null;
            }
            return list1;
        }

        /*
         *20230920
         *检查是否全部归还
          */

        /*
       private static bool checkReturnAll(string UfDbName, string ID)
        {
            bool result = true;
            string strSql = "";
            try
            {
                //借出数据
                strSql = @"select a.AutoID,sum(isnull(c.iquantity,0)) iquantity from HY_DZ_BorrowOuts a 
                    left join RdRecords09 c on a.AutoID=c.iDebitIDs
                    where a.ID=" + ID + " group by a.AutoID";
                //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "checkReturnAll-JYCK: " + strSql);
                DataTable dtJYCK = Ufdata.getDatatableFromSql(UfDbName, strSql);
                decimal qtyJYCK = 0;
                if (dtJYCK != null)
                {
                    if (dtJYCK.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtJYCK.Rows)
                        {
                            decimal qtyJYGH = 0;
                            qtyJYCK = Convert.ToDecimal(dr["iquantity"]);
                            //归还数据
                            strSql = @"select a.UpAutoID,sum(isnull(c.iquantity,0)) iquantity from HY_DZ_BorrowOutBacks a 
                            left join  HY_DZ_BorrowOuts b on a.UpAutoID=b.AutoID
                            left join RdRecords08 c on a.AutoID=c.iDebitIDs
                            where b.ID=" + ID + " and a.UpAutoID=" + dr["AutoID"].ToString() + " group by a.UpAutoID ";
                            //LogHelper.WriteLog(typeof(BorrowLedgerEntity), "checkReturnAll-JYGH: " + strSql);
                            DataTable dtJYGH = Ufdata.getDatatableFromSql(UfDbName, strSql);
                            if (dtJYGH != null)
                            {
                                if (dtJYGH.Rows.Count > 0)
                                {
                                    foreach (DataRow dr1 in dtJYGH.Rows)
                                    {
                                        qtyJYGH += Convert.ToDecimal(dr1["iquantity"]);
                                    }
                                }
                            }
                            if (qtyJYCK != qtyJYGH)
                            {
                                result = false;
                                return result;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowLedgerEntity), "checkReturnAll exception:" + ex.Message);
            }
            return result;
        }
        */
        

        /*
        *20230924
        *组合数据发送
        */
        private static OutMain getOutMain(U8Login.clsLogin m_ologin, DataTable dtResultPaged, InMain inMain, OutMain outMainTmp)
        {
            OutMain outMain = new OutMain();
            outMain = outMainTmp;
            DataTable tempTable = newTempTable();
            try
            {
                //写入临时表
                foreach (DataRow drResult in dtResultPaged.Rows)
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

                    applyBorrowNum = Convert.ToDecimal(drResult["iquantity"]);
                    borrowNum = getBorrowNum(drResult["AutoID"].ToString(), m_ologin.UfDbName);
                    returnNum = getReturnNum(drResult["AutoID"].ToString(), m_ologin.UfDbName);

                    drTemp["applyBorrowNum"] = applyBorrowNum;// U8借出借⽤单-⾏⼦件产品申请借⽤数量
                    drTemp["borrowNum"] = borrowNum;// U8借出借⽤单-⾏⼦件产品实际出库数量（借⽤数量，不可以⼤于借出借⽤单产品⾏⼦件的申请借⽤数量）
                    drTemp["returnNum"] = returnNum;// U8借出借⽤单-⾏⼦件产品归还数量（归还数量）
                    drTemp["reqId"] = drResult["cbdefine21"].ToString();// 产品明细唯⼀标识（可能为空，⽆CRM试⽤申请单的情况）


                    drTemp["personCode"] = drResult["cpersoncode"].ToString();
                    drTemp["personName"] = drResult["cPersonName"].ToString();

                    drTemp["u8Presale"] = false;

                    if (drResult["cRdName"].ToString().IndexOf("预售机借⽤") > 0)
                    {
                        drTemp["u8Presale"] =true;
                    }

                    tempTable.Rows.Add(drTemp);


                }

                //组装data对象
                if (tempTable != null)
                {
                    foreach (DataRow drPaged in tempTable.Rows)
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

                        outData.borrowSncodes = getBorrowSncodes(drPaged["u8RowId"].ToString(), m_ologin.UfDbName);
                        outData.returnSncodes = getReturnSncodes(drPaged["u8RowId"].ToString(), m_ologin.UfDbName);


                        //20231115 add
                        outData.personCode = drPaged["personCode"].ToString();
                        outData.personName = drPaged["personName"].ToString();
                        outData.u8Presale = Convert.ToBoolean(drPaged["u8Presale"]);

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

    }
}