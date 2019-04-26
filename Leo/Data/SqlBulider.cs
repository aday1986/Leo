using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 单表查询语句。
    /// </summary>
    public class SqlBulider : ISqlBulider
    {
        private static Dictionary<string, string> SqlCache = new Dictionary<string, string>();

        public virtual string GetInsertSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"Insert|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                var infos = modelType.GetProperties();
                if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt)) tableName = tableatt.TableName;
                string strFields = "";
                string values = "";
                foreach (var info in infos)
                {
                    string fieldName = info.Name;
                    if (ColumnAttribute.TryGetColumnAttribute(info, out ColumnAttribute columnAttribute))
                    {
                        if (columnAttribute.IsIdentity) continue;
                        fieldName = columnAttribute.ColumnName;
                    }
                    strFields += $"[{fieldName}],";
                    values += $"@{fieldName},";
                }
                sql += $"Insert into [{tableName}]";
                sql += $"({strFields.TrimEnd(',')}) Values({values.TrimEnd(',')})";
                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetDeleteSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"Delete|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt)) tableName = tableatt.TableName;
                if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<string, ColumnAttribute> keys))
                {
                    sql += $"Delete From {tableName} Where ";
                    foreach (var key in keys)
                    {
                        string keyName = key.Value.ColumnName ?? key.Key;
                        sql += $"[{keyName}]=@{keyName} and";
                    }
                    sql = sql.Remove(sql.LastIndexOf(" and"));
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
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute)) tableName = tableAttribute.TableName;
            parameters = new Dictionary<string, object>();
            sql = $"Delete from [{tableAttribute}]";
            string strWhere = GetWhere(conditions, out parameters);
            if (!string.IsNullOrEmpty(strWhere)) sql += $" Where {strWhere}";
            return sql;
        }

        public virtual string GetUpdateSql<T>()
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            string cacheKey = $"Update|{tableName}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                var infos = modelType.GetProperties();
                if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt)) tableName = tableatt.TableName;
                string strWhere = "";
                if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<string, ColumnAttribute> keys))
                {
                    foreach (var key in keys)
                    {
                        string attName = key.Value.ColumnName ?? key.Key;
                        strWhere += $" [{attName}]=@{attName} and";//条件
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
                    if (ColumnAttribute.TryGetColumnAttribute(info, out ColumnAttribute columnAttribute))
                    {
                        if (columnAttribute.IsPrimaryKey || columnAttribute.IsIdentity || columnAttribute.NoUpdate) continue;
                        fieldName = columnAttribute.ColumnName;
                    }
                    strFields += $"[{fieldName}]=@{fieldName},";//声明参数
                }
                sql += $"Update {tableName} set ";
                sql += strFields.Trim(',');
                sql += " Where " + strWhere.Remove(strWhere.LastIndexOf(" and"));
                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int? top = null)
        {
            string sql = string.Empty;
            Type modelType = typeof(T);
            string tableName = modelType.Name;
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute)) tableName = tableAttribute.TableName;
            parameters = new Dictionary<string, object>();
            sql = $"Select {(top.HasValue ? $"Top {top.Value}" : "")} * from [{tableName}]";
            string strWhere = GetWhere(conditions, out parameters);
            if (!string.IsNullOrEmpty(strWhere))
                sql += $" Where {strWhere}";
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
                whereSql += " And ";
            }
            if (whereSql.Length > 4)
                whereSql = whereSql.Remove(whereSql.Length - 4);
            return whereSql;
        }
    }
}
