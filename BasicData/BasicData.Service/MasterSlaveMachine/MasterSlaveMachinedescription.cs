using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using BasicData.Infrastructure.Configuration;
using BasicData.Service.BasicService;
using SqlServerDataAdapter;

namespace BasicData.Service.MasterSlaveMachine
{
    public class MasterSlaveMachinedescription
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory =  new SqlServerDataFactory(_connStr);
        private static readonly BasicDataHelper _dataHelper = new BasicDataHelper(_connStr);

        public static DataTable GetMasterMachineInfo(string myDcsId)
        {
            string m_Sql = @"Select
                    A.ID as Id,  
                    A.OrganizationID as OrganizationId, 
                    B.Name as OrganizationName, 
                    A.VariableName as VariableName,
                    A.VariableDescription as VariableDescription,
                    A.Record as Record,
                    A.ValidValues as ValidValues,
                    A.Remarks as Remarks,
					A.KeyID as KeyId  
                    from system_MasterMachinedescription A 
                    left join system_Organization_Instrumentation B on A.OrganizationID = B.LevelCode
					where A.OrganizationID=@OrganizationId";
            try
            {
                SqlParameter[] m_Parameters = { new SqlParameter("@OrganizationId", myDcsId)};
                DataTable m_Result = _dataFactory.Query(m_Sql, m_Parameters);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DataTable GetMasterMachineInfobyId(string myId)
        {
            string m_Sql = @"Select
                    A.ID as Id,  
                    A.OrganizationID as OrganizationId, 
                    B.Name as OrganizationName, 
                    A.VariableName as VariableName,
                    A.VariableDescription as VariableDescription,
                    A.Record as Record,
                    A.ValidValues as ValidValues,
                    A.Remarks as Remarks,
					A.KeyID as KeyId  
                    from system_MasterMachinedescription A 
                    left join system_Organization_Instrumentation B on A.OrganizationID = B.OrganizationID
					where A.ID=@Id";
            try
            {
                SqlParameter[] m_Parameters = { new SqlParameter("@Id", myId) };
                DataTable m_Result = _dataFactory.Query(m_Sql, m_Parameters);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static int AddMasterMachineInfo(string myOrganizationId, string myVariableName, string myVariableDescription, string myRecord, string myValidValues, string myRemarks)
        {
            string m_Sql = @" Insert into system_MasterMachinedescription 
                ( OrganizationID, VariableName, VariableDescription, Record, ValidValues, Remarks) 
                values
                (@OrganizationID,@VariableName,@VariableDescription,@Record,@ValidValues,@Remarks)";
            SqlParameter[] m_Parameters = { new SqlParameter("@OrganizationID", myOrganizationId),
                                          new SqlParameter("@VariableName", myVariableName),
                                          new SqlParameter("@VariableDescription", myVariableDescription),
                                          new SqlParameter("@Record", myRecord),
                                          new SqlParameter("@ValidValues", myValidValues),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch
            {
                return -1;
            }
        }
        public static int ModifyMasterMachineInfo(string myId, string myOrganizationId, string myVariableName, string myVariableDescription, string myRecord, string myValidValues, string myRemarks)
        {
            string m_Sql = @"UPDATE system_MasterMachinedescription SET 
                            OrganizationID=@OrganizationID, 
                            VariableName=@VariableName, 
                            VariableDescription=@VariableDescription, 
                            Record=@Record, 
                            ValidValues=@ValidValues, 
                            Remarks=@Remarks
                            where ID=@ID";
            SqlParameter[] m_Parameters = {new SqlParameter("@ID", myId),
                                          new SqlParameter("@OrganizationID", myOrganizationId),
                                          new SqlParameter("@VariableName", myVariableName),
                                          new SqlParameter("@VariableDescription", myVariableDescription),
                                          new SqlParameter("@Record", myRecord),
                                          new SqlParameter("@ValidValues", myValidValues),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static int DeleteMasterMachineInfo(string myId)
        {
            string m_Sql = @"DELETE FROM system_MasterMachinedescription where ID=@ID";
            SqlParameter[] m_Parameters = {new SqlParameter("@ID", myId)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static DataTable GetSlaveMachineInfo(string myKeyId)
        {
            string m_Sql = @"Select
                    A.ID as Id,  
                    A.OrganizationID as OrganizationId, 
                    B.Name as OrganizationName, 
                    A.KeyID as KeyId, 
                    C.VariableDescription as KeyName, 
                    A.VariableName as VariableName,
                    A.VariableDescription as VariableDescription,
                    A.ValidValues as ValidValues,
                    A.TimeDelay as TimeDelay,
                    A.Remarks as Remarks
                    from system_SlaveMachinedescription A 
                    left join system_Organization_Instrumentation B on A.OrganizationID = B.LevelCode 
                    left join system_MasterMachinedescription C on A.KeyID = C.ID
					where A.KeyID=@KeyId";
            try
            {
                SqlParameter[] m_Parameters = { new SqlParameter("@KeyId", myKeyId) };
                DataTable m_Result = _dataFactory.Query(m_Sql, m_Parameters);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DataTable GetSlaveMachineInfobyId(string myId)
        {
            string m_Sql = @"Select
                    A.ID as Id,  
                    A.OrganizationID as OrganizationId, 
                    B.Name as OrganizationName, 
                    A.KeyID as KeyId, 
                    C.VariableDescription as KeyName, 
                    A.VariableName as VariableName,
                    A.VariableDescription as VariableDescription,
                    A.ValidValues as ValidValues,
                    A.TimeDelay as TimeDelay,
                    A.Remarks as Remarks
                    from system_SlaveMachinedescription A 
                    left join system_Organization_Instrumentation B on A.OrganizationID = B.OrganizationID 
                    left join system_MasterMachinedescription C on A.KeyID = C.ID
					where A.ID=@Id";
            try
            {
                SqlParameter[] m_Parameters = { new SqlParameter("@Id", myId) };
                DataTable m_Result = _dataFactory.Query(m_Sql, m_Parameters);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static int AddSlaveMachineInfo(string myOrganizationId, string myKeyId, string myVariableName, string myVariableDescription, string myValidValues, string myTimeDelay, string myRemarks)
        {
            string m_Sql = @" Insert into system_SlaveMachinedescription 
                ( OrganizationID, KeyID, VariableName, VariableDescription, ValidValues, TimeDelay, Remarks) 
                values
                (@OrganizationID, @KeyID, @VariableName, @VariableDescription, @ValidValues, @TimeDelay, @Remarks)";
            SqlParameter[] m_Parameters = { new SqlParameter("@OrganizationID", myOrganizationId),
                                          new SqlParameter("@KeyID", myKeyId),
                                          new SqlParameter("@VariableName", myVariableName),
                                          new SqlParameter("@VariableDescription", myVariableDescription),
                                          new SqlParameter("@ValidValues", myValidValues),
                                          new SqlParameter("@TimeDelay", myTimeDelay),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch
            {
                return -1;
            }
        }
        public static int ModifySlaveMachineInfo(string myId, string myOrganizationId, string myKeyId, string myVariableName, string myVariableDescription, string myValidValues, string myTimeDelay, string myRemarks)
        {
            string m_Sql = @"UPDATE system_SlaveMachinedescription SET              
                OrganizationID=@OrganizationID, 
                KeyID=@KeyID, 
                VariableName=@VariableName, 
                VariableDescription=@VariableDescription, 
                ValidValues=@ValidValues, 
                TimeDelay=@TimeDelay, 
                Remarks=@Remarks 
                where ID=@ID";
            SqlParameter[] m_Parameters = {new SqlParameter("@ID", myId),
                                          new SqlParameter("@OrganizationID", myOrganizationId),
                                          new SqlParameter("@KeyID", myKeyId),
                                          new SqlParameter("@VariableName", myVariableName),
                                          new SqlParameter("@VariableDescription", myVariableDescription),
                                          new SqlParameter("@ValidValues", myValidValues),
                                          new SqlParameter("@TimeDelay", myTimeDelay),
                                          new SqlParameter("@Remarks", myRemarks)};
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static int DeleteSlaveMachineInfo(string myId)
        {
            string m_Sql = @"DELETE FROM system_SlaveMachinedescription where ID=@ID";
            SqlParameter[] m_Parameters = { new SqlParameter("@ID", myId) };
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public static int DeleteAllSlaveMachineInfoByKeyId(string myKeyId)
        {
            string m_Sql = @"DELETE FROM system_SlaveMachinedescription where KeyID=@KeyID";
            SqlParameter[] m_Parameters = { new SqlParameter("@KeyID", myKeyId) };
            try
            {
                return _dataFactory.ExecuteSQL(m_Sql, m_Parameters);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
