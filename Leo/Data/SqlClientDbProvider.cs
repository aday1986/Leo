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

        public SqlClientDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbDataAdapter CreateAdapter()
        {
            Leo.ThirdParty.Autofac.ContainerBuilder builder=new ThirdParty.Autofac.ContainerBuilder();

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
    }
}
