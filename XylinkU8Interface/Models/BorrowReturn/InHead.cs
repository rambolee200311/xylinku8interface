using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
 *20230827 
 * 归还head
 */
namespace XylinkU8Interface.Models.BorrowReturn
{
    /*
     "head": {
                "ordcode": "ZJSQ202307260001", // CRM单据编号-U8外部订单号
                "ddate": "2023-07-26", // 单据⽇期
                "cmemo": "备注", // 备注
                "custName": "客户名称", // 客户名称
                "personName": "唐媛媛", // 申请⼈-业务员
                "cwhname": "⼩⻥仓", // 虚拟仓库名称
                "recvType": "借出归还⼊库"
             }
 
     */
    public class InHead
    {
       public String ordcode{get;set;}// CRM单据编号-U8外部订单号
       public String ddate{get;set;}// 单据⽇期
       public String cmemo{get;set;}// 备注
       public String custName{get;set;}// 客户名称
       public String personName{get;set;}// 申请⼈-业务员
       public String cwhname{get;set;} // 虚拟仓库名称
       public String recvType{get;set;}//借出归还⼊库"
    }
}