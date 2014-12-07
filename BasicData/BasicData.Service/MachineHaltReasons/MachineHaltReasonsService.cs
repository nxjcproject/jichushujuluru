using BasicData.Infrastructure.Configuration;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicData.Service.MachineHaltReasons
{
    public class MachineHaltReasonsService
    {
        /// <summary>
        /// 获取所有的停机原因
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMachineHaltReasons()
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("system_MachineHaltReason");
            query.AddOrderByClause(new SqlServerDataAdapter.Infrastruction.OrderByClause("MachineHaltReasonID", false));

            return factory.Query(query);
        }

        /// <summary>
        /// 保存停机原因
        /// </summary>
        public static void SaveMachineHaltReasons(DataTable data)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Delete delete = new Delete("system_MachineHaltReason");
            factory.Remove(delete);

            factory.Save("system_MachineHaltReason", data);

        }
    }
}
