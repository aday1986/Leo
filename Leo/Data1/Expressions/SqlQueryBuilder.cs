using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Leo.Data1.Expressions.Adapter;

namespace Leo.Data1.Expressions
{
    /// <summary>
    /// 实现整个SQL的构建逻辑。
    /// </summary>
    public partial class SqlQueryBuilder
    {
        internal ISqlAdapter Adapter { get; set; }

        private const string PARAMETER_PREFIX = "Param";

        public List<string> TableNames { get; } = new List<string>();

        public List<string> JoinExpressions { get; } = new List<string>();
        public List<string> SelectionList { get; } = new List<string>();

        public List<string> WhereConditions { get; } = new List<string>();

        public List<string> OrderByList { get; } = new List<string>();

        public List<string> GroupByList { get; } = new List<string>();

        public List<string> HavingConditions { get; } = new List<string>();

        public List<string> SplitColumns { get; } = new List<string>();

        public int CurrentParamIndex { get; private set; }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", JoinExpressions);
                return string.Format("{0} {1}", Adapter.Table(TableNames.First()), joinExpression);
            }
        }

        private string Selection
        {
            get
            {
                if (SelectionList.Count == 0)
                    return string.Format("{0}.*", Adapter.Table(TableNames.First()));
                else
                    return string.Join(", ", SelectionList);
            }
        }

        private string Conditions
        {
            get
            {
                if (WhereConditions.Count == 0)
                    return "";
                else
                    return "WHERE " + string.Join("", WhereConditions);
            }
        }

        private string Order
        {
            get
            {
                if (OrderByList.Count == 0)
                    return "";
                else
                    return "ORDER BY " + string.Join(", ", OrderByList);
            }
        }

        private string Grouping
        {
            get
            {
                if (GroupByList.Count == 0)
                    return "";
                else
                    return "GROUP BY " + string.Join(", ", GroupByList);
            }
        }

        private string Having
        {
            get
            {
                if (HavingConditions.Count == 0)
                    return "";
                else
                    return "HAVING " + string.Join(" ", HavingConditions);
            }
        }

        public IDictionary<string, object> Parameters { get; private set; }               

        public string QueryString
        {
            get { return Adapter.QueryString(Selection, Source, Conditions, Grouping, Having, Order); }
        }

        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            if (pageNumber.HasValue)
            {
                if (OrderByList.Count == 0)
                    throw new Exception("Pagination requires the ORDER BY statement to be specified");

                return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize, pageNumber.Value);
            }
            
            return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize);
        }

        internal SqlQueryBuilder(string tableName, ISqlAdapter adapter)
        {
            TableNames.Add(tableName);
            Adapter = adapter;
            Parameters = new ExpandoObject();
            CurrentParamIndex = 0;
        }

        public void BeginExpression()
        {
            WhereConditions.Add("(");
        }

        public void EndExpression()
        {
            WhereConditions.Add(")");
        }

        public void And()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" AND ");
        }

        public void Or()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" OR ");
        }

        public void Not()
        {
            WhereConditions.Add(" NOT ");
        }

        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} {1} {2}",
                Adapter.Field(tableName, fieldName),
                op,
                Adapter.Parameter(paramId));

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} LIKE {1}",
                Adapter.Field(tableName, fieldName),
                Adapter.Parameter(paramId));

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldNull(string tableName, string fieldName)
        {
            WhereConditions.Add(string.Format("{0} IS NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            WhereConditions.Add(string.Format("{0} IS NOT NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            string newCondition = string.Format("{0} {1} {2}",
            Adapter.Field(leftTableName, leftFieldName),
            op,
            Adapter.Field(rightTableName, rightFieldName));

            WhereConditions.Add(newCondition);
        }

      

        public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
        {
            var joinString = string.Format("JOIN {0} ON {1} = {2}",
                                           Adapter.Table(joinTableName),
                                           Adapter.Field(originalTableName, leftField),
                                           Adapter.Field(joinTableName, rightField));
            TableNames.Add(joinTableName);
            JoinExpressions.Add(joinString);
            SplitColumns.Add(rightField);
        }

        public void OrderBy(string tableName, string fieldName, bool desc = false)
        {
            var order = Adapter.Field(tableName, fieldName);
            if (desc)
                order += " DESC";

            OrderByList.Add(order);
        }

        public void Select(string tableName)
        {
            var selectionString = string.Format("{0}.*", Adapter.Table(tableName));
            SelectionList.Add(selectionString);
        }

        public void Select(string tableName, string fieldName, string alias = null)
        {
            SelectionList.Add(Adapter.Field(tableName, fieldName, alias));
        }

        public void Select(string tableName, string fieldName, string selectFunction, string alias)
        {
            SelectionList.Add(Adapter.SelectFunction(tableName, fieldName, selectFunction, alias));
        }

        public void GroupBy(string tableName, string fieldName)
        {
            GroupByList.Add(Adapter.Field(tableName, fieldName));
        }

        #region helpers
        private string NextParamId()
        {
            ++CurrentParamIndex;
            return PARAMETER_PREFIX + CurrentParamIndex.ToString(CultureInfo.InvariantCulture);
        }

        private void AddParameter(string key, object value)
        {
            if(!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }
        #endregion

        public void QueryByIsIn(string tableName, string fieldName, SqlLambdaBase sqlQuery)
        {
            var innerQuery = sqlQuery.QueryString;
            foreach (var param in sqlQuery.QueryParameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), innerQuery);

            WhereConditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
            {
                var paramId = NextParamId();
                AddParameter(paramId, x);
                return Adapter.Parameter(paramId);
            });

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), string.Join(",", paramIds));
            WhereConditions.Add(newCondition);
        }
    }
}
