using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using U8toOAInterface.UFIDA;
using U8toOAInterface.Models.U8WriteSn;
using System.Xml;

namespace U8toOAInterface
{
    public class SaleOutEntity
    {
        public static bool Saleout_audit_after(string vID,ADODB.Connection conn)
        {
            /*
             select  isnull(f.cSOCode,'') cSOCode,isnull(f.cDefine10,'') cDefine10,a.cInvCode,c.cInvName,iif(isnull(b.cinvsn,'')='', a.iQuantity,1) iquantity,isnull(b.cInvSN,'') cInvSN
                
             */
            string urlp = "";
            string fwddh="";
            string strResult = "";
            string strSql = "select  isnull(a.cDefine22,'') kdgs,isnull(a.cDefine23,'') kddh,isnull(f.cSOCode,'') cSOCode,isnull(f.cDefine10,'') cDefine10,a.cInvCode,c.cInvName,case when isnull(b.cinvsn,'')='' then a.iQuantity else 1 end iquantity,isnull(b.cInvSN,'') cInvSN"
                + ",g.cbdefine4,g.cbdefine5,g.cbdefine9,b.irowno,a.autoid,a.cDefine23 wxkddh,g.cbdefine21 reqid,rdrecord32.dDate fhrq,d1.cDefine10 fhdh"
                + " from rdrecords32 a"
                + " join rdrecord32 on rdrecord32.ID=a.ID"
                + " left join ST_SNDetail_SaleOut b on a.AutoID=b.iVouchsID"
                + " inner join inventory c on a.cInvCode=c.cInvCode"
                + " left join DispatchLists d on a.iDLsID=d.iDLsID"
                + " left join DispatchList d1 on d.DLID=d1.DLID"
                + " left join SO_SODetails e on d.iSOsID=e.iSOsID"
                + " inner join SO_SOMain f on e.ID=f.ID"
                + " left join rdrecords32_extradefine g on a.autoid=g.autoid"
                + " where d1.bReturnFlag=0 and rdrecord32.ID=" + vID;
            LogHelper.WriteLog(typeof(SaleOutEntity), strSql);
            DataTable dt = DBHelper.getDataTableFromSql(conn, strSql);
            if (dt!=null)
            {
                if ((dt.Rows.Count>0)&&(dt.Rows[0]["fhdh"].ToString().IndexOf("WX")!=0))
                {
                    #region//订单出库
                    List_SN_data listsndata = new List_SN_data();
                    listsndata.sn_data = new List<SN_data>();
                    foreach(DataRow dr in dt.Rows)
                    {
                        fwddh = dr["cDefine10"].ToString();
                        SN_data sndata = new SN_data();
                        sndata.cpbm = dr["cInvCode"].ToString();
                        sndata.cpmc = dr["cInvName"].ToString();
                        sndata.snm = dr["cInvSN"].ToString();
                        sndata.u8ddh = dr["cSOCode"].ToString();
                        sndata.oaddh = dr["cDefine10"].ToString();
                        sndata.kdgs = dr["kdgs"].ToString();
                        sndata.kddh = dr["kddh"].ToString();
                        sndata.shr = dr["cbdefine4"].ToString();
                        sndata.shrdh = dr["cbdefine5"].ToString();
                        sndata.shxxdz = dr["cbdefine9"].ToString();
                        sndata.req_id = dr["autoid"].ToString() + dr["irowno"].ToString();
                        sndata.sl = Convert.ToDecimal(dr["iquantity"]);
                        listsndata.sn_data.Add(sndata);
                    }
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Models\\U8WriteSn\\UrlParams.xml");
                    
                    /*
                        直销：ZXDD U8WriteSn
                        伙伴：HBDD dlsddU8WriteSn
                        样机：YJ   yjsq_U8WrieSn
                        二级经销商样机：EJYJ  U8WriteSNTOjxsyj
                     */
                    if (fwddh.IndexOf("ZXDD") == 0)
                    { urlp = UrlParamHelper.ToParameter("U8WriteSn"); }
                    else if (fwddh.IndexOf("HBDD") == 0)
                    { urlp = UrlParamHelper.ToParameter("dlsddU8WriteSn"); }
                    else if (fwddh.IndexOf("YJ") == 0)
                    { urlp = UrlParamHelper.ToParameter("yjsq_U8WrieSn"); }
                    else if (fwddh.IndexOf("EJYJ") == 0)
                    { urlp = UrlParamHelper.ToParameter("U8WriteSNTOjxsyj"); }

                    if (urlp != "")
                    {
                        //urlp += JsonHelper.ToJson(listsndata);
                        LogHelper.WriteLog(typeof(SaleOutEntity), urlp);
                        LogHelper.WriteLog(typeof(SaleOutEntity),"params="+JsonHelper.ToJson(listsndata));
                        strResult = HttpPostHelper.sendInsert(urlp, "params=" + JsonHelper.ToJson(listsndata));
                        LogHelper.WriteLog(typeof(SaleOutEntity), strResult);
                    }
                    #endregion
                }
                else if ((dt.Rows.Count > 0) && (dt.Rows[0]["fhdh"].ToString().IndexOf("WX") == 0))
                {
                    //维修出库
                    foreach(DataRow dr in dt.Rows)
                    {
                        urlp = "http://39.105.96.42:80/api/hrm/interfaces/u8tooa/xiaoyutousnm"
                            +"?reqid="+dr["reqid"].ToString()
                            + "&sn=" + dr["cInvSN"].ToString()
                            + "&wxkddh=" + dr["wxkddh"].ToString()
                            + "&fhrq=" +Convert.ToDateTime( dr["fhrq"]).ToShortDateString();
                        LogHelper.WriteLog(typeof(SaleOutEntity), urlp);
                        strResult = HttpPostHelper.sendU8SN(urlp);
                        LogHelper.WriteLog(typeof(SaleOutEntity), strResult);
                    }
                }
            }
            return true;
        }
    }
}
