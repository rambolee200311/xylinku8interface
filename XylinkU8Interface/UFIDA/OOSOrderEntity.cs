using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.OOSOrder;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
namespace XylinkU8Interface.UFIDA
{
    public class OOSOrderEntity
    {
        public static ClsResponse postResquest(ClsRequest req)
        {
            string strResult,strSql;
            ClsResponse rep=new ClsResponse();
            MSXML2.IXMLDOMDocument2 dom_head,dom_body;
            dom_head = new MSXML2.DOMDocument30();
            dom_body = new MSXML2.DOMDocument30();
            rep.oacode = req.head.ccode;
            rep.companycode = req.companycode;
            try
            {
                #region//init

                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(req.companycode);
                if (m_ologin == null)
                {
                    strResult = "帐套" + req.companycode + "登录失败";
                    rep.recode = "111";
                    rep.remsg = strResult;
                    return rep;
                }
               
                strSql = "select a.cDefine12 from rdrecord09 a inner join rdrecords09 b on a.ID=b.ID where a .cdefine12='" + req.head.ccode + "'";
                switch (req.head.category)
                {
                    case "售后换货出库-CRM入库":
                    case "试⽤业务SN的调换-CRM入库":
                        strSql += " and b.iquantity<0";
                        dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherouthead_red.xml");
                        dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otheroutbody_red.xml");
                        break;
                    case "售后换货出库-CRM出库":
                    case "试⽤业务SN的调换-CRM出库":
                        strSql += " and b.iquantity>0";
                        dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherouthead_blue.xml");
                        dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otheroutbody_blue.xml");
                        break;
                    default:
                        strSql += " and b.iquantity>0";
                        dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherouthead_blue.xml");
                        dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otheroutbody_blue.xml");
                        break;
                }
                /*
               if (!string.IsNullOrEmpty(Ufdata.getDataReader(m_ologin.UfDbName, strSql)))
               {
                   strResult = req.head.ccode + "已存在其他出库单，不能重复同步";
                   rep.recode = "222";
                   rep.remsg = strResult;
                   return rep;
               }
               */
                #endregion

                #region//api params
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = m_ologin;

                //第三步：设置API地址标识(Url)
                //当前API：添加新单据的地址标识为：U8API/otherout/Add
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Add");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：09
                broker.AssignNormalValue("sVouchType", "09");


                #endregion

                #region//head
                    string cwhcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + req.body[0].warehouse + "'");
                    if (string.IsNullOrEmpty(cwhcode))
                    {
                        strResult = req.body[0].warehouse + "在仓库档案中不存在";
                        rep.recode = "333";
                        rep.remsg = strResult;
                        return rep;
                    }
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cwhcode").text = cwhcode;
                    //20220915 取消客户档案校验
                    //string ccuscode = Ufdata.getDataReader(m_ologin.UfDbName, "select ccuscode from customer where ccusname='" + req.head.custName + "'");
                    //if (string.IsNullOrEmpty(ccuscode))
                    //{
                    //    strResult = req.head.custName + "在客户档案中不存在";
                    //    rep.recode = "333";
                    //    rep.remsg = strResult;
                    //    return rep;
                    //}
                    //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccuscode").text = ccuscode;

                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cmaker").text = req.head.personName;
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ddate").text =Convert.ToDateTime(req.head.ddate).ToShortDateString();
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccode").text = req.head.ccode;
                    //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdefine10").text = req.head.category;
                    //20220907 使用表头扩展自定义项35
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("chdefine35").text = req.head.category;
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdefine12").text = req.head.ccode;
                    dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cmemo").text = req.head.cmemo;
                    if ((req.head.category == "试⽤业务SN的调换-CRM出库")||(req.head.category == "试⽤业务SN的调换-CRM入库"))
                    {
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("csource").text = "借出借用单";
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cbuscode").text =Ufdata.getDataReader(m_ologin.UfDbName,
                            "select a.cCODE from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on b.ID=a.ID inner join HY_DZ_BorrowOuts_extradefine c on c.AutoID=b.AutoID where c.cbdefine21='"+req.body[0].ori_reqid +"'");
                        //20220915 取原借用借出单的客户编码
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccuscode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                            "select a.bObjectCode from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on b.ID=a.ID inner join HY_DZ_BorrowOuts_extradefine c on c.AutoID=b.AutoID where c.cbdefine21='" + req.body[0].ori_reqid + "'");
                    }
                    else
                    {
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("csource").text = "库存";
                        dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cbuscode").text = "";
                    }
                    dom_head.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherouthead_red111.xml");
                    broker.AssignNormalValue("domhead", dom_head);
                #endregion

                #region//body
                    MSXML2.IXMLDOMNode xnModel = null;
                    xnModel = dom_body.selectSingleNode("//rs:data//z:row");
                    int i = 1;
                    foreach (ClsRequestBody reqBody in req.body)
                    {
                        MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);

