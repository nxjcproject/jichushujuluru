using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicData.Service.BasicService
{
    public class BasicDataHelper
    {
        private ISqlServerDataFactory _dataFactory;

        public BasicDataHelper(string connString)
        {
            _dataFactory = new SqlServerDataFactory(connString);
        }
        /// <summary>
        /// Clone表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable CreateTableStructure(string tableName)
        {
            string queryString = @"SELECT TOP 1 * FROM " + tableName;
            DataTable sourceTable = _dataFactory.Query(queryString);
            DataTable result = sourceTable.Clone();

            return result;
        }
        /// <summary>
        /// Clone表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable CreateTempPlanTableStructure()
        {
            string queryString = @"select top 1 '' as OrganizationID, 
                            0 as Type,
                            plan_EnergyConsumptionYearlyPlan.DisplayIndex as IdSort, 
                            plan_EnergyConsumptionYearlyPlan.QuotasID as QuotasID, 
                            plan_EnergyConsumptionYearlyPlan.QuotasName as IndicatorName, 
                            plan_EnergyConsumptionYearlyPlan.January as January,
                            plan_EnergyConsumptionYearlyPlan.February as February,
                            plan_EnergyConsumptionYearlyPlan.March as March,
                            plan_EnergyConsumptionYearlyPlan.April as April,
                            plan_EnergyConsumptionYearlyPlan.May as May,
                            plan_EnergyConsumptionYearlyPlan.June as June,
                            plan_EnergyConsumptionYearlyPlan.July as July,
                            plan_EnergyConsumptionYearlyPlan.August as August,
                            plan_EnergyConsumptionYearlyPlan.September as September,
                            plan_EnergyConsumptionYearlyPlan.October as October,
                            plan_EnergyConsumptionYearlyPlan.November as November,
                            plan_EnergyConsumptionYearlyPlan.December as December, 
                            plan_EnergyConsumptionYearlyPlan.Totals as Totals, 
                            plan_EnergyConsumptionYearlyPlan.Remarks as Remarks 
                            from plan_EnergyConsumptionYearlyPlan";
            DataTable sourceTable = _dataFactory.Query(queryString);
            DataTable result = sourceTable.Clone();

            return result;
        }
    }
}
