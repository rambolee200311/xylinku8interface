using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Customer;
using XylinkU8Interface.Models.Result;
using MSXML2;
using System.Xml;

namespace XylinkU8Interface.UFIDA
{
    public class CustomerEntity
    {
        public static Results add_cust(Custs custs)//批量增加客户
        {
            LogHelper.WriteLog(typeof(CustomerEntity), JsonHelper.ToJson(custs));
            Results res = new Results();
            res.result = new List<Result>();
            res.companycode = custs.companycode;
            string strResult = "";
            
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(custs.companycode);

            if (m_ologin==null)
            { strResult ="帐套"+ custs.companycode + "登录失败"; }

            foreach(Cust cust in custs.cust)
            {
                Result re = new Result();
                re = AddResult(cust, strResult, m_ologin);
                res.result.Add(re);
            }
            return res;
        }

        public static Result AddResult(Cust cust, string strResult, U8Login.clsLoginClass m_ologin)//增加单个客户
        {
            string strResultAdd = "";
            Result re = new Result();
            try
            {

                bool bResult = false;
                bool bDup = false;
                if (string.IsNullOrEmpty(strResult))
                {
                    if (Ufdata.getDataReader(m_ologin.UfDbName, "select 1 from Customer where ccusname='" + cust.name + "'") != "")
                    {
                        re.oacode = cust.code;
                        re.remsg = cust.name + "已存在同名客户";
                        re.recode = "222";
                        return re;
                    }
                    if (Ufdata.getDataReader(m_ologin.UfDbName, "select 1 from Customer where cCusDefine7='" + cust.code + "'or cMemo='" + cust.code + "'") != "")
                    {
                        //re.oacode = cust.code;
                        //re.remsg = cust.code + "已存在同GUID客户";
                        //re.recode = "222";
                        //return re;
                        bDup = true;
                    }
                    U8SrvTrans.IClsCommonClass ust = new U8SrvTrans.IClsCommonClass();
                    ust.SetLogin(m_ologin);
                    ust.Init(m_ologin.UfDbName);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\customer.xml");

                    xmlDoc.SelectSingleNode("data/customer/cCusCode").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/cCusHeadCode").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/cInvoiceCompany").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/cCusCreditCompany").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/iId").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/cCusName").InnerText = cust.name;
                    xmlDoc.SelectSingleNode("data/customer/cCusAbbName").InnerText = cust.name;
                    xmlDoc.SelectSingleNode("data/customer/cCCCode").InnerText = cust.typecode;
                    xmlDoc.SelectSingleNode("data/customer/dCusDevDate").InnerText = DateTime.Now.ToShortDateString();
                    xmlDoc.SelectSingleNode("data/customer/dModifyDate").InnerText = DateTime.Now.ToShortDateString();
                    xmlDoc.SelectSingleNode("data/customer/dModifyDate").InnerText = DateTime.Now.ToShortDateString();
                    xmlDoc.SelectSingleNode("data/customer/cMemo").InnerText = cust.code;
                    xmlDoc.SelectSingleNode("data/customer/cCusDefine7").InnerText = cust.code;
                    if (bDup == false)
                    { bResult = ust.Add(xmlDoc.OuterXml, "Customer", ref strResultAdd); }
                    else
                    {
                        Ufdata.execSqlcommand(m_ologin.UfDbName, "update customer set ccusname='" + cust.name + "',ccusabbname='" + cust.name + "' where cCusDefine7='" + cust.code + "' or cMemo='" + cust.code + "'");
                        //bResult = ust.Modify(xmlDoc.OuterXml, "Customer", ref strResultAdd); 
                        bResult = true;
                    }

                    if (bResult)
                    {
                        re.oacode = cust.code;
                        re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select cCusCode from Customer where cCusDefine7='" + cust.code + "' or cMemo='"+cust.code+"'");
                        re.recode = "0";                       

                    }
                    else
                    {
                        LogHelper.WriteLog(typeof(CustomerEntity),strResultAdd);
                        re.oacode = cust.code;
                        re.remsg = strResultAdd;
                        re.recode = "333";
                        
                    }
                }
                else
                {
                    re.oacode = cust.code;
                    re.recode = "111";
                    re.remsg = strResult;
                    
                }
            }
            catch (Exception ex)
            {
                re.oacode = cust.code;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(CustomerEntity),ex);
            }
            LogHelper.WriteLog(typeof(CustomerEntity),JsonHelper.ToJson(re));
            return re;
        }
    }
}