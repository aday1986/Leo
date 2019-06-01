using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Leo.Data.Expressions;

namespace Leo.Data.SqlClient
{
    public class SqlClientDbProvider : IDbProvider
    {
        private readonly string connectionString;

        public  SqlClientDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);

        }

        public IDbDataParameter CreateDataParameter(string key, object value)
        {
           return new SqlParameter(key,value);
        }

        public ISqlAdapter CreateSqlAdapter()
        {
            return new SqlClientSqlAdapter();
        }
    }
}
