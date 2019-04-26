using System.Data;
using System.Data.SQLite;

namespace Leo.Data
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
           return new  SQLiteDataAdapter();
        }

        public IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public IDbDataParameter CreateDataParameter(string key, object value)
        {
            return new SQLiteParameter(key, value);
        }
    }
}
