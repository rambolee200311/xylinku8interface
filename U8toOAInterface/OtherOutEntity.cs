using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using U8toOAInterface.UFIDA;
using U8toOAInterface.Models.cpsyU8WriteSn;
using System.Xml;
namespace U8toOAInterface
{
    public class OtherOutEntity
    {
        public static bool Otherout_audit_after(string vID, ADODB.Connection conn)
        {
            string strResult;
            string strSql = "select isnull(b.cDefine22,'') kdgs,isnull(b.cDefine23,'') kddh,isnull(f.ccode,'') ccode,isnull(f.cDefine12,'') cDefine10,b.cInvCode,d.cInvName,case when isnull(e.cinvsn,'')='' then b.iQuantity else 1 end iquantity,isnull(e.cInvSN,'') cInvSN"
                            +",c.cbdefine4,c.cbdefine5,c.cbdefine9,b.autoid,e.irowno "
                            +"from RdRecord09 a "
                            +"inner join RdRecords09 b on a.ID=b.ID "
                            +"inner join rdrecords09_extradefine c on b.AutoID=c.AutoID "
                            +"inner join inventory d on b.cInvCode=d.cInvCode "
                            +"left join ST_SNDetail_OtherOut e on b.AutoID=e.iVouchsID "
                            +"left join HY_DZ_BorrowOut f on a.cSource='借出借用单' and a.cBusCode=f.cCODE and f.cDefine12 like 'SY%' "
                            + "where a.id="+vID;
            LogHelper.WriteLog(typeof(OtherOutEntity),strSql);
             DataTable dt = DBHelper.getDataTableFromSql(conn, strSql);
             if (dt != null)
             {
                 List_SN_data listsndata = new List_SN_data();
                 listsndata.sn_data = new List<SN_data>();
                 foreach (DataRow dr in dt.Rows)
                 {
                     SN_data sndata = new SN_data();
                     sndata.cpbm = dr["cInvCode"].ToString();
                     sndata.cpmc = dr["cInvName"].ToString();
                     sndata.snm = dr["cInvSN"].ToString();
                     sndata.u8ddh = dr["ccode"].ToString();
                     sndata.oaddh = dr["cDefine10"].ToString();
                     sndata.kdgs = dr["kdgs"].ToString();
                     sndata.kddh = dr["kddh"].ToString();
                     sndata.shr = dr["cbdefine4"].ToString();
                     sndata.shrdh = dr["cbdefine5"].ToString();
                     sndata.shxxdz = dr["cbdefine9"].ToString();
                     sndata.sl =Convert.ToDecimal(dr["iQuantity"]);
                     sndata.req_id = dr["autoid"].ToString() + dr["irowno"].ToString();
                     listsndata.sn_data.Add(sndata);
                 }
                 string urlp = UrlParamHelper.ToParameter("cpsyU8WriteSn");
                 LogHelper.WriteLog(typeof(SaleOutEntity), urlp);
                 LogHelper.WriteLog(typeof(SaleOutEntity), "params=" + JsonHelper.ToJson(listsndata));
                 strResult = HttpPostHelper.sendInsert(urlp, "params=" + JsonHelper.ToJson(listsndata));
                 LogHelper.WriteLog(typeof(SaleOutEntity), strResult);
                 //urlp += JsonHelper.ToJson(listsndata);
                 //LogHelper.WriteLog(typeof(OtherOutEntity), urlp);
                 //strResult = HttpPostHelper.sendU8SN(urlp);
                 //LogHelper.WriteLog(typeof(OtherOutEntity), strResult);
             }
             
            return true;
        }
    }
}
