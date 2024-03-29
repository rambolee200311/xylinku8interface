﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using XylinkU8Interface.Models.TrialSale;
using XylinkU8Interface.Models.DispatchReturnBack;
using XylinkU8Interface.Models.Borrowoutback;
using XylinkU8Interface.Models.Result;
using XylinkU8Interface.Models.OOSOrder;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
namespace XylinkU8Interface.UFIDA
{
    public class STSNEntity
    {
        //其他出库单红字蓝字，售后换货出库-CRM入库 CRM出库
        public static string add_STSN(U8Login.clsLoginClass m_ologin, ClsRequest req, string VouchIdRet)
        {
            string result = "";
            string strSql = "";
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            switch (req.head.category)
            {
                case "售后换货出库-CRM入库":
                case "试⽤业务SN的调换-CRM入库":
                    domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherout_red_sn.xml");
                    break;
                case "售后换货出库-CRM出库":
                case "试⽤业务SN的调换-CRM出库":
                    domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherout_blue_sn.xml");
                    break;
                default:
                    domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherout_red_sn.xml");
                    break;
            }
            

            try
            {
                domHead = getOtherOutDom(m_ologin, VouchIdRet, "domhead");
                domBody = getOtherOutDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord09 where ID=" + VouchIdRet);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);
                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                foreach (ClsRequestBody reqBody in req.body)
                {
                    rowno = 1;
                    if ((!string.IsNullOrEmpty(reqBody.sncode))||(!string.IsNullOrEmpty(reqBody.old_sncode)))
                    {
                        MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                        xnNow.attributes.getNamedItem("ivouchid").text = VouchIdRet;
                        
                        xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                        xnNow.attributes.getNamedItem("cinvcode").text = reqBody.invcode;
                        xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                            "select cwhcode from rdrecord09 where ID=" + VouchIdRet + "");
                        xnNow.attributes.getNamedItem("editprop").text = "A";
                        //switch(req.head.category)
                        //{
                            //case "试⽤业务SN的调换-CRM出库":
                            //    xnNow.attributes.getNamedItem("cinvsn").text = reqBody.new_sncode;  
                            //    xnNow.attributes.getNamedItem("csource").text = "借出借用单";
                            //    break;
                            //case "试⽤业务SN的调换-CRM入库":
                            //    xnNow.attributes.getNamedItem("cinvsn").text = reqBody.old_sncode;
                            //    xnNow.attributes.getNamedItem("csource").text = "借出借用单";
                            //    break;
                            //default:
                                xnNow.attributes.getNamedItem("cinvsn").text = reqBody.sncode;
                        //        break;
                        //}
                        xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,
                           "select a.AutoID from rdrecords09 a inner join rdrecords09_extradefine b on a.AutoID=b.AutoID where a.ID="
                           + VouchIdRet + " and b.cbdefine21='" + reqBody.reqId + "' and a.cInvCode='" + reqBody.invcode + "'");// and a.iquantity="+reqBody.iquantity.ToString());
                        xnNow.attributes.getNamedItem("ufts").text = ufts;
                        domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                        rowno++;
                        
                        /*
                        if (req.head.category == "试⽤业务SN的调换")
                        {
                            MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                            xnNowClone.attributes.getNamedItem("irowno").text = rowno.ToString();
                            xnNowClone.attributes.getNamedItem("cinvsn").text = reqBody.new_sncode;
                            xnNowClone.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,
                           "select a.AutoID from rdrecords09 a inner join rdrecords09_extradefine b on a.AutoID=b.AutoID where a.ID="
                           + VouchIdRet + " and b.cbdefine21='" + reqBody.reqId + "' and a.cInvCode='" + reqBody.invcode + "' and a.iquantity=" +(-1* reqBody.iquantity).ToString());
                            xnNowClone.attributes.getNamedItem("isnprestate").text = "2";
                            domSN.selectSingleNode("//rs:data").appendChild(xnNowClone);
                            rowno++;
                        }
                        */
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherout_red_sn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "09", "add", ref domHead, ref domBody, domSN, ref result, false);
                    if (bResult)
                    { result = ""; }
                    
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        
        //删除其他出库单SNdetail
        public static string del_STSN(U8Login.clsLoginClass m_ologin, string VouchIdRet)
        {
            string result = "";
            string strSql = "";
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domSN1 = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();

            try
            {
                domHead = getOtherOutDom(m_ologin, VouchIdRet, "domhead");
                domBody = getOtherOutDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord09 where ID=" + VouchIdRet);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;

                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);

                //usn.Load(conn, "09", ref domSN, ref domSN1, result, ref domHead, ref domBody, true, true);
                //foreach (MSXML2.IXMLDOMNode doel in domSN.selectSingleNode("//rs:data").childNodes)
                //{
                //    doel.attributes.getNamedItem("editprop").text = "D";
                //}
                DataTable dt = Ufdata.getDatatableFromSql(m_ologin.UfDbName, 
                    "select  autoid,ivouchsid,ivouchid,irowno,cinvcode,cwhcode,isnprestate,isnoperatecount,cinvsn from ST_SNDetail_OtherOut where ivouchid=" + VouchIdRet);
                domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherout_red_sn.xml");
                MSXML2.IXMLDOMNode xmlModel = domSN.selectSingleNode("//rs:data//z:row");
                foreach (DataRow dr in dt.Rows)
                {
                    MSXML2.IXMLDOMNode xmlNow = xmlModel.cloneNode(true);
                    xmlNow.attributes.getNamedItem("editprop").text = "D";
                    xmlNow.attributes.getNamedItem("ufts").text = ufts;
                    xmlNow.attributes.getNamedItem("autoid").text =dr["autoid"].ToString();
                    xmlNow.attributes.getNamedItem("ivouchsid").text = dr["ivouchsid"].ToString();
                    xmlNow.attributes.getNamedItem("ivouchid").text = dr["ivouchid"].ToString();
                    xmlNow.attributes.getNamedItem("irowno").text = dr["irowno"].ToString();
                    xmlNow.attributes.getNamedItem("cinvcode").text = dr["cinvcode"].ToString();
                    xmlNow.attributes.getNamedItem("cwhcode").text = dr["cwhcode"].ToString();
                    xmlNow.attributes.getNamedItem("isnprestate").text = dr["isnprestate"].ToString();
                    xmlNow.attributes.getNamedItem("isnoperatecount").text = dr["isnoperatecount"].ToString();
                    xmlNow.attributes.getNamedItem("cinvsn").text = dr["cinvsn"].ToString();
                    domSN.selectSingleNode("//rs:data").appendChild(xmlNow);
                }
                domSN.selectSingleNode("//rs:data").removeChild(xmlModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherout_red_sn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "09", "delete", ref domHead, ref domBody, domSN, ref result, false);
                    if (bResult)
                    { result = ""; }

                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
           

        //销售出库单红字
        public static void add_STSN(U8Login.clsLoginClass m_ologin,string cType,DispatchReturnBack so,string VouchIdRet)
        {
            string result = "";
            string strSql = "";
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleouthead_red_sn.xml");
            try 
            {
                //getSaleOutDom(m_ologin, VouchIdRet, ref domHead, ref domBody);
                domHead = getSaleOutDom(m_ologin, VouchIdRet, "domhead");
                domBody = getSaleOutDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord32 where ID=" + VouchIdRet);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;
                //domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("editprop").text = "U";
                foreach (MSXML2.IXMLDOMNode xn in domSN.selectSingleNode("//rs:data").childNodes)
                {
                    xn.attributes.getNamedItem("editprop").text = "A";
                }
                domHead.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleouthead_red_sn111.xml");
                domBody.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleoutbody_red_sn111.xml");
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);
                
                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                string idautoidcinvcode = "";
                foreach(DispatchReturnBack_body sob in so.body)
                {
                    #region//oldsn
                    //if (cType=="32")
                    //{
                        //strSql="select a.id,b.autoid,b.cinvcode,cinvname,a.cwhcode,e.cwhname,b.idlsid"
                        //        +" from rdrecord32 a inner join rdrecords32 b on a.ID=b.ID"
                        //        +" left join rdrecords32_extradefine c on c.AutoID=b.AutoID"
                        //        +" inner join inventory d on b.cInvCode=d.cInvCode"
                        //        +" inner join warehouse e on a.cWhCode=e.cWhCode"
                        //        + " where c.cbdefine21='" + sob.requestid + "' and b.cinvcode='" + sob.cinv_code + "' and a.id=" + VouchIdRet;
                        //strSql = "select a.ID,a.AutoID,d.cbdefine21,g.cInvCode,g.cInvSN,h.cInvName,a1.cwhcode,i.cWhName,a.iDLsID"
                        //        +" from  rdrecords32 a" 
                        //        +" inner join rdrecord32 a1 on a.ID=a1.ID"
                        //        + " inner join DispatchLists b on a.iDLsID=b.iDLsID and b.iQuantity<0" 
                        //        +" inner join SO_SODetails c on b.isosid=c.isosid" 
                        //        +" inner join SO_SODetails_extradefine d on c.iSOsID=d.iSOsID"
                        //        + " inner join DispatchLists e on e.iSOsID=c.iSOsID and e.iQuantity>0"
                        //        +" inner join rdrecords32 f on f.iDLsID=e.iDLsID and f.iQuantity>0"
                        //        +" inner join ST_SNDetail_SaleOut g on f.AutoID=g.iVouchsID and f.ID=g.iVouchID"
                        //        +" inner join inventory h on g.cInvCode=h.cInvCode"
                        //        +" inner join warehouse i on a1.cWhCode=i.cWhCode"
                        //        +" where a.ID=" + VouchIdRet                               
                        //        +" and d.cbdefine21='"+sob.ori_req_id+"'"
                        //        + " order by a.ID,a.AutoID,g.cInvCode";
                        //LogHelper.WriteLog(typeof(STSNEntity), strSql);
                    //}
                    //DataTable dt=Ufdata.getDatatableFromSql(m_ologin.UfDbName,strSql);
                    //if (dt!=null)
                    //{
                    //    if (dt.Rows.Count>=1)
                    //    {
                    //        DataRow dr = dt.Rows[0];
                    //        if (dr["id"].ToString()+dr["autoid"].ToString()+dr["cinvcode"].ToString()!=idautoidcinvcode)
                    //        {
                    //            idautoidcinvcode = dr["id"].ToString() + dr["autoid"].ToString() + dr["cinvcode"].ToString();
                    //            rowno = 1;
                    //        }
                    //        else
                    //        { rowno++; }
                    //        //int i = 1;
                    //        //foreach (DataRow dr in dt.Rows)
                    //        //{
                            
                    //            MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                    //            xnNow.attributes.getNamedItem("ivouchid").text = dr["id"].ToString();
                    //            xnNow.attributes.getNamedItem("ivouchsid").text = dr["autoid"].ToString();
                    //            xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                    //            xnNow.attributes.getNamedItem("cinvcode").text = dr["cinvcode"].ToString();
                    //            xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                    //            xnNow.attributes.getNamedItem("cwhcode").text = dr["cwhcode"].ToString();
                    //            xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                    //            xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                    //            //xnNow.attributes.getNamedItem("cinvsn").text = sob.sn;
                    //            xnNow.attributes.getNamedItem("ufts").text = ufts;
                    //            domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                    //            //i++;
                    //        //}
                    //    }
                    //}
                    #endregion

                    foreach(BodyDetail bodyDetail in sob.detail)
                    {
                        rowno = 1;
                        foreach(SNinfo snInfo in bodyDetail.sncodes)
                        {
                            if (!string.IsNullOrEmpty(snInfo.sncode))
                            {
                                MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                                xnNow.attributes.getNamedItem("ivouchid").text = VouchIdRet;
                                xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                   "select * from rdrecords32 a inner join rdrecords32_extradefine b on a.AutoID=b.AutoID where a.ID=" + VouchIdRet + " and b.cbdefine21='" + sob.req_id + "' and a.cInvCode='" + bodyDetail.cinv_code + "'");
                                xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                                xnNow.attributes.getNamedItem("cinvcode").text = bodyDetail.cinv_code;
                                //xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                                xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                    "select cwhcode from warehouse where cwhname='"+sob.cwhname+"'");
                                //xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                                //xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                                xnNow.attributes.getNamedItem("cinvsn").text = snInfo.sncode;
                                xnNow.attributes.getNamedItem("ufts").text = ufts;
                                domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                                rowno++;
                            }
                        }
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleoutbody_red_domsn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "32", "add", ref domHead, ref domBody, domSN, ref result, false);
                    //bool bResult = usn.AddNewline(ref domHead, ref domBody, ref domSN, "32R");
                    if (bResult)
                    { verifySaleOut(m_ologin, VouchIdRet); }
                    result = bResult.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(STSNEntity),ex);
            }
            finally
            {
                conn.Close();
            }
        }
        //借用转销售-销售出库单
        public static void add_STSN(U8Login.clsLoginClass m_ologin, string cType, TrialSale so, string VouchIdRet,ref Result re)
        {
            string result = "";
            string strSql = "";
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleouthead_red_sn.xml");

            try
            {
                //getSaleOutDom(m_ologin, VouchIdRet, ref domHead, ref domBody);
                domHead = getSaleOutDom(m_ologin, VouchIdRet, "domhead");
                domBody = getSaleOutDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord32 where ID=" + VouchIdRet);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;
                //domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("editprop").text = "U";
                foreach (MSXML2.IXMLDOMNode xn in domSN.selectSingleNode("//rs:data").childNodes)
                {
                    xn.attributes.getNamedItem("editprop").text = "A";
                }
                //domHead.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleouthead_sn111.xml");
                //domBody.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleoutbody_sn111.xml");
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);

                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                string idautoidcinvcode = "";
                foreach (TrialSale_body sob in so.body)
                {                  

                    foreach (TrialSale_body_detail bodyDetail in sob.detail)
                    {
                        rowno = 1;
                        foreach (TrialSale_body_detail_sncodes snInfo in bodyDetail.sncodes)
                        {
                            if (!string.IsNullOrEmpty(snInfo.sncode))
                            {
                                MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                                xnNow.attributes.getNamedItem("ivouchid").text = VouchIdRet;
                                xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                   "select * from rdrecords32 a inner join rdrecords32_extradefine b on a.AutoID=b.AutoID where a.ID=" + VouchIdRet + " and b.cbdefine21='" + sob.req_id + "' and a.cInvCode='" + bodyDetail.cinv_code + "'");
                                xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                                xnNow.attributes.getNamedItem("cinvcode").text = bodyDetail.cinv_code;
                                //xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                                xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                    "select cwhcode from warehouse where cwhname='" + sob.cwhname + "'");
                                //xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                                //xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                                xnNow.attributes.getNamedItem("cinvsn").text = snInfo.sncode;
                                xnNow.attributes.getNamedItem("ufts").text = ufts;
                                domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                                rowno++;
                            }
                        }
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                //domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleoutbody_domsn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "32", "add", ref domHead, ref domBody, domSN, ref result, false);
                    //bool bResult = usn.AddNewline(ref domHead, ref domBody, ref domSN, "32R");
                    //result = bResult.ToString();
                    if (bResult)
                    {
                        //re.u8code += ",序列号生成成功";
                        if (bResult)
                        { verifySaleOut(m_ologin, VouchIdRet); }
                    }
                    else
                    {
                        re.recode = "4444";
                        re.remsg += result;
                    }
                }
                else//20230627无SN自动审核
                {
                    verifySaleOut(m_ologin, VouchIdRet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(STSNEntity), ex);
            }
            finally
            {
                conn.Close();
            }
        }
        //退货入库-其他入库单
        public static  Result add_otherinSTSN(BorrowOutBack bob)
        {
            string result = "";
            string strSql = "";
            string vouchid;
            string VouchIdRet = bob.head.cmemo;
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            Result re = new Result();
            re.oacode = bob.head.ccode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(bob.companycode);
            if (m_ologin==null)
            {
                re.recode = "1111";
                re.remsg = "登录失败";
                return re;
            }           
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherinsn.xml");
            strSql = "select id from RdRecord08 where ccode='"+VouchIdRet+"'";
            vouchid = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
            try
            {
                domHead =getOtherInDom(m_ologin, VouchIdRet, "domhead");
                domBody =getOtherInDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord08 where ccode='" + VouchIdRet+"'");
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;
                
                foreach (MSXML2.IXMLDOMNode xn in domSN.selectSingleNode("//rs:data").childNodes)
                {
                    xn.attributes.getNamedItem("editprop").text = "A";
                }
                domHead.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_head_111.xml");
                domBody.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_body_111.xml");
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);

                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                foreach(BorrowOutBack_body body in bob.body)
                {
                    foreach(BorrowOutBack_body_detail detail in body.detail)
                    {
                        foreach(BorrowOutBack_body_detail_sncodes sncode in detail.sncodes)
                        {
                            MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                            xnNow.attributes.getNamedItem("ivouchid").text = vouchid;
                            strSql = "select a.autoid from RdRecords08 a left join HY_DZ_BorrowOutBacks b on a.iDebitIDs=b.AutoID"
                                    +" left join HY_DZ_BorrowOuts c on b.UpAutoID=c.AutoID"
                                    + " left join HY_DZ_BorrowOuts_extradefine d on c.AutoID=d.AutoID"
                                    +" where a.id="+vouchid+" and a.cinvcode='"+detail.cinv_code+"' and d.cbdefine21='"+body.ori_req_id+"'";
                            xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,strSql);
                            xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                            xnNow.attributes.getNamedItem("cinvcode").text = detail.cinv_code;
                            //xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                            xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                " select cWhCode from RdRecord08 where id=" + vouchid);
                            //xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                            //xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                            xnNow.attributes.getNamedItem("cinvsn").text = sncode.sncode;
                            xnNow.attributes.getNamedItem("ufts").text = ufts;
                            domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                            rowno++;
                        }
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_domsn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "08", "add", ref domHead, ref domBody, domSN, ref result, false);
                    if (bResult)
                    {
                        re.recode = "0";
                        
                    }
                    else
                    {
                        re.recode="3333";
                        re.remsg = result;
                        return re;
                    }
                }

