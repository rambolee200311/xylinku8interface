using System;
using System.Collections.Generic;
using System.Web;
using HYBorrowOut.Models.Borrowoutback;
using HYBorrowOut.Models.Result;
using HYBorrowOut.Helper;
using MSXML2;
using System.Data;
//using System.Data.Linq;

namespace HYBorrowOut.UFIDA
{
    public class HYBorrowOutBackEntity
    {
        public Result Add_Borrow_Out_Back(BorrowOutBack bo)
        {
            Result re = new Result();
            string strResult = "";
            string strSql = "";
            string errMsg = "";
            string oricode = "";
            string oristrvoid = "";
            int iVouchID=0;
            bool bresult = false;
            bool insert = true;
            oricode = bo.head.oriccode;
            try
            {
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(bo.companycode);
                if (m_ologin == null)
                {
                    strResult = "帐套" + bo.companycode + "登录失败";
                    re.oacode = bo.head.ccode;
                    re.recode = "111";
                    re.remsg = strResult;
                    return re;
                }

                if (string.IsNullOrEmpty(oricode))
                {
                    re.oacode = bo.head.ccode;
                    re.recode = "1111";
                    re.remsg = "借出借用单编号不能为空";
                    return re;
                }

                oristrvoid = Ufdata.getDataReader(m_ologin.UfDbName, "select ID from [dbo].[HY_DZ_BorrowOut] where cdefine12='" + oricode + "'");
                if (string.IsNullOrEmpty(oristrvoid))
                {
                    re.oacode = bo.head.ccode;
                    re.recode = "1111";
                    re.remsg = "借出借用单编号不存在";
                    return re;
                }
                //2021-10-18 重复导入控制
                if (!string.IsNullOrEmpty(
                    Ufdata.getDataReader(m_ologin.UfDbName, "select * from HY_DZ_BorrowOutBack where cdefine12='" + bo.head.ccode + "'")
                ))
                {
                    strResult = "借出归还单重复导入";
                    LogHelper.WriteLog(typeof(HYBorrowOutEntity), strResult);
                    re.oacode = bo.head.ccode;
                    re.recode = "111";
                    re.remsg = strResult;
                    return re;
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
                #region//head
                string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + bo.head.cust_name + "'");
                string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + bo.head.dept_name + "'");
                string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + bo.head.person_name + "'");
                string cwhcode = "";
                if (!string.IsNullOrEmpty(ccuscode))
                {
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("bObjectCode").text = ccuscode;
                }
                else
                {
                    re.recode = "222";
                    re.remsg = bo.head.cust_name + "客户档案不存在";
                    return re;
                }
                if (!string.IsNullOrEmpty(cdepcode))
                {
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdepcode").text = cdepcode;
                }
                else
                {
                    cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cDepCode from Person where cPersonName='" + bo.head.person_name + "'");
                    if (string.IsNullOrEmpty(cdepcode))
                    {
                        re.recode = "222";
                        re.remsg = bo.head.dept_name + "部门档案不存在";
                        return re;
                    }
                    else
                    {
                        ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdepcode").text = cdepcode;
                    }
                }
                if (!string.IsNullOrEmpty(cpersoncode))
                {
                    ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cpersoncode").text = cpersoncode;
                }
                else
                {
                    re.recode = "222";
                    re.remsg = bo.head.person_name + "人员档案不存在";
                    return re;
                }
                ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ddate").text = bo.head.ddate.ToShortDateString();
                ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("dmDate").text = bo.head.ddate.ToShortDateString();
                ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cmemo").text = "借出借用单(" + oristrvoid + ")生单";
                ohead.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cdefine12").text = bo.head.ccode;
                #endregion

                #region//body
                IXMLDOMNode xmlMode = obody.selectSingleNode("//xml//rs:data").childNodes[0];
                foreach (BorrowOutBack_body body in bo.body)
                {
                    if ((body.detail != null) && (body.detail.Count > 0))
                    {
                        foreach (BorrowOutBack_body_detail detail in body.detail)
                        {
                            IXMLDOMNode xmlNow = xmlMode.cloneNode(true);
                            xmlNow.attributes.getNamedItem("cinvcode").text = detail.cinv_code;
                            xmlNow.attributes.getNamedItem("iquantity").text = detail.iquantity.ToString();
                            strSql = "select b.autoid from HY_DZ_BorrowOuts b left join HY_DZ_BorrowOuts_extradefine c on b.AutoID=c.AutoID"
                                    + " where c.cbdefine21='" + body.ori_req_id + "' and b.cinvcode='" + detail.cinv_code + "'";
                            xmlNow.attributes.getNamedItem("UpAutoID").text = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                            strSql = "select cwhcode from warehouse where cwhname='" + body.cwhname + "'";
                            xmlNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                            obody.selectSingleNode("//xml//rs:data").appendChild(xmlNow);
                        }
                    }

                }
                obody.selectSingleNode("//xml//rs:data").removeChild(xmlMode);
                #endregion

