using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MSXML2;
using XylinkU8Interface.Helper;
using XylinkU8Interface.UFIDA;
using XylinkU8Interface.Models.RevokeOOSOrder;
using XylinkU8Interface.Models.Result;
using UFIDA.U8.U8MOMAPIFramework;
using UFIDA.U8.U8APIFramework;
using UFIDA.U8.U8APIFramework.Meta;
using UFIDA.U8.U8APIFramework.Parameter;
namespace XylinkU8Interface.UFIDA
{
    public class RevokeOOSOrderEntity
    {
        public static ResultDatas revokeOOSOrder(ClsRequest req)
        {
            string strResult, strSql;
            ResultDatas res = new ResultDatas();
            res.companycode = req.companycode;
            res.datas = new List<Result>();
            try
            {

                U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(req.companycode);
                if (m_ologin == null)
                {
                    Result re = new Result();
                    strResult = "帐套" + req.companycode + "登录失败";
                    re.recode = "111";
                    re.remsg = strResult;
                    res.datas.Add(re);
                    return res;
                }

                foreach (ClsRequestCode code in req.codes)
                {
                    Result re = new Result();
                    re.oacode = code.code;
                    #region//audit
                    //第二步：构造环境上下文对象，传入login，并按需设置其它上下文参数
                    U8EnvContext envContext = new U8EnvContext();
                    envContext.U8Login = m_ologin;

                    //第三步：设置API地址标识(Url)
                    //当前API：添加新单据的地址标识为：U8API/otherout/Add
                    U8ApiAddress myApiAddress = new U8ApiAddress("U8API/otherout/Delete");

                    //第四步：构造APIBroker
                    U8ApiBroker broker = new U8ApiBroker(myApiAddress, envContext);

                    //第五步：API参数赋值

                    //给普通参数sVouchType赋值。此参数的数据类型为System.String，此参数按值传递，表示单据类型：09
                    broker.AssignNormalValue("sVouchType", "09");

                    //给普通参数VouchId赋值。此参数的数据类型为System.String，此参数按值传递，表示单据Id
                    //Ufdata.execSqlcommand(m_ologin.UfDbName, "delete from ST_SNDetail_OtherOut where iVouchID=(select ID from rdrecord09 where ccode='" + code.code + "' and isnull(cHandLer,'')='')");

                    strResult = STSNEntity.del_STSN(m_ologin, Ufdata.getDataReader(m_ologin.UfDbName, "select ID from rdrecord09 where ccode='" + code.code + "'"));
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        re.recode = "444";
                        re.remsg = "其他出库单:" + code.code + "序列号删除失败:" +strResult;                       
                        res.datas.Add(re);
                        return res;
                    }

                    broker.AssignNormalValue("VouchId", Ufdata.getDataReader(m_ologin.UfDbName, "select ID from rdrecord09 where ccode='" + code.code + "'"));


                    #region//proc
                    ////给普通参数domPosition赋值。此参数的数据类型为System.Object，此参数按引用传递，表示货位：传空
                    //broker.AssignNormalValue("domPosition", new System.Object());

                    ////该参数errMsg为OUT型参数，由于其数据类型为System.String，为一般值类型，因此不必传入一个参数变量。在API调用返回时，可以通过GetResult("errMsg")获取其值

                    ////给普通参数cnnFrom赋值。此参数的数据类型为ADODB.Connection，此参数按引用传递，表示连接对象,如果由调用方控制事务，则需要设置此连接对象，否则传空
                    //broker.AssignNormalValue("cnnFrom", new ADODB.Connection());

                    //该参数domMsg为OUT型参数，由于其数据类型为MSXML2.IXMLDOMDocument2，非一般值类型，因此必须传入一个参数变量。在API调用返回时，可以直接使用该参数
                    MSXML2.DOMDocumentClass domMsg = new MSXML2.DOMDocumentClass();
                    broker.AssignNormalValue("dommsg", domMsg);

                    //给普通参数bCheck赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示是否控制可用量。
                    broker.AssignNormalValue("bCheck", false);

                    //给普通参数bBeforCheckStock赋值。此参数的数据类型为System.Boolean，此参数按值传递，表示检查可用量
                    broker.AssignNormalValue("bBeforCheckStock", false);

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
                        re.remsg = "其他出库单:" + code.code + "删除失败:" + errMsgRet;
                    }
                    else
                    {
                        //获取普通INOUT参数VouchId。此返回值数据类型为System.String，在使用该参数之前，请判断是否为空
                        System.String VouchIdRet = broker.GetResult("VouchId") as System.String;
                        re.recode = "0";
                        //STSNEntity.add_STSN(m_ologin, "32", so, VouchIdRet);
                        re.u8code =code.code;
                        re.remsg = "其他出库单:" + code.code + "删除成功";
                    }
                    #endregion
                    #endregion
                    res.datas.Add(re);
                }
            }
            catch (Exception ex)
            {
                Result re = new Result();
                strResult = ex.Message;
                re.recode = "999";
                re.remsg = strResult;
                //res.result.Clear();
                res.datas.Add(re);
                return res;
            }
            return res;
        } 
    
    }
}