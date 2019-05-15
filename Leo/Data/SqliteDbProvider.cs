using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Leo.Data
{
    public class SqliteDbProvider : IDbProvider
    {
        private readonly string connectionString;

        public SqliteDbProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDbDataAdapter CreateAdapter()
        {
            return new SQLiteDataAdapter();
        }

        public override IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        public override IDbConnection CreateConnection()
        {
            var db = new SQLiteConnection(connectionString);
            return db;
        }

        public override IDbDataParameter CreateDataParameter(string key, object value)
        {
            return new SQLiteParameter(key, value);
        }

        public override string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int pageSize, int pageIndex)
        {
         var sql= GetSelectSql<T>(conditions, out parameters);
            sql = $"{sql} limit {(pageIndex-1)*pageSize},{pageSize}";
            return sql;
        }

        public override string GetCreate<T>()
        {
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            StringBuilder sb = new StringBuilder();
            sb.Append($@"CREATE TABLE IF NOT EXISTS {tableName}(");
            Type type = typeof(T);
            var infos = modelType.GetProperties();
            if (modelType.TryGetTableAttribute(out TableAttribute tableatt)) tableName = tableatt.TableName ?? modelType.Name;
            string strFields = string.Empty;
            string values = string.Empty;
            foreach (var info in infos)
            {
                string fieldName = info.Name;
                if (info.TryGetColumnAttribute(out ColumnAttribute att))
                {
                    fieldName = att.ColumnName ?? info.Name;
                }
                sb.Append(fieldName);
                switch (info.PropertyType.ToString())
                {
                    case "System.Int32":
                    case "System.Boolean":
                        sb.Append(" INTEGER");
                        break;

                    case "System.Double":
                        sb.Append(" REAL");
                        break;
                    default:
                        sb.Append(" TEXT");
                        break;
                }
                if (att != null)
                {
                    if (att.IsPrimaryKey) sb.Append(" PRIMARY KEY");
                    if (att.IsIdentity) sb.Append(" AUTOINCREMENT");
                    if (!att.Nullable) sb.Append(" NOT NULL");
                }
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',') + ")";
        }
    }
}