                #region//process
                ohead.save("d:\\borrowoutback_head_20210104.xml");
                obody.save("d:\\borrowoutback_body_20210104.xml");
                bresult = cosc.SaveVouch(ref ohead, ref obody, insert, ref errMsg, ref iVouchID);
                if (bresult)
                {
                    re.oacode = bo.head.ccode;
                    re.recode = "0";
                    errMsg = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                    re.u8code = errMsg;
                    //re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                    re.remsg = "借出归还单[" + errMsg + "]导入成功,";
                    Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBack] set cdefine13='" + bo.head.ccode.ToString() + "' where id=" + iVouchID.ToString());
                    #region//部分归还
                    /*
                //return re;
                DataTable dtbacks = Ufdata.getDatatableFromSql(m_ologin.UfDbName, "select c.cbdefine21,a.* from HY_DZ_BorrowOutBacks a"
                                                                                +" left join HY_DZ_BorrowOuts b on a.UpAutoID=b.AutoID"
                                                                                +" left join HY_DZ_BorrowOuts_extradefine c on b.AutoID=c.AutoID"
                                                                                +" where a.id="+iVouchID.ToString());
                if (dtbacks!=null)
                {
                    foreach(DataRow drbacks in dtbacks.Rows)
                    {
                        string invcode = drbacks["cinvcode"].ToString();
                        string cbdefine21 = drbacks["cbdefine21"].ToString();
                        decimal iquantity = Convert.ToDecimal(drbacks["iquantity"]);
                        int autoid=Convert.ToInt32(drbacks["autoid"]); 
                            List<BorrowOutBack_body> list1 = bo.body.FindAll(delegate(BorrowOutBack_body bobody) { return bobody.ori_req_id == cbdefine21; });// bo.body.Where(x => x.cinv_code == invcode).ToList();
                            if (list1==null)
                            {
                                Ufdata.execSqlcommand(m_ologin.UfDbName, "delete from [dbo].[HY_DZ_BorrowOutBacks] where id=" + iVouchID.ToString() + " and autoid=" + autoid.ToString());
                                Ufdata.execSqlcommand(m_ologin.UfDbName, "delete from [dbo].[HY_DZ_BorrowOutBacks2] where upid=" + iVouchID.ToString() + " and upautoid=" + autoid.ToString());
                            }
                            else
                            {                            
                                    foreach(BorrowOutBack_body_detail body_detail in list1[0].detail)
                                    {
                                        if (Convert.ToDecimal(body_detail.iquantity) < iquantity)
                                        {
                                            //Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks] set iquantity=" + list1[0].iquantity.ToString() + " where id=" + iVouchID.ToString() + " and autoid=" + autoid.ToString());
                                            //Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks2] set iquantity=" + list1[0].iquantity.ToString() + ",MycdefineB8=" + list1[0].iquantity.ToString() + " where upid=" + iVouchID.ToString() + " and upautoid=" + autoid.ToString());
                                            Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks] set iquantity=" + body_detail.iquantity.ToString() + " where id=" + iVouchID.ToString() + " and autoid=" + autoid.ToString());
                                            Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks2] set iquantity=" + body_detail.iquantity.ToString() + ",MycdefineB8=" + body_detail.iquantity.ToString() + " where upid=" + iVouchID.ToString() + " and upautoid=" + autoid.ToString());
                                        }                                
                                    }
                            }                       
                    }
                }
                */
                    #endregion


                    //审核借出归还单
                    bresult = cosc.Verify(iVouchID.ToString(), ref errMsg);
                    if (bresult)
                    {
                        re.remsg += "借出归还单[" + errMsg + "]审核成功,";
                        //生成其他入库单
                        strResult = cosc.PushOtherIn(iVouchID);
                        errMsg = getCCode(strResult);
                        if (string.IsNullOrEmpty(errMsg))
                        {
                            re.recode = "2244";
                            re.remsg += "其他入库单生成失败," + strResult;
                            return re;
                        }
                        else
                        {
                            re.remsg += strResult+",";
                        }
                        //其他入库单序列号
                        bo.head.cmemo = errMsg;
                        strResult = HttpPostHelper.sendInsert("http://127.0.0.1/XylinkU8Interface/api/OtherInSn", JsonHelper.ToJson(bo));
                        Result reSN = JsonHelper.FromJson<Result>(strResult);
                        if (reSN.recode=="0")
                        {
                            re.remsg += "序列号导入成功";
                        }
                        else
                        {
                            re.recode = "2255";
                            re.remsg += "序列号导入失败"+reSN.remsg;
                        }
                        
                    }
                    else
                    {
                        re.recode = "2233";
                        re.remsg += "借出归还单[" + errMsg + "]审核失败," + errMsg;
                        return re;
                    }

                }
                else
                {
                    re.oacode = bo.head.ccode;
                    re.recode = "2222";
                    //re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                    re.remsg = "借出归还单导入失败," + errMsg;
                    return re;
                }

            #endregion

            }
            catch(Exception ex)
            {
                re.oacode = bo.head.ccode;
                re.recode = "9999";
                re.remsg = ex.Message;
                return re;
            }
            return re;
        }
        public string getCCode(string strSource)
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