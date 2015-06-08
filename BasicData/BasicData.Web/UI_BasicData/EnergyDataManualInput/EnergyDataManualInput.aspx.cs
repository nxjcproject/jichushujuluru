using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicData.Web.UI_BasicData.EnergyDataManualInput
{
    public partial class EnergyDataManualInput : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();

#if DEBUG
            ////////////////////调试用,自定义的数据授权
            List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc" };
            AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
            this.OrganisationTree.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
            this.OrganisationTree.PageName = "EnergyDataManualInput.aspx";                                     //向web用户控件传递当前调用的页面名称
            this.OrganisationTree.LeveDepth = 5;
        }

        [WebMethod]
        public static string GetEnergyDataManualInputData(string organizationId)
        {
            DataTable dt = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.GetEnergyDataManualInput(organizationId);
            string result = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(dt);
            return result;
        }
        [WebMethod]
        public static string GetVariableNameData()
        {
            DataTable dt = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.GetVariableNameData();
            string result = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(dt);
            return result;
        }
        [WebMethod]
        public static string AddEnergyDataManualInputData(string maddData)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.AddEnergyDataManualInput(maddData) ;

            if (result == 1)
                return "1";
            else
                return "-1";
        }
        [WebMethod]
        public static string DeleteEnergyDataManualInputData(string id)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.DeleteEnergyDataManualInput(id);

            if (result == 1)
                return "1";
            else
                return "-1";
        }
        [WebMethod]
        public static string EditEnergyDataManualInputData(string editData)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.EditEnergyDataManualInput(editData);

            if (result == 1)
                return "1";
            else
                return "-1";
        }
    }
}