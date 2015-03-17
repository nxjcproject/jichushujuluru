﻿using BasicData.Infrastructure.Configuration;
using BasicData.Service.BasicService;
using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BasicData.Service.PeakValleyFlat
{
    public class PeakValleyFlatService
    {
        private readonly static string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        private static BasicDataHelper _dataHelper = new BasicDataHelper(_connStr);

        public static DataTable GetPVFList(string organizationId, string startUsingDate)
        {
            string queryStr = @"SELECT * FROM system_PVF WHERE OrganizationID=@organizationId AND StartUsing>=@startUsingDate";
            SqlParameter[] parameters = { new SqlParameter("@organizationId", organizationId), new SqlParameter("@startUsingDate", startUsingDate) };
            DataTable result = _dataFactory.Query(queryStr, parameters);
            return result;
        }

        public static DataTable GetPVFDetail(string keyId)
        {
            string queryStr = @"SELECT * FROM system_PVF_Detail WHERE KeyID=@keyid";
            DataTable result = _dataFactory.Query(queryStr, new SqlParameter("@keyid", keyId));
            return result;
        }

        public static int SavePVFData(string organizationId, string[] dataDetails)
        {
            int result;
            Guid keyId = Guid.NewGuid();

            DataTable pvfTable = _dataHelper.CreateTableStructure("system_PVF");
            DataRow newRow = pvfTable.NewRow();
            newRow["OrganizationID"] = organizationId;
            //newRow["StartUsing"] = startUsing;
            newRow["ID"] = keyId;
            newRow["KeyID"] = keyId;
            pvfTable.Rows.Add(newRow);

            DataTable detailTable = _dataHelper.CreateTableStructure("system_PVF_Detail");
            DataTable dt = EasyUIJsonParser.DataGridJsonParser.JsonToDataTable(dataDetails, detailTable);
            foreach (DataRow dr in dt.Rows)
            {
                Guid detailID = Guid.NewGuid();
                dr["ID"] = detailID;
                dr["KeyID"] = keyId;
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    _dataFactory.Save("system_PVF", pvfTable);
                    _dataFactory.Save("system_PVF_Detail", dt);
                    scope.Complete();
                    result = 1;
                }
                catch
                {
                    result = -1;
                    scope.Dispose();
                }
            }

            return result;
        }

        public static int DeletePVFData(string keyId)
        {
            int result;
            string deletePVF = @"DELETE FROM system_PVF WHERE KeyID=@keyId";
            string deleteDetail = @"DELETE FROM system_PVF_Detail WHERE KeyID=@keyId";

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    _dataFactory.ExecuteSQL(deletePVF, new SqlParameter("@keyId", keyId));
                    _dataFactory.ExecuteSQL(deleteDetail, new SqlParameter("@keyId", keyId));
                    scope.Complete();
                    result = 1;
                }
                catch
                {
                    result = -1;
                }
            }
            return result;
        }

        public static int UpdatePVFData(string id, string startUsing, string endUsing, string flag)
        {
            int result;
            string update = @"UPDATE system_PVF SET StartUsing=@startUsing,EndUsing=@endUsing,Flag=@flag WHERE ID=@id";
            try
            {
                _dataFactory.ExecuteSQL(update, new SqlParameter("@id", id), new SqlParameter("@startUsing", startUsing), new SqlParameter("@endUsing", endUsing), new SqlParameter("@flag", flag));
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return result;
        }
    }
}
