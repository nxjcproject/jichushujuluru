﻿using BasicData.Service.KpiDefinition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicData.Web.UI_BasicData.KpiDefinition
{
    public partial class KPIndex : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
                ////////////////////调试用,自定义的数据授权
#if DEBUG
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                mPageOpPermission = "0000";
#elif RELEASE
#endif
                //this.OrganisationTree.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                //this.OrganisationTree.PageName = "KPIndex.aspx";                                     //向web用户控件传递当前调用的页面名称
            }
        }
        /// <summary>
        /// 增删改查权限控制
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static char[] AuthorityControl()
        {
            return mPageOpPermission.ToArray();
        }

        [WebMethod]
        public static string GetStandardType()
        {
            DataTable table = KPIndexService.GetStandardTypeTable();
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;             
        }
        [WebMethod]
        public static string GetConsumpationType(string myStandardId)
        {
            DataTable table = KPIndexService.GetConsumpationTypeTable(myStandardId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;       
        }
        [WebMethod]
        public static string GetIndexData(string myConsumpation, string myKeyId, string myStandardType, string myInOutSide)
        {
            DataTable table = KPIndexService.GetIndexDataTable(myConsumpation, myKeyId, myStandardType, myInOutSide);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;   
        }
        [WebMethod]
        public static string GetObjectNameList()
        {
            DataTable table = KPIndexService.GetObjectNameListTable();
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;   
        }
        [WebMethod]
        public static string GetKPIndexOrganizationData()
        {
            string[] temArr={"熟料","水泥磨","余热发电"};
            List<string> typeItems=new List<string>(temArr);
            DataTable m_DcsOrganization = WebUserControls.Service.OrganizationSelector.OrganisationTree.GetOrganisationTree(GetDataValidIdGroup("ProductionOrganization"), "", typeItems,7,true);
            return EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(m_DcsOrganization, "LevelCode", "OrganizationID", "Name");
        }
        [WebMethod]
        public static string GetNameListData(string myOrganizationId)
        {
            DataTable table = KPIndexService.GetNameListTable(myOrganizationId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;   
        }
        [WebMethod]
        public static void AddKPIndex(string mStandardLevel, string mEnergyType, string mObjName, string mOrganizationId, string mLevelType, string mVariableId, string mValueType, string mtype, string mUnit, float mStandardValue ,string mLevel, string mCreator)
        {
//#if DEBUG
//            WebStyleBaseForEnergy.webStyleBase.mUserId = "1";
//#endif
            if (mPageOpPermission.ToArray()[1] == '1')
            {
                //mStandardLevel,mEnergyType,mObjName,mOrganizationId,mLevelType,mVariableId,mValueType,mtype,mUnit,mStandardValue
                KPIndexService.AddKPIList(mStandardLevel, mEnergyType, mObjName, mOrganizationId, mLevelType, mVariableId, mValueType, mtype, mUnit, mStandardValue, mLevel, mCreator);
            }
         }

        //"{mStandardLevel:'InternationalStandard',mEnergyType:'Comprehensive',mObjName:'test',mOrganizationId:'zc_nxjc_byc_byf_cementmill01',mLevelType:'Process',mVariableId:'cementGrind',mValueType:'ElectricityConsumption',mtype:'Energy',mUnit:'kW·h/t',mStandardValue:''}"
     [WebMethod]
        public static void DeleteKPIndexData(string standardItemId) 
        {
            if (mPageOpPermission.ToArray()[3] == '1')
            {
                KPIndexService.DeleteKPIndex(standardItemId);
            }
        }
    }
}