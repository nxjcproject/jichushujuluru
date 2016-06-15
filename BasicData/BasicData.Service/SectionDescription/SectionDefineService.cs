using BasicData.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BasicData.Service.SectionDescription
{
   public class SectionDefineService
    {
       public static DataTable GetWorkingSectionDatas(string organizationId)
       {
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string mySql = @"SELECT * FROM system_WorkingSection
                             where OrganizationID=@myOrganizationID
                            ";
           SqlParameter parameter = new SqlParameter("myOrganizationID", organizationId);
           DataTable result = dataFactory.Query(mySql, parameter);
           return result;
       }
       public static DataTable GetOrganizationIds(string organizationId)
       {
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string mySql = @"SELECT  A.OrganizationID,A.Name,A.LevelType  
                                FROM system_Organization AS A 
                                WHERE A.LevelCode LIKE (select C.LevelCode from system_Organization as C where C.OrganizationID=@myOrganizationID)+'%'
								or A.LevelCode =(select C.LevelCode from system_Organization as C where C.OrganizationID=@myOrganizationID)
                                AND  A.LevelType='ProductionLine'
								and (A.OrganizationID like '%clinker%' or A.OrganizationID like '%cementmill%')";
           SqlParameter parameter = new SqlParameter("myOrganizationID",organizationId);
           DataTable result = dataFactory.Query(mySql, parameter);
           return result;
       }
       public static DataTable GetVariableNameTable(string organizationId)
       {
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string mySql = @"select LevelCode,OrganizationID,Name,VariableId,VariableName from
                (select B.LevelCode,A.OrganizationID,C.Name,B.VariableId,B.Name as VariableName,A.State,A.Type,A.ENABLE,B.Visible
                from [dbo].[tz_Formula] A,[dbo].[formula_FormulaDetail] B,[dbo].[system_Organization] C
                where A.KeyID=B.KeyID
				    and A.OrganizationID=C.OrganizationID
	                and A.State='0'
	                and A.Type='2'
	                and A.ENABLE=1
	                and B.Visible=1) T
					where OrganizationID=@myOrganizationID
                group by LevelCode,OrganizationID,Name,VariableId,VariableName 
                order by OrganizationID";
           SqlParameter parameter = new SqlParameter("myOrganizationID",organizationId);
           DataTable result = dataFactory.Query(mySql, parameter);
           return result;
       }
       public static DataTable GetOutputTable(string organizationId)
       {
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string mySql = @"select A.OrganizationID,A.Name as produceLine,B.VariableId,B.Name as Out_put,B.Type 
                         from [dbo].[tz_Material] A,[dbo].[material_MaterialDetail] B
                         where A.KeyID=B.KeyID
                          and A.ENABLE='1'
                          and A.Type='2'
                          and A.State='0'
                          and B.Visible='1'
                        and B.VariableId like '%Output'
                        and A.OrganizationID=@myOrganizationID
                        group by A.OrganizationID,A.Name,B.VariableId,B.Name,B.Type";
           SqlParameter parameter = new SqlParameter("myOrganizationID", organizationId);
           DataTable result = dataFactory.Query(mySql, parameter);
           return result;
       }
       public static DataTable GetPulverizedCoalTable(string organizationId)
       {
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string mySql = @"select A.OrganizationID,A.Name as produceLine,B.VariableId,B.Name as PulverizedCoalInput,B.Type 
                         from [dbo].[tz_Material] A,[dbo].[material_MaterialDetail] B
                         where A.KeyID=B.KeyID
                          and A.ENABLE='1'
                          and A.Type='2'
                          and A.State='0'
                          and B.Visible='1'
                        and B.VariableId like '%PulverizedCoalInput'
                        and A.OrganizationID=@myOrganizationID
                        group by A.OrganizationID,A.Name,B.VariableId,B.Name,B.Type";
           SqlParameter parameter = new SqlParameter("myOrganizationID", organizationId);
           DataTable result = dataFactory.Query(mySql, parameter);
           return result;
       }
       public static string SaveSectionDefine(string WorkingSectionName, string OrganizationID, string VariableName,
            string Out_put, string PulverizedCoalInput, string Creator, string Remarks)
       {
           string ElectricityQuantityId = VariableName + "__ElectricityQuantity";
           string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string myCheckSql = @"SELECT * FROM [dbo].[system_WorkingSection] 
                                where WorkingSectionName=@WorkingSectionName 
                                AND OrganizationID=@OrganizationID
                                AND ElectricityQuantityId=@VariableName
                                AND OutputId=@Output
                                AND Creator=@Creator";
           SqlParameter[] m_checkParameters ={new SqlParameter("@WorkingSectionName",WorkingSectionName),
                                            new SqlParameter("@OrganizationID",OrganizationID),
                                            new SqlParameter("@VariableName",ElectricityQuantityId),
                                            new SqlParameter("@Output",Out_put),
                                            new SqlParameter("@Creator",Creator)};
           DataTable table = dataFactory.Query(myCheckSql, m_checkParameters);
           if (table.Rows.Count > 0)
           {
               return "该岗位定义已经存在";
           }

           string mySql = @"INSERT INTO [dbo].[system_WorkingSection]
                                    ([WorkingSectionItemID]
                                    ,[WorkingSectionID]
                                    ,[WorkingSectionName]
                                    ,[Type]
                                    ,[OrganizationID]

                                    ,[ElectricityQuantityId]
                                    ,[OutputId]
                                    ,[CoalWeightId]
                                    ,[Creator]
                                    ,[CreatedTime]
                                    ,[Enabled]
                                    ,[Remarks])
                                VALUES
                                    (@WorkingSectionItemID
                                    ,@WorkingSectionID
                                    ,@WorkingSectionName
                                    ,@Type
                                    ,@OrganizationID

                                    ,@ElectricityQuantityId
                                    ,@OutputId
                                    ,@CoalWeightId
                                    ,@Creator
                                    ,@CreatedTime
                                    ,@Enabled
                                    ,@Remarks)";
           Guid WorkingSectionItemID = Guid.NewGuid();
           Guid WorkingSectionID = Guid.NewGuid();
           DateTime CreatedTime = DateTime.Now;
           string type = "test";
           int Enabled = 1;
           SqlParameter[] m_Parameters ={
                                            new SqlParameter("@WorkingSectionItemID",WorkingSectionItemID),
                                            new SqlParameter("@WorkingSectionID",WorkingSectionID),
                                            new SqlParameter("@WorkingSectionName",WorkingSectionName),                                             
                                              new SqlParameter("@Type",type),
                                            new SqlParameter("@OrganizationID",OrganizationID),                                           
                                            //new SqlParameter("@DisplayIndex",DisplayIndex),                                           
                                            new SqlParameter("@ElectricityQuantityId",ElectricityQuantityId),
                                            new SqlParameter("@OutputId",Out_put),
                                            new SqlParameter("@CoalWeightId",PulverizedCoalInput),
                                            new SqlParameter("@Creator",Creator),
                                            new SqlParameter("@CreatedTime",CreatedTime),
                                            new SqlParameter("@Enabled",Enabled),
                                            new SqlParameter("@Remarks", Remarks)};
           int num = dataFactory.ExecuteSQL(mySql, m_Parameters);
           if (num == 0) { return "保存失败"; } else { return "保存成功"; }
       }
       public static string RemoveSectionDefine(string WorkingSectionItemID_old, string WorkingSectionID_old)
       {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
           ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
           string removeSql = @"DELETE FROM [dbo].[system_WorkingSection] 
                                WHERE  WorkingSectionItemID=@WorkingSectionItemID
                                AND WorkingSectionID=@WorkingSectionID";
           SqlParameter[] removeParameters = {new SqlParameter("@WorkingSectionItemID",WorkingSectionItemID_old),
                                            new SqlParameter("@WorkingSectionID",WorkingSectionID_old)};
           int n = dataFactory.ExecuteSQL(removeSql, removeParameters);
           if (0 == n)
               return "删除设备失败";
           else
               return "删除成功";      
       }
        public static string UpdateSectionDefine(string WorkingSectionItemID_old,string WorkingSectionID_old,string WorkingSectionName, string OrganizationID, string VariableName,
            string Out_put, string Creator, string Remarks)
        {
           
            //string connectionString = ConnectionStringFactory.NXJCConnectionString;
            //ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            //string updateSql = @"UPDATE system_EquipmentAccount 
            //                        SET 
            //                        VariableId=@VariableId, OrganizationID=@OrganizationID,EquipmentName=@EquipmentName,MonitorType=@MonitorType,
            //                        PowerSupply=@PowerSupply,VoltageGrade=@VoltageGrade,RatedCT=@RatedCT,AmmeterCode=@AmmeterCode,ActualCT=@ActualCT,
            //                        [Power]=@Power,Unit=@Unit,[Current]=@Current,PowerSupplyPosition=@PowerSupplyPosition,Remarks=@Remarks
            //                        WHERE
            //                        VariableId=@VariableId_old AND OrganizationID=@OrganizationID_old";
            //SqlParameter[] m_Parameters = {new SqlParameter("@VariableId_old", variableId_old),
            //                                  new SqlParameter("@OrganizationID_old", organizationId_old),
            //                                  new SqlParameter("@VariableId", VariableId),
            //                              new SqlParameter("@OrganizationID", OrganizationID),
            //                              new SqlParameter("@EquipmentName", EquipmentName),
            //                              new SqlParameter("@MonitorType", MonitorType),
            //                              new SqlParameter("@PowerSupply", PowerSupply),
            //                              new SqlParameter("@VoltageGrade", VoltageGrade),
            //                              new SqlParameter("@RatedCT", RatedCT),
            //                              new SqlParameter("@AmmeterCode", AmmeterCode),
            //                              new SqlParameter("@ActualCT", ActualCT),
            //                              new SqlParameter("@Power", Power),
            //                              new SqlParameter("@Unit", Unit),
            //                              new SqlParameter("@Current", Current),
            //                              new SqlParameter("@PowerSupplyPosition", PowerSupplyPosition),
            //                              new SqlParameter("@Remarks", Remarks)};
            //int n = dataFactory.ExecuteSQL(updateSql, m_Parameters);
            //if (0 == n)
            //    return "更新数据失败";
            //else
            //    return "更新数据成功";



            string ElectricityQuantityId = VariableName + "__ElectricityQuantity";
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string updateSql = @"UPDATE [dbo].[system_WorkingSection] 
                                  SET 
                                    [WorkingSectionName]=@WorkingSectionName, 
                                    [OrganizationID]=@OrganizationID,
                                    [ElectricityQuantityId]=@VariableName,
                                    [OutputId]=@Output,                             
                                    [Creator]=@Creator,[Remarks]=@Remarks
                                 WHERE
                                    [WorkingSectionItemID]=@WorkingSectionItemID_old AND [WorkingSectionID]=@WorkingSectionID_old"; 
             SqlParameter[] m_updateParameters ={
                                            new SqlParameter("WorkingSectionItemID_old",WorkingSectionItemID_old),
                                            new SqlParameter("WorkingSectionID_old",WorkingSectionID_old),
                                            new SqlParameter("@WorkingSectionName",WorkingSectionName),
                                            new SqlParameter("@OrganizationID",OrganizationID),
                                            new SqlParameter("@VariableName",ElectricityQuantityId),
                                            new SqlParameter("@Output",Out_put),
                                            //new SqlParameter("@PulverizedCoalInput",PulverizedCoalInput),
                                            new SqlParameter("@Creator",Creator),
                                            new SqlParameter("@Remarks", Remarks)};

            int n = dataFactory.ExecuteSQL(updateSql, m_updateParameters);
            if (0 == n)
              return "更新数据失败"; 
            else
             return "更新数据成功"; 
        }
    }
}
