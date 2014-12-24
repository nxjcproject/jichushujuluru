﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using WebStyleBaseForEnergy;

namespace BasicData.Web.UI_BasicData.MasterSlaveMachine
{
    public partial class MasterSlaveMachinedescription : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
                ////////////////////调试用,自定义的数据授权
                //List<string> m_DataValidIdItems = new List<string>() { "C41B1F47-A48A-495F-A890-0AABB2F3BFF7                            ", "43F1EA8C-FF77-4BC5-BACB-531DC56A2512                            " };
                //AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                this.TagsSelector_DcsTags.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.TagsSelector_DcsTags.PageName = "MasterSlaveMachinedescription.aspx";                                     //向web用户控件传递当前调用的页面名称
            }
        }
        [WebMethod]
        public static string GetMasterMachineInfo(string myDcsId)
        {
            DataTable m_MasterMachineInfo = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.GetMasterMachineInfo(myDcsId);
            return EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_MasterMachineInfo);
        }
        [WebMethod]
        public static string GetMasterMachineInfobyId(string myId)
        {
            DataTable m_MasterMachineInfo = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.GetMasterMachineInfobyId(myId);
            return EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_MasterMachineInfo);
        }
        [WebMethod]
        public static string AddMasterMachineInfo(string myOrganizationId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myRecord, string myValidValues, string myRemarks)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.AddMasterMachineInfo(myOrganizationId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myRecord, myValidValues, myRemarks);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifyMasterMachineInfo(string myId, string myOrganizationId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myRecord, string myValidValues, string myRemarks)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.ModifyMasterMachineInfo(myId, myOrganizationId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myRecord, myValidValues, myRemarks);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteMasterMachineInfo(string myId)
        {
            if (mUserId != "")
            {
                BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.DeleteAllSlaveMachineInfoByKeyId(myId); //删除所有从机
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.DeleteMasterMachineInfo(myId);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        //////////////////////////////////////从机//////////////////////////////////////

        /// <summary>
        /// 获得从机信息
        /// </summary>
        /// <param name="myKeyId"></param>
        /// <returns></returns>

        [WebMethod]
        public static string GetSlaveMachineInfo(string myKeyId)
        {
            DataTable m_SlaveMachineInfo = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.GetSlaveMachineInfo(myKeyId);
            return EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_SlaveMachineInfo);
        }
        [WebMethod]
        public static string GetSlaveMachineInfobyId(string myId)
        {
            DataTable m_SlaveMachineInfo = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.GetSlaveMachineInfobyId(myId);
            return EasyUIJsonParser.DataGridJsonParser.DataTableToJson(m_SlaveMachineInfo);
        }

        [WebMethod]
        public static string AddSlaveMachineInfo(string myOrganizationId, string myKeyId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myValidValues, string myTimeDelay, string myRemarks)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.AddSlaveMachineInfo(myOrganizationId, myKeyId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myValidValues, myTimeDelay, myRemarks);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string ModifySlaveMachineInfo(string myId, string myOrganizationId, string myKeyId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myValidValues, string myTimeDelay, string myRemarks)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.ModifySlaveMachineInfo(myId, myOrganizationId, myKeyId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myValidValues, myTimeDelay, myRemarks);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteSlaveMachineInfo(string myId)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.DeleteSlaveMachineInfo(myId);
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        [WebMethod]
        public static string DeleteAllSlaveMachineInfoByKeyId(string myKeyId)
        {
            if (mUserId != "")
            {
                int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.DeleteAllSlaveMachineInfoByKeyId(myKeyId);
                ReturnValue = ReturnValue > 1 ? 1 : ReturnValue;
                return ReturnValue.ToString();
            }
            else
            {
                return "非法的用户操作!";
            }
        }
        /// <summary>
        /// ////////////////////////////////////////获得组织机构
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetDcsOrganization()
        {
            DataTable m_DcsOrganization = WebUserControls.Service.TagsSelector.TagsSelector_Dcs.GetDCSTagsDataBase(GetDataValidIdGroup("ProductionOrganization"), true);
            return EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(m_DcsOrganization, "LevelCode", "OrganizationID", "Name");
        }


    }
}