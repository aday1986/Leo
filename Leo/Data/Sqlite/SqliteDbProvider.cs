using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Leo.Data.Expressions;

namespace Leo.Data.Sqlite
{
    public class SqliteDbProvider : IDbProvider
    {
        private readonly string connectionString;

        public SqliteDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbDataAdapter CreateAdapter()
        {
            return new SQLiteDataAdapter();
        }

        public IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        public IDbConnection CreateConnection()
        {
            var db = new SQLiteConnection(connectionString);
            return db;
        }

        public IDbDataParameter CreateDataParameter(string key, object value)
        {
            return new SQLiteParameter(key, value);
        }

        public LambdaResolver CreateResolver()
        {
            return new LambdaResolver(new SqliteSqlAdapter());
        }
    }
}
