using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HYBorrowOut.Helper;
using HYBorrowOut.Models.BorrowReturn;
using HYBorrowOut.Models.Result;
using MSXML2;
/*
 * 20230903
 * lijianqiang
 * BorrowReturn 归还（归还、⽣成其他⼊库单并审核通过)
 */
namespace HYBorrowOut.UFIDA
{
    public class BorrowReturnEntity
    {
        public static OutMain Add_Borrow_Out_Back(InMain inMain)
        {
            OutMain outMain = new OutMain();
            string strResult = "";
            string strSql = "";
            bool bresult = false;
            bool insert = true;
            string errMsg = "";
            int iVouchID = 0;
            outMain.companycode = inMain.companycode;
            try
            {
                outMain.dataList = new List<OutData>();
                OutData outData = new OutData();
                #region //ready
                    U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode);
                    if (m_ologin == null)
                    {
                        strResult = "帐套" + inMain.companycode + "登录失败";
                        return getErrorOutMain(inMain.companycode,inMain.head.ccode, "1111", strResult);
                    }
                    //2021-10-18 重复导入控制
                    strSql="select ccode from HY_DZ_BorrowOutBack where cdefine12='" + inMain.head.ccode + "'";
                    string ccode = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                    if (!string.IsNullOrEmpty(ccode))
                    {
                        strResult = "借出归还单重复导入";
                        LogHelper.WriteLog(typeof(BorrowReturnEntity), strResult);
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "111", strResult);
                    }
                    HY_DZ_BorrowOutBack.clsBorrowOutSrvClass cosc = new HY_DZ_BorrowOutBack.clsBorrowOutSrvClass();
                    cosc.Init(m_ologin);
                    // bool bresult = cosc.MakeVouchFromBorrowOut(Convert.ToInt32(oristrvoid), ref errMsg, ref iVouchID);
                    MSXML2.IXMLDOMDocument2 ohead = new MSXML2.DOMDocument60();
                    MSXML2.IXMLDOMDocument2 obody = new MSXML2.DOMDocument60();
                    ohead.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowoutback_head.xml");
                    obody.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowoutback_body.xml");
                    ohead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                    obody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                #endregion

