using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;

namespace U8toOAInterface.UFIDA
{
    public class UrlParamHelper
    {
        public static string ToParameter(string eventkey)
        {
            var buff = new StringBuilder(string.Empty);
            string url = "";
            /*
             <url>http://39.105.96.42:80/api/esb/execute</url>
              <appkey>64caed2d-ab47-4116-b1be-6caec02a2fa1</appkey>
              <username></username>
              <password></password>
              <timestamp></timestamp>
              <format>json</format>
              <eventkey>U8WriteSn</eventkey>
              <params></params>
             */

            buff.Append("http://39.105.96.42:80/api/esb/execute?");
            buff.Append("appkey=64caed2d-ab47-4116-b1be-6caec02a2fa1&");
            buff.Append("username=&");
            buff.Append("password=&");
            buff.Append("timestamp="+TimeStampHelper.GetTimeStamp(true).ToString()+"&");
            buff.Append("format=json&");
            //buff.Append("eventkey=U8WriteSn&");
            buff.Append("eventkey="+eventkey+"&");
            //buff.Append("params=&");
            url = buff.ToString().Trim('&');
            return url;
        }
    }
}