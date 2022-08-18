using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Runtime.InteropServices;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.SaleOrder;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.MomServiceCommon;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
using XylinkU8Interface.UFIDA;

namespace XylinkU8Interface
{
    public partial class WebHelper : System.Web.UI.Page
    {
        String sSubId = "AS";
        String sAccID = "995";
        String sYear = "2022";
        String sUserID = "demo";
        String sPassword = "123@QAZxsw";
        String sDate = "2022-08-14";
        String sServer = "localhost";
        String sSerial = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(getDispatchlist());
            //Response.Write(getHelper("saleouthead_red"));
            //Response.Write(getHelper("saleoutbody_red"));
            //Response.Write(getHelper("saleouthead"));
            //Response.Write(getHelper("saleoutbody"));
            //Response.Write(getDispatchlistreturn());
            //Response.Write(getSalebillvouch());
            //Response.Write(getBomdom());
            //Response.Write(MD5CryptoHelper.GetMD5("U8F00CC4106B784FE9A28613059F2A8C0920201012021420"));
            //Response.Write(getSaleOut());
           // Response.Write(setSaleOut());
            //Response.Write(addSaleOut());
            //Response.Write(getVOSN());
            //Response.Write(getOtherIn());
            //Response.Write(verifyOtherIn());
            //Response.Write(verifySaleOut());
            //Response.Write(getOtherOut());
            Response.Write(delOtherOutSn());
        }

        private string getHelper(string sname)
        {
            string result = "";
            XmlDocument xmlMode = new XmlDocument();
            //string sname = "saleouthead_red";
            xmlMode.Load(AppDomain.CurrentDomain.BaseDirectory + "Helper\\" + sname + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlElement xdata = xmlDoc.CreateElement("data");

            XmlNode xnode = xmlMode.SelectSingleNode("xml").ChildNodes[1].FirstChild;
            foreach (XmlAttribute xatt in xnode.Attributes)
            {

                XmlElement xel = xmlDoc.CreateElement(xatt.Name.ToString());
                xel.InnerText = "{" + xatt.Value + "}";
                xdata.AppendChild(xel);

            }
            xmlDoc.AppendChild(xdata);
            xmlDoc.Save("D:\\"+sname+ "_model.xml");
            
            result = xmlDoc.OuterXml;
            return result;
        }
        private string getBorrowOut()
        {
            string result = "";
            //U8Login.clsLogin m_ologin = new U8Login.clsLogin();
            //m_ologin = U8LoginEntity.getU8LoginEntityInterop("001");
            //HY_DZ_BorrowOut.clsBorrowOutSrvClass cls_Borrow_Out = new HY_DZ_BorrowOut.clsBorrowOutSrvClass();
            //cls_Borrow_Out.Init(ref m_ologin);
            //MSXML2.IXMLDOMDocument2 oHead = new MSXML2.DOMDocument30Class();
            //MSXML2.IXMLDOMDocument2 oBody = new MSXML2.DOMDocument30Class();
            //string err_Msg = "";
            //cls_Borrow_Out.LoadVoucher("ID=1000002556", ref oHead, ref oBody, ref err_Msg);
            //oHead.save("d:\\abc\\borrow_out_head.xml");
            //oBody.save("d:\\abc\\borrow_out_body.xml");
            //if (!string.IsNullOrEmpty(err_Msg))
            //{
            //    result = err_Msg;
            //}
            //if (string.IsNullOrEmpty(result))
            //{
            //    result = "it is ok!";
            //}
            //HY_Borrow_out.ClsBorrowOut cls_Borrow_Out = new HY_Borrow_out.ClsBorrowOut();
           // result = cls_Borrow_Out.Set_Borrow_Out();
            return result;
        }
    
        private string getDispatchlist()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {
                
                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }

            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 9); //上下文数据类型：int，含义：单据类型：9

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/Consignment/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/Consignment/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
            broker.AssignNormalValue("VouchID", "1000046103");

