using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Leo.Data
{
    public class SqlClientDbProvider : IDbProvider
    {
        private readonly string connectionString;

        public  SqlClientDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDbDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter();
        }

        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);

        }

        public override IDbDataParameter CreateDataParameter(string key, object value)
        {
           return new SqlParameter(key,value);
        }
    }
}
