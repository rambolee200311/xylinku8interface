using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.SaleOrder;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;

namespace XylinkU8Interface.UFIDA
{
    public class SaleOrderEntity
    {
        public static Result Add_SO(Saleorder so)
        {
            //LogHelper.WriteLog(typeof(SaleOrderEntity), JsonHelper.ToJson(so));
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            string strSql;
            decimal taxrate = 0;
            decimal kl = 0;
            decimal dkl = 0;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(so.companycode);
            //U8Login.clsLogin m_ologin = U8LoginEntity.getU8LoginEntityInterop(so.companycode);
            //ADODB.Connection conn = new Connection();
            if (m_ologin == null)
            { 
                strResult = "帐套" + so.companycode + "登录失败";
                re.oacode =so.head.ccode;
                re.recode = "111";
                re.remsg = strResult;
                return re;
            }
            try
            {
                if (Ufdata.getDataReader(m_ologin.UfDbName,"select cdefine10 from so_somain where cdefine10='"+so.head.ccode+"'")!="")
                {
                    strResult = so.head.ccode+"已存在销售订单，不能重复同步";
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




                #region//head
                    MSXML2.IXMLDOMDocument2 dom_head;
                    dom_head = new MSXML2.DOMDocument30();
                    dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleorder_head.xml");                

                    MSXML2.IXMLDOMDocument2 dom_head_mode;
                    dom_head_mode = new MSXML2.DOMDocument30();  
                    dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory+"Helper\\saleorder_head_model.xml");                
                
                    foreach (IXMLDOMNode doel in dom_head_mode.selectSingleNode("data").childNodes)
                    {
                        errMsg = doel.nodeName.ToString().Trim();
                        if (!string.IsNullOrEmpty(doel.text))
                        {
                            switch (doel.nodeName.ToString().Trim())
                            {
                                case "ccuscode":
                                    string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + so.head.cust_name + "'");
                                    if (!string.IsNullOrEmpty(ccuscode))
                                    {
                                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = ccuscode;
                                    }
                                    else
                                    {
                                        re.recode = "222";
                                        re.remsg = so.head.cust_name+"客户档案不存在";
                                        return re;
                                    }                                    
                                    break;
                                case "cdepcode":
                                    string cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cdepcode from department where cdepname='" + so.head.dept_name + "'");
                                    if (!string.IsNullOrEmpty(cdepcode))
                                    {
                                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
                                    }
                                    else
                                    {
                                        strSql = "select cDepCode from Person where cPersonName='" + so.head.person_name + "'";
                                        LogHelper.WriteLog(typeof(SaleOrderEntity), strSql);
                                        cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, strSql);
                                        if (string.IsNullOrEmpty(cdepcode))
                                        {
                                            re.recode = "222";
                                            re.remsg = so.head.dept_name + "部门档案不存在";
                                            return re;
                                        }
                                        else
                                        {
                                            dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem(doel.nodeName.ToString()).text = cdepcode;
                                        }
                                    }
                                    break;
                                case "cpersoncode":
                                    string cpersoncode = Ufdata.getDataReader(m_ologin.UfDbName, "select cpersoncode from person where cpersonname='" + so.head.person_name + "'");
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
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("itaxrate").text = so.body[0].taxrate.ToString(); ;
                    broker.AssignNormalValue("domHead", dom_head);

                #endregion

                #region//body
                    MSXML2.IXMLDOMDocument2 dom_body;
                    dom_body = new MSXML2.DOMDocument30();
                    dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleorder_body.xml");

                    MSXML2.IXMLDOMDocument2 dom_body_mode;
                    dom_body_mode = new MSXML2.DOMDocument30();
                    dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\saleorder_body_model.xml");
                    MSXML2.IXMLDOMNode xnModel = dom_body.selectSingleNode("//rs:data//z:row");
                    //for (int j = 1; j < so.body.Count; j++)//追加行
                    //{
                    //    dom_body.selectSingleNode("//rs:data").appendChild(dom_body.selectSingleNode("//rs:data//z:row").cloneNode(true));
                    //}

                    //int i = 0;
                    foreach (Saleorder_body sob in so.body)
                    {
                         //dom_body.selectSingleNode("//rs:data").appendChild(xnModel.cloneNode(true));
                        MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                        taxrate = 0;
                        if (sob.taxrate != null)
                        {
                            taxrate = Convert.ToDecimal(sob.taxrate);
                        }
                        kl = 100;
                        if (sob.iquoteprice != 0)
                        { kl = ((1-(( sob.iquoteprice-(Convert.ToDecimal(sob.itaxunitprice))) / sob.iquoteprice))) * 100; }
                        dkl = 0;
                        dkl = (sob.iquoteprice * sob.iquantity - sob.isum);
                        decimal itax = Convert.ToDecimal(sob.isum) * taxrate/(100 + taxrate);
                        decimal imoney = Convert.ToDecimal(sob.isum) - itax;
                        decimal iunitprice = imoney / Convert.ToDecimal(sob.iquantity);
                        foreach (IXMLDOMNode doel in dom_body_mode.selectSingleNode("data").childNodes)
                        {

                            errMsg = doel.nodeName.ToString().Trim();
                            if (!string.IsNullOrEmpty(doel.text))
                            {
                                switch (doel.nodeName.ToString().Trim())
                                {
                                    case "dpremodate":
                                    case "dpredate":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = so.head.ddate.ToShortDateString();
                                        break;
                                    case "itaxrate":                                        
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = taxrate.ToString();
                                        break;
                                    case "iunitprice":
                                    case "inatunitprice":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text =iunitprice.ToString();
                                        break;
                                    case "imoney":
                                    case "inatmoney":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = imoney.ToString();
                                        break;
                                    case "itax":
                                    case "inattax":
                                         xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = itax.ToString();
                                        break;
                                    case "kl":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = kl.ToString();
                                        break;
                                    case "idiscount":
                                    case "inatdiscount":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = dkl.ToString();
                                        break;
                                    default:
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = getItemValue(doel.text, sob);
                                        break;
                                }
                            }
                            else
                            {
                                xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = "";
                            }
                        }
                        getPortfolio(sob.cinv_code, sob.iquantity,ref dom_body, xnNow, m_ologin);
                        //dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                        //i++;
                    }
                    if (dom_body.selectSingleNode("//rs:data").childNodes.length>1)
                    {
                        dom_body.selectSingleNode("//rs:data").removeChild(xnModel);
                    }

                    broker.AssignNormalValue("domBody", dom_body);
                    errMsg = "";
                    dom_body.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\so_sodetail_111.xml");
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
                        re.u8code =Ufdata.getDataReader(m_ologin.UfDbName,"select csocode from SO_SOMain where ID="+ vNewIDRet);
                    }
                    else
                    {
                        re.recode = "111";
                        re.remsg = result;
                        LogHelper.WriteLog(typeof(SaleOrderEntity), result);
                    }
                
                

            }
            catch(Exception ex)
            {
                re.oacode = so.head.ccode;
                re.recode = "999";
                re.remsg =ex.Message;
                LogHelper.WriteLog(typeof(SaleOrderEntity), ex);
            }
            finally
            {
                //conn.Close();
            }
            return re;
        }
        private static string getItemValue(string itemName, Saleorder_head item)//得到head参数值
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
            catch(Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        private static string getItemValue(string itemName, Saleorder_body item)//得到body参数值
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
                        if (itemName != "bill_date")
                        { result += v.ToString(); }
                        else
                        { result += Convert.ToDateTime(v).ToShortDateString(); }
                    }
                    break;
            }
            return result;
        }

        //组合销售
        private static void getPortfolio(string cinvcode, decimal iquantity, ref MSXML2.IXMLDOMDocument2 dom_body, MSXML2.IXMLDOMNode xnNow, U8Login.clsLoginClass m_ologin)
        {
            string PTOModel = Ufdata.getDataReader(m_ologin.UfDbName, "select bPTOModel from inventory where cinvcode='"+cinvcode+"'");
            string FromSql = " from [dbo].[bom_opcomponent] a inner join bom_parent b on a.BomId=b.BomId inner join bas_part c on b.ParentId=c.PartId inner join bom_bom d on a.BomId=d.BomId inner join bas_part e on a.ComponentId=e.PartId inner join inventory f on e.InvCode=f.cInvCode";
            string WhereSQL = " where c.InvCode='"+cinvcode+"' and d.Status=3 and d.AuditStatus=1";
            string strSql = "select cInvCode,cInvName,baseQty,round(100*(1)/baseQty/bb.sumQty,8) fchildrate,ParentId from (select b.ParentId,f.cInvCode,f.cInvName,a.ComponentId,BaseQtyN/BaseQtyD baseQty"
                +FromSql+WhereSQL+") aa,("
                +"select COUNT(a.ComponentId) sumQty"
                +FromSql+WhereSQL+") bb";
            string guid = Guid.NewGuid().ToString();
            
            if ((PTOModel=="1")||(PTOModel.ToLower()=="true"))
            {
                xnNow.attributes.getNamedItem("cparentcode").text = guid;
                dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                DataTable dtComponent = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                foreach (DataRow dr in dtComponent.Rows)
                {
                    MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                    xnNowClone.attributes.getNamedItem("cparentcode").text = "";
                    xnNowClone.attributes.getNamedItem("cchildcode").text = guid;
                    xnNowClone.attributes.getNamedItem("cinvcode").text = dr["cInvCode"].ToString();
                    xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                    //xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                    xnNowClone.attributes.getNamedItem("iquantity").text = (iquantity * Convert.ToDecimal(dr["baseQty"])).ToString();
                    //xnNow.attributes.getNamedItem("ipartid").text = dr["cInvCode"].ToString();
                    xnNowClone.attributes.getNamedItem("fchildqty").text = dr["baseQty"].ToString();
                    xnNowClone.attributes.getNamedItem("fchildrate").text = dr["fchildrate"].ToString();
                    xnNowClone.attributes.getNamedItem("inatunitprice").text = "0";
                    xnNowClone.attributes.getNamedItem("iunitprice").text = "0";
                    xnNowClone.attributes.getNamedItem("itaxunitprice").text = "0";
                    xnNowClone.attributes.getNamedItem("imoney").text = "0";
                    xnNowClone.attributes.getNamedItem("inatmoney").text = "0";
                    xnNowClone.attributes.getNamedItem("isum").text = "0";
                    xnNowClone.attributes.getNamedItem("inatsum").text = "0";
                    xnNowClone.attributes.getNamedItem("itax").text = "0";
                    xnNowClone.attributes.getNamedItem("inattax").text = "0";
                    xnNowClone.attributes.getNamedItem("kl").text = "100";
                    xnNowClone.attributes.getNamedItem("idiscount").text = "0";
                    xnNowClone.attributes.getNamedItem("inatdiscount").text = "0";
                    /*
                     case "iunitprice":
                                    case "inatunitprice":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text =( Convert.ToDecimal(sob.itaxunitprice) * 100 / (100 + taxrate)).ToString();
                                        break;
                                    case "imoney":
                                    case "inatmoney":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = (Convert.ToDecimal(sob.isum) * 100 / (100 + taxrate)).ToString();
                                        break;
                                    case "itax":
                                    case "inattax":
                                         xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = (Convert.ToDecimal(sob.isum)-Convert.ToDecimal(sob.isum) * 100 / (100 + taxrate)).ToString();
                                        break;
                                    case "kl":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = kl.ToString();
                                        break;
                                    case "idiscount":
                                    case "inatdiscount":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = dkl.ToString();
                                        break;
                     */
                    dom_body.selectSingleNode("//rs:data").appendChild(xnNowClone);
                }
            }
            else
            {
                dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
            }
        }

    
    }
}