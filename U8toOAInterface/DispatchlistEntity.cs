using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using U8toOAInterface.Models.U8WriteFhData;
using U8toOAInterface.UFIDA;

namespace U8toOAInterface
{
    public class DispatchlistEntity
    {
        public static bool Dispatchlist_audit_after(string vID, ADODB.Connection conn)
        {
            try
            {
                string strSql = "select b.AutoID,a.cCusCode,c.cCusName khmc,e.cDefine10 fwddbm,e.cSOCode u8ddbm,b.cInvCode cpbm,f.cInvName cpmc,"
                            + "b.iQuantity,b.iSettleQuantity,b.iQuantity-isnull(b.iSettleQuantity,0) wkpsl,"
                            + "b.iSum,b.iSettleNum,b.isum-isnull(b.iSettleNum,0) wkpje,"
                            + "a.cDLCode u8fhdbm,f.cComUnitCode danwe,b.iQuantity fhsl,d.iQuantity ddsl,b.iTaxUnitPrice hsdj,b.isum jshj,b.DLID,b.cParentCode"
                            + " from DispatchList a"
                            + " inner join DispatchLists b on a.DLID=b.DLID"
                            + " inner join customer c on a.cCusCode=c.cCusCode"
                            + " left join SO_SODetails d on b.iSOsID=d.iSOsID"
                            + " inner join SO_SOMain e on d.cSOCode=e.cSOCode"
                            + " inner join inventory f on b.cInvCode=f.cInvCode"
                            + " where a.cDLCode='" + vID + "' and b.cChildCode is null";
                DataTable dt = DBHelper.getDataTableFromSql(conn, strSql);
                if (dt != null)
                {
                    
                    foreach (DataRow dr in dt.Rows)
                    {
                        FHData fhdata = new FHData();
                        fhdata.header = new Header();
                        fhdata.header.systemid = "U8";
                        string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        fhdata.header.Md5 = MD5CryptoHelper.GetMD5("U8F00CC4106B784FE9A28613059F2A8C09" + datetime);
                        fhdata.header.currentDateTime = datetime;
                        fhdata.data = new List<SData>();
                        SData sdata = new SData();
                        sdata.operationinfo = new Operationinfo();
                        sdata.operationinfo.operationDate = DateTime.Now.ToString("yyyy-MM-dd");
                        sdata.operationinfo.operaTor = "1";
                        sdata.operationinfo.operationTime = DateTime.Now.ToString("HH:mm:ss");

                        sdata.mainTable = new MainTable();
                        sdata.mainTable.fwddbm = dr["fwddbm"].ToString();
                        sdata.mainTable.u8ddbm = dr["u8ddbm"].ToString();
                        sdata.mainTable.cpbm = dr["cpbm"].ToString();
                        sdata.mainTable.cpmc = dr["cpmc"].ToString();
                        sdata.mainTable.wkpsl = Convert.ToDecimal(dr["wkpsl"]);
                        sdata.mainTable.wkpje = Convert.ToDecimal(dr["wkpje"]);
                        sdata.mainTable.khmc = dr["khmc"].ToString();
                        sdata.mainTable.u8fhdbm = dr["u8fhdbm"].ToString();
                        sdata.mainTable.danwe = getUnitCode(dr["danwe"].ToString());
                        sdata.mainTable.ddsl = Convert.ToDecimal(dr["ddsl"]);
                        sdata.mainTable.jshj = Convert.ToDecimal(dr["jshj"]);
                        sdata.mainTable.hsdj = Convert.ToDecimal(dr["hsdj"]);
                        sdata.mainTable.fhsl = Convert.ToDecimal(dr["fhsl"]);

                        //子件
                        strSql ="select b.AutoID,b.cInvCode zjbm,f.cInvName zjmc,b.iQuantity zjsl,b.iTaxUnitPrice hsdj,iUnitPrice wsdj"
                        +" from DispatchList a"
                        +" inner join DispatchLists b on a.DLID=b.DLID"
                        +" inner join customer c on a.cCusCode=c.cCusCode"
                        + " inner join inventory f on b.cInvCode=f.cInvCode"
                        + " where b.dlid='" + dr["DLID"].ToString() + "' and b.cChildCode='" + dr["cParentCode"].ToString() + "'";

                        DataTable dts = DBHelper.getDataTableFromSql(conn, strSql);
                        if (dts!=null)
                        {
                            if (dts.Rows.Count>1)
                            {
                                sdata.detail1 = new List<SDetail>();
                                foreach(DataRow drs in dts.Rows)
                                {
                                    SDetail sd = new SDetail();
                                    sd.operate = new Operate();
                                    sd.operate.action = "SaveOrUpdate";
                                    sd.operate.actionDescribe = "";
                                    sd.data = new Detail1Data();
                                    sd.data.zjbm = drs["zjbm"].ToString();
                                    sd.data.zjmc = drs["zjmc"].ToString();
                                    sd.data.zjsl = Convert.ToDecimal(drs["zjsl"]);
                                    sd.data.hsdj = Convert.ToDecimal(drs["hsdj"]);
                                    sd.data.wsdj = Convert.ToDecimal(drs["wsdj"]);
                                    sdata.detail1.Add(sd);
                                }

                            }

                        }



                        fhdata.data.Add(sdata);
                        strSql = "datajson=" + fhdata.ToJson().Replace("operaTor", "operator");
                        LogHelper.WriteLog(typeof(DispatchlistEntity), strSql);
                        string strResult = HttpPostHelper.sendInsert("http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8WriteFhData", strSql);
                        LogHelper.WriteLog(typeof(DispatchlistEntity), strResult);
                    }

                   
                }
            }
            catch (Exception ex2)
            {
                LogHelper.WriteLog(typeof(DispatchlistEntity), ex2);
                return true;
            }
            return true;
        }

        private static int getUnitCode(string value)
        {
            int unitcode = 0;
            switch (value.Trim())
            {
                case "01":
                    unitcode = 0;
                    break;
                case "02":
                    unitcode = 1;
                    break;
                case "03":
                    unitcode = 2;
                    break;
                case "04":
                    unitcode = 3;
                    break;
                case "05":
                    unitcode = 4;
                    break;
                case "06":
                    unitcode = 5;
                    break;
                case "07":
                    unitcode = 6;
                    break;
                case "08":
                    unitcode = 7;
                    break;
                case "09":
                    unitcode = 8;
                    break;
            }
            return unitcode;
        }
    }
}