            //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
            broker.AssignNormalValue("blnAuth", false);

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
                        return "系统异常：" + sysEx.Message;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);
                        return  "API异常：" + bizEx.Message;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        return "异常原因：" + exReason;
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                //return;
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
            result = broker.GetReturnValue() as System.String;

            //获取out/inout参数值

            //out参数domHead为BO对象(表头)，此BO对象的业务类型为发货单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("domHead") as MSXML2.DOMDocument
            MSXML2.DOMDocument ohead = broker.GetResult("domHead") as MSXML2.DOMDocument;
            MSXML2.DOMDocument obody = broker.GetResult("domBody") as MSXML2.DOMDocument;
            ohead.save("d:\\dispatchlist_head.xml");
            obody.save("d:\\dispatchlist_body.xml");
            return result;
        }

        private string getDispatchlistreturn()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }

            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 10); //上下文数据类型：int，含义：单据类型：10


            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/Consignment/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/ReturnOrder/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
            broker.AssignNormalValue("VouchID", "1000046104");

            //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
            broker.AssignNormalValue("blnAuth", false);

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
                        return "系统异常：" + sysEx.Message;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);
                        return "API异常：" + bizEx.Message;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        return "异常原因：" + exReason;
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                //return;
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
            result = broker.GetReturnValue() as System.String;

            //获取out/inout参数值

            //out参数domHead为BO对象(表头)，此BO对象的业务类型为发货单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("domHead") as MSXML2.DOMDocument
            MSXML2.DOMDocument ohead = broker.GetResult("domHead") as MSXML2.DOMDocument;
            MSXML2.DOMDocument obody = broker.GetResult("domBody") as MSXML2.DOMDocument;
            ohead.save("d:\\dispatchlistreturn_head.xml");
            obody.save("d:\\dispatchlistreturn_body.xml");
            if (string.IsNullOrEmpty(result))
            {
                result = "<return>it is ok</return>";
            }
            return result;
        }

        private string getSalebillvouch()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }

            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //销售所有接口均支持内部独立事务和外部事务，默认内部事务
            //如果是外部事务，则需要传递ADO.Connection对象，并将IsIndependenceTransaction属性设置为false
            //envContext.BizDbConnection = new ADO.Connection();
            //envContext.IsIndependenceTransaction = false;

            //设置上下文参数
            envContext.SetApiContext("VoucherType", 2); //上下文数据类型：int，含义：单据类型：2

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/NormalInvoice/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/NormalInvoice/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数VouchID赋值。此参数的数据类型为string，此参数按值传递，表示单据号
            broker.AssignNormalValue("VouchID", "1000006096");

            //给普通参数blnAuth赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制权限：true
            broker.AssignNormalValue("blnAuth", false);



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
                        return "系统异常：" + sysEx.Message;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);
                        return "API异常：" + bizEx.Message;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        return "异常原因：" + exReason;
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                //return;
            }

            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.String，此参数按值传递，表示成功返回空串
            result = broker.GetReturnValue() as System.String;

            //获取out/inout参数值

            //out参数domHead为BO对象(表头)，此BO对象的业务类型为发货单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("domHead") as MSXML2.DOMDocument
            MSXML2.DOMDocument ohead = broker.GetResult("domHead") as MSXML2.DOMDocument;
            MSXML2.DOMDocument obody = broker.GetResult("domBody") as MSXML2.DOMDocument;
            ohead.save("d:\\salebillvouch_head.xml");
            obody.save("d:\\salebillvouch_body.xml");
            if (string.IsNullOrEmpty(result))
            {
                result = "<return>it is ok</return>";
            }
            return result;
        }

        public string getBomdom()
        {
            string strResult = "it is ok";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
           
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            ////U8Login.clsLoginClass u8Login = U8LoginEntity.getU8LoginEntity("995");
            
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：查询物料清单的地址标识为：U8API/BOM/BomLoad
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/BOM/BomLoad");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数partid赋值。此参数的数据类型为System.Int32，此参数按值传递，表示物料ID
            broker.AssignNormalValue("partid", 1963);

            //给普通参数bomtype赋值。此参数的数据类型为System.Int32，此参数按值传递，表示BOM类型(1主/2替代)
            broker.AssignNormalValue("bomtype", 1);

            //给普通参数versionoridencode赋值。此参数的数据类型为System.String，此参数按值传递，表示主版本或替代标识
            broker.AssignNormalValue("versionoridencode", "10");

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
                        //todo:异常处理
                        strResult = "系统异常：" + sysEx.Message;
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);
                        strResult = "API异常：" + bizEx.Message;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        strResult = "异常原因：" + exReason;
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                return strResult;
            }
            //MSXML2.DOMDocument30 extbo = broker.GetResult("extbo") as MSXML2.DOMDocument30;
            //extbo.save("d:\\extbo.xml");
            ExtensionBusinessEntity extboRet = broker.GetExtBoEntity("extbo");
            //strResult=extboRet[0]["InvCode"].ToString();
            ExtensionBusinessEntity Bom_Component = extboRet[0].SubEntity["Bom_Component"];
            //strResult = Bom_Component[0]["partid"].ToString();
            MSXML2.DOMDocument30 extbo = new DOMDocument30();
            extbo.loadXML(extboRet.ToString());
            extbo.save("d:\\extbo.xml");
            return strResult;
        }

        private string getSaleOut()
        {
            string result = "";
            try
            {
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                
                if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                {

                    Marshal.FinalReleaseComObject(u8Login);
                    return "登陆失败，原因：" + u8Login.ShareString;
                }
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = u8Login;

                //第三步：设置API地址标识(Url)
                //当前API：装载单据的地址标识为：U8API/saleout/Load
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Load");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:32
                broker.AssignNormalValue("sVouchType", "32");

                //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
                broker.AssignNormalValue("sWhere", "ccode='XSCK202203210032'");

                //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument30Class();
                broker.AssignNormalValue("domPos", domPos);

                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
                broker.AssignNormalValue("bGetBlank", false);

                //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
                broker.AssignNormalValue("sBodyWhere_Order", "cinvcode");

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
                            return "系统异常：" + sysEx.Message;
                            //todo:异常处理
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            //Console.WriteLine("API异常：" + bizEx.Message);
                            return "API异常：" + bizEx.Message;
                            //todo:异常处理
                        }
                        //异常原因
                        String exReason = broker.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            //Console.WriteLine("异常原因：" + exReason);
                            return "异常原因：" + exReason;
                        }
                    }
                    //结束本次调用，释放API资源
                    broker.Release();
                    //return;
                }
                //第七步：获取返回结果

                //获取返回值
                //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
                System.Boolean bresult = Convert.ToBoolean(broker.GetReturnValue());
                result = bresult.ToString();
                //获取out/inout参数值

                //out参数DomHead为BO对象(表头)，此BO对象的业务类型为销售出库单。BO参数均按引用传递，具体请参考服务接口定义
                //如果要取原始的XMLDOM对象结果，请使用GetResult("DomHead") as MSXML2.DOMDocument
                //BusinessObject DomHeadRet = broker.GetBoParam("DomHead");
                MSXML2.DOMDocument domHead = new MSXML2.DOMDocument();
                MSXML2.DOMDocument domBody = new MSXML2.DOMDocument();
                domHead = (MSXML2.DOMDocument)broker.GetResult("domhead");// as MSXML2.DOMDocument30Class;
                domBody = (MSXML2.DOMDocument)broker.GetResult("dombody");// as MSXML2.DOMDocument30Class;
                domHead.save("d:\\saleouthead_red.xml");
                domBody.save("d:\\saleoutbody_red.xml");
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(typeof(WebHelper), e);
            }
            return result;
        }
        private string setSaleOut()
        {
            string result = "";
            try
            {
                U8Login.clsLogin u8Login = new U8Login.clsLogin();
                
                if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
                {

                    Marshal.FinalReleaseComObject(u8Login);
                    return "登陆失败，原因：" + u8Login.ShareString;
                }
                //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                U8EnvContext envContext = new U8EnvContext();
                envContext.U8Login = u8Login;

                //第三步：设置API地址标识(Url)
                U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Add");

                //第四步：构造APIBroker
                U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                //第五步：API参数赋值

                //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：32
                broker.AssignNormalValue("sVouchType", "32");
                MSXML2.IXMLDOMDocument2 dom_body;
                MSXML2.IXMLDOMDocument2 dom_head;
                dom_head = new MSXML2.DOMDocument30();
                dom_body = new MSXML2.DOMDocument30();
                dom_head.load("d:\\saleouthead_red.xml");
                dom_body.load("d:\\saleoutbody_red.xml");
                broker.AssignNormalValue("domHead", dom_head);
                broker.AssignNormalValue("domBody", dom_body);

                #region//process
                //给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                System.Object domPosition = null;
                broker.AssignNormalValue("domPosition", domPosition);

                //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
                System.String errmsg = "";
                broker.AssignNormalValue("errMsg", errmsg);

                //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                ADODB.Connection cnnForm = null;
                broker.AssignNormalValue("cnnFrom", cnnForm);

                //该参数VouchId为INOUT型普通参数。此参数的数据类型为System.String，此参数按值传递。在API调用返回时，可以通过GetResult("VouchId")获取其值
                System.String VouchId = "";
                broker.AssignNormalValue("vouchid", VouchId);

                //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                MSXML2.IXMLDOMDocument2 domMsg = new MSXML2.DOMDocument30();
                broker.AssignNormalValue("domMsg", domMsg);
                //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                broker.AssignNormalValue("bCheck", false);

                //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                broker.AssignNormalValue("bBeforCheckStock", false);

                //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
                  broker.AssignNormalValue("bIsRedVouch", false);
                
                //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
                broker.AssignNormalValue("sAddedState", VouchId);

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
                            
                            result = "系统异常：" + sysEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        else if (apiEx is MomBizException)
                        {
                            MomBizException bizEx = apiEx as MomBizException;
                            //Console.WriteLine("API异常：" + bizEx.Message);
                           
                            result = "API异常：" + bizEx.Message;
                            //return re;
                            //todo:异常处理
                        }
                        //异常原因
                        String exReason = broker.GetExceptionString();
                        if (exReason.Length != 0)
                        {
                            //Console.WriteLine("异常原因：" + exReason);
                           
                            result = "异常原因：" + exReason;
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
                    
                    result = errMsgRet;
                }
                else
                {
                    //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                    System.String VouchIdRet = broker.GetResult("VouchId") as System.String;

                    result= "销售出库单:" + Ufdata.getDataReader(u8Login.UfDbName, "select ccode from rdrecord32 where ID=" + VouchIdRet);
                }
                //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
                //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));

                //结束本次调用，释放API资源
                #endregion
            }
            catch (Exception ex)
            {                
                result =  ex.Message;
                LogHelper.WriteLog(typeof(WebHelper), ex);
            }
            return result;
        }
        private string addSaleOut()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            String sSubId = "AS";
            String sAccID = "995";
            String sYear = "2020";
            String sUserID = "yonyou";
            String sPassword = "";
            String sDate = "2020-11-27";
            String sServer = "localhost";
            String sSerial = "";
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

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
            MSXML2.DOMDocumentClass domHead = new MSXML2.DOMDocumentClass();
            domHead.load("d:\\saleouthead.xml");
            MSXML2.DOMDocumentClass domBody = new MSXML2.DOMDocumentClass();
            domBody.load("d:\\saleoutbody.xml");
            MSXML2.DOMDocumentClass domMsg = new MSXML2.DOMDocumentClass();
            broker.AssignNormalValue("domhead", domHead);
            broker.AssignNormalValue("dombody", domBody);
            broker.AssignNormalValue("dommsg", domMsg);
            //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
            broker.AssignNormalValue("bCheck", false);

            //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
            broker.AssignNormalValue("bBeforCheckStock",false);

            //给普通参数bIsRedVouch赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否红字单据
            broker.AssignNormalValue("bIsRedVouch",false);

            //给普通参数sAddedState赋值。此参数的数据类型为System.String，此参数按值传递，表示传空字符串
            broker.AssignNormalValue("sAddedState", "");

            //给普通参数bReMote赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否远程：转入false
            broker.AssignNormalValue("bReMote", false);
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
                        return "系统异常：" + sysEx.Message;
                        //todo:异常处理
                    }
                    else if (apiEx is MomBizException)
                    {
                        MomBizException bizEx = apiEx as MomBizException;
                        //Console.WriteLine("API异常：" + bizEx.Message);
                        return "API异常：" + bizEx.Message;
                        //todo:异常处理
                    }
                    //异常原因
                    String exReason = broker.GetExceptionString();
                    if (exReason.Length != 0)
                    {
                        //Console.WriteLine("异常原因：" + exReason);
                        return "异常原因：" + exReason;
                    }
                }
                //结束本次调用，释放API资源
                broker.Release();
                //return;
            }
            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
            System.Boolean bresult = Convert.ToBoolean(broker.GetReturnValue());
            result = bresult.ToString();
            //获取out/inout参数值

            //获取普通OUT参数errMsg。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String errMsgRet = broker.GetResult("errMsg") as System.String;

            //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
            System.String VouchIdRet = broker.GetResult("VouchId") as System.String;

            //获取普通OUT参数domMsg。此返回值数据类型为MSXML2.IXMLDOMDocument2，在使用该参数之前，请判断是否为空
            //MSXML2.IXMLDOMDocument2 domMsgRet = Convert.ToObject(broker.GetResult("domMsg"));

            //结束本次调用，释放API资源
            broker.Release();
            

            return result;
        }
    
        private string getVOSN()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            UFSTSNCO.clsUFSTSNCOClass usn = new UFSTSNCO.clsUFSTSNCOClass();
            ADODB.Connection conn=new ADODB.Connection();
            conn.Open(u8Login.UfDbName);
            usn.Init(u8Login,conn,result);
            MSXML2.IXMLDOMDocument2 domSn = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domSn1 = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument();
            //domHead.load("d:\\saleouthead_red.xml");
            //domBody.load("d:\\saleouthead_red.xml");
            domHead.load("d:\\app\\otherouthead_blue111.xml");
            domBody.load("d:\\app\\otheroutbody_blue111.xml");
            object obj1 = null;
            //usn.Load(conn,"32", ref domSn, ref domSn1, result,ref domHead,ref domBody, true,true);
            //domSn.save("d:\\app\\saleouthead_red_sn.xml");
            //domSn1.save("d:\\app\\saleoutbody_red_sn.xml");
            usn.Load(conn, "09", ref domSn, ref domSn1, result, ref domHead, ref domBody, true, true);
            domSn.save("d:\\app\\otherouthead_blue_sn.xml");
            domSn1.save("d:\\app\\otheroutbody_blue_sn1.xml");
            conn.Close();

            return result;
        }

        private string getOtherIn()
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/otherin/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherin/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:08
            broker.AssignNormalValue("sVouchType","08");

            //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
            broker.AssignNormalValue("sWhere", "ID=1000000137");

            //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument();
            broker.AssignNormalValue("domPos", domPos);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
            broker.AssignNormalValue("bGetBlank",false);

            //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
            broker.AssignNormalValue("sBodyWhere_Order", "cinvcode");

            broker.Invoke();
            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
            System.Boolean bresult = Convert.ToBoolean(broker.GetReturnValue());
            result = bresult.ToString();
            //获取out/inout参数值

            //out参数DomHead为BO对象(表头)，此BO对象的业务类型为销售出库单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("DomHead") as MSXML2.DOMDocument
            //BusinessObject DomHeadRet = broker.GetBoParam("DomHead");
            MSXML2.DOMDocument domHead = new MSXML2.DOMDocument();
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocument();
            domHead = (MSXML2.DOMDocument)broker.GetResult("domhead");// as MSXML2.DOMDocument30Class;
            domBody = (MSXML2.DOMDocument)broker.GetResult("dombody");// as MSXML2.DOMDocument30Class;
            domHead.save("d:\\app\\otherinhead111.xml");
            domBody.save("d:\\app\\otherinbody111.xml");

            return result;
        }
        private string verifyOtherIn()
        {
            string strResult = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：审核单据的地址标识为：U8API/otherin/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherin/Audit");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：08
            broker.AssignNormalValue("sVouchType", "08");

            //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
            broker.AssignNormalValue("VouchId", "1000054455");

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值
            //ADODB.Connection conn = new ADODB.Connection();
            //conn.Open(u8Login.UfDbName);
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
            broker.AssignNormalValue("oGenVouchIds",null);
            if(broker.Invoke())
            {
                strResult = "it is ok";
            }
            else
            {
                strResult = broker.GetResult("errMsg") as System.String;

            }
            return strResult;
        }

        private string verifySaleOut()
        {
            string strResult = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：审核单据的地址标识为：U8API/saleout/Audit
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/saleout/Audit");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：32
            broker.AssignNormalValue("sVouchType", "32");

            //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
            broker.AssignNormalValue("VouchId", "1000054456");

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象：调用方控制事务时需要传入连接对象
            broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

            //给普通参数TimeStamp赋值。此参数的数据类型为System.Object，此参数按值传递，表示单据时间戳，用于检查单据是否修改，空串时不检查
            broker.AssignNormalValue("TimeStamp",null);

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
            broker.AssignNormalValue("MakeWheres",null);

            //给普通参数sWebXml赋值。此参数的数据类型为System.String，此参数按值传递，表示传入空串
            broker.AssignNormalValue("sWebXml", "");

            //给普通参数oGenVouchIds赋值。此参数的数据类型为Scripting.IDictionary，此参数按值传递，表示返回审核时自动生成的单据的id列表,传空
            broker.AssignNormalValue("oGenVouchIds",null);

            if (broker.Invoke())
            {
                strResult = "it is ok";
            }
            else
            {
                strResult = broker.GetResult("errMsg") as System.String;

            }
            return strResult;
        }

        private string getOtherOut()//其他出库单
        {
            string result = "";
            U8Login.clsLogin u8Login = new U8Login.clsLogin();
            
            if (!u8Login.Login(ref sSubId, ref sAccID, ref sYear, ref sUserID, ref sPassword, ref sDate, ref sServer, ref sSerial))
            {

                Marshal.FinalReleaseComObject(u8Login);
                return "登陆失败，原因：" + u8Login.ShareString;
            }
            //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
            U8EnvContext envContext = new U8EnvContext();
            envContext.U8Login = u8Login;

            //第三步：设置API地址标识(Url)
            //当前API：装载单据的地址标识为：U8API/otherin/Load
            U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Load");

            //第四步：构造APIBroker
            U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

            //第五步：API参数赋值

            //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型:08
            broker.AssignNormalValue("sVouchType", "09");

            //给普通参数sWhere赋值。此参数的数据类型为System.String，此参数按值传递，表示条件串
            broker.AssignNormalValue("sWhere", "ID=1000060340");

            //该参数domPos为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
            MSXML2.IXMLDOMDocument2 domPos = new MSXML2.DOMDocument();
            broker.AssignNormalValue("domPos", domPos);

            //该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

            //给普通参数bGetBlank赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否获取空白单据:传入false
            broker.AssignNormalValue("bGetBlank", false);

            //给普通参数sBodyWhere_Order赋值。此参数的数据类型为System.String，此参数按值传递，表示表体排序方式字段
            broker.AssignNormalValue("sBodyWhere_Order", "cinvcode");

            broker.Invoke();
            //第七步：获取返回结果

            //获取返回值
            //获取普通返回值。此返回值数据类型为System.Boolean，此参数按值传递，表示返回值:true:成功,false:失败
            System.Boolean bresult = Convert.ToBoolean(broker.GetReturnValue());
            result = bresult.ToString();
            //获取out/inout参数值

            //out参数DomHead为BO对象(表头)，此BO对象的业务类型为销售出库单。BO参数均按引用传递，具体请参考服务接口定义
            //如果要取原始的XMLDOM对象结果，请使用GetResult("DomHead") as MSXML2.DOMDocument
            //BusinessObject DomHeadRet = broker.GetBoParam("DomHead");
            MSXML2.DOMDocument domHead = new MSXML2.DOMDocument();
            MSXML2.DOMDocument domBody = new MSXML2.DOMDocument();
            domHead = (MSXML2.DOMDocument)broker.GetResult("domhead");// as MSXML2.DOMDocument30Class;
            domBody = (MSXML2.DOMDocument)broker.GetResult("dombody");// as MSXML2.DOMDocument30Class;
            domHead.save("d:\\app\\otherouthead_blue111.xml");
            domBody.save("d:\\app\\otheroutbody_blue111.xml");

            return result;
        }
    
        private string delOtherOutSn()//删除其他出库单的SN
        {
            string result = "";
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity("995");
            result = STSNEntity.del_STSN(m_ologin, "1000060354");
            return result;
        }
    }
}