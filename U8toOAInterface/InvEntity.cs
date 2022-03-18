using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using U8toOAInterface.Models.Inventory;
using U8toOAInterface.UFIDA;
using MSXML2;

namespace U8toOAInterface
{
    public class InvEntity
    {
        public static bool Inventory_modify_after(MSXML2.IXMLDOMDocument2 archivedata,ADODB.Connection conn)
        {
            try
            {
                string strResult="";
                Inventory inv = new Inventory();
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
				string invcode = archivedata.selectSingleNode("inventory").selectSingleNode("cinvcode").text;
                //LogHelper.WriteLog(typeof(ClsU8toOAarchive), "part_invcode:" + invcode);
                string bSyncCrm = archivedata.selectSingleNode("inventory").selectSingleNode("cinvdefine8").text;
                //LogHelper.WriteLog(typeof(ClsU8toOAarchive), "bsynccrm:" + bSyncCrm);
				if (!(bSyncCrm == "是"))
				{
					return true;
				}
                string strSql = "select d.cinvcname,a.cInvMnemCode,a.cinvstd,a.cInvCode,a.cInvName,b.PartId,c.cidefine5,c.cidefine6,a.cInvDefine2 sftj,a.cInvDefine7 sfyfw,a.cInvDefine10 sfyj,c.cidefine2 crmbxs,a.cInvDefine6 cpdjfz,a.cInvDefine5 cpldfz,isnull(a.iTaxRate,0) jssl,isnull(iInvSCost,0) hsbj,isnull(iInvSPrice,0) ckcb,a.cInvDefine3 ggyfwqxdw,0 dwz,a.cComUnitCode jbdw from inventory a inner join bas_part b on a.cInvCode=b.InvCode left join Inventory_extradefine c on a.cInvCode=c.cInvCode inner join inventoryclass d on a.cinvccode=d.cinvccode where a.cInvCode='" + invcode + "'";
				DataTable dtHead = DBHelper.getDataTableFromSql(conn, strSql);
                //LogHelper.WriteLog(typeof(ClsU8toOAarchive), strSql);
				sdata.mainTable = new MainTable();
				if (dtHead != null && dtHead.Rows.Count >= 1)
				{
					sdata.mainTable.wlbm = dtHead.Rows[0]["cInvCode"].ToString();
					sdata.mainTable.wlmc = dtHead.Rows[0]["cInvName"].ToString();
					sdata.mainTable.qrsmc = dtHead.Rows[0]["cidefine5"].ToString();
					sdata.mainTable.fzlx = 2;
					sdata.mainTable.cpfl = getCpfl(dtHead.Rows[0]["cidefine6"].ToString());
					sdata.mainTable.dwz = Convert.ToInt32(dtHead.Rows[0]["dwz"]);
					sdata.mainTable.jbdw = getUnitCode(dtHead.Rows[0]["jbdw"].ToString());
					sdata.mainTable.sftj = getShiFou(dtHead.Rows[0]["sftj"].ToString());
					sdata.mainTable.sfyfw = getShiFou(dtHead.Rows[0]["sfyfw"].ToString());
					sdata.mainTable.sfyj = getShiFou(dtHead.Rows[0]["sfyj"].ToString());
					sdata.mainTable.crmbxs = getShiFou(dtHead.Rows[0]["crmbxs"].ToString());
					sdata.mainTable.cpdjfz = dtHead.Rows[0]["cpdjfz"].ToString();
					sdata.mainTable.cpldfz = dtHead.Rows[0]["cpldfz"].ToString();
					sdata.mainTable.jssl = Convert.ToDecimal(dtHead.Rows[0]["jssl"]);
					sdata.mainTable.hsbj = Convert.ToDecimal(dtHead.Rows[0]["hsbj"]);
                    sdata.mainTable.ckcb = Convert.ToDecimal(dtHead.Rows[0]["ckcb"]);
                    sdata.mainTable.chfl = getChfl(dtHead.Rows[0]["cinvcname"].ToString());
					if (!string.IsNullOrEmpty(dtHead.Rows[0]["ggyfwqxdw"].ToString()))
					{
						sdata.mainTable.ggyfwqxdw = getGgyfwqxdw(dtHead.Rows[0]["ggyfwqxdw"].ToString());
					}
				}
				sdata.detail1 = new List<SDetail>();
				strSql = "select a.cInvMnemCode,a.cinvstd,a.cInvCode,a.cInvName,b.PartId,c.cidefine5,d.BaseQtyN/d.BaseQtyD zjsl,a.cInvDefine2 sftj,a.cInvDefine7 sfyfw,a.cInvDefine10 sfyj,c.cidefine2 crmbxs,a.cInvDefine6 cpdjfz,a.cInvDefine5 cpldfz,isnull(a.iTaxRate,0) jssl,isnull(iInvSCost,0) hsbj,isnull(iInvSPrice,0) ckcb,a.cInvDefine3 ggyfwqxdw,0 dwz,a.cComUnitCode jbdw from inventory a inner join bas_part b on a.cInvCode=b.InvCode left join Inventory_extradefine c on a.cInvCode=c.cInvCode left join bom_opcomponent d on b.PartId=d.ComponentId where d.bomid=(select bom_bom.BomId from bom_bom inner join bom_parent on bom_bom.BomId=bom_parent.BomId and bom_parent.ParentId in (select PartId from [dbo].[bas_part] where invcode='" + invcode + "') and Status=3 and AuditStatus=1)";
				DataTable dtBody = DBHelper.getDataTableFromSql(conn, strSql);
                //LogHelper.WriteLog(typeof(ClsU8toOAarchive), strSql);
				if (dtBody != null)
				{
					if (dtBody.Rows.Count >= 1)
					{
						sdata.mainTable.fzlx = 1;
						foreach (DataRow dr in dtBody.Rows)
						{
							SDetail sdetail = new SDetail();
							sdetail.data = new Detail1Data();
							sdetail.operate = new Operate();
							sdetail.operate.action = "SaveOrUpdate";
							sdetail.operate.actionDescribe = "新增或修改";
							sdetail.data.wlbm = dr["cInvCode"].ToString();
							sdetail.data.wlmc = dr["cInvName"].ToString();
							sdetail.data.qrsrjmc = dr["cidefine5"].ToString();
							sdetail.data.ggxh = dr["cinvstd"].ToString();
							sdetail.data.zjm = dr["cInvMnemCode"].ToString();
							sdetail.data.jbdw = getUnitCode(dr["jbdw"].ToString());
							sdetail.data.jldw = 0;
							sdetail.data.jssl = Convert.ToDecimal(dr["jssl"]);
							sdetail.data.hsbj = Convert.ToDecimal(dr["hsbj"]);
							sdetail.data.ckcb = Convert.ToDecimal(dr["ckcb"]);
							sdetail.data.sfyfw = getShiFou(dr["sfyfw"].ToString());
							if (!string.IsNullOrEmpty(dr["ggyfwqxdw"].ToString()))
							{
								sdetail.data.yfwqxdw = getGgyfwqxdw(dr["ggyfwqxdw"].ToString()).ToString();
							}
							sdetail.data.zjsl = Convert.ToInt32(dr["zjsl"]);
							sdata.detail1.Add(sdetail);
						}
					}
					else
					{
						SDetail sdetail = new SDetail();
						sdetail.data = new Detail1Data();
						sdetail.operate = new Operate();
						sdetail.operate.action = "SaveOrUpdate";
						sdetail.operate.actionDescribe = "新增或修改";
						sdetail.data.wlbm = dtHead.Rows[0]["cInvCode"].ToString();
						sdetail.data.wlmc = dtHead.Rows[0]["cInvName"].ToString();
						sdetail.data.qrsrjmc = dtHead.Rows[0]["cidefine5"].ToString();
						sdetail.data.ggxh = dtHead.Rows[0]["cinvstd"].ToString();
						sdetail.data.zjm = dtHead.Rows[0]["cInvMnemCode"].ToString();
						sdetail.data.jbdw = getUnitCode(dtHead.Rows[0]["jbdw"].ToString());
						sdetail.data.jldw = 0;
						sdetail.data.jssl = Convert.ToDecimal(dtHead.Rows[0]["jssl"]);
						sdetail.data.hsbj = Convert.ToDecimal(dtHead.Rows[0]["hsbj"]);
						sdetail.data.ckcb = Convert.ToDecimal(dtHead.Rows[0]["ckcb"]);
						sdetail.data.sfyfw = getShiFou(dtHead.Rows[0]["sfyfw"].ToString());
						if (!string.IsNullOrEmpty(dtHead.Rows[0]["ggyfwqxdw"].ToString()))
						{
							sdetail.data.yfwqxdw = getGgyfwqxdw(dtHead.Rows[0]["ggyfwqxdw"].ToString()).ToString();
						}
						sdetail.data.zjsl = 1;
						sdata.detail1.Add(sdetail);
					}
				}
				inv.data.Add(sdata);
				strSql = "datajson=" + inv.ToJson().Replace("operaTor", "operator");
                LogHelper.WriteLog(typeof(InvEntity), strSql);
				strResult = HttpPostHelper.sendInsert("http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8addProduct",strSql);
                LogHelper.WriteLog(typeof(InvEntity), strResult);
			}
			catch (Exception ex2)
			{
				LogHelper.WriteLog(typeof(InvEntity), ex2);
				return true;
			}
            return true;
        }

