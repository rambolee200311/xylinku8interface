using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.TrialSale;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;

namespace XylinkU8Interface.UFIDA
{
    public class TrialSaleOrderEntity
    {
        public static Result add_SO(TrialSale so)
        {
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            string companycode = "";
            string cwhcode = "";
            string ordercode = "";
            string orireqid = "";
            string strSql = "";
            string PTOModel = "";
            string bService = "";
            decimal taxrate = 0;
            decimal kl = 0;
            decimal dkl = 0;
            companycode = so.companycode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
            if (m_ologin == null)
            {
                strResult = "帐套" + so.companycode + "登录失败";
                re.oacode = so.head.ccode;
                re.recode = "111";
                re.remsg = strResult;
                return re;
            }
            if (Ufdata.getDataReader(m_ologin.UfDbName, "select cdefine10 from so_somain where cdefine10='" + so.head.ordcode + "'") != "")
            {
                strResult = so.head.ccode + "已存在销售订单，不能重复同步";
                re.oacode = so.head.ccode;
                re.recode = "222";
                re.remsg = strResult;
                return re;
            }
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
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型：12

            //第三步：设置API地址标识(Url)
            //当前API：新增或修改的地址标识为：U8API/SaleOrder/Save
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Save");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值
            try
            {
                
                #region//head
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument30();
                dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleorder_head.xml");

                string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + so.head.cust_name + "'");
                //string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + so.head.dept_name + "'");
                string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cDepCode from Person where cPersonName='" + so.head.person_name + "'");
                string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + so.head.person_name + "'");

                if (!string.IsNullOrEmpty(ccuscode))
                {
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccuscode").text = ccuscode;
                }
                else
                {
                    re.recode = "222";
                    re.remsg = so.head.cust_name + "客户档案不存在";
                    return re;
                }

                if (!string.IsNullOrEmpty(cdepcode))
                {
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdepcode").text = cdepcode;
                }
                else
                {
                    cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cDepCode from Person where cPersonName='" + so.head.person_name + "'");
                    if (string.IsNullOrEmpty(cdepcode))
                    {
                        re.recode = "222";
                        re.remsg = so.head.dept_name + "部门档案不存在";
                        return re;
                    }
                    else
                    {
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdepcode").text = cdepcode;
                    }
                }

                if (!string.IsNullOrEmpty(cpersoncode))
                {
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cpersoncode").text = cpersoncode;
                }
                else
                {
                    re.recode = "222";
                    re.remsg = so.head.person_name + "人员档案不存在";
                    return re;
                }
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("itaxrate").text = so.body[0].taxrate.ToString();
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdefine10").text = so.head.ordcode;
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdefine12").text = so.head.ccode;
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ddate").text = so.head.ddate.ToShortDateString();
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("dcreatesystime").text = so.head.ddate.ToShortDateString();
                //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cmemo").text =so.head.cmemo.ToString();
                //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("chdefine1").text = so.body[0].cord_code.ToString();
                dom_head.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\trialsaleorder_head_111.xml");
                broker.AssignNormalValue("domHead", dom_head);

                #endregion

                #region//body
                MSXML2.IXMLDOMDocument2 dom_body;
                dom_body = new MSXML2.DOMDocument30();
                dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleorder_body.xml");
                MSXML2.IXMLDOMNode xnModel = dom_body.selectSingleNode("//rs:data//z:row");

