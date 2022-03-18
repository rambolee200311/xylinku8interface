﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

//using System.Web.Script.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Text;
namespace U8toOAInterface.UFIDA
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;
        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                       _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;            
            _jsonSettings.Converters.Add(datetimeConverter);
            
        }

        /// <summary> 
        /// 将指定的对象序列化成 JSON 数据。 
        /// </summary> 
        /// <param name="obj">要序列化的对象。</param> 
        /// <returns></returns> 
        public static string ToJson(this object obj)
        {
            try
            {
                if (null == obj)
                    return null;

                return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(JsonHelper),ex);
                return null;
            }
        }

        /// <summary> 
        /// 将指定的 JSON 数据反序列化成指定对象。 
        /// </summary> 
        /// <typeparam name="T">对象类型。</typeparam> 
        /// <param name="json">JSON 数据。</param> 
        /// <returns></returns> 
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
                //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                //{
                //    return (T)ser.ReadObject(ms);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(JsonHelper), ex);
                return default(T);
            }
        }


    }
}