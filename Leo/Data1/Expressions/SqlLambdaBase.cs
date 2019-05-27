using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leo.Data1.Expressions.Adapter;
using Leo.Data1.Expressions.Resolver;

namespace Leo.Data1.Expressions
{
    /// <summary>
    /// Base functionality for the SqlLam class that is not related to any specific entity type
    /// </summary>
    public abstract class SqlLambdaBase
    {
        internal static ISqlAdapter _defaultAdapter = new SqlServerAdapter();
        internal SqlQueryBuilder _builder;
        internal LambdaResolver _resolver;

        public SqlQueryBuilder SqlBuilder { get { return _builder; } }

        public string QueryString
        {
            get { return _builder.QueryString; }
        }

        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            return _builder.QueryStringPage(pageSize, pageNumber);
        }

        public IDictionary<string, object> QueryParameters
        {
            get { return _builder.Parameters; }
        }

        public override string ToString()
        {
            return this.QueryString;
        }

        public string[] SplitColumns
        {
            get { return _builder.SplitColumns.ToArray(); }
        }

    }
}