        public  static bool Inventory_modify_after2(MSXML2.IXMLDOMDocument2 archivedata,ADODB.Connection conn)
        {

            DataTable dt = DBHelper.getDataTableFromSql(conn, "select cinvcode from inv_sale");// where cinvcode>='VCV-0000-2024'");
                foreach(DataRow drinv in dt.Rows)
                {
                    try
                    {
                        string invcode = drinv["cinvcode"].ToString();
                        string strResult="";
                        Inventory inv = new Inventory();
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
                        //string invcode = archivedata.selectSingleNode("inventory").selectSingleNode("cinvcode").text;
                        //LogHelper.WriteLog(typeof(ClsU8toOAarchive), "part_invcode:" + invcode);
                        //string bSyncCrm = archivedata.selectSingleNode("inventory").selectSingleNode("cinvdefine8").text;
                        string bSyncCrm = DBHelper.getStrResultFromSQLscript(conn, "select cinvdefine8 from inventory where cinvcode='"+invcode+"'");
                        //LogHelper.WriteLog(typeof(ClsU8toOAarchive), "bsynccrm:" + bSyncCrm);
				        if (!(bSyncCrm == "是"))
				        {
					        return true;
				        }
                        string strSql = "select cinvcname,a.cInvMnemCode,a.cinvstd,a.cInvCode,a.cInvName,b.PartId,c.cidefine5,c.cidefine6,a.cInvDefine2 sftj,a.cInvDefine7 sfyfw,a.cInvDefine10 sfyj,c.cidefine2 crmbxs,a.cInvDefine6 cpdjfz,a.cInvDefine5 cpldfz,isnull(a.iTaxRate,0) jssl,isnull(iInvSCost,0) hsbj,isnull(iInvSPrice,0) ckcb,a.cInvDefine3 ggyfwqxdw,0 dwz,a.cComUnitCode jbdw from inventory a inner join bas_part b on a.cInvCode=b.InvCode left join Inventory_extradefine c on a.cInvCode=c.cInvCode inner join inventoryclass d on a.cinvccode=d.cinvccode where a.cInvCode='" + invcode + "'";
				        DataTable dtHead = DBHelper.getDataTableFromSql(conn, strSql);
                        //LogHelper.WriteLog(typeof(ClsU8toOAarchive), strSql);
				        sdata.mainTable = new MainTable();
				        if (dtHead != null && dtHead.Rows.Count >= 1)
				        {
					        sdata.mainTable.wlbm = dtHead.Rows[0]["cInvCode"].ToString();
					        sdata.mainTable.wlmc = dtHead.Rows[0]["cInvName"].ToString();
					        sdata.mainTable.qrsmc = dtHead.Rows[0]["cidefine5"].ToString();
					        sdata.mainTable.fzlx = 2;
					        sdata.mainTable.cpfl = getCpfl(dtHead.Rows[0]["cidefine6"].ToString());
					        sdata.mainTable.dwz = Convert.ToInt32(dtHead.Rows[0]["dwz"]);
					        sdata.mainTable.jbdw = getUnitCode(dtHead.Rows[0]["jbdw"].ToString());
					        sdata.mainTable.sftj = getShiFou(dtHead.Rows[0]["sftj"].ToString());
					        sdata.mainTable.sfyfw = getShiFou(dtHead.Rows[0]["sfyfw"].ToString());
					        sdata.mainTable.sfyj = getShiFou(dtHead.Rows[0]["sfyj"].ToString());
					        sdata.mainTable.crmbxs = getShiFou(dtHead.Rows[0]["crmbxs"].ToString());
					        sdata.mainTable.cpdjfz = dtHead.Rows[0]["cpdjfz"].ToString();
					        sdata.mainTable.cpldfz = dtHead.Rows[0]["cpldfz"].ToString();
					        sdata.mainTable.jssl = Convert.ToDecimal(dtHead.Rows[0]["jssl"]);
					        sdata.mainTable.hsbj = Convert.ToDecimal(dtHead.Rows[0]["hsbj"]);
					        sdata.mainTable.ckcb = Convert.ToDecimal(dtHead.Rows[0]["ckcb"]);
                            sdata.mainTable.chfl =getChfl(dtHead.Rows[0]["cinvcname"].ToString());
					        if (!string.IsNullOrEmpty(dtHead.Rows[0]["ggyfwqxdw"].ToString()))
					        {
						        sdata.mainTable.ggyfwqxdw = getGgyfwqxdw(dtHead.Rows[0]["ggyfwqxdw"].ToString());
					        }
				        }
				        sdata.detail1 = new List<SDetail>();
				        strSql = "select a.cInvMnemCode,a.cinvstd,a.cInvCode,a.cInvName,b.PartId,c.cidefine5,d.BaseQtyN/d.BaseQtyD zjsl,a.cInvDefine2 sftj,a.cInvDefine7 sfyfw,a.cInvDefine10 sfyj,c.cidefine2 crmbxs,a.cInvDefine6 cpdjfz,a.cInvDefine5 cpldfz,isnull(a.iTaxRate,0) jssl,isnull(iInvSCost,0) hsbj,isnull(iInvSPrice,0) ckcb,a.cInvDefine3 ggyfwqxdw,0 dwz,a.cComUnitCode jbdw from inventory a inner join bas_part b on a.cInvCode=b.InvCode left join Inventory_extradefine c on a.cInvCode=c.cInvCode left join bom_opcomponent d on b.PartId=d.ComponentId where d.bomid=(select bom_bom.BomId from bom_bom inner join bom_parent on bom_bom.BomId=bom_parent.BomId and bom_parent.ParentId in (select PartId from [dbo].[bas_part] where invcode='" + invcode + "') and Status=3 and AuditStatus=1)";
				        DataTable dtBody = DBHelper.getDataTableFromSql(conn, strSql);
                        //LogHelper.WriteLog(typeof(ClsU8toOAarchive), strSql);
				        if (dtBody != null)
				        {
					        if (dtBody.Rows.Count >= 1)
					        {
						        sdata.mainTable.fzlx = 1;
						        foreach (DataRow dr in dtBody.Rows)
						        {
							        SDetail sdetail = new SDetail();
							        sdetail.data = new Detail1Data();
							        sdetail.operate = new Operate();
							        sdetail.operate.action = "SaveOrUpdate";
							        sdetail.operate.actionDescribe = "新增或修改";
							        sdetail.data.wlbm = dr["cInvCode"].ToString();
							        sdetail.data.wlmc = dr["cInvName"].ToString();
							        sdetail.data.qrsrjmc = dr["cidefine5"].ToString();
							        sdetail.data.ggxh = dr["cinvstd"].ToString();
							        sdetail.data.zjm = dr["cInvMnemCode"].ToString();
							        sdetail.data.jbdw = getUnitCode(dr["jbdw"].ToString());
							        sdetail.data.jldw = 0;
							        sdetail.data.jssl = Convert.ToDecimal(dr["jssl"]);
							        sdetail.data.hsbj = Convert.ToDecimal(dr["hsbj"]);
							        sdetail.data.ckcb = Convert.ToDecimal(dr["ckcb"]);
							        sdetail.data.sfyfw = getShiFou(dr["sfyfw"].ToString());
							        if (!string.IsNullOrEmpty(dr["ggyfwqxdw"].ToString()))
							        {
								        sdetail.data.yfwqxdw = getGgyfwqxdw(dr["ggyfwqxdw"].ToString()).ToString();
							        }
							        sdetail.data.zjsl = Convert.ToInt32(dr["zjsl"]);
							        sdata.detail1.Add(sdetail);
						        }
					        }
					        else
					        {
						        SDetail sdetail = new SDetail();
						        sdetail.data = new Detail1Data();
						        sdetail.operate = new Operate();
						        sdetail.operate.action = "SaveOrUpdate";
						        sdetail.operate.actionDescribe = "新增或修改";
						        sdetail.data.wlbm = dtHead.Rows[0]["cInvCode"].ToString();
						        sdetail.data.wlmc = dtHead.Rows[0]["cInvName"].ToString();
						        sdetail.data.qrsrjmc = dtHead.Rows[0]["cidefine5"].ToString();
						        sdetail.data.ggxh = dtHead.Rows[0]["cinvstd"].ToString();
						        sdetail.data.zjm = dtHead.Rows[0]["cInvMnemCode"].ToString();
						        sdetail.data.jbdw = getUnitCode(dtHead.Rows[0]["jbdw"].ToString());
						        sdetail.data.jldw = 0;
						        sdetail.data.jssl = Convert.ToDecimal(dtHead.Rows[0]["jssl"]);
						        sdetail.data.hsbj = Convert.ToDecimal(dtHead.Rows[0]["hsbj"]);
						        sdetail.data.ckcb = Convert.ToDecimal(dtHead.Rows[0]["ckcb"]);
						        sdetail.data.sfyfw = getShiFou(dtHead.Rows[0]["sfyfw"].ToString());
						        if (!string.IsNullOrEmpty(dtHead.Rows[0]["ggyfwqxdw"].ToString()))
						        {
							        sdetail.data.yfwqxdw = getGgyfwqxdw(dtHead.Rows[0]["ggyfwqxdw"].ToString()).ToString();
						        }
						        sdetail.data.zjsl = 1;
						        sdata.detail1.Add(sdetail);
					        }
				        }
                        if ((sdata.mainTable != null) && (sdata.detail1 != null))
                        {
                            inv.data.Add(sdata);
                            strSql = "datajson=" + inv.ToJson().Replace("operaTor", "operator");
                            LogHelper.WriteLog(typeof(InvEntity), strSql);
                            strResult = HttpPostHelper.sendInsert("http://39.105.96.42/api/cube/restful/interface/saveOrUpdateModeData/U8addProduct", strSql);
                            LogHelper.WriteLog(typeof(InvEntity), strResult);
                        }
			        }
			        catch (Exception ex2)
			        {
				        LogHelper.WriteLog(typeof(InvEntity), ex2);
				        return true;
			        }
                }
            return true;

        }
        private static int getCpfl(string value)
        {
            int cpfl =0;
            switch (value.Trim())
            {
                case "软件":
                    cpfl = 1;
                    break;
                case "硬件":
                    cpfl = 0;
                    break;
                case "云服务期限必须是1":
                    cpfl = 2;
                    break;
            }
            return cpfl;
        }

