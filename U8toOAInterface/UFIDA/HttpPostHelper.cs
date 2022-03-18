using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace U8toOAInterface.UFIDA
{
    public class HttpPostHelper
    {
        public static string sendInsert(string url,string bodyParams)
        {
            string strResult = "";
            LogHelper.WriteLog(typeof(HttpPostHelper), "url="+url);
            try
            {
                string method = "post";
                //string url = "http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8addProduct";
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;

                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method;
                req.KeepAlive = false;
                //req.UseDefaultCredentials = true;
                //req.ServicePoint.Expect100Continue = false;
                //req.UserAgent = "fiddler";
                req.AllowAutoRedirect = false;
                req.Proxy = null;
                req.ProtocolVersion = HttpVersion.Version10;
                req.Timeout = 5000;
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                //Content-Type: application/x-www-form-urlencoded; charset=utf-8
                //BuildHeader(headerParams, req);
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyParams);
                byte[] postData = Encoding.UTF8.GetBytes(bodyParams);
                req.ContentLength = postData.Length;
                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                strResult = GetResponseAsString(rsp, encoding);
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
                LogHelper.WriteLog(typeof(HttpPostHelper),ex);
            }
            return strResult;
        }

        public static string sendInsertCkdaXml(string bodyParams)
        {
            string strResult = "";
            try
            {
                string method = "post";
                string url = "http://39.105.96.42/services/ModeDateService";
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;

                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method;
                req.KeepAlive = false;
                //req.UseDefaultCredentials = true;
                //req.ServicePoint.Expect100Continue = false;
                //req.UserAgent = "fiddler";
                req.AllowAutoRedirect = false;
                req.Proxy = null;
                req.ProtocolVersion = HttpVersion.Version10;
                req.Timeout = 5000;
                req.ContentType = "application/soap+xml;charset=utf-8";
                //Content-Type: application/x-www-form-urlencoded; charset=utf-8
                //BuildHeader(headerParams, req);
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyParams);
                byte[] postData = Encoding.UTF8.GetBytes(bodyParams);
                req.ContentLength = postData.Length;
                reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                strResult = GetResponseAsString(rsp, encoding);
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
                LogHelper.WriteLog(typeof(HttpPostHelper), ex);
            }
            return strResult;
        }
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            string strResult = "";
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
                LogHelper.WriteLog(typeof(HttpPostHelper), ex);

            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();

            }
            return strResult;
        }
    
        public static string sendU8SN(string url)
        {
            string strResult = "";
            try
            {
                string method = "post";
                //string url = "http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8addProduct";
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;

                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method;
                req.KeepAlive = false;
                //req.CachePolicy = WebRequest.DefaultCachePolicy;
                //req.UseDefaultCredentials = true;
                //req.ServicePoint.Expect100Continue = false;
                //req.UserAgent = "fiddler";
                req.AllowAutoRedirect = false;
                req.Proxy = null;
                req.ProtocolVersion = HttpVersion.Version10;
                req.Timeout = 5000;
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                //Content-Type: application/x-www-form-urlencoded; charset=utf-8
                //BuildHeader(headerParams, req);
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyParams);
                //byte[] postData = Encoding.UTF8.GetBytes(bodyParams);
                //req.ContentLength = postData.Length;
                //reqStream = req.GetRequestStream();
                //reqStream.Write(postData, 0, postData.Length);
                rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                strResult = GetResponseAsString(rsp, encoding);
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
                LogHelper.WriteLog(typeof(HttpPostHelper), ex);
            }
            return strResult;
        }
    }
}
