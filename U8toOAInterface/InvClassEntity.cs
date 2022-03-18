using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using U8toOAInterface.UFIDA;
using U8toOAInterface.Models.Productclassification;

namespace U8toOAInterface
{
    public class InvClassEntity
    {

        public static bool InventoryClass_modify_after2(MSXML2.IXMLDOMDocument2 archivedata, ADODB.Connection conn)
        {
            string strSql = "";
            DataTable dt = DBHelper.getDataTableFromSql(conn, "select * from InventoryClass");// where cinvcode>='VCV-0000-2024'");
            foreach (DataRow drinv in dt.Rows)
            {
                try
                {
                   
                    string strResult = "";
                    Productclassification inv = new Productclassification();
                    inv.header = new Header();
                    inv.header.systemid = "U8";
                    string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    inv.header.Md5 = MD5CryptoHelper.GetMD5("U8F00CC4106B784FE9A28613059F2A8C09" + datetime);
                    inv.header.currentDateTime = datetime;
                    inv.data = new List<SData>();
                    SData sdata = new SData();
                    sdata.operationinfo = new OperationInfo();
                    sdata.operationinfo.operationDate = DateTime.Now.ToString("yyyy-MM-dd");
                    sdata.operationinfo.operaTor = "1";
                    sdata.operationinfo.operationTime = DateTime.Now.ToString("HH:mm:ss");

                    sdata.mainTable = new ProductClass();
                    sdata.mainTable.sjid = (100000000 + Convert.ToInt32(drinv["cinvccode"])).ToString();
                    sdata.mainTable.dqfl = drinv["cinvcname"].ToString();
                    sdata.mainTable.sjfl =getParentCode( drinv["cinvcname"].ToString(),Convert.ToInt32(drinv["iinvcgrade"]),conn);
                    if (string.IsNullOrEmpty(sdata.mainTable.sjfl))
                    {
                        sdata.mainTable.sjfl = "存货分类";
                    }
                    inv.data.Add(sdata);

                    strSql = "datajson=" + inv.ToJson().Replace("operaTor", "operator");
                    LogHelper.WriteLog(typeof(InvClassEntity), strSql);
                    strResult = HttpPostHelper.sendInsert("http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8addProductclassification", strSql);
                    LogHelper.WriteLog(typeof(InvClassEntity), strResult);
                    
                }
                catch (Exception ex2)
                {
                    LogHelper.WriteLog(typeof(InvClassEntity), ex2);
                    return true;
                }
            }
            return true;

        }

        public static string getParentCode(string ChildCode, int IGrade,ADODB.Connection conn)
        {
            string strResult = "";
            string ruleCode = DBHelper.getStrResultFromSQLscript(conn, "select cValue　from　AccInformation where cSysID='AA' and cName='cGoodClass'");

            if (IGrade == 1)
            {
                return "";
            }
            else
            {
                ruleCode = ruleCode.Substring(IGrade - 1, 1);
                strResult = ChildCode.Substring(0, ChildCode.Length - Convert.ToInt32(ruleCode));
            }
            return strResult;
        }
    }
}
