using System.Collections.Generic;
using System.Reflection;

namespace Leo.Data.Expressions
{
    public class SqlServerSqlAdapter : ISqlAdapter
    {
        public string QueryString(string selection, string source, string conditions, string order, string grouping, string having)
        {
            return string.Format("SELECT {0} FROM {1} {2} {3} {4} {5}",
                                 selection, source, conditions, order, grouping, having);
        }
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize)
        {
            return string.Format("SELECT TOP({4}) {0} FROM {1} {2} {3}",
                    selection, source, conditions, order, pageSize);
        }

        public string QueryStringPage(string source, string selection, string conditions, string order,
          int pageSize, int pageNumber)
        {
            var innerQuery = string.Format("SELECT {0},ROW_NUMBER() OVER ({1}) AS RN FROM {2} {3}",
                                           selection, order, source, conditions);

            return string.Format("SELECT TOP {0} * FROM ({1}) InnerQuery WHERE RN > {2} ORDER BY RN",
                                 pageSize, innerQuery, pageSize * (pageNumber - 1));
            // return string.Format("SELECT {0} FROM {1} {2} {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY",
            //selection, source, conditions, order, pageSize * (pageNumber - 1), pageSize);
        }


        public string Table(string tableName)
        {
            return string.Format("[{0}]", tableName);
        }

        public string Field(string tableName, string fieldName, string alias = null)
        {
            return $"[{tableName}].[{fieldName}]{(string.IsNullOrEmpty(alias) || alias == fieldName ? "" : $" AS {alias}")}";
        }

        public string Parameter(string parameterId,string alias=null)
        {
            return $"@{parameterId}{(string.IsNullOrEmpty(alias)? "":$" AS {alias}")}";
        }

        public string Func(string func, string parms, string alias = null)
        {
            return $"{func}({parms}){(string.IsNullOrEmpty(alias) ? "" : $" AS {alias}")}";
        }

        public bool IsFunc(MethodInfo info)
        {
            return (info.IsStatic && info.ReflectedType == typeof(AggFunc));
           
        }
    }
}
