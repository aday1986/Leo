using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Leo.Data.Expressions
{
    public abstract class ISqlAdapter
    {
        private string N(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : $" {value}";
        }
        public virtual string QuerySql(string selection, string source, string conditions,
            string order, string grouping, string having,
            int? size,int? skip)
        {
            if (size.HasValue && skip.HasValue)
            {
                var innerQuery = string.Format("SELECT {0},ROW_NUMBER() OVER ({1}) AS RN FROM {2} {3}",
                                          selection, order, source, conditions);

                return string.Format("SELECT TOP {0} * FROM ({1}) InnerQuery WHERE RN > {2} ORDER BY RN",
                                     size, innerQuery, skip);
                // return string.Format("SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY",
                //selection, source, conditions, order, pageSize * (pageNumber - 1), pageSize);
            }
            else if(size.HasValue)
            {
              return  $"SELECT TOP {size} {selection} FROM {source}{N(conditions)}{N(order)}{N(grouping)}{N(having)}";
            }
            else
            {
                return $"SELECT {selection} FROM {source}{N(conditions)}{N(order)}{N(grouping)}{N(having)}";
            }
          
        }
      

        /// <summary>
        /// 返回[table] AS alias或(query) AS alias。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual string Source(SourceInfo source)
        {
            if (source.IsQuery)
            {
                return $"({source.SourceText}) AS {source.Alias}";
            }
            else
            {
                return $"[{source.SourceText}]{Alias(source.SourceText, source.Alias)}";
            }
        }

        public virtual string Field(ColumnInfo column,bool hasAlias)
        {
            //实体字段的名称最终必须是属性名，即别名应该是属性名。
            return $"{GetSourceName(column.Source)}.[{column.ColumnText}]{(hasAlias? Alias(column.ColumnText,column.Alias):"")}";
        }

        /// <summary>
        /// 返回实际显示的别名，如果相同，则返回空。
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public virtual string Alias(string sourceName, string alias)
        {
            if (sourceName==alias || string.IsNullOrEmpty(alias))
            {
                return "";
            }
            else
            {
                return $" AS {alias}";
            }
        }

        /// <summary>
        /// 如果有别名，返回别名，否则返回表名称。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual string GetSourceName(SourceInfo source)
        {
            if (!string.IsNullOrEmpty(source.Alias))
            {
                return source.Alias;
            }
            else if(!source.IsQuery)
            {
                return $"[{source.SourceText}]";
            }
            else
            {
                throw new Exception($"无法获取{source.SourceText}名称。");
            }
        }

        public virtual string Parameter(string parameterId, string alias = null)
        {
            return $"@{parameterId}{(string.IsNullOrEmpty(alias) ? "" : $" AS {alias}")}";
        }

        public virtual string Func(string func, string parms, string alias = null)
        {
            return $"{func}({parms}){(string.IsNullOrEmpty(alias) ? "" : $" AS {alias}")}";
        }

        public virtual bool IsFunc(MethodInfo info)
        {
            return (info.IsStatic && (info.ReflectedType == typeof(AggFunc)));

        }

        public virtual string InsertSql(IEnumerable<string> fields, string source)
        {
            return $"INSERT INTO {source}({string.Join(",", fields.Select(s => $"[{s}]"))}) VALUES({string.Join(",", fields.Select(s => $"@{s}"))})";
        }

        public virtual string UpdateSql(IEnumerable<string> fields, string source, string conditions)
        {
            return $"UPDATE {source} SET {String.Join(",", fields.Select(s => $"[{s}]=@{s}"))}{N(conditions)}";
        }

        public virtual string DeleteSql(string source, string conditions)
        {
            return $"DELETE FROM {source}{N(conditions)}";
        }

    }
}
