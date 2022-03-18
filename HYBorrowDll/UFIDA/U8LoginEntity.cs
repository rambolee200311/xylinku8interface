using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using HYBorrowDll.Helper;

namespace HYBorrowDll.UFIDA
{
    public static class U8LoginEntity
    {
        //登录对象
        public static U8Login.clsLoginClass getU8LoginEntity(string accid)
        {

            U8Login.clsLoginClass m_ologin = new U8Login.clsLoginClass();
            XmlDocument xmlDoc = new XmlDocument();
            string user = "";
            string password = "";
            string server = "";

            try
            {
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\MainConfig.xml");
                if (xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']") != null)
                {
                    user = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("user").InnerText;
                    LogHelper.WriteLog(typeof(U8LoginEntity), user);
                    password = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("password").InnerText;
                    LogHelper.WriteLog(typeof(U8LoginEntity), password);
                    server = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("server").InnerText;
                    LogHelper.WriteLog(typeof(U8LoginEntity), server);
                }
                else
                {
                    return null;
                }
                string sYear = "";
                //string sPeriod = "";
                string sDate = "";
                if (accid == "999")
                {
                    sYear = "2015";
                    sDate = "2015-01-01";
                }
                else
                {
                    sYear = DateTime.Now.Year.ToString();
                    sDate = DateTime.Now.ToShortDateString();
                }
                //LogHelper.WriteLog(typeof(U8LoginEntity), "1");
                if (!m_ologin.Login("AA", accid, sYear, user, password, sDate, server, ""))
                {
                    return null;
                }
                //LogHelper.WriteLog(typeof(U8LoginEntity), "2");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(U8LoginEntity), ex);
                return null;
            }
            return m_ologin;
        }
        public static U8Login.clsLogin getU8LoginEntityInterop(string accid)
        {

            U8Login.clsLogin m_ologin = new U8Login.clsLogin();
            XmlDocument xmlDoc = new XmlDocument();
            string user = "";
            string password = "";
            string server = "";

            try
            {
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\MainConfig.xml");
                if (xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']") != null)
                {
                    user = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("user").InnerText;
                    password = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("password").InnerText;
                    server = xmlDoc.SelectSingleNode("ufinterface/ACC[@id='" + accid + "']").SelectSingleNode("server").InnerText;
                }
                else
                {
                    return null;
                }
                string sYear = "";
                //string sPeriod = "";
                string sDate = "";
                if (accid == "999")
                {
                    sYear = "2015";
                    sDate = "2015-01-01";
                }
                else
                {
                    sYear = DateTime.Now.Year.ToString();
                    sDate = DateTime.Now.ToShortDateString();
                }

                if (!m_ologin.Login("ST", accid, sYear, user, password, sDate, server, ""))
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(U8LoginEntity), ex);
                return null;
            }
            return m_ologin;
        }

        public static U8Login.clsLoginClass getU8LoginEntity()
        {
            U8Login.clsLoginClass m_ologin = new U8Login.clsLoginClass();
            XmlDocument xmlDoc = new XmlDocument();
            string user = "";
            string password = "";
            string server = "";
            string accid = "999";
            try
            {
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\MainConfig.xml");
                if (xmlDoc.SelectSingleNode("ufinterface/ACC[@func='main']") != null)
                {
                    user = xmlDoc.SelectSingleNode("ufinterface/ACC[@func='main']").SelectSingleNode("user").InnerText;
                    password = xmlDoc.SelectSingleNode("ufinterface/ACC[@func='main']").SelectSingleNode("password").InnerText;
                    server = xmlDoc.SelectSingleNode("ufinterface/ACC[@func='main']").SelectSingleNode("server").InnerText;
                    accid  = xmlDoc.SelectSingleNode("ufinterface/ACC[@func='main']").Attributes["id"].Value;
                }
                else
                {
                    return null;
                }
                string sYear = "";
                //string sPeriod = "";
                string sDate = "";
                if (accid == "999")
                {
                    sYear = "2015";
                    sDate = "2015-01-01";
                }
                else
                {
                    sYear = DateTime.Now.Year.ToString();
                    sDate = DateTime.Now.ToShortDateString();
                }

                if (!m_ologin.Login("AA", accid, sYear, user, password, sDate, server, ""))
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(U8LoginEntity), ex);
                return null;
            }
            return m_ologin;
        }
    }
}