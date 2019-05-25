using System;
using System.Collections.Generic;

namespace Leo.Data1.Expressions
{
    /// <summary>
    /// SQL adapter provides db specific functionality related to db specific SQL syntax
    /// </summary>
    interface ISqlAdapter
    {
        string QueryString(string selection, string source, string conditions, 
            string order, string grouping, string having);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize, int pageNumber);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize);

        string Table(string tableName);
        string Field(string tableName, string fieldName,string alias=null);
        string SelectFunction(string tableName, string fieldName, string selectFunction, string alias);
        string Parameter(string parameterId);
    }
}
