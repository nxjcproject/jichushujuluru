using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Transactions;
using BasicData.Infrastructure.Configuration;
using BasicData.Service.BasicService;
using SqlServerDataAdapter;

namespace BasicData.Service.EnergyConsumption
{
    public class EnergyConsumptionPlan
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        private static readonly BasicDataHelper _dataHelper = new BasicDataHelper(_connStr);
        public static DataTable GetEnergyConsumptionInfo(string myProductionLineType, string myOrganizationId, string myPlanYear)
        {
            string m_Sql = @"Select
                    A.QuotasID as QuotasID,  
                    A.QuotasName as QuotasName, 
                    B.January as January,
                    B.February as February,
                    B.March as March,
                    B.April as April,
                    B.May as May,
                    B.June as June,
                    B.July as July,
                    B.August as August,
                    B.September as September,
                    B.October as October,
                    B.November as November,
                    B.December as December,
                    B.Totals as Totals,
                    B.Remarks as Remarks
                    from plan_EnergyConsumptionPlan_Template A 
                    left join plan_EnergyConsumptionYearlyPlan B on A.QuotasID = B.QuotasID 
                                                                    and B.OrganizationID=@OrganizationID
                                                                    and B.PlanYear=@PlanYear
                    where A.ProductionLineType = @ProductionLineType
					order by A.DisplayIndex";
            try
            {
                SqlParameter[] m_Parameters = { new SqlParameter("@ProductionLineType", myProductionLineType), 
                                                  new SqlParameter("@OrganizationID", myOrganizationId), 
                                                  new SqlParameter("@PlanYear", myPlanYear) };
                DataTable m_Result = _dataFactory.Query(m_Sql, m_Parameters);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DataTable CreateTableStructure(string myDataTableName)
        {
            try
            {
                return _dataHelper.CreateTableStructure(myDataTableName);
            }
            catch
            {
                return null;
            }
        }
        public static int SaveEnergyConsumptionInfo(string myDataTableName, DataTable myDataTable)
        {
            return _dataFactory.Save(myDataTableName, myDataTable);
        }
        public static int DeleteEnergyConsumptionInfo(string myPlanYear, string myOrganizationId)
        {
            string m_Sql = @"DELETE FROM plan_EnergyConsumptionYearlyPlan where OrganizationID=@OrganizationID and PlanYear=@PlanYear";
            SqlParameter[] m_Parameters = { new SqlParameter("@OrganizationID", myOrganizationId), new SqlParameter("@PlanYear", myPlanYear) };
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
