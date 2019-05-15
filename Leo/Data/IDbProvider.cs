using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Leo.Data
{


    /// <summary>
    /// 表示用于创建数据库连接等相关组件及Sql语句的一组类。
    /// </summary>
    public abstract class IDbProvider
    {
        //可以在这个接口里写需要Sql语句的方法。


        public abstract IDbConnection CreateConnection();
        public abstract IDbDataAdapter CreateAdapter();
        public abstract IDbCommand CreateCommand();
        public abstract IDbDataParameter CreateDataParameter(string key, object value);

        /// <summary>
        /// 用于存储Sql语句缓存。
        /// </summary>
        protected static Dictionary<string, string> SqlCache = new Dictionary<string, string>();

        public virtual string GetInsertSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"INSERT|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                var infos = modelType.GetProperties();
                if (modelType.TryGetTableAttribute(out TableAttribute tableatt)) tableName = tableatt.TableName ?? modelType.Name;
                string strFields = string.Empty;
                string values = string.Empty;
                foreach (var info in infos)
                {
                    string fieldName = info.Name;
                    if (info.TryGetColumnAttribute(out ColumnAttribute columnAttribute))
                    {
                        if (columnAttribute.IsIdentity) continue;
                        fieldName = columnAttribute.ColumnName ?? info.Name;
                    }
                    strFields += $"[{fieldName}],";
                    values += $"@{fieldName},";
                }
                sql += $"INSERT INTO [{tableName}]";
                sql += $"({strFields.TrimEnd(',')}) VALUES({values.TrimEnd(',')})";
                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetDeleteSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"DELETE|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                if (modelType.TryGetTableAttribute(out TableAttribute tableatt)) tableName = tableatt.TableName ?? modelType.Name;
                if (modelType.TryGetKeyColumns(out Dictionary<string, ColumnAttribute> keys))
                {
                    sql += $"DELETE FROM {tableName} WHERE ";
                    foreach (var key in keys)
                    {
                        string keyName = key.Value.ColumnName ?? key.Key;
                        sql += $"[{keyName}]=@{keyName} AND";
                    }
                    sql = sql.Remove(sql.LastIndexOf(" AND"));
                }
                else
                {
                    throw new Exception($"实体{typeof(T).Name}没有主键，无法被删除。");
                }

                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetDeleteSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters)
        {
            if (conditions == null)
                throw new NullReferenceException("条件不能为Null。");
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            if (modelType.TryGetTableAttribute(out TableAttribute tableAttribute)) tableName = tableAttribute.TableName ?? modelType.Name;
            parameters = new Dictionary<string, object>();
            sql = $"DELETE FROM [{tableName}]";
            string strWhere = GetWhere(conditions, out parameters);
            if (!string.IsNullOrEmpty(strWhere)) sql += $" WHERE {strWhere}";
            return sql;
        }

        public virtual string GetUpdateSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"UPDATE|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                var infos = modelType.GetProperties();
                if (modelType.TryGetTableAttribute(out TableAttribute tableatt)) tableName = tableatt.TableName ?? modelType.Name;
                string strWhere = string.Empty;
                if (modelType.TryGetKeyColumns(out Dictionary<string, ColumnAttribute> keys))
                {
                    foreach (var key in keys)
                    {
                        string attName = key.Value.ColumnName ?? key.Key;
                        strWhere += $" [{attName}]=@{attName} AND";//条件
                    }
                }
                else
                {
                    throw new Exception($"实体{modelType.Name}没有可被匹配的主键，无法更新。");
                }
                string strFields = "";
                foreach (var info in infos)
                {
                    string fieldName = info.Name;
                    if (info.TryGetColumnAttribute(out ColumnAttribute columnAttribute))
                    {
                        if (columnAttribute.IsPrimaryKey || columnAttribute.IsIdentity || columnAttribute.NoUpdate) continue;
                        fieldName = columnAttribute.ColumnName ?? info.Name;
                    }
                    strFields += $"[{fieldName}]=@{fieldName},";//声明参数
                }
                sql += $"UPDATE {tableName} SET ";
                sql += strFields.Trim(',');
                sql += " WHERE " + strWhere.Remove(strWhere.LastIndexOf(" AND"));
                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int? top = null)
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute)) tableName = tableAttribute.TableName ?? modelType.Name;
            parameters = new Dictionary<string, object>();
            sql = $"SELECT {(top.HasValue ? $"TOP {top.Value}" : "")} * FROM [{tableName}]";
            string strWhere = GetWhere(conditions, out parameters);
            if (!string.IsNullOrEmpty(strWhere))
                sql += $" WHERE {strWhere}";
            return sql;
        }

        public virtual string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public virtual string GetWhere(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters)
        {
            string whereSql = string.Empty;
            parameters = new Dictionary<string, object>();
            if (conditions == null) return whereSql;
            foreach (var condition in conditions)
            {
                whereSql += $"[{condition.Key}] ";
                switch (condition.ConditionType)
                {
                    case ConditionEnum.Equal:
                        whereSql += "=";
                        break;
                    case ConditionEnum.NotEqual:
                        whereSql += "<>";
                        break;
                    case ConditionEnum.Greater:
                        whereSql += ">";
                        break;
                    case ConditionEnum.GreaterEqual:
                        whereSql += ">=";
                        break;
                    case ConditionEnum.Less:
                        whereSql += "<";
                        break;
                    case ConditionEnum.LessEqual:
                        whereSql += "<=";
                        break;
                    default:
                        whereSql += condition.ConditionType.ToString();
                        break;
                }
                string parmKey = $"@parm{parameters.Count}";
                whereSql += " " + parmKey;
                parameters.Add(parmKey, condition.Value);
                whereSql += " AND ";
            }
            if (whereSql.Length > 4)
                whereSql = whereSql.Remove(whereSql.Length - 4);
            return whereSql;
        }

        public virtual string GetCreate<T>()
        {
            throw new NotImplementedException();
        }

    }
}