                #region //head
                    string ccuscode ="";
                    if (!string.IsNullOrEmpty(inMain.head.custName))
                    {
                        ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + inMain.head.custName + "'");
                    }
                    string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from person where cpersonname='" + inMain.head.personName + "'");  
                    string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + inMain.head.personName + "'");
                    string cwhcode = "";// Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + inMain.head.cwhname + "'");
                    if (!string.IsNullOrEmpty(ccuscode))
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = ccuscode;
                    }
                    //else
                    //{
                    //    strResult = inMain.head.custName + "客户档案不存在";
                    //    return getErrorOutMain(inMain.companycode, inMain.head.ccode, "222", strResult);
                    //}                    
                    if (!string.IsNullOrEmpty(cpersoncode))
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cpersoncode").text = cpersoncode;
                    }
                    else
                    {
                        strResult = inMain.head.personName + "人员档案不存在";
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "222", strResult);
                    }
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ddate").text =Convert.ToDateTime(inMain.head.ddate).ToShortDateString();
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("dmDate").text = Convert.ToDateTime(inMain.head.ddate).ToShortDateString();
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cmemo").text = "借出借用单(" + inMain.body[0].oriU8Code + ")生单 ";
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine12").text = inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCode").text = inMain.head.ordcode + DateTime.Now.ToString("yyyyMMddHHmmss").Substring(8);
                    //接口2生成的其它入库单类别应该是【借出还回入库】
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("crdcode").text = "104";
                #endregion


                #region //body
                    IXMLDOMNode xmlMode = obody.selectSingleNode("//xml//rs:data").childNodes[0];
                    foreach (InBody body in inMain.body)
                    {
                        if ((body.detail != null) && (body.detail.Count > 0))
                        {
                            string cinvcode = body.cinvCode;
                            decimal iquantity = body.iquantity;
                            if (string.IsNullOrEmpty(ccuscode))
                            {
                                strSql = "select a.bObjectCode from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts	b on a.ID=b.ID where b.AutoID='"+body.oriU8RowId+"'";
                                ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                                ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = ccuscode;
                            }


                            foreach (InDetail detail in body.detail)
                            {
                                IXMLDOMNode xmlNow = xmlMode.cloneNode(true);
                                if (!string.IsNullOrEmpty(detail.cinvCode))
                                {
                                    cinvcode = detail.cinvCode;
                                }
                                if (detail.iquantity != null)
                                {
                                    iquantity = detail.iquantity;
                                }
                                xmlNow.attributes.getNamedItem("cinvcode").text = cinvcode;
                                xmlNow.attributes.getNamedItem("iquantity").text = iquantity.ToString();
                                //strSql = "select b.autoid from HY_DZ_BorrowOuts b left join HY_DZ_BorrowOuts_extradefine c on b.AutoID=c.AutoID"
                                //        + " where c.cbdefine21='" + body.ori_req_id + "' and b.cinvcode='" + detail.cinv_code + "'";
                                xmlNow.attributes.getNamedItem("UpAutoID").text = body.oriU8RowId; //Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                                strSql = "select cwhcode from warehouse where cwhname='" + body.cwhname + "'";
                                xmlNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                                obody.selectSingleNode("//xml//rs:data").appendChild(xmlNow);
                            }
                        }
                    }
                    obody.selectSingleNode("//xml//rs:data").removeChild(xmlMode);
                #endregion

                #region//process
                    
                    ohead.save("d:\\borrowoutreturn_head_20210104.xml");
                    obody.save("d:\\borrowoutreturn_body_20210104.xml");
                    bresult = cosc.SaveVouch(ref ohead, ref obody, insert, ref errMsg, ref iVouchID);
                    if (bresult)
                    {
                        outData.oacode = inMain.head.ccode;                        
                        errMsg = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                        outData.recode = "0";
                        outData.u8code = errMsg;
                        //re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                        outData.remsg = "借出归还单[" + errMsg + "]导入成功,";
                        Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBack] set cdefine12='" + inMain.head.ccode.ToString() 
                            + "',cdefine13='" + inMain.head.ccode.ToString() + "' where id=" + iVouchID.ToString());
                    }
                    else
                    {
                        strResult = "借出归还单导入失败,"+errMsg;
                        LogHelper.WriteLog(typeof(BorrowReturnEntity), strResult);
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "999", strResult);
                    }

                    //审核借出归还单
                    bresult = cosc.Verify(iVouchID.ToString(), ref errMsg);
                    if (bresult)
                    {
                        outData.remsg += "借出归还单[" + errMsg + "]审核成功,";
                    }
                    else
                    {
                        strResult = "借出归还单审核失败," + errMsg;
                        outData.remsg += strResult;
                        outData.recode = "881";
                        outMain.dataList.Add(outData);
                        return outMain;
                    }                    
                    
                    //生成其他入库单
                    strResult = cosc.PushOtherIn(iVouchID);
                    errMsg = getCCode(strResult);
                    
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        outData.recode = "882";
                        outData.remsg += "其他入库单生成失败," + strResult;
                        outMain.dataList.Add(outData);
                        return outMain;
                    }
                    else
                    {
                        outData.u8rdcode = errMsg;
                        outData.remsg += strResult + ",";
                        //接口2生成的其它入库单类别应该是【104 借出还回入库】
                        Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[RdRecord08] set crdcode='104'" 
                            +" where ccode='" + errMsg+"'");
                    }

                    //其他入库单序列号
                    inMain.head.cmemo = errMsg;
                    strResult=JsonHelper.ToJson(inMain);
                    LogHelper.WriteLog(typeof(BorrowReturnEntity), strResult);
                    strResult = HttpPostHelper.sendInsert("http://127.0.0.1/XylinkU8Interface/api/BorrowReturnSN", strResult);
                    Result reSN = JsonHelper.FromJson<Result>(strResult);
                    if (reSN.recode == "0")
                    {
                        outData.remsg += "序列号导入成功";
                    }
                    else
                    {
                        outData.recode = "994";
                        outData.remsg += "序列号导入失败" + reSN.remsg;
                        outMain.dataList.Add(outData);
                        return outMain;
                    }
                #endregion


                outMain.dataList.Add(outData);
                return outMain;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowReturnEntity), ex);
                strResult =ex.Source.ToString() + " " + ex.Message;
                return getErrorOutMain(inMain.companycode, inMain.head.ccode, "9999", strResult);
            }
            
        }
        //错误返回
        private static OutMain getErrorOutMain(string companycode,string oacode, string recode, string remsg)
        {
            OutMain outMain = new OutMain();
            outMain.companycode = companycode;
            outMain.dataList = new List<OutData>();
            OutData outData = new OutData();
            outData.recode = recode;
            outData.remsg = remsg;
            outMain.dataList.Add(outData);
            return outMain;
        }
        private static string getCCode(string strSource)
        {
            string strResult = "";
            //string strSource = "成功生成[1]张其他入库单,单号[QTRK202110030003]";
            if (strSource.IndexOf("成功") >= 0)
            {
                int iBegin = strSource.IndexOf("单号[");
                strSource = strSource.Substring(iBegin);
                strResult = strSource.Substring(3, strSource.Length - 6);

            }
            return strResult;
        }
    }

    
}