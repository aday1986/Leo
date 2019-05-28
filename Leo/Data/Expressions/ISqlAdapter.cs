using System;
using System.Collections.Generic;
using System.Reflection;

namespace Leo.Data.Expressions
{
    public interface ISqlAdapter
    {
        string QueryString(string selection, string source, string conditions,
          string order, string grouping, string having);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize, int pageNumber);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize);

        string Table(string tableName);
        string Field(string tableName, string fieldName, string alias = null);
        string Func( string func, string parms, string alias=null);
        string Parameter(string parameterId, string alias = null);

        bool IsFunc(MethodInfo info);

    }
}
