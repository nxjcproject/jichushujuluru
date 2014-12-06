using BasicData.Service.PeakValleyFlat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyUIJsonParser;
using System.Web.Services;

namespace BasicData.Web.UI_BasicData.PeakValleyFlat
{
    public partial class Edit : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string Save(string myJsonData)
        {
            string startUsing = myJsonData.JsonPick("tzStartDate");
            string[] dataDetails = myJsonData.JsonPickArray("dataDetail");

            int result = PeakValleyFlatService.SavePVFData("123", startUsing, dataDetails);

            if (result == 1)
                return "1";
            else
                return "-1";
        }
    }
}