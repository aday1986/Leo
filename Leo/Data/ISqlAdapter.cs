using System;
using System.Collections.Generic;
using System.Reflection;

namespace Leo.Data
{
    public interface ISqlAdapter
    {
        string QuerySql(string selection, string source, string conditions,
          string order, string grouping, string having);
        string InsertSql(IEnumerable<string> fields, string source);
        string UpdateSql(IEnumerable<string> fields, string source, string conditions);
        string DeleteSql( string source, string conditions);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize, int pageNumber);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize);

        string Table(string tableName);
        string Field(string tableName, string fieldName, string alias = null);
        string Func(string func, string parms, string alias = null);
        string Parameter(string parameterId, string alias = null);

        /// <summary>
        /// 判断是否为内部定义的函数。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool IsFunc(MethodInfo info);

    }
}
