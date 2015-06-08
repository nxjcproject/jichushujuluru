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
    public partial class EnergyDataManualInputContrast : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            ////////////////////调试用,自定义的数据授权
            //List<string> m_DataValidIdItems = new List<string>() { "C41B1F47-A48A-495F-A890-0AABB2F3BFF7                            ", "43F1EA8C-FF77-4BC5-BACB-531DC56A2512                            " };
            //AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
        }

        [WebMethod]
        public static string GetEnergyDataManualInputContrastData(string variableName)
        {
            DataTable dt = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.GetEnergyDataManualInputContrast(variableName);
            string result = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(dt);
            return result;
        }

        [WebMethod]
        public static string AddEnergyDataManualInputContrastData(string maddData)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.AddEnergyDataManualInputContrast(maddData);

            if (result == 1)
                return "1";
            else
                return "-1";
        }

        [WebMethod]
        public static string DeleteEnergyDataManualInputContrastData(string variableId)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.DeleteEnergyDataManualInputContrast(variableId);

            if (result == 1)
                return "1";
            else
                return "-1";
        }

        [WebMethod]
        public static string EditEnergyDataManualInputContrastData(string editData)
        {
            int result = BasicData.Service.EnergyDataManualInput.EnergyDataManualInputService.EditEnergyDataManualInputContrast(editData);

            if (result == 1)
                return "1";
            else
                return "-1";
        }
    }
}