        private static int getShiFou(string value)
        {
            int shifou = 1;
            switch (value.Trim())
            {
                case "是":
                    shifou = 0;
                    break;
                case "否":
                    shifou = 1;
                    break;
            }
            return shifou;
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

        private static int getGgyfwqxdw(string value)
        {
            int ggyfwqxdw = 0;
            switch (value.Trim())
            {
                case "月":
                    ggyfwqxdw = 0;
                    break;
                case "年":
                    ggyfwqxdw = 1;
                    break;
                case "小时":
                    ggyfwqxdw = 2;
                    break;
                case "日":
                    ggyfwqxdw = 3;
                    break;
            }
            return ggyfwqxdw;
        }
        private static string getChfl(string cinvcname)
        {
            /*
                成品：2_14
                case "桌面一体终端":strResult="2_19";break;
                case "一体终端":strResult="2_20";break;
                case "视频终端":strResult="2_21";break;
                case "科技终端":strResult="2_22";break;
                case "其他终端":strResult="2_23";break;
                case "双师":strResult="2_25";break;
                case "配件":strResult="2_15";break;
                case "虚拟":strResult="2_16";break;
                case "原材料":strResult="2_17";break;
                case "其他":strResult="2_18";break;
                case "虚拟套装":strResult="2_24";break;
                case "项目实施服务":strResult="2_26";break;
                case "存货分类":strResult="2_27";break;
             */
            string strResult = "";
            switch(cinvcname)
            {
                case "成品":strResult="2_14"; break;
                case "桌面一体终端": strResult = "2_19"; break;
                case "一体终端": strResult = "2_20"; break;
                case "视频终端": strResult = "2_21"; break;
                case "科技终端": strResult = "2_22"; break;
                case "其他终端": strResult = "2_23"; break;
                case "双师": strResult = "2_25"; break;
                case "配件": strResult = "2_15"; break;
                case "虚拟": strResult = "2_16"; break;
                case "原材料": strResult = "2_17"; break;
                case "其他": strResult = "2_18"; break;
                case "虚拟套装": strResult = "2_24"; break;
                case "项目实施服务": strResult = "2_26"; break;
                case "存货分类": strResult = "2_27"; break;
            }

            return strResult;
        }
    }
}
