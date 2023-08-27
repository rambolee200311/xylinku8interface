using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Xml;
using XylinkU8Interface.Models;
namespace XylinkU8Interface.Helper
{
    public static class Ufdata
    {
        public static string getDataReader(string strConn, string strSql)//根据sql得到结果
        {
            string strResult = "";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            try 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql;
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        strResult = reader[0].ToString();                       
                    }
                }
                //reader.Dispose();
                cmd.Dispose();
            }
            finally { conn.Close(); }           
            return strResult;
        }

        public static void execSqlcommand(string strConn, string strSql)//根据sql执行
        {
           
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;                
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                //reader.Dispose();
                cmd.Dispose();
            }
            
            finally { conn.Close(); }   
        }
        public static void execSqlcommand(string strConn, string strSql, List<Param> myparams)//根据sql执行
        {

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;                
                cmd.CommandText = strSql;
                foreach(Param p in myparams)
                {
                    cmd.Parameters.Add(p.paramname, p.paramtype);
                    cmd.Parameters[p.paramname].Value = p.paramvalue;
                }
                cmd.ExecuteNonQuery();
                //reader.Dispose();
                cmd.Dispose();
            }

            finally { conn.Close(); }
        }
        public static DataTable getDatatableFromSql(string strConn, string strSql)//根据sql得到datatable
        {
            DataTable dtResult = null;
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                DataSet ds = new DataSet();                
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, conn);
                adapter.Fill(ds);
                dtResult = ds.Tables[0];
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(typeof(Ufdata), ex);
            }
            finally { conn.Close(); }  
            return dtResult;
        }
        public static DataTable getDatatableFromSql(string strConn, string strSql, List<Param> myparams)//根据sql得到datatable
        {
            DataTable dtResult = null;
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                DataSet ds = new DataSet();
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql;
                foreach (Param p in myparams)
                {
                    cmd.Parameters.Add(p.paramname,p.paramtype);

                    cmd.Parameters[p.paramname].Value = p.paramvalue;
                }
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                dtResult = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Ufdata), ex);
            }
            finally { conn.Close(); }
            return dtResult;
        }

        public static void updateMysqlProduct(DataSet dsr, string strProc, string strNow, string strDb, string strAccID)//重新更新一次product_sync
        {
            string strResult = "";
            OleDbConnection conn = new OleDbConnection(strDb);
            //LikuaiB2BInWs.WebServiceSQLSelect ws = new LikuaiB2BInWs.WebServiceSQLSelect();
            DataSet ds = new DataSet();
            DataTable dTable = new DataTable();
            dTable.Columns.Add("strItemKey", typeof(string));
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                
                for (int i = 0; i < dsr.Tables[0].Rows.Count; i++)
                {
                    cmd.CommandText = "select strItemkey from zzz_tbl_xmlResponse where strSucceed=0 and strRoottag='vendor' and dtResponseDateTime='" + strNow + "' and strProc='" + strProc.ToLower() + "' and strItemkey='" + dsr.Tables[0].Rows[i]["sku_id"].ToString() + "'";
                    //LogHelper.WriteLog(typeof(Ufdata), cmd.CommandText);
                    OleDbDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        DataRow drow = dTable.NewRow();
                        drow["strItemKey"] = dr[0].ToString();
                        dTable.Rows.Add(dr);
                    }
                    dr.Close();
                }

                ds.Tables.Add(dTable);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        switch (strProc.ToLower())
                        {
                            case "add":
                                //strResult = ws.SetSyncStatus(ds, Convert.ToDateTime(strNow), "A", "vendor",strAccID);
                                //LogHelper.WriteLog(typeof(Ufdata), "likuai:"+strResult);
                                break;
                            case "edit":
                                //strResult = ws.SetSyncStatus(ds, Convert.ToDateTime(strNow), "U", "vendor", strAccID);
                                //LogHelper.WriteLog(typeof(Ufdata), "likuai:" + strResult);
                                break;
                        }
                    }
                }
                
            }
            finally
            {
                conn.Close();
            }
            
        }

        public static void CreateArchiveHelper(string strConn)
        {
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strSql = "if not exists(select 1 from sysobjects where [name]='zzz_archive_helper' and [xtype]='u') begin"
                + " CREATE TABLE [dbo].[zzz_archive_helper]([id] [int] IDENTITY(1,1) NOT NULL,[ctype] [nvarchar](100) NOT NULL,[code] [nvarchar](100) NOT NULL,[qgbcode] [nvarchar](100) NOT NULL,"
                +" CONSTRAINT [PK_zzz_archive_herlper] PRIMARY KEY CLUSTERED ([id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] end;";
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
                //reader.Dispose();
                cmd.Dispose();
            }
            finally { conn.Close(); }
        }
        //得到分页表 currentPage 当前页，size 页大小
        public static DataTable getPagedTable(DataTable dt, int currentPage, int size)
        {
            DataTable dtt;
            if (currentPage == 0)
            { dtt = dt; }
            dtt = dt.Copy();
            dtt.Clear();
            int rowbegin = (currentPage - 1) * size;
            int rowend = currentPage * size;
            if (rowbegin >= dt.Rows.Count)
            { dtt = dt; }
            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }

            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = dtt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                dtt.Rows.Add(newdr);
            }
            return dtt;
        }
    }
}
