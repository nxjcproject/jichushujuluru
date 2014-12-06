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
    }
}
