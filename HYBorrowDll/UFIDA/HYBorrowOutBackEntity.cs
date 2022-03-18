using System;
using System.Collections.Generic;
using System.Web;
using HYBorrowDll.Models.Borrowoutback;
using HYBorrowDll.Models.Result;
using HYBorrowDll.Helper;
using MSXML2;
using System.Data;
using System.Data.Linq;

namespace HYBorrowDll.UFIDA
{
    public class HYBorrowOutBackEntity
    {
        public Result Add_Borrow_Out_Back(BorrowOutBack bo)
        {
            Result re = new Result();
            string strResult = "";
            string errMsg = "";
            string oricode = "";
            string oristrvoid = "";
            int iVouchID=0;
            oricode = bo.head.oriccode;
            
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(bo.companycode);            
            if (m_ologin == null)
            { 
                strResult = "帐套" + bo.companycode + "登录失败";
                re.oacode =bo.head.ccode;
                re.recode = "111";
                re.remsg = strResult;
                return re;
            }

            if (string.IsNullOrEmpty(oricode))
            {
                re.oacode = bo.head.ccode;
                re.recode = "1111";
                re.remsg = "借出借用单编号不能为空";
                return re;
            }

            oristrvoid = Ufdata.getDataReader(m_ologin.UfDbName, "select ID from [dbo].[HY_DZ_BorrowOut] where cdefine12='"+oricode+"'");
            if(string.IsNullOrEmpty(oristrvoid))
            {
                re.oacode = bo.head.ccode;
                re.recode = "1111";
                re.remsg = "借出借用单编号不存在";
                return re;
            }

            HY_DZ_BorrowOutBack.clsBorrowOutSrvClass cosc = new HY_DZ_BorrowOutBack.clsBorrowOutSrvClass();
            cosc.Init(m_ologin);
            bool bresult = cosc.MakeVouchFromBorrowOut(Convert.ToInt32(oristrvoid), ref errMsg, ref iVouchID);

            if (bresult)
            {
                re.oacode = bo.head.ccode;
                re.recode = "0";
                re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                re.remsg = "";
                Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBack] set cdefine13='"+bo.head.ccode.ToString()+"' where id=" + iVouchID.ToString() );
                //return re;
                DataTable dtbacks = Ufdata.getDatatableFromSql(m_ologin.UfDbName, "select * from [dbo].[HY_DZ_BorrowOutBacks] where id="+iVouchID.ToString());
                if (dtbacks!=null)
                {
                    foreach(DataRow drbacks in dtbacks.Rows)
                    {
                        string invcode = drbacks["cinvcode"].ToString();
                        decimal iquantity = Convert.ToDecimal(drbacks["iquantity"]);
                        int autoid=Convert.ToInt32(drbacks["autoid"]);

                        List<BorrowOutBack_body> list1 = bo.body.FindAll(delegate(BorrowOutBack_body bobody) { return bobody.cinv_code == invcode; });// bo.body.Where(x => x.cinv_code == invcode).ToList();
                        if (list1==null)
                        {
                            Ufdata.execSqlcommand(m_ologin.UfDbName, "delete from [dbo].[HY_DZ_BorrowOutBacks] where id=" + iVouchID.ToString() + " and autoid=" + autoid.ToString());
                            Ufdata.execSqlcommand(m_ologin.UfDbName, "delete from [dbo].[HY_DZ_BorrowOutBacks2] where upid=" + iVouchID.ToString() + " and upautoid=" + autoid.ToString());
                        }
                        else
                        {
                            if (Convert.ToDecimal(list1[0].iquantity)<iquantity)
                            {
                                Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks] set iquantity="+list1[0].iquantity.ToString()+" where id=" + iVouchID.ToString() + " and autoid=" + autoid.ToString());
                                Ufdata.execSqlcommand(m_ologin.UfDbName, "update [dbo].[HY_DZ_BorrowOutBacks2] set iquantity=" + list1[0].iquantity.ToString() + ",MycdefineB8=" + list1[0].iquantity.ToString() + " where upid=" + iVouchID.ToString() + " and upautoid=" + autoid.ToString());
                            }
                        }

                    }
                }

            }
            else
            {
                re.oacode = bo.head.ccode;
                re.recode = "2222";
                //re.u8code = Ufdata.getDataReader(m_ologin.UfDbName, "select ccode from [dbo].[HY_DZ_BorrowOutBack] where ID=" + iVouchID.ToString() + "");
                re.remsg =errMsg;
            }
            return re;
        }
    }
}