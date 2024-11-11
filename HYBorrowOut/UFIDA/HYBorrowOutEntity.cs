using System;
using System.Collections.Generic;
using System.Web;
using HYBorrowOut.Models.Borrowout;
using HYBorrowOut.Models.Result;
using HYBorrowOut.Helper;
using MSXML2;
using System.Data;

namespace HYBorrowOut.UFIDA
{
    public class HYBorrowOutEntity
    {
        public Result Set_Borrow_Out()
        {
            string strResult = "";
            Result re = new Result();
            ADODB.Connection conn = new ADODB.Connection();
            try
            {
                //1 登录U8

                U8Login.clsLoginClass m_login = new U8Login.clsLoginClass();
                m_login.Login("AA", "995", "2020", "yonyou", "", "2020-11-16", "127.0.0.1", null);

                conn.ConnectionString = m_login.UfDbName;
                conn.Open();
                HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                bool billNo = cls_Borrow_Out.Init(m_login);
                MSXML2.IXMLDOMDocument2 oHead = new MSXML2.DOMDocument60Class();
                MSXML2.IXMLDOMDocument2 oBody = new MSXML2.DOMDocument60Class();
                string err_Msg = "";
                int vouch_Id = 1000002560;
                oHead.load("d:\\ohead_2222.xml");
                oHead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                oBody.load("d:\\obody_2222.xml");
                oBody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                string vouchcode = "JCJY202009010023";
                //object obj1= cls_Borrow_Out.ExecRequestAudit(vouchcode);
                //
                ////bool billNo = cls_Borrow_Out.GetBillNumberChecksucceed(ref oHead, ref vouchcode, err_Msg);
                billNo = cls_Borrow_Out.GetBillNumberChecksucceed(ref oHead, ref vouchcode, ref err_Msg);
                //billNo = cls_Borrow_Out.ExecSubOpen(vouchcode, ref err_Msg);

                //HY_DZ_BorrowOut.InvokeApiAClass iac = new HY_DZ_BorrowOut.InvokeApiAClass();
                //iac.VoucherAddSave(m_login, oHead, oBody);
                //err_Msg = iac.errMsg;
                oHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row").attributes.getNamedItem("cCODE").text = vouchcode;
                oHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row").attributes.getNamedItem("cCode").text = vouchcode;
                oHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row").attributes.getNamedItem("ccode").text = vouchcode;
                oHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row").attributes.getNamedItem("MycdefineT1").text = "借出借用单(" + vouchcode + ")生单";
                //oHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row").attributes.getNamedItem("VoucherCode").text = vouchcode;
                cls_Borrow_Out.SaveVouch(ref oHead, ref oBody, true, ref err_Msg, ref vouch_Id, ref conn);
                if (!string.IsNullOrEmpty(err_Msg))
                {
                    strResult = err_Msg;
                    re.remsg = err_Msg;
                }
                else
                {
                    strResult = vouch_Id.ToString();
                    re.remsg = "ok";
                    re.u8code = strResult;
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return re;
        }

        public Result Add_Borrow_Out(Borrowout bo)
        {
            //LogHelper.WriteLog(typeof(HYBorrowOutEntity),JsonHelper.ToJson(bo));
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            int vouchid;
            ADODB.Connection conn = new ADODB.Connection();
            try
            {

                bool billNo = false;
                string vouchcode = "";
                HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), "3");
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(bo.companycode);
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), "4");
                //U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop(so.companycode);
                //ADODB.Connection conn = new Connection();
                if (m_ologin == null)
                {
                    strResult = "帐套" + bo.companycode + "登录失败";
                    LogHelper.WriteLog(typeof(HYBorrowOutEntity), strResult);
                    re.oacode = bo.head.ccode;
                    re.recode = "111";
                    re.remsg = strResult;
                    return re;
                }
                conn.ConnectionString = m_ologin.UfDbName;
                conn.Open();
                //2021-10-18 重复导入控制
                if (!string.IsNullOrEmpty(
                    Ufdata.getDataReader(m_ologin.UfDbName, "select * from HY_DZ_BorrowOut where cdefine12='" + bo.head.ccode + "'")
                ))
                {
                    strResult = "借出借用单重复导入";
                    LogHelper.WriteLog(typeof(HYBorrowOutEntity), strResult);
                    re.oacode = bo.head.ccode;
                    re.recode = "111";
                    re.remsg = strResult;
                    return re;
                }