                        xnNow.attributes.getNamedItem("irowno").text = i.ToString();
                        xnNow.attributes.getNamedItem("cinvcode").text = reqBody.invcode;
                        xnNow.attributes.getNamedItem("cbdefine21").text = reqBody.reqId;
                        //if (req.head.category == "试⽤业务SN的调换-CRM入库")
                        //{
                        //    xnNow.attributes.getNamedItem("iquantity").text =(-1* reqBody.iquantity).ToString();
                        //}
                        //else
                        //{
                            xnNow.attributes.getNamedItem("iquantity").text = reqBody.iquantity.ToString();
                        //}
                        xnNow.attributes.getNamedItem("editprop").text = "A";
                        //xnNow.attributes.getNamedItem("cbinvsn").text = reqBody.sncode;
                        if ((req.head.category == "试⽤业务SN的调换-CRM出库") || (req.head.category == "试⽤业务SN的调换-CRM入库"))
                        {
                            string idebitids = Ufdata.getDataReader(m_ologin.UfDbName, 
                                "select c.autoid from HY_DZ_BorrowOuts_extradefine c inner join HY_DZ_BorrowOuts b on b.autoid=c.autoid where c.cbdefine21='" 
                                + reqBody.ori_reqid + "' and b.cinvcode='"+reqBody.invcode+"'");
                            xnNow.attributes.getNamedItem("idebitids").text = idebitids;
                            xnNow.attributes.getNamedItem("idebitchildids").text = idebitids;
                        }

                        xnNow.attributes.getNamedItem("cbdefine4").text = reqBody.receiver;
                        xnNow.attributes.getNamedItem("cbdefine5").text = reqBody.recemobi;
                        xnNow.attributes.getNamedItem("cbdefine9").text = reqBody.receaddress;

                        i++;
                        dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                        /*
                        if (req.head.category == "试⽤业务SN的调换")
                        {
                            MSXML2.IXMLDOMNode xnNowClone = xnNow.cloneNode(true);
                            xnNowClone.attributes.getNamedItem("irowno").text = i.ToString();
                            xnNowClone.attributes.getNamedItem("iquantity").text = (Convert.ToDecimal(reqBody.iquantity) * -1).ToString();
                            dom_body.selectSingleNode("//rs:data").appendChild(xnNowClone);
                            i++;
                        }
                         */
                    }
                    
                    
                    if (dom_body.selectSingleNode("//rs:data").childNodes.length > 1)
                    {
                        dom_body.selectSingleNode("//rs:data").removeChild(xnModel);
                    }
                    dom_body.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otheroutbody_red111.xml");
                    broker.AssignNormalValue("dombody", dom_body);
                #endregion

                #region//proc
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
                    switch (req.head.category)
                    {
                        case "售后换货出库-CRM入库":
                        case "试⽤业务SN的调换-CRM入库":
                            broker.AssignNormalValue("bIsRedVouch", true);
                            break;
                        case "售后换货出库-CRM出库":
                        case "试⽤业务SN的调换-CRM出库":
                            broker.AssignNormalValue("bIsRedVouch", false);
                            break;
                        default:
                            broker.AssignNormalValue("bIsRedVouch", true);
                            break;
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
                                rep.recode = "999";
                                rep.remsg = "系统异常：" + sysEx.Message;
                                //return re;
                                //todo:异常处理
                            }
                            else if (apiEx is MomBizException)
                            {
                                MomBizException bizEx = apiEx as MomBizException;
                                //Console.WriteLine("API异常：" + bizEx.Message);
                                rep.recode = "999";
                                rep.remsg = "API异常：" + bizEx.Message;
                                //return re;
                                //todo:异常处理
                            }
                            //异常原因
                            String exReason = broker.GetExceptionString();
                            if (exReason.Length != 0)
                            {
                                //Console.WriteLine("异常原因：" + exReason);
                                rep.recode = "888";
                                rep.remsg = "异常原因：" + exReason;
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
                        rep.recode = "111";
                        rep.remsg = errMsgRet;
                    }
                    else
                    {
                        //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                        rep.recode = "0";
                        //STSNEntity.add_STSN(m_ologin, "32", so, VouchIdRet);
                        rep.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from rdrecord09 where ID=" + VouchIdRet);


                        strResult = STSNEntity.add_STSN(m_ologin, req, VouchIdRet);
                        if (!string.IsNullOrEmpty(strResult))
                        {
                            rep.recode = "555";
                            rep.remsg += "序列号保存失败：" + strResult;
                        }
                        
                    }
                #endregion
            }
            catch (Exception ex)
            {

                rep.recode = "999";
                rep.remsg = ex.Message;
                LogHelper.WriteLog(typeof(OOSOrderEntity), ex);
            }

            return rep;
        }
    }
}