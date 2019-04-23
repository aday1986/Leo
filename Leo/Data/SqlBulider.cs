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
            string cacheKey = $"Insert|{typeof(T).Name}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                if (ColumnAttribute.TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnatts)
                && TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt))
                {
                    var fields = columnatts.Where(att => !att.Value.IsIdentity);
                    if (!fields.Any())
                        throw new Exception($"实体{typeof(T).Name}没有可被匹配的字段，无法新增。");
                    string strFields = "";
                    string values = "";
                    foreach (var field in fields)
                    {
                        string attName = field.Value.ColumnName ?? field.Key;
                        strFields += $"[{attName}],";
                        values += $"@{attName},";
                    }
                    sql += $"Insert {tableatt.TableName ?? typeof(T).Name}";
                    sql += $"({strFields.TrimEnd(',')}) Values({values.TrimEnd(',')})";
                }

                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetDeleteSql<T>()
        {
            string sql = string.Empty;
            string cacheKey = $"Delete|{typeof(T).Name}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                if (ColumnAttribute.TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnatts)
               && TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt))
                {
                    var keys = columnatts.Where(att => att.Value.IsPrimaryKey);
                    if (!keys.Any())
                        throw new Exception($"实体{typeof(T).Name}没有主键，无法被删除。");
                    sql += $"Delete From {tableatt.TableName ?? typeof(T).Name} Where ";
                    foreach (var key in keys)
                    {
                        string keyName = key.Value.ColumnName ?? key.Key;
                        sql += $"[{keyName}]=@{keyName} and";
                    }
                    sql = sql.Remove(sql.LastIndexOf(" and"));
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
            parameters = new Dictionary<string, object>();
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute))
            {
                sql = $"Delete from [{tableAttribute.TableName ?? typeof(T).Name}]";
                string strWhere = GetWhere(conditions, out parameters);
                if (!string.IsNullOrEmpty(strWhere))
                    sql += $" Where {strWhere}";
            }
            return sql;
        }

        public virtual string GetUpdateSql<T>()
        {
            string sql = string.Empty;
            string cacheKey = $"Update|{typeof(T).Name}";
            if (!SqlCache.TryGetValue(cacheKey, out sql))
            {
                if (ColumnAttribute.TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnatts)
               && TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableatt))
                {
                    var keys = columnatts.Where(att => att.Value.IsPrimaryKey);
                    if (!keys.Any())
                        throw new Exception($"实体{typeof(T).Name}没有可被匹配的主键，无法更新。");
                    var fields = columnatts.Where(att => !att.Value.IsPrimaryKey && !att.Value.IsIdentity && !att.Value.NoUpdate);
                    if (!fields.Any())
                        throw new Exception($"实体{typeof(T).Name}没有可被匹配的字段，无法更新。");
                    string strWhere = "";
                    foreach (var key in keys)
                    {
                        string attName = key.Value.ColumnName ?? key.Key;
                        strWhere += $" [{attName}]=@{attName} and";//条件
                    }
                    string strFields = "";
                    foreach (var field in fields)
                    {
                        string attName = field.Value.ColumnName ?? field.Key;

                        strFields += $"[{attName}]=@{attName},";//声明参数
                    }
                    sql += $"Update {tableatt.TableName ?? typeof(T).Name} set ";
                    sql += strFields.Trim(',');
                    sql += " Where " + strWhere.Remove(strWhere.LastIndexOf(" and"));
                }
                SqlCache[cacheKey] = sql;
            }
            return sql;
        }

        public virtual string GetSelectSql<T>(IEnumerable<Condition> conditions, out Dictionary<string, object> parameters, int? top = null)
        {
            string sql = string.Empty;
            parameters = new Dictionary<string, object>();
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute))
            {
                sql = $"Select {(top.HasValue ? $"Top {top.Value}" : "")} * from [{tableAttribute.TableName ?? typeof(T).Name}]";
                string strWhere = GetWhere(conditions, out parameters);
                if (!string.IsNullOrEmpty(strWhere))
                    sql += $" Where {strWhere}";
            }
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
            if (conditions == null)
                return "";
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