                cls_Borrow_Out.Init(m_ologin);
                re.oacode = bo.head.ccode;
                #region//head
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument60();
                //IXMLDOMDocument2 dom1=new DOMDocument60
                dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_head.xml");
                dom_head.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");

                MSXML2.IXMLDOMDocument2 dom_head_mode = new MSXML2.DOMDocument60();
                dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_head_model.xml");

                foreach (MSXML2.IXMLDOMNode doel in dom_head_mode.selectSingleNode("data").childNodes)
                {
                    errMsg ="head:"+ doel.nodeName.ToString().Trim();
                    string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + bo.head.cust_name + "'");
                    string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + bo.head.dept_name + "'");
                    string cpersoncode = "";
                    if (!string.IsNullOrEmpty(doel.text))
                    {
                        switch (doel.nodeName.ToString().Trim())
                        {
                            case "ccuscode":
                                //ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + bo.head.cust_name + "'");
                                if (!string.IsNullOrEmpty(ccuscode))
                                {
                                    dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = ccuscode;
                                }
                                else
                                {
                                    re.recode = "222";
                                    re.remsg = bo.head.cust_name + "客户档案不存在";
                                    return re;
                                }
                                break;
                            case "cdepcode":
                                //cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + bo.head.dept_name + "'");
                                if (!string.IsNullOrEmpty(cdepcode))
                                {
                                    dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
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
                                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
                                    }
                                }
                                break;
                            case "cpersoncode":
                                cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + bo.head.person_name + "'");
                                if (!string.IsNullOrEmpty(cpersoncode))
                                {
                                    dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cpersoncode;
                                }
                                else
                                {
                                    re.recode = "222";
                                    re.remsg = bo.head.person_name + "人员档案不存在";
                                    return re;
                                }
                                break;
                            case "bObjectCode":
                                switch (bo.head.ctype.ToString())
                                {
                                    case "部门":
                                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
                                        break;
                                    case "客户":
                                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = ccuscode;
                                        break;
                                    default:
                                        re.recode = "222";
                                        re.remsg = bo.head.ctype.ToString() + "类型不正确";
                                        return re;
                                        break;
                                }
                                break;
                            case "bObjectName":
                                switch (bo.head.ctype.ToString())
                                {
                                    case "部门":
                                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = bo.head.dept_name;
                                        break;
                                    default:
                                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = bo.head.cust_name;
                                        break;
                                }
                                break;
                            default:
                                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, bo.head);
                                break;
                        }

                    }
                    else
                    {
                        dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                    }
                }

                billNo = cls_Borrow_Out.GetBillNumberChecksucceed(ref dom_head, ref vouchcode, ref errMsg);
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCODE").text = vouchcode;
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("cCode").text = vouchcode;
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ccode").text = vouchcode;
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("VoucherCode").text = vouchcode;
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("dDate").text = bo.head.ddate.ToShortDateString();
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("ddate").text = bo.head.ddate.ToShortDateString();
                dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("MycdefineT1").text = "借出借用单(" + vouchcode + ")生单";
                //dom_head.selectSingleNode("//xml//rs:data//z:row").attributes.getNamedItem("csysbarcode").text = "||stjc|" + vouchcode;
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), dom_head.xml);
                #endregion

                #region//body
                MSXML2.IXMLDOMDocument2 dom_body = new MSXML2.DOMDocument60();
                dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_body.xml");
                dom_body.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");

                MSXML2.IXMLDOMDocument2 dom_body_mode = new MSXML2.DOMDocument60();
                dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\borrowout_body_model.xml");

                for (int j = 1; j < bo.body.Count; j++)//追加行
                {
                    //dom_body.selectSingleNode("//xml//rs:data").appendChild(dom_body.selectSingleNode("//xml//rs:data//z:row").cloneNode(true));
                }

                int i = 0;
                foreach (Borrowout_body sob in bo.body)
                {
                    foreach (MSXML2.IXMLDOMNode doel in dom_body_mode.selectSingleNode("data").childNodes)
                    {
                        errMsg ="body:"+ doel.nodeName.ToString().Trim();
                        if (!string.IsNullOrEmpty(doel.text))
                        {
                            switch (doel.nodeName.ToString().Trim())
                            {
                                //case "backdate":
                                //    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = bo.head.ddate.ToShortDateString();
                                //    break;
                                case "cbsysbarcode":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = "||stjc|" + vouchcode + "|" + (i + 1).ToString();
                                    break;
                                case "igrouptype":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select iGroupType from inventory where cInvCode='" + sob.cinv_code + "'"); ;
                                    break;
                                case "cgroupcode":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select cGroupCode from inventory where cInvCode='" + sob.cinv_code + "'"); ;
                                    break;
                                case "ccomunitcode":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select cComUnitCode from inventory where cInvCode='" + sob.cinv_code + "'"); ;
                                    break;
                                case "cgroupname":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select cGroupName from ComputationGroup where cGroupCode=(select cGroupCode from inventory where cInvCode='" + sob.cinv_code + "')"); ;
                                    break;
                                case "ccomunitname":
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select cComUnitName from ComputationUnit where cComunitCode=(select cComUnitCode from inventory where cInvCode='" + sob.cinv_code + "')"); ;
                                    break;
                                default:
                                    dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, sob);
                                    break;
                            }
                        }
                        else
                        {
                            dom_body.selectSingleNode("//xml//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                        }
                        
                    }
                    getPortfolio(sob.cinv_code, sob.iquantity, ref dom_body, dom_body.selectSingleNode("//xml//rs:data").childNodes[i], m_ologin);
                    //i++;
                }
                dom_body.selectSingleNode("//xml//rs:data").removeChild(dom_body.selectSingleNode("//xml//rs:data").childNodes[i]);
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), dom_body.xml);
                #endregion
                HY_DZ_BorrowOut.clsBorrowOutSrv iac = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
                //HY_DZ_BorrowOut.InvokeApiAClass iac = new HY_DZ_BorrowOut.InvokeApiAClass();
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), "6");
                //iac.VoucherAddSave(m_ologin, dom_head, dom_body);

                dom_head.save(AppDomain.CurrentDomain.BaseDirectory + "temp\\borrowout_head" + bo.head.ccode + ".xml");
                dom_body.save(AppDomain.CurrentDomain.BaseDirectory + "temp\\borrowout_body" + bo.head.ccode + ".xml");

                vouchid = 1;
                bool bResult = iac.SaveVouch(ref dom_head, ref dom_body, true, ref errMsg, ref vouchid, ref conn);
                //LogHelper.WriteLog(typeof(HYBorrowOutEntity), "7");
                //errMsg = iac.errMsg;
                //int vouchid = 0;
                //cls_Borrow_Out.SaveVouch(dom_head, dom_body, true, ref errMsg, ref vouchid);

                if (!bResult)
                {
                    LogHelper.WriteLog(typeof(HYBorrowOutEntity), errMsg);
                    re.recode = "222";
                    re.recode = errMsg;
                }
                else
                {
                    re.recode = "0";
                    vouchcode = "select ccode from [dbo].[HY_DZ_BorrowOut] where id=" + vouchid.ToString();
                    re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, vouchcode);
                    //LogHelper.WriteLog(typeof(HYBorrowOutEntity), vouchcode);
                    //autoid
                    //string autoid = Ufdata.getDataReader(m_ologin.UfDbName, "select max(autoid) from [dbo].[HY_DZ_BorrowOuts]");
                    //Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOuts] set AutoID=AutoID+" + autoid + "+1 where id=(select id from [dbo].[HY_DZ_BorrowOut] where cCODE='" + vouchcode + "')");
                    //Ufdata.execSqlcommand(m_ologin.UfDbName, "update UFSystem.dbo.UA_Identity set iChildId=(select max(autoid)+1-1000000000 from [dbo].[HY_DZ_BorrowOuts]) where cAcc_Id='" + bo.companycode + "' and (cVouchType='HYJCGH001' or cVouchType='hy_DZ_BorrowOuts')");
                    //bdefine

                    //string txtsql = "insert into HY_DZ_BorrowOuts_extradefine (";
                    //foreach (IXMLDOMNode doen in dom_body_mode.selectSingleNode("data").childNodes)
                    //{
                    //    if (doen.nodeName.IndexOf("cbdefine") >= 0)
                    //    {
                    //        txtsql += doen.nodeName + ",";
                    //    }
                    //}
                    //txtsql += "autoid) values (";
                    //i = 1;

                    //foreach (Borrowout_body sob in bo.body)
                    //{
                    //    string strSql = txtsql;
                    //    string sysbarcode = "||stjc|" + vouchcode + "|" + i.ToString();
                    //    autoid = Ufdata.getDataReader(m_ologin.UfDbName, "select autoid from [dbo].[HY_DZ_BorrowOuts] where cbsysbarcode='" + sysbarcode + "'");
                    //    foreach (IXMLDOMNode doen in dom_body_mode.selectSingleNode("data").childNodes)
                    //    {
                    //        if (doen.nodeName.IndexOf("cbdefine") >= 0)
                    //        {
                    //            strSql += "'" + getItemValue(doen.text, sob) + "',";
                    //        }
                    //    }
                    //    strSql += autoid + ")";
                    //    LogHelper.WriteLog(typeof(HYBorrowOutEntity), strSql);
                    //    Ufdata.execSqlcommand(m_ologin.UfDbName, strSql);

                    //    i++;
                    //}



                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HYBorrowOutEntity), ex);
                re.oacode = bo.head.ccode;
                re.recode = "999";
                re.remsg = errMsg + " " + ex.Message;
            }
            finally
            {
                conn.Close();
            }
            //LogHelper.WriteLog(typeof(HYBorrowOutEntity), JsonHelper.ToJson(re));
            return re;
        }

        private static string getItemValue(string itemName, Borrowout_head item)//得到head参数值
        {
            string result = "";
            //if ((itemName.Substring(0,1)=="{")||(itemName.Substring(0,1)=="["))
            try
            {
                switch (itemName.Substring(0, 1))
                {
                    case "{":
                        result = itemName.Substring(1, itemName.Length - 2);
                        break;
                    case "[":
                        itemName = itemName.Substring(1, itemName.Length - 2);
                        Object v = item.GetType().GetProperty(itemName).GetValue(item, null);
                        if (v != null)
                        {
                            if (itemName != "ddate")
                            { result += v.ToString(); }
                            else
                            { result += Convert.ToDateTime(v).ToShortDateString(); }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        private static string getItemValue(string itemName, Borrowout_body item)//得到body参数值
        {
            string result = "";
            //if ((itemName.Substring(0,1)=="{")||(itemName.Substring(0,1)=="["))
            switch (itemName.Substring(0, 1))
            {
                case "{":
                    result = itemName.Substring(1, itemName.Length - 2);
                    break;
                case "[":
                    itemName = itemName.Substring(1, itemName.Length - 2);
                    Object v = item.GetType().GetProperty(itemName).GetValue(item, null);
                    if (v != null)
                    {
                        if ((itemName != "ddate") && (itemName != "backdate"))
                        { result += v.ToString(); }
                        else
                        { result += Convert.ToDateTime(v).ToShortDateString(); }
                    }
                    break;
            }
            return result;
        }
        //组合销售
        private static void getPortfolio(string cinvcode, decimal iquantity, ref MSXML2.IXMLDOMDocument2 dom_body, MSXML2.IXMLDOMNode xnNow, U8Login.clsLoginClass m_ologin)
        {
            string PTOModel = Ufdata.getDataReader(m_ologin.UfDbName, "select bPTOModel from inventory where cinvcode='" + cinvcode + "'");
            string FromSql = " from [dbo].[bom_opcomponent] a inner join bom_parent b on a.BomId=b.BomId inner join bas_part c on b.ParentId=c.PartId inner join bom_bom d on a.BomId=d.BomId inner join bas_part e on a.ComponentId=e.PartId inner join inventory f on e.InvCode=f.cInvCode";
            string WhereSQL = " where c.InvCode='" + cinvcode + "' and d.Status=3 and d.AuditStatus=1";
            string strSql = "select cInvCode,cInvName,baseQty,round(100*(1)/baseQty/bb.sumQty,8) fchildrate,ParentId from (select b.ParentId,f.cInvCode,f.cInvName,a.ComponentId,BaseQtyN/BaseQtyD baseQty"
                + FromSql + WhereSQL + ") aa,("
                + "select COUNT(a.ComponentId) sumQty"
                + FromSql + WhereSQL + ") bb";
            string guid = Guid.NewGuid().ToString();
            try
            {
                if ((PTOModel == "1") || (PTOModel.ToLower() == "true"))
                {
                    //xnNow.attributes.getNamedItem("cparentcode").text = guid;
                    //dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                    DataTable dtComponent = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                    foreach (DataRow dr in dtComponent.Rows)
                    {
                        MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                        //xnNowClone.attributes.getNamedItem("cparentcode").text = "";
                        //xnNowClone.attributes.getNamedItem("cchildcode").text = guid;
                        xnNowClone.attributes.getNamedItem("cinvcode").text = dr["cInvCode"].ToString();
                        xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                        //xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                        //20241111 增加四舍五入
                        decimal iqty=Math.Round( iquantity * Convert.ToDecimal(dr["baseQty"]),2);
                        xnNowClone.attributes.getNamedItem("iquantity").text = iqty.ToString("#.##");
                        //xnNow.attributes.getNamedItem("ipartid").text = dr["cInvCode"].ToString();
                        //xnNowClone.attributes.getNamedItem("fchildqty").text = dr["baseQty"].ToString();
                        //xnNowClone.attributes.getNamedItem("fchildrate").text = dr["fchildrate"].ToString();
                        //xnNowClone.attributes.getNamedItem("inatunitprice").text = "0";
                        //xnNowClone.attributes.getNamedItem("iunitprice").text = "0";
                        //xnNowClone.attributes.getNamedItem("itaxunitprice").text = "0";
                        //xnNowClone.attributes.getNamedItem("imoney").text = "0";
                        //xnNowClone.attributes.getNamedItem("inatmoney").text = "0";
                        //xnNowClone.attributes.getNamedItem("isum").text = "0";
                        //xnNowClone.attributes.getNamedItem("inatsum").text = "0";
                        //xnNowClone.attributes.getNamedItem("itax").text = "0";
                        //xnNowClone.attributes.getNamedItem("inattax").text = "0";
                        //xnNowClone.attributes.getNamedItem("kl").text = "100";
                        //xnNowClone.attributes.getNamedItem("idiscount").text = "0";
                        //xnNowClone.attributes.getNamedItem("inatdiscount").text = "0";
                        /*
                         case "iunitprice":
                                        case "inatunitprice":
                                            xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text =( Convert.ToDecimal(sob.itaxunitprice) * 100 / (100 + taxrate)).ToString();
                                            break;
                                        case "imoney":
                                        case "inatmoney":
                                            xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = (Convert.ToDecimal(sob.isum) * 100 / (100 + taxrate)).ToString();
                                            break;
                                        case "itax":
                                        case "inattax":
                                             xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = (Convert.ToDecimal(sob.isum)-Convert.ToDecimal(sob.isum) * 100 / (100 + taxrate)).ToString();
                                            break;
                                        case "kl":
                                            xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = kl.ToString();
                                            break;
                                        case "idiscount":
                                        case "inatdiscount":
                                            xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = dkl.ToString();
                                            break;
                         */
                        dom_body.selectSingleNode("//rs:data").appendChild(xnNowClone);
                    }
                }
                else
                {
                    MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                    dom_body.selectSingleNode("//rs:data").appendChild(xnNowClone);
                }
            }

            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HYBorrowOutEntity), ex);
            }
        }
    }
}