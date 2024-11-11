using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.BorrowTrial;
using XylinkU8Interface.Models.Result;

using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
/*
    *20230910 
    *lijianqiang
    * 试用、⽣成其他出库单并审核通过
 */
namespace XylinkU8Interface.UFIDA
{
    public class BorrowTrialEntity
    {
        //新增其他出库单
        public static Result addBorrowTrial(InMain inMain)
        {
            string strResult, strSql;
            Result result = new Result();
            MSXML2.IXMLDOMDocument2 dom_head, dom_body;
            dom_head = new MSXML2.DOMDocument30();
            dom_body = new MSXML2.DOMDocument30();

            try
            {
                #region//init

                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(inMain.companycode);
                if (m_ologin == null)
                {
                    strResult = "帐套" + inMain.companycode + "登录失败";
                    result.recode = "111";
                    result.remsg = strResult;
                    return result;
                }
                dom_head.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherouthead_blue.xml");
                dom_body.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otheroutbody_blue.xml");
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
                string cwhcode = Ufdata.getDataReader(m_ologin.UfDbName, "select cwhcode from warehouse where cwhname='" + inMain.body[0].cwhname.ToString() + "'");
                if (string.IsNullOrEmpty(cwhcode))
                {
                    strResult = inMain.body[0].cwhname + "在仓库档案中不存在";
                    result.recode = "333";
                    result.remsg = strResult;
                    return result;
                }
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cwhcode").text = cwhcode;
               

                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cmaker").text = inMain.head.personName;
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ddate").text = Convert.ToDateTime(inMain.head.ddate).ToShortDateString();
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccode").text = inMain.head.ccode;
                
                //20220907 使用表头扩展自定义项35
                //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("chdefine35").text = req.head.category;
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdefine12").text = inMain.head.ccode;
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cmemo").text = inMain.head.cmemo;
                
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("csource").text = "借出借用单";
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cbuscode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                    "select a.cCODE from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on b.ID=a.ID inner join HY_DZ_BorrowOuts_extradefine c on c.AutoID=b.AutoID where c.cbdefine21='" + inMain.body[0].reqId + "'");
                //20220915 取原借用借出单的客户编码
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ccuscode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                    "select a.bObjectCode from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on b.ID=a.ID inner join HY_DZ_BorrowOuts_extradefine c on c.AutoID=b.AutoID where c.cbdefine21='" + inMain.body[0].reqId + "'");

                //220 试用机借用
                //dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("crdcode").text = "220";
                // U8其他出库单-出库类型（2023-12-07 新增字段）
                string rdcode = Ufdata.getDataReader(m_ologin.UfDbName,"select cRdCode from Rd_Style where cRdName='"+inMain.head.sendType+"'");
                if (string.IsNullOrEmpty(rdcode))
                {
                    rdcode = "220";
                }
                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("crdcode").text = rdcode;

                dom_head.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("cdepcode").text = Ufdata.getDataReader(m_ologin.UfDbName, 
                    "select a.cdepcode from HY_DZ_BorrowOut a inner join HY_DZ_BorrowOuts b on b.ID=a.ID inner join HY_DZ_BorrowOuts_extradefine c on c.AutoID=b.AutoID where c.cbdefine21='" + inMain.body[0].reqId + "'");

                dom_head.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherouthead_red111.xml");
                broker.AssignNormalValue("domhead", dom_head);
                #endregion

