using System;
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
    public class DispatchReturnEntity
    {
        public static Result Add_SO(DispatchReturn so,string cexchan)
        {
            //LogHelper.WriteLog(typeof(DispatchReturnEntity), JsonHelper.ToJson(so));
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            string companycode = "";
            string cwhcode = "";
            string ordercode = "";
            string orireqid = "";
            string strSql = "";
            companycode = so.companycode;
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
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
            if (cexchan == "red")
            {
                strSql = "select cdefine10 from dispatchlist where cdefine10='" + so.head.ccode + "' and bReturnFlag=1";
            }
            else
            {
                 strSql = "select cdefine10 from dispatchlist where cdefine10='" + so.head.ccode + "' and bReturnFlag=0";
            }
                
                
            if (Ufdata.getDataReader(m_ologin.UfDbName, strSql) != "")
            {
                strResult = so.head.ccode + "已存在销售发货单/退货单，不能重复同步";
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
                U8ApiAddress myApiAddress;
                //设置上下文参数
                if (cexchan == "red")
                {
                    envContext.SetApiContext("VoucherType", 10); //上下文数据类型：int，含义：单据类型：10
                    myApiAddress = new U8ApiAddress("U8API/ReturnOrder/Save");
                }
                else
                {
                    envContext.SetApiContext("VoucherType", 9); //上下文数据类型：int，含义：单据类型：9
                    myApiAddress = new U8ApiAddress("U8API/Consignment/Save");

                }

                //第三步：设置API地址标识(Url)
                //当前API：新增或修改的地址标识为：U8API/SaleOrder/Save
               


                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
            try
            {
                //第五步：API参数赋值




                #region//head
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument30();
                dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\dispatchlistreturn_head.xml");

                MSXML2.IXMLDOMDocument2 dom_head_mode;
                dom_head_mode = new MSXML2.DOMDocument30();
                dom_head_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\dispatchlistreturn_head_model.xml");

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
                                    cdepcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cDepCode from Person where cPersonName='" + so.head.person_name + "'");
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

                if (cexchan == "red")
                {
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("breturnflag").text = "1";
                }
                else
                {
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("breturnflag").text = "0";
                }

                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("chdefine1").text = so.body[0].cord_code.ToString();

                dom_head.save(AppDomain.CurrentDomain.BaseDirectory+"Logs\\dispatchreturn_head111.xml");

                broker.AssignNormalValue("domHead", dom_head);

                #endregion

                #region//body
                MSXML2.IXMLDOMDocument2 dom_body;
                dom_body = new MSXML2.DOMDocument30();
                dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\dispatchlistreturn_body.xml");
                MSXML2.IXMLDOMNode xnModel = dom_body.selectSingleNode("//rs:data//z:row");
                MSXML2.IXMLDOMDocument2 dom_body_mode;
                dom_body_mode = new MSXML2.DOMDocument30();
                dom_body_mode.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\dispatchlistreturn_body_model.xml");

                //for (int j = 1; j < so.body.Count; j++)//追加行
                //{
                //    dom_body.selectSingleNode("//rs:data").appendChild(dom_body.selectSingleNode("//rs:data//z:row").cloneNode(true));
                //}

                int i = 0;
                string invcodereqid = "";
                string invcodereqidsn = "";
                string guid ="";
                MSXML2.IXMLDOMNode xnNow = null; 
                foreach (Dispatchreturn_body sob in so.body)
                {
                    decimal iquantity = 0;
                    
                    if (sob.cinv_code + sob.req_id != invcodereqid)
                    {
                        xnNow = xnModel.cloneNode(true);
                        invcodereqid = sob.cinv_code + sob.req_id;
                        foreach (IXMLDOMNode doel in dom_body_mode.selectSingleNode("data").childNodes)
                        {
                            errMsg = doel.nodeName.ToString().Trim();

                            if (cexchan == "red")
                            {
                                iquantity = sob.iquantity * -1;
                            }
                            else
                            {
                                iquantity = sob.iquantity;
                            }
                            decimal isum = iquantity * sob.itaxunitprice;
                            cwhcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + sob.cwhname + "'"); ;
                            if (!string.IsNullOrEmpty(doel.text))
                            {
                                switch (doel.nodeName.ToString().Trim())
                                {
                                    //case "icorid":
                                    //    xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select id from so_somain where csocode='"+sob.cord_code+"'");
                                    //    break;
                                    case "isosid":
                                        ordercode = sob.cord_code;
                                        orireqid = sob.ori_req_id;
                                        LogHelper.WriteLog(typeof(DispatchReturnEntity), "select a.isosid from SO_SODetails a inner join SO_SODetails_extradefine b on a.isosid=b.isosid where ID in (select id from so_somain where cdefine10='" + sob.cord_code + "') and cinvcode='" + sob.cinv_code + "' and b.cbdefine21='" + sob.ori_req_id + "'");
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select a.isosid from SO_SODetails a inner join SO_SODetails_extradefine b on a.isosid=b.isosid where ID in (select id from so_somain where cdefine10='" + sob.cord_code + "') and cinvcode='" + sob.cinv_code + "' and b.cbdefine21='"+sob.ori_req_id+"'");                                        
                                        break;
                                    //case "dlid":
                                    //    xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select dlid from dispatchlist where cdlcode='" + sob.cdsp_code + "'");
                                    //    break;
                                    //case "idlsid":
                                    case "icorid":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select idlsid from dispatchlists where dlid in (select dlid from dispatchlist where cdlcode='" + sob.cdsp_code + "') and cinvcode='" + sob.cinv_code + "'");
                                        break;
                                    case "cordercode":
                                    case "csocode":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select csocode from so_somain where cdefine10='" + sob.cord_code + "'");
                                        break;
                                    case "iquantity":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = iquantity.ToString();
                                        break;
                                    case "cwhcode":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = cwhcode;
                                        break;
                                    case "isum":
                                    case "imoney":
                                    case "inatsum":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = isum.ToString();
                                        break;
                                    case "ccorcode":
                                        xnNow.attributes.getNamedItem(doel.nodeName.ToString()).text = Ufdata.getDataReader(m_ologin.UfDbName, "select csocode from so_somain where cdefine10='" + sob.cord_code + "'");
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
                        invcodereqidsn = sob.cinv_code + sob.req_id + sob.sn;
                        guid = Guid.NewGuid().ToString();
                        getPortfolio(sob.cinv_code, iquantity, ref dom_body,ref xnNow, m_ologin,cwhcode,ordercode,orireqid,sob.sn,guid );
                    }
                    //else//sn不同继续
                    //{                        
                    //    if (sob.cinv_code+sob.req_id+sob.sn!=invcodereqidsn)
                    //    {
                    //        invcodereqidsn = sob.cinv_code + sob.req_id + sob.sn;
                    //        getPortfolio(sob.cinv_code, iquantity, ref dom_body,ref xnNow, m_ologin, cwhcode, ordercode, orireqid, sob.sn,false,guid);
                    //    }
                    //}
                    
                    //i++;
                }
                if (dom_body.selectSingleNode("//rs:data").childNodes.length > 1)
                {
                    dom_body.selectSingleNode("//rs:data").removeChild(xnModel);
                }
                dom_body.save(AppDomain.CurrentDomain.BaseDirectory+"Logs\\dispatchreturn_body111.xml");
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

                

                if (string.IsNullOrEmpty(result))
                {
                    re.recode = "0";
                    re.u8code ="销售发货单/退货单："+Ufdata.getDataReader(m_ologin.UfDbName,"select cdlcode from DispatchList where DLID=" +vNewIDRet);
                    verify_so(so,ref re, vNewIDRet, dom_head, dom_body, companycode,cexchan);
                }
                else
                {
                    re.recode = "111";
                    re.remsg = result;
                    LogHelper.WriteLog(typeof(DispatchReturnEntity),result);
                }



            }
            catch (Exception ex)
            {
                re.oacode = so.head.ccode;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(DispatchReturnEntity), ex);
            }
            finally
            {
                //conn.Close();
                //结束本次调用，释放API资源
                broker.Release();
            }
            return re;
        }
        private static string getItemValue(string itemName, Dispatchreturn_head item)//得到head参数值
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
        private static string getItemValue(string itemName, Dispatchreturn_body item)//得到body参数值
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

        //组合销售
        private static void getPortfolio(string cinvcode, decimal iquantity, ref MSXML2.IXMLDOMDocument2 dom_body,ref MSXML2.IXMLDOMNode xnNow, U8Login.clsLoginClass m_ologin,string cwhcode,string ordercode,string orireqid,string invsn,string guid )
        {
            string PTOModel = Ufdata.getDataReader(m_ologin.UfDbName, "select bPTOModel from inventory where cinvcode='" + cinvcode + "'");
            string bService = Ufdata.getDataReader(m_ologin.UfDbName, "select bservice from inventory where cinvcode='" + cinvcode + "'");
            LogHelper.WriteLog(typeof(DispatchReturnEntity), "select bservice from inventory where cinvcode='" + cinvcode + "'");
            LogHelper.WriteLog(typeof(DispatchReturnEntity),"bService:"+bService);
            string FromSql = " from [dbo].[bom_opcomponent] a inner join bom_parent b on a.BomId=b.BomId inner join bas_part c on b.ParentId=c.PartId inner join bom_bom d on a.BomId=d.BomId inner join bas_part e on a.ComponentId=e.PartId inner join inventory f on e.InvCode=f.cInvCode";// inner join (select distinct cInvSN,cInvCode from ST_SNDetail_SaleOut) g on f.cinvcode=g.cinvcode";
            string WhereSQL = " where c.InvCode='" + cinvcode + "' and d.Status=3 and d.AuditStatus=1";// and g.cInvSN='"+invsn+"'";
            string strSql = "select cInvCode,cInvName,baseQty,round(100*(1)/baseQty/bb.sumQty,8) fchildrate,ParentId,bservice from (select b.ParentId,f.cInvCode,f.cInvName,a.ComponentId,BaseQtyN/BaseQtyD baseQty,f.bservice"
                + FromSql + WhereSQL + ") aa,("
                + "select COUNT(a.ComponentId) sumQty"
                + FromSql + WhereSQL + ") bb";

            LogHelper.WriteLog(typeof(DispatchReturnEntity), strSql);
            

            if ((PTOModel == "1") || (PTOModel.ToLower() == "true"))
            {
                //if (newParent)
                //{
                    xnNow.attributes.getNamedItem("cparentcode").text = guid;
                    xnNow.attributes.getNamedItem("cwhcode").text = "";
                    if ((bService == "0") || (bService.ToLower() == "false"))
                    {
                        xnNow.attributes.getNamedItem("cwhcode").text = cwhcode;
                    }
                    dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                //}
                DataTable dtComponent = Ufdata.getDatatableFromSql(m_ologin.UfDbName, strSql);
                foreach (DataRow dr in dtComponent.Rows)
                {
                    MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                    xnNowClone.attributes.getNamedItem("cparentcode").text = "";
                    xnNowClone.attributes.getNamedItem("cchildcode").text = guid;
                    xnNowClone.attributes.getNamedItem("cinvcode").text = dr["cInvCode"].ToString();
                    xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                    xnNowClone.attributes.getNamedItem("cinvname").text = dr["cInvName"].ToString();
                    //if (string.IsNullOrEmpty(invsn))
                    //{
                    //    xnNowClone.attributes.getNamedItem("iquantity").text = iquantity.ToString();
                    //}
                    //else
                    //{
                    //    xnNowClone.attributes.getNamedItem("iquantity").text = "-1";
                    //}
                    xnNowClone.attributes.getNamedItem("iquantity").text = (iquantity * Convert.ToDecimal(dr["baseQty"])).ToString();
                    //xnNow.attributes.getNamedItem("ipartid").text = dr["cInvCode"].ToString();
                    xnNowClone.attributes.getNamedItem("fchildqty").text = dr["baseQty"].ToString();
                    xnNowClone.attributes.getNamedItem("fchildrate").text = dr["fchildrate"].ToString();
                    LogHelper.WriteLog(typeof(DispatchReturnEntity), "select a.isosid from SO_SODetails a inner join SO_SODetails_extradefine b on a.isosid=b.isosid where ID in (select id from so_somain where cdefine10='" + ordercode + "') and cinvcode='" + dr["cInvCode"].ToString() + "' and b.cbdefine21='" +orireqid + "'");
                    xnNowClone.attributes.getNamedItem("isosid").text = Ufdata.getDataReader(m_ologin.UfDbName, "select a.isosid from SO_SODetails a inner join SO_SODetails_extradefine b on a.isosid=b.isosid where ID in (select id from so_somain where cdefine10='" + ordercode + "') and cinvcode='" + dr["cInvCode"].ToString() + "' and b.cbdefine21='" + orireqid + "'");
                    //if Ufdata.getDataReader(m_ologin.UfDbName,"select bService from inventory where cinvcode='"+dr["cInvCode"].ToString()"'")
                    if (dr["bservice"].ToString()=="1")
                    {
                        xnNowClone.attributes.getNamedItem("cwhcode").text = "";
                    }
                    else
                    {
                        xnNowClone.attributes.getNamedItem("cwhcode").text = cwhcode;
                    }
                    dom_body.selectSingleNode("//rs:data").appendChild(xnNowClone);
                }
            }
            else
            {
                
                    if ((bService == "1") || (bService.ToLower() == "true"))
                    {
                        xnNow.attributes.getNamedItem("cwhcode").text = "";
                        //if (newParent)
                        { dom_body.selectSingleNode("//rs:data").appendChild(xnNow); }
                    }
                    else
                    {
                        dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                    }
                    
                
            }
        }
        //审核发货单
        private static void verify_so(DispatchReturn so, ref Result re, string vNewIDRet, MSXML2.IXMLDOMDocument2 dom_head, MSXML2.IXMLDOMDocument2 dom_body, string oacode, string cexchan)
        {
            
                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(oacode);
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = (U8Login.clsLogin)m_ologin;

                //销售所有接口均支持内部独立事务和外部事务，默认内部事务
                //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
                //envContext.BizDbConnection = new ADO.Connection();
                //envContext.IsIndependenceTransaction = false;

                //设置上下文参数
                envContext.SetApiContext("VoucherType", 10); //上下文数据类型：int，含义：单据类型：10

                //第三步：设置API地址标识(Url)
                //当前API：审核或弃审的地址标识为：U8API/ReturnOrder/Audit
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ReturnOrder/Audit");
            
                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);
            try
            {
                //方法一是直接传入MSXML2.DOMDocumentClass对象
                broker.AssignNormalValue("domHead", dom_head);

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
                        re.u8code += ",审核成功";
                        if (so.head.cexchan != "换货")
                        { SaleOutEntity.Add_so(ref re, cexchan, m_ologin, vNewIDRet, so); }
                    }
                    else
                    {
                        re.recode = "111";
                        re.remsg = result;
                        LogHelper.WriteLog(typeof(DispatchReturnEntity), result);
                    }
                }
            }
            catch(Exception ex)
            {
                //re.oacode = oacode;
                re.recode = "999";
                re.remsg = ex.Message;
                LogHelper.WriteLog(typeof(DispatchReturnEntity), ex);
            }
            finally
            {
                //结束本次调用，释放API资源
                //broker.Release();
            }
        }
    }
}