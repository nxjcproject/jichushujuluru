﻿using BasicData.Service.MachineHaltReasons;
using EasyUIJsonParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BasicData.Web.UI_BasicData.MachineHaltReasons
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetMachineHaltReasonsWithTreeGridFormat()
        {
            DataTable dt = MachineHaltReasonsService.GetMachineHaltReasons();
            DataColumn parentIdColumn = new DataColumn("ParentID");
            dt.Columns.Add(parentIdColumn);

            foreach (DataRow row in dt.Rows)
            {
                string levelcode = row["MachineHaltReasonID"].ToString().Trim();
                if (levelcode.Length > 3)
                    row["ParentID"] = levelcode.Substring(0, levelcode.Length - 2);
            }
            return TreeGridJsonParser.DataTableToJson(dt, "MachineHaltReasonID", "ParentID", "ReasonText", "Remarks");
        }

        [WebMethod]
        public static void SaveMachineHaltReasons(string json)
        {
            DataTable dt = TreeGridJsonParser.JsonToDataTable(json);

            MachineHaltReasonsService.SaveMachineHaltReasons(dt);
        }
    }
}