                #region//body
                MSXML2.IXMLDOMNode xnModel = null;
                xnModel = dom_body.selectSingleNode("//rs:data//z:row");
                int i = 1;
                foreach (InBody inBody in inMain.body)
                {
                    foreach (InDetail inDetail in inBody.detail)
                    {
                        MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);

                        xnNow.attributes.getNamedItem("irowno").text = i.ToString();
                        xnNow.attributes.getNamedItem("cinvcode").text = inDetail.cinvCode;
                        xnNow.attributes.getNamedItem("cbdefine21").text = inBody.reqId;

                        xnNow.attributes.getNamedItem("iquantity").text = inDetail.iquantity.ToString();

                        xnNow.attributes.getNamedItem("editprop").text = "A";


                        string idebitids = Ufdata.getDataReader(m_ologin.UfDbName,
                            "select c.autoid from HY_DZ_BorrowOuts_extradefine c inner join HY_DZ_BorrowOuts b on b.autoid=c.autoid where c.cbdefine21='"
                            + inBody.reqId + "' and b.cinvcode='" + inDetail.cinvCode + "'");
                        xnNow.attributes.getNamedItem("idebitids").text = idebitids;
                        xnNow.attributes.getNamedItem("idebitchildids").text = idebitids;


                        xnNow.attributes.getNamedItem("cbdefine4").text = inBody.recvName;
                        xnNow.attributes.getNamedItem("cbdefine5").text = inBody.recvPhone;
                        xnNow.attributes.getNamedItem("cbdefine9").text = inBody.recvAddress;

                        //2024-04-17 bcosting=false
                        xnNow.attributes.getNamedItem("bcosting").text = "False";


                        i++;
                        dom_body.selectSingleNode("//rs:data").appendChild(xnNow);
                    }
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
                 broker.AssignNormalValue("bIsRedVouch", false);
                      

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
                            result.recode = "999";
                            result.remsg = "系统异常：" + sysEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            //Console.WriteLine("API异常：" + bizEx.Message);
                            result.recode = "999";
                            result.remsg = "API异常：" + bizEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        //异常原因
                        String exReason = broker.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            //Console.WriteLine("异常原因：" + exReason);
                            result.recode = "888";
                            result.remsg = "异常原因：" + exReason;
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
                    result.recode = "111";
                    result.remsg = errMsgRet;
                }
                else
                {
                    //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                    result.recode = "0";
                    result.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from rdrecord09 where ID=" + VouchIdRet);
                    /*
                    2022-11-17
                    修复其他出库单表体自定义项显示业务号和出库单号                       
                    */

                    strSql = "update rdrecords09 set cDefine32=b.cCode,cDefine33=b.cBusCode,bcosting=0 from rdrecords09 a,rdrecord09 b where a.id=b.ID and  cDefine32 is null and cDefine33 is null and b.cCode='" + result.u8code + "'";
                    Ufdata.execSqlcommand(m_ologin.UfDbName, strSql);

                    //其他出库单SN
                    strResult = addSTSNOtherout(inMain,m_ologin, VouchIdRet);
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        result.recode = "555";
                        result.remsg += "序列号保存失败：" + strResult;
                        return result;
                    }
                    //审核其他出库单
                    strResult =verifyOtherIn(m_ologin, VouchIdRet);
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        result.recode = "666";
                        result.remsg += "其他出库单审核失败：" + strResult;
                        return result;
                    }
                }
                #endregion
           
            }
            catch (Exception ex)
            {
                result.recode = "9999";
                result.remsg = ex.Message;
                LogHelper.WriteLog(typeof(BorrowTrialEntity), ex);
            }
            return result;
        }
        //其他出库单SN
        public static string addSTSNOtherout(InMain inMain, U8Login.clsLoginClass m_ologin, string VouchIdRet)
        {
            string strResult = "";
            Result re = new Result();
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn = new ADODB.Connection();
            MSXML2.IXMLDOMDocument2 domSN = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            domSN.load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\otherinsn.xml");
            try
            {
                domHead = getOtherOutDom(m_ologin, VouchIdRet, "domhead");
                domBody = getOtherOutDom(m_ologin, VouchIdRet, "dombody");
                string ufts = Ufdata.getDataReader(m_ologin.UfDbName, "select convert(money,ufts) ufts from rdrecord09 where ID=" + VouchIdRet);
                domHead.selectSingleNode("//rs:data//z:row").attributes.getNamedItem("ufts").text = ufts;
                conn.Open(m_ologin.UfDbName);
                usn.Init(m_ologin, conn, strResult);
                MSXML2.IXMLDOMNode xnModel = domSN.selectSingleNode("//rs:data//z:row");
                int rowno = 1;
                foreach (InBody inBody in inMain.body)
                {
                    foreach (InDetail inDetail in inBody.detail)
                    {
                        if (inDetail.sncodes != null)
                        {
                            foreach (InSncode sncode in inDetail.sncodes)
                            {
                                rowno = 1;
                                MSXML2.IXMLDOMNode xnNow = xnModel.cloneNode(true);
                                xnNow.attributes.getNamedItem("ivouchid").text = VouchIdRet;

                                xnNow.attributes.getNamedItem("irowno").text = rowno.ToString();
                                xnNow.attributes.getNamedItem("cinvcode").text = inDetail.cinvCode;
                                xnNow.attributes.getNamedItem("cwhcode").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                    "select cwhcode from rdrecord09 where ID=" + VouchIdRet + "");
                                xnNow.attributes.getNamedItem("editprop").text = "A";
                                xnNow.attributes.getNamedItem("ivouchsid").text = Ufdata.getDataReader(m_ologin.UfDbName,
                                   "select a.AutoID from rdrecords09 a inner join rdrecords09_extradefine b on a.AutoID=b.AutoID where a.ID="
                                   + VouchIdRet + " and b.cbdefine21='" + inBody.reqId + "' and a.cInvCode='" + inDetail.cinvCode + "'");
                                xnNow.attributes.getNamedItem("cinvsn").text = sncode.sncode;
                                xnNow.attributes.getNamedItem("ufts").text = ufts;
                                domSN.selectSingleNode("//rs:data").appendChild(xnNow);
                                rowno++;
                            }
                        }
                    }
                    
                }
                domSN.selectSingleNode("//rs:data").removeChild(xnModel);
                domSN.save(AppDomain.CurrentDomain.BaseDirectory + "Logs\\otherout_red_sn111.xml");
                if (domSN.selectSingleNode("//rs:data").childNodes.length >= 1)
                {
                    bool bResult = usn.Save(conn, "09", "add", ref domHead, ref domBody, domSN, ref strResult, false);
                    if (bResult)
                    { 
                        strResult = ""; 
                    }
                    
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return strResult;
            
        }
        //审核其他出库单
        public static string verifyOtherIn(U8Login.clsLoginClass m_ologin, string VouchIdRet)
        {
            string strResult = "";
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = m_ologin;

            //第三步：设置API地址标识(Url)
            //当前API：审核单据的地址标识为：U8API/otherin/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Audit");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：08
            broker.AssignNormalValue("sVouchType", "09");

            //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
            broker.AssignNormalValue("VouchId", VouchIdRet);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象：调用方控制事务时需要传入连接对象
            broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

            //给普通参数TimeStamp赋值。此参数的数据类型为System.Object，此参数按值传递，表示单据时间戳，用于检查单据是否修改，空串时不检查
            broker.AssignNormalValue("TimeStamp", null);

            //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument();
            broker.AssignNormalValue("domMsg", domMsg);

            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock", false);

            //给普通参数bList赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示传入空串
            broker.AssignNormalValue("bList", false);

            //给普通参数MakeWheres赋值。此参数的数据类型为VBA.Collection，此参数按值传递，表示传空
            broker.AssignNormalValue("MakeWheres", null);

            //给普通参数sWebXml赋值。此参数的数据类型为System.String，此参数按值传递，表示传入空串
            broker.AssignNormalValue("sWebXml", "");

            //给普通参数oGenVouchIds赋值。此参数的数据类型为Scripting.IDictionary，此参数按值传递，表示返回审核时自动生成的单据的id列表,传空
            broker.AssignNormalValue("oGenVouchIds", null);
            //broker.Invoke();
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
                        strResult="verifyOtherOut:" + "系统异常：" + sysEx.Message;
                        LogHelper.WriteLog(typeof(STSNEntity), strResult);
                        return strResult;
                        
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        strResult = "verifyOtherOut:" + "API异常：" + bizEx.Message;
                        LogHelper.WriteLog(typeof(STSNEntity), strResult);
                        return strResult;
                        
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        strResult ="verifyOtherOut:" + "异常原因：" + exReason;
                        LogHelper.WriteLog(typeof(STSNEntity), strResult);
                        return strResult;
                    }
                }

            }
            String errMsgRet = "";
            if (broker.GetResult("errMsg") != null)
            { 
                errMsgRet = broker.GetResult("errMsg").ToString(); 
            }
            if (!string.IsNullOrEmpty(errMsgRet))
            { 
                strResult="verifyOtherIn:" + "审核失败：" + errMsgRet;
                LogHelper.WriteLog(typeof(STSNEntity), strResult);
                return strResult;
            }
            //结束本次调用，释放API资源
            broker.Release();
            return strResult;
        }
        //获取其他出库单数据
        public static MSXML2.DOMDocument getOtherOutDom(U8Login.clsLoginClass m_ologin, string VouchIdRet, string ctype)
        {
            MSXML2.DOMDocument domResult = new MSXML2.DOMDocument();
            try
            {
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = m_ologin;

                //第三步：设置API地址标识(Url)
                //当前API：装载单据的地址标识为：U8API/otherin/Load
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Load");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:08
                broker.AssignNormalValue("sVouchType", "09");

                //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
                broker.AssignNormalValue("sWhere", "id=" + VouchIdRet + "");

                //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument(); ;
                broker.AssignNormalValue("domPos", domPos);

                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
                broker.AssignNormalValue("bGetBlank", false);

                //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
                broker.AssignNormalValue("sBodyWhere_Order", "autoid");

                bool bResult = broker.Invoke();
                domResult = (MSXML2.DOMDocument)broker.GetResult(ctype);
            }
            catch (Exception ex)
            {
                //do nothing
            }
            return domResult;
        }
        
    }
}