                int i = 0;
                string invcodereqid = "";
                string invcodereqidsn = "";
                string guid = "";
                MSXML2.IXMLDOMNode xnNow = null;
                foreach (TrialSale_body sob in so.body)
                {
                    xnNow = xnModel.cloneNode(true);
                    decimal iquantity = 0;
                    guid = Guid.NewGuid().ToString();
                    PTOModel = Ufdata.getDataReader(m_ologin.UfDbName, "select bPTOModel from inventory where cinvcode='" + sob.cinv_code + "'");
                    invcodereqid = sob.cinv_code + sob.req_id;

                    iquantity = sob.iquantity;
                    taxrate = 0;
                    if (sob.taxrate != null)
                    {
                        taxrate = Convert.ToDecimal(sob.taxrate);
                    }
                    kl = 100;
                    if (sob.iquoteprice != 0)
                    { kl = ((1 - ((sob.iquoteprice - (Convert.ToDecimal(sob.itaxunitprice))) / sob.iquoteprice))) * 100; }
                    dkl = 0;
                    dkl = (sob.iquoteprice * sob.iquantity - sob.isum);
                    decimal itax = Convert.ToDecimal(sob.isum) * taxrate / (100 + taxrate);
                    decimal imoney = Convert.ToDecimal(sob.isum) - itax;
                    decimal iunitprice = imoney / Convert.ToDecimal(sob.iquantity);
                    decimal itaxunitprice = Convert.ToDecimal(sob.isum) / Convert.ToDecimal(sob.iquantity);
                    if (sob.iquoteprice != 0)
                    { kl = ((1 - ((sob.iquoteprice - (Convert.ToDecimal(sob.itaxunitprice))) / sob.iquoteprice))) * 100; }
                    xnNow.attributes.getNamedItem("iquotedprice").text = sob.iquoteprice.ToString();
                    xnNow.attributes.getNamedItem("iquantity").text = iquantity.ToString();                   
                    xnNow.attributes.getNamedItem("cinvcode").text = sob.cinv_code;
                    xnNow.attributes.getNamedItem("cbdefine21").text = sob.req_id;
                    xnNow.attributes.getNamedItem("cbdefine1").text = sob.ori_req_id;
                    xnNow.attributes.getNamedItem("dpredate").text = so.head.ddate.ToShortDateString(); 
                    xnNow.attributes.getNamedItem("dpremodate").text = so.head.ddate.ToShortDateString();
                    xnNow.attributes.getNamedItem("itaxrate").text =sob.taxrate.ToString();
                    xnNow.attributes.getNamedItem("iunitprice").text = iunitprice.ToString();
                    xnNow.attributes.getNamedItem("itaxunitprice").text = itaxunitprice.ToString();
                    xnNow.attributes.getNamedItem("inatunitprice").text = iunitprice.ToString();
                    xnNow.attributes.getNamedItem("imoney").text = imoney.ToString();
                    xnNow.attributes.getNamedItem("inatmoney").text = imoney.ToString();
                    xnNow.attributes.getNamedItem("itax").text = itax.ToString();
                    xnNow.attributes.getNamedItem("inattax").text = iunitprice.ToString();
                    xnNow.attributes.getNamedItem("kl").text = kl.ToString();
                    xnNow.attributes.getNamedItem("idiscount").text = dkl.ToString();
                    xnNow.attributes.getNamedItem("inatdiscount").text = dkl.ToString();
                    xnNow.attributes.getNamedItem("isum").text = sob.isum.ToString();
                    xnNow.attributes.getNamedItem("inatsum").text = sob.isum.ToString();
                    if ((PTOModel == "1") || (PTOModel.ToLower() == "true"))
                    {
                        xnNow.attributes.getNamedItem("cparentcode").text = guid;

                    }

                    bService = Ufdata.getDataReader(m_ologin.UfDbName, "select bservice from inventory where cinvcode='" + sob.cinv_code + "'");
                    


                    dom_body.selectSingleNode("//rs:data").appendChild(xnNow);

                    //子件
                    if ((PTOModel == "1") || (PTOModel.ToLower() == "true"))
                    {
                        foreach (TrialSale_body_detail detail in sob.detail)
                        {
                            MSXML2.IXMLDOMNode xnNowDetail = xnModel.cloneNode(true);
                            xnNowDetail.attributes.getNamedItem("cinvcode").text = detail.cinv_code;
                            xnNowDetail.attributes.getNamedItem("cbdefine21").text = sob.req_id;
                            xnNowDetail.attributes.getNamedItem("cbdefine1").text = sob.ori_req_id;
                            xnNowDetail.attributes.getNamedItem("iquantity").text = detail.iquantity.ToString();
                            xnNowDetail.attributes.getNamedItem("cchildcode").text = guid;
                            xnNowDetail.attributes.getNamedItem("dpredate").text = so.head.ddate.ToShortDateString();
                            xnNowDetail.attributes.getNamedItem("dpremodate").text = so.head.ddate.ToShortDateString();
                            bService = Ufdata.getDataReader(m_ologin.UfDbName, "select bservice from inventory where cinvcode='" + detail.cinv_code + "'"); 
                            dom_body.selectSingleNode("//rs:data").appendChild(xnNowDetail);

                        }

                    }
                }
                if (dom_body.selectSingleNode("//rs:data").childNodes.length > 1)
                {
                    dom_body.selectSingleNode("//rs:data").removeChild(xnModel);
                }
                dom_body.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\dispatchlist_body111.xml");
                broker.AssignNormalValue("domBody", dom_body);
                errMsg = "";

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
                    re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select csocode from SO_SOMain where ID=" + vNewIDRet);
                    verify_so(m_ologin,vNewIDRet,ref re);
                }
                else
                {
                    re.recode = "111";
                    re.remsg = result;
                    LogHelper.WriteLog(typeof(TrialSaleOrderEntity), result);
                }


            }
            catch (Exception ex)
            {
                re.oacode = so.head.ccode;
                re.recode = "999";
                re.remsg = "error:[" + errMsg + "]" + ex.Message;
                LogHelper.WriteLog(typeof(TrialSaleOrderEntity), ex);
            }
            finally
            {
                //conn.Close();
                //结束本次调用，释放API资源
                broker.Release();
            }







            return re;
        }

        private static void verify_so(U8Login.clsLoginClass m_ologin, string vNewIDRet,ref Result re)
        {

            //U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(oacode);
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = (U8Login.clsLogin)m_ologin;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型:12

            //第三步：设置API地址标识(Url)
            //当前API：审核或弃审的地址标识为：U8API/ReturnOrder/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Audit");


            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
            try
            {
                //方法一是直接传入MSXML2.DOMDocumentClass对象
                broker.AssignNormalValue("domHead", load_so(m_ologin,vNewIDRet));

                //给普通参数bVerify赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示true审核false弃审
                broker.AssignNormalValue("bVerify", true);

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
                else
                {
                    //获取返回值
                    //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
                    System.String result = broker.GetReturnValue() as System.String;
                    if (string.IsNullOrEmpty(result))
                    {
                        re.recode = "0";
                        //re.u8code += ",审核成功";                       
                    }
                    else
                    {
                        re.recode = "111";
                        re.remsg += result;
                        LogHelper.WriteLog(typeof(TrialSaleEntity), result);
                    }
                }
            }
            catch (Exception ex)
            {
                //re.oacode = oacode;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(TrialSaleEntity), ex);
            }
            finally
            {
                //结束本次调用，释放API资源
                //broker.Release();
            }
        }

        private static MSXML2.DOMDocument load_so(U8Login.clsLoginClass m_ologin, string vNewIDRet)
        {
            MSXML2.DOMDocument dom_head = null;
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = m_ologin;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 12); //上下文数据类型：int，含义：单据类型：12

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/SaleOrder/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/SaleOrder/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
            broker.AssignNormalValue("VouchID", vNewIDRet);

            //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
            broker.AssignNormalValue("blnAuth", false);

            if (broker.Invoke())
            {
                dom_head = broker.GetResult("domHead") as MSXML2.DOMDocument;
            }


            return dom_head;
        }
    }
}