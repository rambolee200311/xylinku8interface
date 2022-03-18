using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using MSXML2;
using UFIDA.U8.MomServiceCommon;
using U8toOAInterface.UFIDA;
using U8toOAInterface.Models.Inventory;

namespace U8toOAInterface
{
    public class ClsU8toOAvoucher
    {
        public bool Audit_After(ref MSXML2.IXMLDOMDocument2 domhead,ref MSXML2.IXMLDOMDocument2 dombody,out string errmsg)
        {
            bool bResult = true;
            string strResult = "";
            Nullable<int> intid = null;
            errmsg = "";
            MomCallContextCache envCtxCache = new MomCallContextCache();
            MomCallContext envCtx = new MomCallContext();
            envCtx = envCtxCache.CurrentMomCallContext;
            string vID = "";
            XmlDocument xmlDoc = new XmlDocument();
            XmlNamespaceManager xnm;
            //从上下文获取帐套库连接对象
            ADODB.Connection conn = envCtx.BizDbConnection as ADODB.Connection;

            string strNow = Convert.ToDateTime(envCtx.LoginInfo.Date).ToShortDateString();
            string eventId = envCtx.EventIdentity;
            switch (eventId)
            {
                case "U8API/saleout/Audit_After"://销售出库单审核后事件
                    //bResult = InvEntity.Inventory_modify_after(archivedata, conn);
                    //domhead.save("d:\\saleouthead.xml");
                    //dombody.save("d:\\saleoutbody.xml");
                    
                    xmlDoc.LoadXml(domhead.xml);
                    xnm = new XmlNamespaceManager(xmlDoc.NameTable);
                    //xmlns:s="uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882" xmlns:dt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882" xmlns:rs="urn:schemas-microsoft-com:rowset" xmlns:z="#RowsetSchema"
                    xnm.AddNamespace("rs","urn:schemas-microsoft-com:rowset");
                    xnm.AddNamespace("z", "#RowsetSchema");
                    
                    vID= xmlDoc.SelectSingleNode("//rs:data/rs:update/rs:original/z:row", xnm).Attributes["id"].Value.ToString();
                    string bredvouch = DBHelper.getStrResultFromSQLscript(conn, 
                        "select bredvouch from rdrecord32 where ID=" +vID);
                    if ((bredvouch=="0")||(bredvouch.ToLower()=="false"))
                    {
                        bResult = SaleOutEntity.Saleout_audit_after(vID,conn);
                    }


                    break;
                case "U8API/Consignment/Audit_After"://销售发货单审核后事件
                    //domhead.save("d:\\disptchlisthead.xml");
                    xmlDoc.LoadXml(domhead.xml);
                    xnm = new XmlNamespaceManager(xmlDoc.NameTable);
                    //xmlns:s="uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882" xmlns:dt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882" xmlns:rs="urn:schemas-microsoft-com:rowset" xmlns:z="#RowsetSchema"
                    xnm.AddNamespace("rs","urn:schemas-microsoft-com:rowset");
                    xnm.AddNamespace("z", "#RowsetSchema");
                    
                    vID= xmlDoc.SelectSingleNode("//rs:data/z:row", xnm).Attributes["cdlcode"].Value.ToString();
                    string breturnflag=xmlDoc.SelectSingleNode("//rs:data/z:row", xnm).Attributes["breturnflag"].Value.ToString();
                    if (breturnflag=="0")
                    {
                        bResult=DispatchlistEntity.Dispatchlist_audit_after(vID,conn);
                    }
                    break;
                case "U8API/otherout/Audit_After"://其他出库单审核后事件
                    xmlDoc.LoadXml(domhead.xml);
                    
                    xnm = new XmlNamespaceManager(xmlDoc.NameTable);
                    //xmlns:s="uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882" xmlns:dt="uuid:C2F41010-65B3-11d1-A29F-00AA00C14882" xmlns:rs="urn:schemas-microsoft-com:rowset" xmlns:z="#RowsetSchema"
                    xnm.AddNamespace("rs","urn:schemas-microsoft-com:rowset");
                    xnm.AddNamespace("z", "#RowsetSchema");

                    vID = xmlDoc.SelectSingleNode("//rs:data/rs:update/rs:original/z:row", xnm).Attributes["id"].Value.ToString();
                    bResult = OtherOutEntity.Otherout_audit_after(vID, conn);
                    break;
               
            }
            strResult = errmsg;
            return bResult;
        }
    }
}
