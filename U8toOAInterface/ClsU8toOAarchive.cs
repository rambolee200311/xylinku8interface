using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using MSXML2;
using UFIDA.U8.MomServiceCommon;
using U8toOAInterface.UFIDA;
using U8toOAInterface.Models.Inventory;

namespace U8toOAInterface
{
    public class ClsU8toOAarchive
    {
        public bool Add_After(ref MSXML2.IXMLDOMDocument2 archivedata,out string errmsg)        
        {
            bool bResult = true;
            string strResult = "";
            errmsg = "";
            MomCallContextCache envCtxCache = new MomCallContextCache();
            MomCallContext envCtx = new MomCallContext();
            envCtx = envCtxCache.CurrentMomCallContext;

            //从上下文获取帐套库连接对象
            ADODB.Connection conn = envCtx.BizDbConnection as ADODB.Connection;
            string strNow = Convert.ToDateTime(envCtx.LoginInfo.Date).ToShortDateString();
            string eventId = envCtx.EventIdentity;

            switch (eventId)
            {
                case "U8API/inventory/Add_After"://存货档案新增后事件
                    archivedata.save("D:\\inventory_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml");
                    break;
            }
            strResult = errmsg;

            return bResult;
        }

        public bool Modify_After(ref MSXML2.IXMLDOMDocument2 archivedata,out string errmsg)
        {
            bool bResult = true;
            string strResult = "";
            Nullable<int> intid = null;
            errmsg = "";
            MomCallContextCache envCtxCache = new MomCallContextCache();
            MomCallContext envCtx = new MomCallContext();
            envCtx = envCtxCache.CurrentMomCallContext;

            //从上下文获取帐套库连接对象
            ADODB.Connection conn = envCtx.BizDbConnection as ADODB.Connection;
            
            string strNow = Convert.ToDateTime(envCtx.LoginInfo.Date).ToShortDateString();
            string eventId = envCtx.EventIdentity;

            switch (eventId)
            {
                case "U8API/inventory/Modify_After"://存货档案修改后事件
                        bResult = InvEntity.Inventory_modify_after(archivedata, conn);
                        break;

                case "U8API/warehouse/Modify_After"://仓库档案修改后事件
                        bResult = WarehouseEntity.Warehouse_modify_after(archivedata, conn);
                        break;
                case "U8API/inventoryclass/Modify_After"://存货分类档案修改后事件
                        bResult = InvClassEntity.InventoryClass_modify_after2(archivedata, conn);
                        break;
            }
            strResult = errmsg;

            return bResult;
        }

        
    }
}
