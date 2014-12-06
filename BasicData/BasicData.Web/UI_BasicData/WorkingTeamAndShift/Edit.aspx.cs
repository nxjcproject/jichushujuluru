using BasicData.Service.WorkingTeamAndShift;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyUIJsonParser;

namespace BasicData.Web.UI_BasicData.WorkingTeamAndShift
{
    public partial class Edit : WebStyleBaseForEnergy.webStyleBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            ////////////////////调试用,自定义的数据授权
            if (!mDataValidIdGroup.ContainsKey("ProductionOrganization"))
            {
                mDataValidIdGroup.Add("ProductionOrganization", new List<string>(1));
                mDataValidIdGroup["ProductionOrganization"].Add("O0101");
                mDataValidIdGroup["ProductionOrganization"].Add("O0102");
            }
            this.OrganisationTree.Organizations = mDataValidIdGroup["ProductionOrganization"];                 //向web用户控件传递数据授权参数
            this.OrganisationTree.PageName = "Edit.aspx";                                                       //向web用户控件传递当前调用的页面名称
        }

        [WebMethod]
        public static string GetChargeManComboboxValue()
        {
            DataTable dt = WorkingTeamAndShiftService.GetStaffInfo("123");
            string result = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(dt);
            return result;
        }

        [WebMethod]
        public static string SaveShifts(string shifts)
        {
            string[] sourceData = shifts.JsonPickArray("rows");
            int flag = WorkingTeamAndShiftService.SaveShiftInfo("123", sourceData);

            if (flag == 1)
                return "1";
            else
                return "-1";
        }

        [WebMethod]
        public static string SaveWorkingTeams(string workingTeams)
        {
            string[] souceData = workingTeams.JsonPickArray("rows");

            int result = WorkingTeamAndShiftService.SaveWorkingTeamInfo("123", souceData);

            if (result != -1)
                return "1";
            else
                return "-1";
        }

        [WebMethod]
        public static string QueryShifts()//string organizationId)
        {
            string organizationId = "123";
            DataTable sourcedt = WorkingTeamAndShiftService.QueryShiftsInfo(organizationId);
            string result = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(sourcedt);
            return result;
        }
        [WebMethod]
        public static string QueryWorkingTeam()//string organizationId)
        {
            string organizationId = "123";
            DataTable sourcedt = WorkingTeamAndShiftService.QueryWorkingTeamInfo(organizationId);
            string result = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(sourcedt);
            return result;
        }
    }
}