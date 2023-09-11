using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HYBorrowOut.Models.BorrowTrial;
using HYBorrowOut.Models.Result;
using HYBorrowOut.Helper;
using MSXML2;
using System.Data;
/*
 * 20230903
 * lijianqiang
 * BorrowTrial 产品试用
 */
namespace HYBorrowOut.UFIDA
{
    public class BorrowTrialEntity
    {
        public static OutMain Set_Borrow_Out(InMain inMain)
        {            
            OutMain outMain = new OutMain();
            string strResult = "";
            string strSql = "";
            bool bresult = false;
            bool insert = true;
            string errMsg = "";
            int iVouchID = 0;
            bool billNo = false;
            string vouchcode = "";
            bool bResult = false;
            outMain.companycode = inMain.companycode;
            ADODB.Connection conn = new ADODB.Connection();
            try
            {
                outMain.dataList = new List<OutData>();
                OutData outData = new OutData();
                outData.oacode = inMain.head.ccode;
                #region //ready
                    HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                    U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode);
                    if (m_ologin == null)
                    {
                        strResult = "帐套" + inMain.companycode + "登录失败";
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "1111", strResult);
                    }
                    conn.ConnectionString = m_ologin.UfDbName;
                    conn.Open();
                    //2021-10-18 重复导入控制
                    strSql = "select * from HY_DZ_BorrowOutBack where cdefine12='" + inMain.head.ccode + "'";
                    if (!string.IsNullOrEmpty(Ufdata.getDataReader(m_ologin.UfDbName, strSql)))
                    {
                        strResult = "借出归还单重复导入";
                        LogHelper.WriteLog(typeof(BorrowReturnEntity), strResult);
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "111", strResult);
                    }

                    bResult = cls_Borrow_Out.Init(m_ologin);
                    // bool bresult = cosc.MakeVouchFromBorrowOut(Convert.ToInt32(oristrvoid), ref errMsg, ref iVouchID);
                    MSXML2.IXMLDOMDocument2 ohead;
                    ohead = new MSXML2.DOMDocument60();
                    MSXML2.IXMLDOMDocument2 obody;
                    obody = new MSXML2.DOMDocument60();
                    ohead.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_head.xml");
                    obody.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_body.xml");
                    ohead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                    obody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                #endregion

