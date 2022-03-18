using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using ADODB;

namespace  U8toOAInterface.UFIDA
{
    public class DBHelper
    {

        public static DataTable getDataTableFromSql(ADODB.Connection conn, string strSql)//Sql查询，返回表
        {
            DataTable dt = new DataTable("T0");
            OleDbConnection con = new OleDbConnection();
            OleDbDataAdapter sda = new OleDbDataAdapter();
            ADODB.Recordset rs = new ADODB.Recordset();
            try
            {                
                rs.Open(strSql, conn);
                sda.Fill(dt, rs);
                rs.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                //con.Close(); 
            }
            return dt;
        }
        public static string getStrResultFromSQLscript(ADODB.Connection conn, string sql)//语句查询，返回字符串
        {
            string strResult = "";
            ADODB.Recordset dr = new Recordset();
            try
            {
                dr.Open(sql, conn);
                if (!dr.EOF)
                {
                    strResult = dr.Fields[0].Value.ToString();
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            return strResult;
        }

        public static void InsertIntoDB(ADODB.Connection conn, string strSql)
        {
            try
            {
                ADODB.Command cmd = new ADODB.Command();
                cmd.CommandType = CommandTypeEnum.adCmdText;
                cmd.CommandText = "insert into dbo.zzz_tbl_xmlResponse (GUID, dtResponseDateTime, strRoottag, strProc, strItemkey, strSucceed, strDsc, Memo) "
                                        + strSql;
                cmd.ActiveConnection = conn;
                object dumy;
                cmd.Execute(out dumy);
            }
            catch
            { throw; }
        }
        public static void InsertIntoPOextradefine(ADODB.Connection conn, string strPOID,string strChDefine1)
        {
            try
            {
                ADODB.Command cmd = new ADODB.Command();
                cmd.CommandType = CommandTypeEnum.adCmdText;
                cmd.CommandText = "if exists(select * from PO_Pomain_extradefine where POID="+strPOID+") "
                    +"update PO_Pomain_extradefine set chdefine1='"+strChDefine1+"' where POID="+strPOID
                    +" else insert into PO_Pomain_extradefine (POID,chdefine1) values ("+strPOID+",'"+strChDefine1+"')";
                cmd.ActiveConnection = conn;
                object dumy;
                cmd.Execute(out dumy);
            }
            catch
            { throw; }
        }

        public static void ExecSqlCommand(ADODB.Connection conn, string strSql)
        {
            try
            {
                ADODB.Command cmd = new ADODB.Command();
                cmd.CommandType = CommandTypeEnum.adCmdText;
                cmd.CommandText = strSql;
                cmd.ActiveConnection = conn;
                object dumy;
                cmd.Execute(out dumy);
            }
            catch
            { throw; }
        }
    }
}
