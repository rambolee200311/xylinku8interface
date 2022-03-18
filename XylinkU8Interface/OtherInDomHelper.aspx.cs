using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class OtherInDomHelper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string otherinID = Request.QueryString["otherinid"].ToString();
            string ctype = Request.QueryString["ctype"].ToString();
            string companycode = Request.QueryString["companycode"].ToString();
            Response.Write(getOtherIn(companycode,otherinID,ctype));
        }
        private string getOtherIn(string companycode,string otheridID,string ctype)
        {
            string result = "";
            U8Login.clsLogin u8Login = U8LoginEntity.getU8LoginEntity(companycode);
            
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
            broker.AssignNormalValue("sWhere", "ID="+otheridID);

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
            //domHead.save("d:\\app\\otherinhead111.xml");
            //domBody.save("d:\\app\\otherinbody111.xml");
            switch(ctype)
            {
                case "head":
                    result=domHead.xml;
                    break;
                case "body":
                    result=domBody.xml;
                    break;
            }

            return result;
        }
    }
    
}