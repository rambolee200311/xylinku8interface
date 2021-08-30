using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Person;
using XylinkU8Interface.Models.Result;
using MSXML2;
using System.Xml;
namespace XylinkU8Interface.UFIDA
{
    public class PersonEntity
    {
        public static Results add_person(Persons pers)//批量增加人员
        {
            LogHelper.WriteLog(typeof(PersonEntity), JsonHelper.ToJson(pers));
            Results res = new Results();
            res.result = new List<Result>();
            res.companycode = pers.companycode;
            string strResult = "";

            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(pers.companycode);

            if (m_ologin == null)
            { strResult = "帐套" + pers.companycode + "登录失败"; }

            foreach (Person per in pers.person)
            {
                Result re = new Result();
                re = AddResult(per, strResult, m_ologin);
                res.result.Add(re);
            }
            return res;
        }

        public static Result GetResult(string companycode,string name)
        {
            Result re = new Result();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
            re.oacode = companycode;
            
            if (m_ologin == null)
            {
                re.recode = "0";
                re.remsg = "帐套" + companycode + "登录失败";
                return re;
            }
            re.recode = "1";
            re.remsg = name + "不存在同名职员";
            re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select cPsn_Num from hr_hi_person where cPsn_Name='" + name + "'");
            if (re.u8code != "")
            {
                re.recode="0";
                re.remsg=name+"已存在同名职员";
            }
            return re;
        }
        public static Result AddResult(Person per, string strResult, U8Login.clsLoginClass m_ologin)//增加单个人员
        {
            string strResultAdd = "";
            string depcode = "";
            Result re = new Result();
            try
            {
                bool bResult = false;
                if (string.IsNullOrEmpty(strResult))
                {
                    if (Ufdata.getDataReader(m_ologin.UfDbName, "select 1 from hr_hi_person where cPsn_Name='" + per.name + "'") != "")
                    {
                        re.oacode = per.code;
                        re.remsg = per.name + "已存在同名职员";
                        re.recode = "222";
                        return re;
                    }
                    if (Ufdata.getDataReader(m_ologin.UfDbName, "select 1 from hr_hi_person where vAliaName='" + per.code + "'") != "")
                    {
                        re.oacode = per.code;
                        re.remsg = per.code + "已存在同GUID客户";
                        re.recode = "222";
                        return re;
                    }
                    depcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + per.depname + "'");
                    if (string.IsNullOrEmpty(depcode))
                    {
                        re.oacode = per.code;
                        re.remsg = per.depname + "不存在部门档案";
                        re.recode = "222";
                        return re;
                    }

                    U8SrvTransPsn.IClsCommonClass ust = new U8SrvTransPsn.IClsCommonClass();
                    //U8SrvTrans.IClsCommonClass ust = new U8SrvTrans.IClsCommonClass();
                    ust.SetLogin(m_ologin);
                    ust.Init(m_ologin.UfDbName);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "UFIDA\\person.xml");

                    xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person/cPsn_Num").InnerText = per.code;
                    xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person/cPsn_Name").InnerText = per.name;
                    xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person/vAliaName").InnerText = per.code;
                    xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person/cdept_num").InnerText =depcode;
                    xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person/cdepcode").InnerText = depcode;
                    bResult = ust.Add(xmlDoc.SelectSingleNode("ufinterface/v_aa_hr_hi_person").OuterXml, "hr_hi_person",ref strResultAdd);

                    if (bResult)
                    {
                        re.oacode = per.code;
                        re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select cPsn_Num from hr_hi_person where vAliaName='" + per.code + "'");
                        re.recode = "0";
                    }
                    else
                    {
                        re.oacode = per.code;
                        re.remsg = strResultAdd;
                        re.recode = "333";
                    }
                }
                else
                {
                    re.oacode = per.code;
                    re.recode = "111";
                    re.remsg = strResult;
                }
            }
            catch(Exception ex)
            {
                re.oacode = per.code;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(PersonEntity), ex);
            }
            return re;
        }
    }
}