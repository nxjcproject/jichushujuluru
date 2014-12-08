using BasicData.Service.PeakValleyFlat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicData.Web.UI_BasicData.PeakValleyFlat
{
    public partial class List : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            ////////////////////调试用,自定义的数据授权
            //List<string> m_DataValidIdItems = new List<string>(){"O0101", "O0102"};
            //AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
            this.OrganisationTree.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
            this.OrganisationTree.PageName = "List.aspx";
        }

        [WebMethod]
        public static string GetPVFList(string organizationId, string startUsing)
        {
            DataTable dt = PeakValleyFlatService.GetPVFList(organizationId, startUsing);
            string result = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(dt);
            return result;
        }

        [WebMethod]
        public static string DeletePVFList(string keyId)
        {
            int result = PeakValleyFlatService.DeletePVFData(keyId);

            if (result == 1)
                return "1";
            else
                return "-1";
        }

        [WebMethod]
        public static string UpdatePVFList(string id, string endUsing, string flag)
        {
            int result = PeakValleyFlatService.UpdatePVFData(id, endUsing, flag);

            if (result == 1)
                return "1";
            else
                return "-1";
        }
    }
}