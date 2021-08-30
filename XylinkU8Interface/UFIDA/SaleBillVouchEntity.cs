using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.SaleBill;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;

namespace XylinkU8Interface.UFIDA
{
    public class SaleBillVouchEntity
    {
        public static Result Add_SO(SaleBill so)
        {
            LogHelper.WriteLog(typeof(SaleBillVouchEntity), JsonHelper.ToJson(so));
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(so.companycode);
            //U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop(so.companycode);
            //ADODB.Connection conn = new Connection();
            if (m_ologin == null)
            {
                strResult = "帐套" + so.companycode + "登录失败";
                re.oacode = so.head.ccode;
                re.recode = "111";
                re.remsg = strResult;
                return re;
            }
            try
            {
                re.oacode = so.head.ccode;
                //conn.ConnectionString = m_ologin.UfDbName;
                //conn.Open();
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = (U8Login.clsLogin)m_ologin;
                //envContext.U8Login = m_ologin;
                //销售所有接口均支持内部独立事务和外部事务，默认内部事务
                //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
                //envContext.BizDbConnection = new ADO.Connection();
                //envContext.IsIndependenceTransaction = false;

                //设置上下文参数
                envContext.SetApiContext("VoucherType", 2); //上下文数据类型：int，含义：单据类型：2

                //第三步：设置API地址标识(Url)
                //当前API：新增或修改的地址标识为：U8API/NormalInvoice/Save
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/NormalInvoice/Save");


                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值




                #region//head
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument30();
                dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\salebillvouch_head.xml");

                MSXML2.IXMLDOMDocument2 dom_head_mode;
                dom_head_mode = new MSXML2.DOMDocument30();
                dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\salebillvouch_head_model.xml");

                foreach (IXMLDOMNode doel in dom_head_mode.selectSingleNode("data").childNodes)
                {
                    errMsg = doel.nodeName.ToString().Trim();
                    string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + so.head.cust_name + "'");
                    string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + so.head.dept_name + "'");
                    string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + so.head.person_name + "'");
                    if (!string.IsNullOrEmpty(doel.text))
                    {
                        switch (doel.nodeName.ToString().Trim())
                        {
                            case "ccuscode":
                            case "cauthid":
                            case "ccreditcuscode":
                            case "cinvoicecompany":
                                if (!string.IsNullOrEmpty(ccuscode))
                                {
                                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = ccuscode;
                                }
                                else
                                {
                                    re.recode = "222";
                                    re.remsg = so.head.cust_name + "客户档案不存在";
                                    return re;
                                }
                                break;
                            case "cdepcode":

                                if (!string.IsNullOrEmpty(cdepcode))
                                {
                                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
                                }
                                else
                                {
                                    re.recode = "222";
                                    re.remsg = so.head.dept_name + "部门档案不存在";
                                    return re;
                                }
                                break;
                            case "cpersoncode":

                                if (!string.IsNullOrEmpty(cpersoncode))
                                {
                                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cpersoncode;
                                }
                                else
                                {
                                    re.recode = "222";
                                    re.remsg = so.head.person_name + "人员档案不存在";
                                    return re;
                                }
                                break;
                            case "csocode":
                                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = so.head.cord_code;
                                break;
                            default:
                                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, so.head);
                                break;
                        }

                    }
                    else
                    {
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                    }
                }
                broker.AssignNormalValue("domHead", dom_head);

                #endregion

                #region//body
                MSXML2.IXMLDOMDocument2 dom_body;
                dom_body = new MSXML2.DOMDocument30();
                dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\salebillvouch_body.xml");

                MSXML2.IXMLDOMDocument2 dom_body_mode;
                dom_body_mode = new MSXML2.DOMDocument30();
                dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\salebillvouch_body_model.xml");

                for (int j = 1; j < so.body.Count; j++)//追加行
                {
                    dom_body.selectSingleNode("//rs:data").appendChild(dom_body.selectSingleNode("//rs:data//z:row").cloneNode(true));
                }

                int i = 0;
                foreach (Salebill_body sob in so.body)
                {
                    foreach (IXMLDOMNode doel in dom_body_mode.selectSingleNode("data").childNodes)
                    {
                        errMsg = doel.nodeName.ToString().Trim();
                        decimal iquantity = sob.iquantity;
                        decimal imoney = iquantity * sob.iunitprice;
                        decimal isum = iquantity * sob.itaxunitprice;
                        //string cwhcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + sob.cwhname + "'"); ;
                        if (!string.IsNullOrEmpty(doel.text))
                        {
                            switch (doel.nodeName.ToString().Trim())
                            {
                                //case "icorid":
                                //    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select id from so_somain where csocode='"+sob.cord_code+"'");
                                //    break;
                                case "isosid":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select isosid from SO_SODetails where ID in (select id from so_somain where csocode='" + so.head.cord_code + "') and cinvcode='" + sob.cinv_code + "'");
                                    break;
                                case "idlsid":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = 
                                            Ufdata.getDataReader(m_ologin.UfDbName,"select idlsid from dispatchlists where iSOsID in (select isosid from SO_SODetails where ID in (select id from so_somain where csocode='" + so.head.cord_code + "') and cinvcode='" + sob.cinv_code + "')");

                                    break;
                                //case "idlsid":
                                //case "icorid":
                                //    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select idlsid from dispatchlists where dlid in (select dlid from dispatchlist where cdlcode='" + sob.cdsp_code + "') and cinvcode='" + sob.cinv_code + "'");
                                //    break;
                                case "iquantity":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = iquantity.ToString();
                                    break;
                                case "cwhcode":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = 
                                        Ufdata.getDataReader(m_ologin.UfDbName,"select cwhcode from dispatchlists where iSOsID in (select isosid from SO_SODetails where ID in (select id from so_somain where csocode='" + so.head.cord_code + "') and cinvcode='" + sob.cinv_code + "')");
                                    break;
                                case "isum":                                
                                case "inatsum":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = isum.ToString();
                                    break;
                                case "imoney":
                                case "inatmoney":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = imoney.ToString();
                                    break;
                                case "itaxrate":
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = so.head.tax_rate.ToString();
                                    break;
                                case "cordercode":
                                case "csocode":
                                     dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = so.head.cord_code.ToString();
                                    break;
                                default:
                                    dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, sob);
                                    break;
                            }
                        }
                        else
                        {
                            dom_body.selectSingleNode("//rs:data").childNodes[i].attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                        }
                    }
                    i++;
                }
                broker.AssignNormalValue("domBody", dom_body);
                errMsg = "";
                //dom_body.save("d:\\abc\\1111.xml");
                #endregion

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
                            re.recode = "999";
                            re.remsg = "系统异常：" + sysEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            //Console.WriteLine("API异常：" + bizEx.Message);
                            re.recode = "999";
                            re.remsg = "API异常：" + bizEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        //异常原因
                        String exReason = broker.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            //Console.WriteLine("异常原因：" + exReason);
                            re.recode = "888";
                            re.remsg = "异常原因：" + exReason;
                            //return re;
                        }
                    }
                    //结束本次调用，释放API资源
                    broker.Release();
                    return re;
                }

                //第七步：获取返回结果

                //获取返回值
                //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功为空串
                string result = broker.GetReturnValue() as System.String;

                //获取out/inout参数值

                //获取普通INOUT参数vNewID。此返回值数据类型为string，在使用该参数之前，请判断是否为空
                string vNewIDRet = broker.GetResult("vNewID") as string;

                //结束本次调用，释放API资源
                broker.Release();

                if (string.IsNullOrEmpty(result))
                {
                    re.recode = "0";
                    re.u8code = vNewIDRet;
                }
                else
                {
                    re.recode = "111";
                    re.remsg = result;
                }



            }
            catch (Exception ex)
            {
                re.oacode = so.head.ccode;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(SaleBillVouchEntity), ex);
            }
            finally
            {
                //conn.Close();
            }
            return re;
        }
        private static string getItemValue(string itemName,Salebill_head item)//得到head参数值
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
        private static string getItemValue(string itemName, Salebill_body item)//得到body参数值
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
                        if (itemName != "ddate")
                        { result += v.ToString(); }
                        else
                        { result += Convert.ToDateTime(v).ToShortDateString(); }
                    }
                    break;
            }
            return result;
        }
    }
}