                re.recode = "0";
            }
            catch (Exception ex)
            {
                re.recode = "2222";
                re.remsg = ex.Message;
                return re;
            }

            verifyOtherIn(m_ologin, vouchid);

            return re;
        }
        //20230903 
        //归还（归还、⽣成其他⼊库单并审核通过）
        public static Result add_BorrowReturnSN(XylinkU8Interface.Models.BorrowReturn.InMain inMain)
        {
            string result = "";
            string strSql = "";
            string vouchid;
            string VouchIdRet = inMain.head.cmemo.ToString();
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            Result re = new Result();
            re.oacode = inMain.head.ordcode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode);
            if (m_ologin == null)
            {
                re.recode = "1111";
                re.remsg = "登录失败";
                return re;
            }
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherinsn.xml");
            strSql = "select id from RdRecord08 where ccode='" + VouchIdRet + "'";
            vouchid = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
            try
            {
                domHead = getOtherInDom(m_ologin, VouchIdRet, "domhead");
                domBody = getOtherInDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord08 where ccode='" + VouchIdRet + "'");
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;

                foreach (MSXML2.IXMLDOMNode xn in domSN.selectSingleNode("//rs:data").childNodes)
                {
                    xn.attributes.getNamedItem("editprop").text = "A";
                }
                domHead.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_head_111.xml");
                domBody.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_body_111.xml");
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);

                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                foreach ( XylinkU8Interface.Models.BorrowReturn.InBody body in inMain.body)
                {
                    foreach (XylinkU8Interface.Models.BorrowReturn.InDetail detail in body.detail)
                    {
                        if (detail.sncodes != null)
                        {
                            foreach (XylinkU8Interface.Models.BorrowReturn.InSncode sncode in detail.sncodes)
                            {
                                MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                                xnNow.attributes.getNamedItem("ivouchid").text = vouchid;
                                strSql = "select a.autoid from RdRecords08 a left join HY_DZ_BorrowOutBacks b on a.iDebitIDs=b.AutoID"
                                        + " left join HY_DZ_BorrowOuts c on b.UpAutoID=c.AutoID"
                                    //+ " left join HY_DZ_BorrowOuts_extradefine d on c.AutoID=d.AutoID"
                                        + " where a.id=" + vouchid + " and a.cinvcode='" + detail.cinvCode + "' and c.AutoID='" + body.oriU8RowId + "'";
                                xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                                xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                                xnNow.attributes.getNamedItem("cinvcode").text = detail.cinvCode;
                                //xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                                xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                    " select cWhCode from WareHouse where cwhname='" + body.cwhname + "'");
                                //xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                                //xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                                xnNow.attributes.getNamedItem("cinvsn").text = sncode.sncode;
                                xnNow.attributes.getNamedItem("ufts").text = ufts;
                                domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                                rowno++;
                            }
                        }
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_domsn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "08", "add", ref domHead, ref domBody, domSN, ref result, false);
                    if (bResult)
                    {
                        re.recode = "0";

                    }
                    else
                    {
                        re.recode = "3333";
                        re.remsg = result;
                        return re;
                    }
                }

                re.recode = "0";
            }
            catch (Exception ex)
            {
                re.recode = "2222";
                re.remsg = ex.Message;
                return re;
            }

            verifyOtherIn(m_ologin, vouchid);

            return re;
        }
        //借用转销售-其他入库单
        public static Result add_otherinSTSN(TrialSale bob)
        {
            string result = "";
            string strSql = "";
            string vouchid;
            string VouchIdRet = bob.head.cmemo;
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            Result re = new Result();
            re.oacode = bob.head.ccode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(bob.companycode);
            if (m_ologin == null)
            {
                re.recode = "1111";
                re.remsg = "登录失败";
                return re;
            }
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherinsn.xml");
            strSql = "select id from RdRecord08 where ccode='" + VouchIdRet + "'";
            vouchid = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
            try
            {
                domHead = getOtherInDom(m_ologin, VouchIdRet, "domhead");
                domBody = getOtherInDom(m_ologin, VouchIdRet, "dombody");
                strSql ="select convert(money,ufts) ufts from rdrecord08 where ccode='" + VouchIdRet + "'";
                LogHelper.WriteLog(typeof(STSNEntity),strSql);
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;

                foreach (MSXML2.IXMLDOMNode xn in domSN.selectSingleNode("//rs:data").childNodes)
                {
                    xn.attributes.getNamedItem("editprop").text = "A";
                }
                domHead.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_head_111.xml");
                domBody.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_body_111.xml");
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, result);

                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                foreach (TrialSale_body body in bob.body)
                {
                    foreach (TrialSale_body_detail detail in body.detail)
                    {
                        foreach (TrialSale_body_detail_sncodes sncode in detail.sncodes)
                        {
                            MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                            xnNow.attributes.getNamedItem("ivouchid").text = vouchid;
                            strSql = "select a.autoid from RdRecords08 a left join HY_DZ_BorrowOutBacks b on a.iDebitIDs=b.AutoID"
                                    + " left join HY_DZ_BorrowOuts c on b.UpAutoID=c.AutoID"
                                    + " left join HY_DZ_BorrowOuts_extradefine d on c.AutoID=d.AutoID"
                                    + " where a.id=" + vouchid + " and a.cinvcode='" + detail.cinv_code + "' and d.cbdefine21='" + body.ori_req_id + "'";                            
                            LogHelper.WriteLog(typeof(STSNEntity), strSql);
                            xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                            xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                            xnNow.attributes.getNamedItem("cinvcode").text = detail.cinv_code;
                            //xnNow.attributes.getNamedItem("cinvname").text = dr["cinvname"].ToString();
                            strSql ="select cWhCode from RdRecord08 where id=" + vouchid;
                            LogHelper.WriteLog(typeof(STSNEntity), strSql);
                            xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,strSql);
                            
                            //xnNow.attributes.getNamedItem("cwhname").text = dr["cwhname"].ToString();
                            //xnNow.attributes.getNamedItem("idlsid").text = dr["idlsid"].ToString();
                            xnNow.attributes.getNamedItem("cinvsn").text = sncode.sncode;
                            xnNow.attributes.getNamedItem("ufts").text = ufts;
                            domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                            rowno++;
                        }
                    }
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherin_domsn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "08", "add", ref domHead, ref domBody, domSN, ref result, false);
                    if (bResult)
                    {
                        re.recode = "0";
                        
                    }
                    else
                    {
                        re.recode = "3333";
                        re.remsg = result;
                        return re;
                    }
                }
                verifyOtherIn(m_ologin, vouchid);
                re.recode = "0";
            }
            catch (Exception ex)
            {
                re.recode = "2222";
                re.remsg = ex.Message;
                return re;
            }



            return re;
        }
        //获取销售出库单数据
        public static MSXML2.DOMDocument getSaleOutDom(U8Login.clsLoginClass m_ologin, string VouchIdRet,string ctype)
        {
            MSXML2.DOMDocument domResult = new MSXML2.DOMDocument();
            try
            {
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = m_ologin;

                //第三步：设置API地址标识(Url)
                //当前API：装载单据的地址标识为：U8API/saleout/Load
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Load");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:32
                broker.AssignNormalValue("sVouchType", "32");

                //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
                broker.AssignNormalValue("sWhere", "id=" + VouchIdRet);

                //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument30Class();
                broker.AssignNormalValue("domPos", domPos);

                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
                broker.AssignNormalValue("bGetBlank", false);

                //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
                broker.AssignNormalValue("sBodyWhere_Order", "cinvcode");

                bool bResult = broker.Invoke();
                //domHead = (MSXML2.DOMDocument)broker.GetResult("domhead");// as MSXML2.DOMDocument30Class;
                //domBody = (MSXML2.DOMDocument)broker.GetResult("dombody");// as MSXML2.DOMDocument30Class;
                domResult = (MSXML2.DOMDocument)broker.GetResult(ctype);
            }
            catch(Exception ex)
            {
                domResult = null;
            }
            return domResult;
        }
        //获取其他入库单数据
        public static MSXML2.DOMDocument getOtherInDom(U8Login.clsLoginClass m_ologin, string VouchIdRet,string ctype)
        {
             MSXML2.DOMDocument domResult = new MSXML2.DOMDocument();
             try
             {
                 //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                 U8EnvContext envContext = new U8EnvContext();
                 envContext.U8Login = m_ologin;

                 //第三步：设置API地址标识(Url)
                 //当前API：装载单据的地址标识为：U8API/otherin/Load
                 U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherin/Load");

                 //第四步：构造APIBroker
                 U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                 //第五步：API参数赋值

                 //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:08
                 broker.AssignNormalValue("sVouchType", "08");

                 //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
                 broker.AssignNormalValue("sWhere", "ccode='" + VouchIdRet + "'");

                 //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                 MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument(); ;
                 broker.AssignNormalValue("domPos", domPos);

                 //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                 //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
                 broker.AssignNormalValue("bGetBlank", false);

                 //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
                 broker.AssignNormalValue("sBodyWhere_Order", "autoid");

                 bool bResult = broker.Invoke();
                 domResult = (MSXML2.DOMDocument)broker.GetResult(ctype);
             }
            catch(Exception ex)
             {
                //do nothing
             }
            return domResult;
        }
        //获取其他出库单数据
        public static MSXML2.DOMDocument getOtherOutDom(U8Login.clsLoginClass m_ologin, string VouchIdRet, string ctype)
        {
            MSXML2.DOMDocument domResult = new MSXML2.DOMDocument();
            try
            {
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = m_ologin;

                //第三步：设置API地址标识(Url)
                //当前API：装载单据的地址标识为：U8API/otherin/Load
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Load");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:08
                broker.AssignNormalValue("sVouchType", "09");

                //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
                broker.AssignNormalValue("sWhere", "id=" + VouchIdRet + "");

                //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument(); ;
                broker.AssignNormalValue("domPos", domPos);

                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
                broker.AssignNormalValue("bGetBlank", false);

                //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
                broker.AssignNormalValue("sBodyWhere_Order", "autoid");

                bool bResult = broker.Invoke();
                domResult = (MSXML2.DOMDocument)broker.GetResult(ctype);
            }
            catch (Exception ex)
            {
                //do nothing
            }
            return domResult;
        }
        
        //审核其他入库单
        public static void verifyOtherIn(U8Login.clsLoginClass m_ologin,string VouchIdRet)
        {
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = m_ologin;

            //第三步：设置API地址标识(Url)
            //当前API：审核单据的地址标识为：U8API/otherin/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherin/Audit");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：08
            broker.AssignNormalValue("sVouchType", "08");

            //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
            broker.AssignNormalValue("VouchId", VouchIdRet);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象：调用方控制事务时需要传入连接对象
            broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

            //给普通参数TimeStamp赋值。此参数的数据类型为System.Object，此参数按值传递，表示单据时间戳，用于检查单据是否修改，空串时不检查
            broker.AssignNormalValue("TimeStamp", null);

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
            broker.AssignNormalValue("domMsg", domMsg);

            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock", false);

            //给普通参数bList赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示传入空串
            broker.AssignNormalValue("bList", false);

            //给普通参数MakeWheres赋值。此参数的数据类型为VBA.Collection，此参数按值传递，表示传空
            broker.AssignNormalValue("MakeWheres", null);

            //给普通参数sWebXml赋值。此参数的数据类型为System.String，此参数按值传递，表示传入空串
            broker.AssignNormalValue("sWebXml", "");

            //给普通参数oGenVouchIds赋值。此参数的数据类型为Scripting.IDictionary，此参数按值传递，表示返回审核时自动生成的单据的id列表,传空
            broker.AssignNormalValue("oGenVouchIds", null);
            //broker.Invoke();
            //第六步：调用API
            if (!broker.Invoke())
            {
                //错误处理
                Exception apiEx = broker.GetException();
                if (apiEx != null)
                {
                    if (apiEx is MomSysException)
                    {
                        MomSysException sysEx = apiEx as MomSysException;
                        //Console.WriteLine("系统异常：" + sysEx.Message);
                        //re.recode = "999";
                        //re.remsg = "系统异常：" + sysEx.Message;
                        LogHelper.WriteLog(typeof(STSNEntity), "verifyOtherIn:" + "系统异常：" + sysEx.Message);
                        //return re;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);                        
                        LogHelper.WriteLog(typeof(STSNEntity), "verifyOtherIn:" + "API异常：" + bizEx.Message);
                        //return re;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        LogHelper.WriteLog(typeof(STSNEntity), "verifyOtherIn:"+ "异常原因：" + exReason);
                        //return re;
                    }
                }
                              
            }
            String errMsgRet = "";
            if (broker.GetResult("errMsg") != null)
            { errMsgRet = broker.GetResult("errMsg").ToString(); }
            if (!string.IsNullOrEmpty(errMsgRet))
            { LogHelper.WriteLog(typeof(STSNEntity), "verifyOtherIn:" + "审核失败：" + errMsgRet); }
            //结束本次调用，释放API资源
            broker.Release(); 
        }

        //审核销售出库单
        public static void verifySaleOut(U8Login.clsLoginClass m_ologin,string VouchIdRet)
        {
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = m_ologin;

            //第三步：设置API地址标识(Url)
            //当前API：审核单据的地址标识为：U8API/saleout/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Audit");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：32
            broker.AssignNormalValue("sVouchType", "32");

            //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
            broker.AssignNormalValue("VouchId", VouchIdRet);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象：调用方控制事务时需要传入连接对象
            broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

            //给普通参数TimeStamp赋值。此参数的数据类型为System.Object，此参数按值传递，表示单据时间戳，用于检查单据是否修改，空串时不检查
            broker.AssignNormalValue("TimeStamp", null);

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
            broker.AssignNormalValue("domMsg", domMsg);

            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock", false);

            //给普通参数bList赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示传入空串
            broker.AssignNormalValue("bList", false);

            //给普通参数MakeWheres赋值。此参数的数据类型为VBA.Collection，此参数按值传递，表示传空
            broker.AssignNormalValue("MakeWheres", null);

            //给普通参数sWebXml赋值。此参数的数据类型为System.String，此参数按值传递，表示传入空串
            broker.AssignNormalValue("sWebXml", "");

            //给普通参数oGenVouchIds赋值。此参数的数据类型为Scripting.IDictionary，此参数按值传递，表示返回审核时自动生成的单据的id列表,传空
            broker.AssignNormalValue("oGenVouchIds", null);

            //broker.Invoke();
            //第六步：调用API
            if (!broker.Invoke())
            {
                //错误处理
                Exception apiEx = broker.GetException();
                if (apiEx != null)
                {
                    if (apiEx is MomSysException)
                    {
                        MomSysException sysEx = apiEx as MomSysException;
                        //Console.WriteLine("系统异常：" + sysEx.Message);
                        //re.recode = "999";
                        //re.remsg = "系统异常：" + sysEx.Message;
                        LogHelper.WriteLog(typeof(STSNEntity), "verifySaleOut:" + "系统异常：" + sysEx.Message);
                        //return re;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);                        
                        LogHelper.WriteLog(typeof(STSNEntity), "verifySaleOut:" + "API异常：" + bizEx.Message);
                        //return re;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        LogHelper.WriteLog(typeof(STSNEntity), "verifySaleOut:" + "异常原因：" + exReason);
                        //return re;
                    }
                    
                }
                
            }
            String errMsgRet = "";
            if (broker.GetResult("errMsg") != null)
            { errMsgRet = broker.GetResult("errMsg").ToString(); }
            if (!string.IsNullOrEmpty(errMsgRet))
            { LogHelper.WriteLog(typeof(STSNEntity), "verifySaleOut:" + "审核失败：" + errMsgRet); }
            //结束本次调用，释放API资源
            broker.Release();
        }
    }
}