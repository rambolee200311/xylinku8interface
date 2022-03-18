using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using XylinkU8Interface;
using XylinkU8Interface.Helper;
using XylinkU8Interface.Models.Inventory;
namespace XylinkU8Interface.UFIDA
{
    public class InventoryEntity
    {
        public static InvResult GetInvResult(string companycode,int currentPage,int size)
        {
            InvResult ir = new InvResult();
            ir.products = new List<Inventory>();
            U8Login.clsLoginClass m_ologin = U8LoginEntity.getU8LoginEntity(companycode);
            if (m_ologin==null)
            {
                return ir;
            }
            try
            {
                string sql = "select * from (select row_number() OVER (ORDER BY a.cinvccode,a.cinvcode) n,a.cinvcCode,d.cinvcname,a.cInvMnemCode,a.cinvstd,a.cInvCode,a.cInvName,b.PartId,c.cidefine5,c.cidefine6,a.cInvDefine2,a.cInvDefine7,a.cInvDefine10,c.cidefine2,isnull(cidefine1,'否') cidefine1,"
                            + "isnull(h.cvalue,'') cInvDefine6,a.cInvDefine5,isnull(a.iTaxRate,0) iTaxRate,isnull(iInvSCost,0) hsbj,isnull(iInvSPrice,0) ckcb,a.cInvDefine3,0 dwz,a.cComUnitCode jbdw,e.cGroupName,f.cComUnitName, case when isnull(g.BomId,'')='' then '0' else '1' end has_u8,c.cidefine7,a.bSerial"
                            + " from inventory a"
                            + " inner join bas_part b on a.cInvCode=b.InvCode"
                            + " left join Inventory_extradefine c on a.cInvCode=c.cInvCode"
                            + " inner join inventoryclass d on a.cinvccode=d.cinvccode"
                            + " left join ComputationGroup e on a.cGroupCode=e.cGroupCode"
                            + " left join ComputationUnit f on a.cComUnitCode=f.cComunitCode"
                            + " left join bom_parent g on b.PartId=g.ParentId"
                            + " left join (select * from UserDefine where cid=54) h on a.cInvDefine6=h.cvalue"
                            + " where isnull(a.cinvdefine8,'')='是') a"
                            + " where a.n>="+((currentPage-1)*size+1).ToString()+" and a.n<="+(currentPage*size);
                DataTable dt = Ufdata.getDatatableFromSql(m_ologin.UfDbName, sql);

                sql="select count(cinvcode) c from inventory a where isnull(a.cinvdefine8,'')='是'";
                ir.totalNumber = Convert.ToInt32(Ufdata.getDataReader(m_ologin.UfDbName, sql));
                //ir.totalNumber = dt.Rows.Count;
                //DataTable dtt = GetPagedTable(dt, currentPage, size);
                foreach (DataRow dr in dt.Rows)
                {
                    Inventory inv = new Inventory();
                    inv.productCode = dr["cInvCode"].ToString();//    物料编码    

                    inv.productName = dr["cInvName"].ToString();//    物料名称    

                    inv.productCategoryCode = dr["cinvcCode"].ToString();//  存货分类编码  

                    inv.embedSoftware = dr["cidefine5"].ToString();// 嵌入式软件名称 

                    inv.cloudServiceDeadlineUnit = dr["cInvDefine3"].ToString();//  产品周期单位  

                    inv.standardPrice = Convert.ToDecimal(dr["hsbj"]);//       参考售价    

                    inv.priceGroup = dr["cInvDefine6"].ToString();//  报价分组

                    inv.taxRate = Convert.ToDecimal(dr["iTaxRate"]);//         taxRate          

                    inv.model = dr["cinvstd"].ToString();//    规格型号    

                    inv.rememberCode = dr["cInvMnemCode"].ToString();//     助记码  

                    inv.sampleMachine = (dr["cInvDefine10"].ToString()=="是")?"1":"0";//    是否样机    

                    inv.unitGroup = dr["cGroupName"].ToString();//计量单位组

                    inv.majorUnit = dr["cComUnitName"].ToString();//主计量单位  

                    inv.cloudService = (dr["cInvDefine7"].ToString()=="是")?"1":"0";//是否云服务

                    inv.suit =(dr["cInvDefine2"].ToString()=="是")?"1":"0";//    是否套装    

                    inv.hasU8 = dr["has_u8"].ToString();//有无子件

                    inv.remark = dr["cidefine7"].ToString();//产品描述

                    inv.bvirtual = (dr["cidefine1"].ToString() == "是") ? 1 : 0; //是否虚拟产品  | 1:是，0:否

                    inv.hasSequence = Convert.ToInt32(dr["bSerial"]);
                    ir.products.Add(inv);
                }
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(typeof(InventoryEntity),ex.Message);
            }
            LogHelper.WriteLog(typeof(InventoryEntity), JsonHelper.ToJson(ir));
            return ir;
        }
        public static DataTable GetPagedTable(DataTable dt,int currentPage,int size)
        {
            DataTable dtt;
            if (currentPage==0)
            { dtt = dt; }
            dtt = dt.Copy();
            dtt.Clear();
            int rowbegin = (currentPage - 1)*size;
            int rowend = currentPage*size;
            if (rowbegin>=dt.Rows.Count)
            { dtt = dt; }
            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }

            for (int i = rowbegin; i <= rowend - 1;i++ )
            {
                DataRow newdr = dtt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach(DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                dtt.Rows.Add(newdr);
            }
                return dtt;
        }
    }
}