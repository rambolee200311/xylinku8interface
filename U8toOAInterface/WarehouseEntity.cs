using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using U8toOAInterface.Models.Warehouse;
using U8toOAInterface.UFIDA;
using MSXML2;

namespace U8toOAInterface
{
    public class WarehouseEntity
    {
        public static bool Warehouse_modify_after(MSXML2.IXMLDOMDocument2 archivedata,ADODB.Connection conn)
        {
            string strResult = "";
            string strSql = "";
            try
            {
                string whcode = archivedata.selectSingleNode("warehouse").selectSingleNode("cwhcode").text;
                DataTable dtwh = DBHelper.getDataTableFromSql(conn, "select cWhCode ckbm,cWhName ckmc,cDepName bmmc,cWhAddress ckdz,cWhPhone dh,cPsn_Name fzr,cWhValueStyle jjfs  from warehouse a left join Department b on a.cDepCode=b.cDepCode left join hr_hi_person c on a.cWhPerson=c.cPsn_Num where cWhCode='" + whcode + "'");
                if (dtwh == null || dtwh.Rows.Count < 1)
                {
                    return true;
                }
                Warehouse inv=new Warehouse();
                inv.header=new Header();
                string pwd = "E672C9E87BAC4211A83CBFD3100C7902";
                string usr = "CkU8";
                inv.header.systemid = usr;
				string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
				inv.header.Md5 = MD5CryptoHelper.GetMD5(usr+pwd+ datetime);
				inv.header.currentDateTime = datetime;

                inv.data = new List<SData>();
                SData sdata = new SData();
                sdata.operationinfo = new OperationInfo();
                sdata.operationinfo.operationDate = DateTime.Now.ToString("yyyy-MM-dd");
                sdata.operationinfo.operaTor = "1";
                sdata.operationinfo.operationTime = DateTime.Now.ToString("HH:mm:ss");

                sdata.mainTable = new Uf_ckda();
                sdata.mainTable.ckbm = dtwh.Rows[0]["ckbm"].ToString();
                sdata.mainTable.ckmc = dtwh.Rows[0]["ckmc"].ToString();
                sdata.mainTable.ckdz = dtwh.Rows[0]["ckdz"].ToString();
                sdata.mainTable.dh = dtwh.Rows[0]["dh"].ToString();
                //sdata.mainTable.bmmc = 61;
                //sdata.mainTable.fzr = 157;
                sdata.mainTable.bmmc = dtwh.Rows[0]["bmmc"].ToString();
                sdata.mainTable.fzr = dtwh.Rows[0]["fzr"].ToString();
                sdata.mainTable.jjfs = dtwh.Rows[0]["jjfs"].ToString();

                inv.data.Add(sdata);

                strSql = "datajson=" + inv.ToJson().Replace("operaTor", "operator");
                LogHelper.WriteLog(typeof(WarehouseEntity), strSql);
                strResult = HttpPostHelper.sendInsert("http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/CkdaToU8", strSql);
                LogHelper.WriteLog(typeof(WarehouseEntity), strResult);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(WarehouseEntity),ex);
            }
            return true;
        }
    }
}
