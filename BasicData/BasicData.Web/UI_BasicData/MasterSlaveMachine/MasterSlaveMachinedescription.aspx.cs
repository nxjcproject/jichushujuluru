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
#if DEBUG
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
                mPageOpPermission = "0000";
#elif RELEASE
#endif
                this.TagsSelector_DcsTags.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.TagsSelector_DcsTags.PageName = "MasterSlaveMachinedescription.aspx";                                     //向web用户控件传递当前调用的页面名称
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
        public static string AddMasterMachineInfo(string myOrganizationId, string myVariableId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myRecord, string myValidValues, string myRemarks)
        {
            if (mPageOpPermission.ToArray()[1] == '1')
            {
                if (mUserId != "")
                {
                    int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.AddMasterMachineInfo(myOrganizationId, myVariableId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myRecord, myValidValues, myRemarks);
                    return ReturnValue.ToString();
                }
                else
                {
                    return "非法的用户操作!";
                }
            }
            else
            {
                return "该用户没有添加权限！";
            }
        }
        [WebMethod]
        public static string ModifyMasterMachineInfo(string myId, string myOrganizationId, string myVariableId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myRecord, string myValidValues, string myRemarks)
        {
            if (mPageOpPermission.ToArray()[2] == '1')
            {
                if (mUserId != "")
                {
                    int ReturnValue = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.ModifyMasterMachineInfo(myId, myOrganizationId, myVariableId, myVariableName, myVariableDescription, myDataBaseName, myTableName, myRecord, myValidValues, myRemarks);
                    return ReturnValue.ToString();
                }
                else
                {
                    return "非法的用户操作!";
                }
            }
            else
            {
                return "该用户没有修改权限！";
            }
        }
        [WebMethod]
        public static string DeleteMasterMachineInfo(string myId)
        {
            if (mPageOpPermission.ToArray()[3] == '1')
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
            else
            {
                return "该用户没有删除权限！";
            }
        }
        [WebMethod]
        public static string GetMasterMachineVariableId()
        {
            List<string> m_OrganizationIds = GetDataValidIdGroup("ProductionOrganization");
            DataTable m_MainMachineInfo = BasicData.Service.MasterSlaveMachine.MasterSlaveMachinedescription.GetMainMachineInfo(m_OrganizationIds);
            return EasyUIJsonParser.TreeJsonParser.DataTableToJsonByLevelCodeWithIdColumn(m_MainMachineInfo, "LevelCode", "VariableId", "Name");
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
            if (mPageOpPermission.ToArray()[1] == '1')
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
            else
            {
                return "该用户没有添加权限！";
            }
        }
        [WebMethod]
        public static string ModifySlaveMachineInfo(string myId, string myOrganizationId, string myKeyId, string myVariableName, string myVariableDescription, string myDataBaseName, string myTableName, string myValidValues, string myTimeDelay, string myRemarks)
        {
            if (mPageOpPermission.ToArray()[2] == '1')
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
            else
            {
                return "该用户没有修改权限！";
            }
        }
        [WebMethod]
        public static string DeleteSlaveMachineInfo(string myId)
        {
            if (mPageOpPermission.ToArray()[3] == '1')
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
            else
            {
                return "该用户没有删除权限！";
            }
        }
        [WebMethod]
        public static string DeleteAllSlaveMachineInfoByKeyId(string myKeyId)
        {
            if (mPageOpPermission.ToArray()[3] == '1')
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
            else
            {
                return "该用户没有删除权限！";
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