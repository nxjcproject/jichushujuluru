using BasicData.Service.EquipmentAccount;
using BasicData.Service.SectionDescription;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicData.Web.UI_BasicData.SectionDescription
{
    public partial class SectionDefine : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
                ////////////////////调试用,自定义的数据授权
#if DEBUG
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf", "zc_nxjc_qtx" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                mPageOpPermission = "0000";
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "SectionDefine.aspx";                                     //向web用户控件传递当前调用的页面名称
           //mUserId
            }
        }

        /// <summary>
        /// 增删改查权限控制
        /// </summary>
        /// <returns></returns>
        //[WebMethod]
        //public static char[] AuthorityControl()
        //{
        //    return mPageOpPermission.ToArray();
        //}
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetSectionDefineInfo(string organizationId)
        {
            DataTable table = SectionDefineService.GetWorkingSectionDatas(organizationId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetOrganizationIdInfo(string organizationId)
        {
            DataTable table = SectionDefineService.GetOrganizationIds(organizationId);
            string json = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetJsonofVariableNameTree(string organizationId)
        {
            DataTable table = BasicData.Service.SectionDescription.SectionDefineService.GetVariableNameTable(organizationId);
            string json = EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(table, "LevelCode", "VariableId", "VariableName");
            return json;
        }
        [WebMethod]
        public static string GetJsonofOutputInfo(string organizationId)
        {
            DataTable table = BasicData.Service.SectionDescription.SectionDefineService.GetOutputTable(organizationId);
            string json = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetJsonofPulverizedCoalInfo(string organizationId)
        {
            DataTable table = BasicData.Service.SectionDescription.SectionDefineService.GetPulverizedCoalTable(organizationId);
            string json = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(table);
            return json;
        }
        /// <summary>
        /// 保存设备
        /// </summary>
        /// <param name="VariableId"></param>
        /// <param name="OrganizationID"></param>
        /// <param name="EquipmentName"></param>
        /// <param name="MonitorType"></param>
        /// <param name="PowerSupply"></param>
        /// <param name="VoltageGrade"></param>
        /// <param name="RatedCT"></param>
        /// <param name="AmmeterCode"></param>
        /// <param name="ActualCT"></param>
        /// <param name="Power"></param>
        /// <param name="Unit"></param>
        /// <param name="PowerSupplyPosition"></param>
        /// <param name="Remarks"></param>
        /// <returns></returns>
        [WebMethod]
        public static string SaveSectionDefineInfo(string WorkingSectionName, string OrganizationID, string VariableName,
            string Out_put, string PulverizedCoalInput, string Creator, string Remarks)
        {
            //if (mPageOpPermission.ToArray()[1] == '1')
            //{
            string message = SectionDefineService.SaveSectionDefine(WorkingSectionName, OrganizationID,
                                    VariableName, Out_put, PulverizedCoalInput, Creator, Remarks);
            return message;
            //}
            //else
            //{
            //    return "用户没有修改权限！";
            //}
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="variableId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string RemoveSectionDefineInfo(string WorkingSectionItemID_old, string WorkingSectionID_old)
        {
            //if (mPageOpPermission.ToArray()[3] == '1')
            //{
            return SectionDefineService.RemoveSectionDefine(WorkingSectionItemID_old, WorkingSectionID_old);
            //}
            //else
            //{
            //    return "用户没有删除权限！";
            //}
        }
        /// <summary>
        /// 编辑设备
        /// </summary>
        /// <param name="variableId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string SaveEditSectionDefine(string WorkingSectionItemID_old,string WorkingSectionID_old,string WorkingSectionName, string OrganizationID, string VariableName,
            string Out_put, string Creator, string Remarks)
        {
            //    if (mPageOpPermission.ToArray()[2] == '1')
            //    {
            return SectionDefineService.UpdateSectionDefine(WorkingSectionItemID_old, WorkingSectionID_old,WorkingSectionName, OrganizationID,
                                    VariableName, Out_put, Creator, Remarks);

            //    }
            //    else
            //    {
            //        return "用户没有编辑权限！";
            //    }
            //}

        }
    }
}