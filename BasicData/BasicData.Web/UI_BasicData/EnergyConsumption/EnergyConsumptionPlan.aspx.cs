﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Web.Services;
using WebStyleBaseForEnergy;

namespace BasicData.Web.UI_BasicData.EnergyConsumption
{
    public partial class EnergyConsumptionPlan : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
                ////////////////////调试用,自定义的数据授权
                //List<string> m_DataValidIdItems = new List<string>() { "C41B1F47-A48A-495F-A890-0AABB2F3BFF7                            ", "43F1EA8C-FF77-4BC5-BACB-531DC56A2512                            " };
                //AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "EnergyConsumptionPlan.aspx";                                     //向web用户控件传递当前调用的页面名称
            }
        }

        [WebMethod]
        public static string GetEnergyConsumptionInfo(string myProductionLineType, string myOrganizationId, string myPlanYear)
        {
            string[] m_ColumnText = new string[] { "指标项ID", "指标项目名称", "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月", "年度合计", "备注" };
            int[] m_ColumnWidth = new int[] { 180, 180, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 100, 180 };
            string[] m_FormatString = new string[] { "", "", "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}", 
                "\"type\":\"numberbox\", \"options\":{\"precision\":\"1\"}",
                "\"type\":\"textbox\", \"options\":{\"validType\":\"length[0,200]\", \"invalidMessage\":\"不能超过200个字符!\"}" };

            DataTable m_EnergyConsumptionInfo = BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.GetEnergyConsumptionInfo(myProductionLineType, myOrganizationId, myPlanYear);
            string m_Rows = EasyUIJsonParser.DataGridJsonParser.GetDataRowJson(m_EnergyConsumptionInfo);
            StringBuilder m_Columns = new StringBuilder();
            if (m_Rows == "")
            {
                m_Rows = "\"rows\":[],\"total\":0";
            }
            m_Columns.Append("\"columns\":[");
            for (int i = 0; i < m_EnergyConsumptionInfo.Columns.Count; i++)
            {
                m_Columns.Append("{");
                m_Columns.Append("\"width\":\"" + m_ColumnWidth[i] + "\"");
                m_Columns.Append(", \"title\":\"" + m_ColumnText[i] + "\"");
                m_Columns.Append(", \"field\":\"" + m_EnergyConsumptionInfo.Columns[i].ColumnName.ToString() + "\"");
                if (m_FormatString[i] != "")
                {
                    m_Columns.Append(", \"editor\":{" + m_FormatString[i] + "}");
                }

                m_Columns.Append("}");
                if (i < m_EnergyConsumptionInfo.Columns.Count - 1)
                {
                    m_Columns.Append(",");
                }
            }
            m_Columns.Append("]");

            return "{" + m_Rows + "," + m_Columns + "}";
        }
        [WebMethod]
        public static string SetEnergyConsumptionInfo(string myOrganizationId, string myPlanYear, string myProductionLineType, string myDataGridData)
        {
            DataTable m_DataGridDataStruct = BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.CreateTableStructure("plan_EnergyConsumptionYearlyPlan");
            //m_DataGridDataStruct.Columns.Remove("QuotasItemID");       //去掉ID列，此列的数据由数据库自动生成
            string[] m_DataGridDataGroup = EasyUIJsonParser.Utility.JsonPickArray(myDataGridData,"rows");
            DataTable m_DataGridData = EasyUIJsonParser.DataGridJsonParser.JsonToDataTable(m_DataGridDataGroup, m_DataGridDataStruct);
            ///////////////tz表里查找是否已经存在////////////////
            string m_KeyId = BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.GetKeyIdFromTz(myPlanYear, myOrganizationId, myProductionLineType);
            if (m_KeyId != "")               //表示计划已经存在
            {
                BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.UpdateTzPlan(m_KeyId, mUserId);    //更新TZ引领表
            }
            else
            {
                m_KeyId = Guid.NewGuid().ToString();
                //添加TZ引领
                BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.InsertTzPlan(m_KeyId, myOrganizationId, myProductionLineType, myPlanYear, mUserId);
            }

            for (int i = 0; i < m_DataGridData.Rows.Count; i++)
            {
                m_DataGridData.Rows[i]["KeyID"] = m_KeyId;
                m_DataGridData.Rows[i]["DisplayIndex"] = (i + 1).ToString();
                m_DataGridData.Rows[i]["ProductionLineType"] = myProductionLineType;
            }
            BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.DeleteEnergyConsumptionInfo(m_KeyId);
            int ReturnValue = BasicData.Service.EnergyConsumption.EnergyConsumptionPlan.SaveEnergyConsumptionInfo("plan_EnergyConsumptionYearlyPlan", m_DataGridData);
            ReturnValue = ReturnValue > 0 ? 1 : ReturnValue;
            return ReturnValue.ToString();
        }
    }
}