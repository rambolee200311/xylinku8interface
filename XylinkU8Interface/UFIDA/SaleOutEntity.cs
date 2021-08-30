﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Dispatchreturn;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;

namespace XylinkU8Interface.UFIDA
{
    public class SaleOutEntity
    {
        public static void Add_so(ref Result re,string cexchan,U8Login.clsLoginClass m_ologin,string vNewIDRet,DispatchReturn so)
        {
            string errMsg = "";
            try
            {
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = m_ologin;

                //第三步：设置API地址标识(Url)
                //当前API：添加新单据的地址标识为：U8API/saleout/Add
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Add");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：32
                broker.AssignNormalValue("sVouchType", "32");

                //给BO表头参数DomHead赋值，此BO参数的业务类型为销售出库单，属表头参数。BO参数均按引用传递
                //提示：给BO表头参数DomHead赋值有两种方法

                //方法一是直接传入MSXML2.DOMDocumentClass对象


                #region //head
                DataTable dthead = Ufdata.getDatatableFromSql(m_ologin.UfDbName, "select * from dispatchlist a left join DispatchList_extradefine b on a.DLID=b.dlid where a.dlid=" + vNewIDRet);
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument30();

                //MSXML2.IXMLDOMNode xnModel = dom_head.selectSingleNode("//rs:data//z:row");
                MSXML2.IXMLDOMDocument2 dom_head_mode;
                dom_head_mode = new MSXML2.DOMDocument30();
                if (cexchan == "red")
                {
                    dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleouthead_red.xml");
                    dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleouthead_red_model.xml");
                    foreach (IXMLDOMNode doel in dom_head_mode.selectSingleNode("data").childNodes)
                    {
                        errMsg = doel.nodeName.ToString().Trim();
                        if (!string.IsNullOrEmpty(doel.text))
                        {
                            switch (doel.nodeName.ToString().Trim())
                            {
                                case     "cwhcode":
                                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text =Ufdata.getDataReader(m_ologin.UfDbName, "select top 1 cwhcode from dispatchlists where dlid="+ vNewIDRet+" and isnull(cwhcode,'')!=''");
                                    break;
                                default:
                                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text =getItemValue(doel.text,dthead.Rows[0]);// dthead.Rows[0][doel.text].ToString();
                                    break;
                            }
                        }
                        else
                        {
                            dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                        }
                    }

                }
                else
                {
                    dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody.xml");
                    dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody_model.xml");
                }



                dom_head.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleouthead111.xml");
                broker.AssignNormalValue("domhead", dom_head);
                #endregion





                #region //body
                DataTable dtbody = Ufdata.getDatatableFromSql(m_ologin.UfDbName, "select * from dispatchlists a left join DispatchLists_extradefine b on a.idlsid=b.iDLsID inner join inventory c on a.cInvCode=c.cInvCode where a.dlid=" + vNewIDRet + " and isnull(cwhcode,'')!='' and c.bPTOModel=0 and c.bService=0");
                MSXML2.IXMLDOMDocument2 dom_body;
                dom_body = new MSXML2.DOMDocument30();

                MSXML2.IXMLDOMNode xnModel = null ; 
                MSXML2.IXMLDOMDocument2 dom_body_mode;
                dom_body_mode = new MSXML2.DOMDocument30();
                if (cexchan == "red")
                {
                    dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody_red.xml");
                    dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody_red_model.xml");
                    xnModel =dom_body.selectSingleNode("//rs:data//z:row"); 
                    int i = 1;
                    foreach (DataRow dr in dtbody.Rows)
                    {
                        MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                        foreach (IXMLDOMNode doel in dom_body_mode.selectSingleNode("data").childNodes)
                        {
                            errMsg = doel.nodeName.ToString().Trim();
                            if (!string.IsNullOrEmpty(doel.text))
                            {
                                switch (doel.nodeName.ToString().Trim())
                                {
                                    case "cbdlcode":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = dthead.Rows[0]["cdlcode"].ToString();
                                        break;
                                    case "iuninvsncount":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = (Convert.ToDecimal(dr["iquantity"]) * -1).ToString();
                                        break;
                                    case "irowno":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = i.ToString();
                                        break;
                                    default:
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, dr);
                                        break;
                                }
                            }
                            else
                            {
                                xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                            }
                        }
                        i++;
                        dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                    }
                }
                else
                {
                    dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody.xml");
                    dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleoutbody_model.xml");
                }
                if (dom_body.selectSingleNode("//rs:data").childNodes.length > 1)
                {
                    dom_body.selectSingleNode("//rs:data").removeChild(xnModel);
                }
                dom_body.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\saleoutbody111.xml");
                broker.AssignNormalValue("dombody", dom_body);
                #endregion


                ////给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                //broker.AssignNormalValue("domPosition", new System.Object());

                ////该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                ////给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                //broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

                ////该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                //broker.AssignNormalValue("VouchId", "");

                //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.DOMDocumentClass domMsg = new MSXML2.DOMDocumentClass();
                broker.AssignNormalValue("dommsg", domMsg);
                //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                broker.AssignNormalValue("bCheck", false);

                //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                broker.AssignNormalValue("bBeforCheckStock", false);

                //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                if (cexchan != "red")
                {
                    broker.AssignNormalValue("bIsRedVouch", false);
                }
                else if (cexchan == "red")
                {
                    broker.AssignNormalValue("bIsRedVouch", true);
                }

                //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                broker.AssignNormalValue("sAddedState", "");

                //给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
                broker.AssignNormalValue("bReMote", false);
                //第六步：调用API
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
                    //broker.Release();

                }
                //第七步：获取返回结果

                //获取返回值
                //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                System.Boolean bresult = Convert.ToBoolean(broker.GetReturnValue());
                //result = bresult.ToString();
                //获取out/inout参数值
                if (!bresult)
                {
                    //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String errMsgRet = broker.GetResult("errMsg") as System.String;
                    re.recode = "111";
                    re.remsg = errMsgRet;
                }
                else
                {
                    //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                    re.recode = "0";
                    STSNEntity.add_STSN(m_ologin, "32", so, VouchIdRet);
                    re.u8code += ",销售出库单:" + Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from rdrecord32 where ID=" + VouchIdRet);
                }
                //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));

                //结束本次调用，释放API资源
            }
            catch(Exception ex)
            {
                
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(SaleOutEntity), ex);
            }
        }
        private static string getItemValue(string itemName, DataRow dr)//得到body参数值
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
                    Object v = dr[itemName];
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