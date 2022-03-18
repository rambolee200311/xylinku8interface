using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using HY_DZ_BorrowOutBack;
using HYBorrowOut.UFIDA;
//using UFIDA.U8.MomServiceCommon;
//using UFIDA.U8.U8MOMAPIFramework;
//using UFIDA.U8.U8APIFramework;
//using UFIDA.U8.U8APIFramework.Meta;
//using UFIDA.U8.U8APIFramework.Parameter;
//using MSXML2;

namespace HYBorrowOut
{
    public partial class WebHelper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(Get_Head("body"));
            //Response.Write(setDomback());
            //Response.Write(getDomback());
            //Response.Write(getDomBackEai());
            //Response.Write(setBorrowOutBack());
            //Response.Write(setDomBorrow());
            //Response.Write(getDomSN());
            //Response.Write(getCCode());
           // Response.Write(getDomBorrowOutChange());
            //Response.Write(setDomBorrowOutChange());
        }
        public static string Get_Head(string type)
        {
            string strResult = "it is ok";
            XmlDocument xmlMode = new XmlDocument();
            xmlMode.Load("D:\\o" + type + "_1111.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement xdata = xmlDoc.CreateElement("data");

            XmlNode xnode = xmlMode.SelectSingleNode("xml").ChildNodes[1].FirstChild;
            foreach (XmlAttribute xatt in xnode.Attributes)
            {

                XmlElement xel = xmlDoc.CreateElement(xatt.Name.ToString());
                xel.InnerText ="{"+xatt.Value+"}";
                xdata.AppendChild(xel);

            }
            xmlDoc.AppendChild(xdata);
            xmlDoc.Save("D:\\borrrowout_" + type + "_model.xml");
            return strResult;
        }
        public string getDomback()
        {
            string strResult = "it is ok";
            U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            ADODB.Connection conn = new ADODB.Connection();
            conn.Open(m_ologin.UfDbName);
            HY_DZ_BorrowOutBack.clsBorrowOutSrvClass cbosc = new clsBorrowOutSrvClass();
            cbosc.Init(m_ologin);
            string where = "id=1000000033";
            string errMsg = "";
            MSXML2.IXMLDOMDocument2 ohead = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 obody = new MSXML2.DOMDocument60();

            bool bResult = cbosc.LoadVoucher(where, ref ohead, ref obody, ref errMsg);
            if (!bResult)
            {
                strResult = errMsg;
            }
            else
            {
                //ohead.save("d:\\borrowoutback_head_20210103.xml");
                //obody.save("d:\\borrowoutback_body_20210103.xml");
            }
            errMsg = "HYJCGH005";
            HY_DZ_BorrowOutBack.clsVouchServerClass dcobClass = new clsVouchServerClass();
            
            ohead = dcobClass.GetDomHead(ref conn, ref errMsg, ref where);
            obody = dcobClass.GetDomBody(ref conn, ref errMsg, ref where);
            ohead.save("d:\\borrowoutback_head_20210103.xml");
            obody.save("d:\\borrowoutback_body_20210103.xml");
            conn.Close();
            return strResult;
        }
        public string setDomback()
        {
            string strResult = "it is ok";

            U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            //HY_DZ_BorrowOut.clsBorrowOutSrvClass cbosc1 = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
            HY_DZ_BorrowOutBack.clsBorrowOutSrvClass cbosc = new clsBorrowOutSrvClass();
            //HY_DZ_BorrowOutBack.ClsInterFaceClass cifc = new ClsInterFaceClass();
            cbosc.Init(m_ologin);
            //cbosc1.Init(m_ologin);
            //HY_DZ_BorrowOut.InvokeApiA iaa = new HY_DZ_BorrowOut.InvokeApiA();
            int svouchid = 1000000035;
            int vouchid = 1000000009;
            string errMsg = "";
            bool insert = true;
            bool bResult = false;
            MSXML2.IXMLDOMDocument2 ohead = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 obody = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 ohead1 = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 obody1 = new MSXML2.DOMDocument60();


            //string where = "id=1000000001";
            //cbosc1.LoadVoucher(where, ref ohead1, ref obody1, ref errMsg);
            ohead.load("d:\\borrowoutback_head_20210103.xml");
            obody.load("d:\\borrowoutback_body_20210103.xml");
            ohead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
            obody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
            errMsg = "JCJY202010190006";
            //string vouchcode = "0000000002";
            //bResult = iaa.VoucherAddSave(m_ologin, ohead, obody);
            //bool bResult = cbosc.CheckSubmit("HY_DZ_BorrowOutBack", "ID", ref vouchcode);
            //object obj1=cbosc.ExecRequestAudit(ref vouchcode);
            //cifc.set_Business(cbosc);
            //vouchcode=cifc.GetcCode(vouchid);
            //cbosc.
            bResult = cbosc.SaveVouch(ref ohead, ref obody, insert, ref errMsg, ref vouchid);//, ref ohead1);
            //bResult = cbosc.MakeVouchFromBorrowOut(svouchid, ref errMsg, ref vouchid);
            if (!bResult)
            {
                strResult = errMsg;
            }
            else
            {
                strResult = vouchid.ToString();
                
            }
            //bResult =cbosc.Verify(strResult,ref errMsg);
            //if (!bResult)
            //{
            //    strResult = errMsg;
            //}
            //else
            //{
            //    strResult = vouchid.ToString();

            //}
            //strResult = cbosc.PushOtherIn(vouchid);
            return strResult;
        }
    
        public string getDomBackEai()
        {
            string strResult = "it is ok";

            //U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            //HY_DZ_BorrowOutBack.InvokeApiClass iac = new InvokeApiClass();
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\BorrowOut_Query.xml");
            //strResult = iac.transact(xmlDoc.OuterXml, m_ologin);
            

            return strResult;
        }
        public string setBorrowOutBack()
        {
            string strResult = "it is ok";
            //string errMsg = "";
            //U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            //HY_DZ_BorrowOutBack.clsBorrowOutSrvClass cbosc = new clsBorrowOutSrvClass();
            //cbosc.Init(m_ologin);
            //int svouchid = 1000000004;
            //bool bResult = cbosc.MakeVouchFromBorrowOut(svouchid, ref errMsg, ref svouchid);
            //if (!bResult)
            //{
            //    strResult = errMsg;
            //}
            //else
            //{
            //    strResult = svouchid.ToString();
            //}
            return strResult;
        }
    
        public string setDomBorrow()
        {
            string strResult = "";
            //ADODB.Connection conn = new ADODB.Connection();
            //try
            //{
            //    //1 登录U8

            //    U8Login.clsLogin m_login = U8LoginEntity.getU8LoginEntityInterop("995");

            //    //conn.ConnectionString = m_login.UfDbName;
            //    //conn.Open();
            //    HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
            //    bool billNo = cls_Borrow_Out.Init(m_login);
            //    MSXML2.IXMLDOMDocument2 oHead = new MSXML2.DOMDocument30Class();
            //    MSXML2.IXMLDOMDocument2 oBody = new MSXML2.DOMDocument30Class();
            //    string err_Msg = "";
            //    int vouch_Id = 1000002560;
            //    oHead.load("d:\\ohead_1111.xml");
            //    oBody.load("d:\\obody_1111.xml");
            //    //string vouchcode = "JCJY202009010023";
            //    //object obj1= cls_Borrow_Out.ExecRequestAudit(vouchcode);
            //    //
            //    //bool billNo = cls_Borrow_Out.GetBillNumberChecksucceed(ref oHead, ref vouchcode, err_Msg);
            //    //billNo = cls_Borrow_Out.GetBillNumberChecksucceed(ref oHead, ref vouchcode, ref err_Msg);
            //    //billNo = cls_Borrow_Out.ExecSubOpen(vouchcode, ref err_Msg);
            //    cls_Borrow_Out.SaveVouch(ref oHead, ref oBody, false, ref err_Msg, ref vouch_Id, ref conn);
            //    //HY_DZ_BorrowOut.InvokeApiAClass iac = new HY_DZ_BorrowOut.InvokeApiAClass();
            //    //iac.VoucherAddSave(m_login, oHead, oBody);
            //    //err_Msg = iac.errMsg;
            //    if (!string.IsNullOrEmpty(err_Msg))
            //    {
            //        strResult = err_Msg;
            //    }
            //    else
            //    {
            //        strResult = vouch_Id.ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    strResult = ex.Message;
            //}
            //finally
            //{
            //    //conn.Close();
            //}
            return strResult;
        }
    
        public string getDomSN()
        {
            string result = "";
            U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            UFSTSNCO.clsUFSTSNCOClass snco = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn=new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 domSN1 = new MSXML2.DOMDocument60();
            conn.Open(m_ologin.UfDbName);
            try
            {
                snco.Init(m_ologin,conn,result);
                domHead.load("d:\\app\\otherinhead111.xml");
                domHead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                domBody.load("d:\\app\\otherinbody111.xml");
                domBody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
                snco.Load(conn, "08", ref domSN, ref domSN1, result, domHead, domBody);
                domSN.save("D:\\APP\\otherinsn111.xml");
            }
            catch(Exception ex)
            {
                //
                result = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        
        public string getCCode()
        {
            string strResult="";
            string strSource = "成功生成[1]张其他入库单,单号[QTRK202110030003]";
            if (strSource.IndexOf("成功")>=0)
            {
                int iBegin = strSource.IndexOf("单号[");
                strSource = strSource.Substring(iBegin);
                strResult = strSource.Substring(3, strSource.Length - 4);
                
            }
            return strResult;
        }

        public string getDomBorrowOutChange()
        {
            string result = "id=1000000001";
            U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            HY_DZ_BorrowOutChange.clsBorrowOutSrvClass boc = new HY_DZ_BorrowOutChange.clsBorrowOutSrvClass();
            boc.Init(m_ologin);
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument60();
            bool bResult = boc.LoadVoucher(result, ref domHead, ref domBody, ref result);
            if (bResult)
            {
                domHead.save("d:\\borrowoutchange_head_20211004.xml");
                domBody.save("d:\\borrowoutchange_body_20211004.xml");
                result = "it is ok!";
            }
            return result;
        }
        public string setDomBorrowOutChange()
        {
            string result = "id=1000000001";
            int iVouchID = 0;
            U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop("995");
            HY_DZ_BorrowOutChange.clsBorrowOutSrvClass boc = new HY_DZ_BorrowOutChange.clsBorrowOutSrvClass();
            boc.Init(m_ologin);
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument60();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument60();
            domHead.load("d:\\borrowoutchange_head_20211004.xml");
            domBody.load("d:\\borrowoutchange_body_20211004.xml");
            domHead.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
            domBody.setProperty("SelectionNamespaces", "xmlns:rs='urn:schemas-microsoft-com:rowset' xmlns:z='#RowsetSchema'");
            //bool bResult = boc.LoadVoucher(result, ref domHead, ref domBody, ref result);
            bool bResult = boc.SaveVouch(domHead, domBody, true,ref result, ref iVouchID);
            if (bResult)
            {
                domHead.save("d:\\borrowoutchange_head_20211004.xml");
                domBody.save("d:\\borrowoutchange_body_20211004.xml");
                result = iVouchID.ToString();
            }
            return result;
        }
    }
}