                #region //head
                    string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + inMain.head.custName + "'");
                    string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from person where cpersonname='" + inMain.head.personName + "'");
                    string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + inMain.head.personName + "'");
                    string cwhcode = "";// Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + inMain.head.cwhname + "'");
                    string cdepname = "";
                    if (!string.IsNullOrEmpty(ccuscode))
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = ccuscode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectName").text = inMain.head.custName;
                    }
                    else
                    {
                        strResult = inMain.head.custName + "客户档案不存在";
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "222", strResult);
                    }
                    if (!string.IsNullOrEmpty(cpersoncode))
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cpersoncode").text = cpersoncode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cpersonname").text = inMain.head.personName;
                        cdepname = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepname from department where cdepcode='" + cdepcode + "'");
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdepname").text = cdepname;
                    }
                    else
                    {
                        strResult = inMain.head.personName + "人员档案不存在";
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "222", strResult);
                    }
                    
                    switch (inMain.head.ctype.ToString())
                    {
                        case "部门":
                            ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = cdepcode;
                            ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectName").text = cdepname;
                            break;
                        case "客户":
                            ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = ccuscode;
                            ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectName").text = inMain.head.custName;
                            break;
                        default:
                            strResult = inMain.head.ctype.ToString() + "类型不正确";
                            return getErrorOutMain(inMain.companycode, inMain.head.ccode, "222", strResult);
                            break;
                    }
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCODE").text = "JCJY202009010023";// inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCode").text = "JCJY202009010023";// inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ccode").text = "JCJY202009010023";// inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("MycdefineT1").text = "借出借用单(" + "JCJY202009010023" + ")生单";

                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("VoucherCode").text = inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("MycdefineT1").text = "借出借用单(" + inMain.head.ccode + ")生单";
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("editprop").text = "A";
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdepcode").text = cdepcode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ccuscode").text = ccuscode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cpersoncode").text = cpersoncode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cType").text = inMain.head.ctype.ToString();
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine12").text = inMain.head.ccode;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine2").text = inMain.body[0].recvName;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine3").text = inMain.body[0].recvPhone;
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine11").text = inMain.body[0].recvAddress;                    
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("dDate").text = Convert.ToDateTime(inMain.head.ddate).ToShortDateString();
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ddate").text = Convert.ToDateTime(inMain.head.ddate).ToShortDateString();
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("dmDate").text = DateTime.Now.ToShortDateString();
                    string cmemo = "";
                    if (!string.IsNullOrEmpty(cmemo))
                    {
                        cmemo=inMain.head.cmemo.ToString();
                    }
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cmemo").text = cmemo;
                    bResult = cls_Borrow_Out.GetBillNumberChecksucceed(ref ohead, ref vouchcode, ref errMsg);
                    
                    if (!string.IsNullOrEmpty(vouchcode))
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCODE").text = vouchcode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCode").text = vouchcode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ccode").text = vouchcode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("VoucherCode").text = vouchcode;
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("MycdefineT1").text = "借出借用单(" + vouchcode + ")生单";
                    }
                    
                    
                #endregion

                #region //body
                    int i = 0;
                    IXMLDOMNode xmlMode = obody.selectSingleNode("//xml//rs:data").childNodes[0];
                    foreach (InBody body in inMain.body)
                    {
                        if ((body.detail != null) && (body.detail.Count > 0))
                        {
                            string cinvcode = body.cinvCode;
                            string cinvname = body.cinvName;
                            decimal iquantity = body.iquantity;
                            foreach (InDetail detail in body.detail)
                            {
                                IXMLDOMNode xmlNow = xmlMode.cloneNode(true);
                                if (!string.IsNullOrEmpty(detail.cinvCode))
                                {
                                    cinvcode = detail.cinvCode;
                                }
                                if (!string.IsNullOrEmpty(detail.cinvName))
                                {
                                    cinvname = detail.cinvCode;
                                }
                                if (detail.iquantity != null)
                                {
                                    iquantity = detail.iquantity;
                                }
                                /*
                                 <cbdefine4>[recv_name]</cbdefine4>
                                  <cbdefine5>[recv_phone]</cbdefine5>
                                  <cbdefine9>[recv_address]</cbdefine9>
                                 */
                                xmlNow.attributes.getNamedItem("cinvcode").text = cinvcode;
                                xmlNow.attributes.getNamedItem("cinvname").text = cinvname;
                                xmlNow.attributes.getNamedItem("cwhcode").text =
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + body.cwhname.ToString() + "'");
                                xmlNow.attributes.getNamedItem("igrouptype").text = 
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select iGroupType from inventory where cInvCode='" + cinvcode + "'");
                                xmlNow.attributes.getNamedItem("cgroupcode").text =
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select cGroupCode from inventory where cInvCode='" + cinvcode + "'");
                                xmlNow.attributes.getNamedItem("cgroupname").text =
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select cGroupName from ComputationGroup where cGroupCode=(select cGroupCode from inventory where cInvCode='" + cinvcode + "')");
                                xmlNow.attributes.getNamedItem("ccomunitcode").text =
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select cComUnitCode from inventory where cInvCode='" + cinvcode + "'");
                                xmlNow.attributes.getNamedItem("ccomunitcode").text =
                                    Ufdata.getDataReader(m_ologin.UfDbName, "select cComUnitName from ComputationUnit where cComunitCode=(select cComUnitCode from inventory where cInvCode='" + cinvcode + "')");
                                xmlNow.attributes.getNamedItem("iquantity").text = iquantity.ToString();
                                xmlNow.attributes.getNamedItem("backdate").text =Convert.ToDateTime(body.backDate).ToShortDateString();
                                xmlNow.attributes.getNamedItem("cbdefine21").text = body.reqId;
                                xmlNow.attributes.getNamedItem("cbdefine4").text = body.recvName;
                                xmlNow.attributes.getNamedItem("cbdefine5").text = body.recvPhone;
                                xmlNow.attributes.getNamedItem("cbdefine9").text = body.recvAddress;
                                xmlNow.attributes.getNamedItem("cbsysbarcode").text = "||stjc|" + vouchcode + "|" + (i + 1).ToString();


                                obody.selectSingleNode("//xml//rs:data").appendChild(xmlNow);
                            }
                        }
                    }
                    obody.selectSingleNode("//xml//rs:data").removeChild(xmlMode);
                #endregion

                #region //process
                    //新增借用借出单
                    //HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                    HY_DZ_BorrowOut.clsBorrowOutSrv iac = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                    iVouchID = 1;
                    bResult = iac.SaveVouch(ref ohead, ref obody, true, ref errMsg, ref iVouchID, ref conn);
                    if (!bResult)
                    {
                        LogHelper.WriteLog(typeof(HYBorrowOutEntity), errMsg);
                        return getErrorOutMain(inMain.companycode, inMain.head.ccode, "999", errMsg);
                    }
                    else
                    {
                        outData.recode = "0";
                        vouchcode = Ufdata.getDataReader(m_ologin.UfDbName,"select ccode from [dbo].[HY_DZ_BorrowOut] where id=" + iVouchID.ToString());
                        outData.u8code =vouchcode;                        
                    }
                    //审核借用借出单
                    //bResult = iac.Verify(iVouchID.ToString(), ref errMsg, "", ref conn);
                    bResult = cls_Borrow_Out.Verify(iVouchID.ToString(), ref errMsg);
                    if (!bResult)
                    {
                        LogHelper.WriteLog(typeof(HYBorrowOutEntity), errMsg);
                        strResult = "借用借出单[" + vouchcode + "]审核失败," + errMsg;
                        outData.remsg += strResult;
                        outData.recode = "881";
                        outMain.dataList.Add(outData);
                        return outMain;
                    }
                    else
                    {
                        outData.remsg += "借用借出单[" + vouchcode + "]审核成功,";
                        strResult = JsonHelper.ToJson(inMain);
                        LogHelper.WriteLog(typeof(BorrowReturnEntity), strResult);
                        strResult = HttpPostHelper.sendInsert("http://127.0.0.1/XylinkU8Interface/api/BorrowTrialSN", strResult);
                        Result reSN = JsonHelper.FromJson<Result>(strResult);
                        outData.u8rdcode = reSN.u8code;
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

                    }
                    
                    //其他出库单
                    
                #endregion

                outMain.dataList.Add(outData);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(BorrowTrialEntity), ex);
                strResult = ex.Source.ToString() + " " + ex.Message;
                return getErrorOutMain(inMain.companycode, inMain.head.ccode, "9999", strResult);
            }
            
            return outMain;
        }
        //错误返回
        private static OutMain getErrorOutMain(string companycode, string oacode, string recode, string remsg)
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
    }
}