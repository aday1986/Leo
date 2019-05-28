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

    public enum SqlPartEnum
    {
        Select,
        From,
        Join,
        Where,
        Order,
        Group,
        Having

    }

    /// <summary>
    /// 实现整个SQL的构建逻辑。
    /// </summary>
    public partial class SqlQueryBuilder
    {
        internal ISqlAdapter Adapter { get; set; }

        private const string PARAMETER_PREFIX = "Param";

        public Dictionary<SqlPartEnum, List<string>> SqlPart { get; } = new Dictionary<SqlPartEnum, List<string>>();

        public List<string> SplitColumns { get; } = new List<string>();

        public int CurrentParamIndex { get; private set; }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", SqlPart[SqlPartEnum.Join]);
                return string.Format("{0} {1}", Adapter.Table(SqlPart[SqlPartEnum.From].First()), joinExpression);
            }
        }

        private string Selection
        {
            get
            {
                if (SqlPart[SqlPartEnum.Select].Count == 0)
                    return string.Format("{0}.*", Adapter.Table(SqlPart[SqlPartEnum.From].First()));
                else
                    return string.Join(", ", SqlPart[SqlPartEnum.Select]);
            }
        }

        private string Conditions
        {
            get
            {
                if (SqlPart[SqlPartEnum.Where].Count == 0)
                    return "";
                else
                    return "WHERE " + string.Join("", SqlPart[SqlPartEnum.Where]);
            }
        }

        private string Order
        {
            get
            {
                if (SqlPart[SqlPartEnum.Order].Count == 0)
                    return "";
                else
                    return "ORDER BY " + string.Join(", ", SqlPart[SqlPartEnum.Order]);
            }
        }

        private string Grouping
        {
            get
            {
                if (SqlPart[SqlPartEnum.Group].Count == 0)
                    return "";
                else
                    return "GROUP BY " + string.Join(", ", SqlPart[SqlPartEnum.Group]);
            }
        }

        private string Having
        {
            get
            {
                if (SqlPart[SqlPartEnum.Having].Count == 0)
                    return "";
                else
                    return "HAVING " + string.Join(" ", SqlPart[SqlPartEnum.Having]);
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
                if (SqlPart[SqlPartEnum.Order].Count == 0)
                    throw new Exception("Pagination requires the ORDER BY statement to be specified");

                return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize, pageNumber.Value);
            }

            return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize);
        }

        internal SqlQueryBuilder(string tableName, ISqlAdapter adapter)
        {
            var e = Enum.GetValues(typeof(SqlPartEnum));
           
            foreach (SqlPartEnum item in e)
            {
                SqlPart.Add(item, new List<string>());
            }
            SqlPart[SqlPartEnum.From].Add(tableName);
            Adapter = adapter;
            Parameters = new ExpandoObject();
            CurrentParamIndex = 0;
        }

        public void BeginExpression()
        {
            SqlPart[SqlPartEnum.Where].Add("(");
        }

        public void EndExpression()
        {
            SqlPart[SqlPartEnum.Where].Add(")");
        }

        public void And()
        {
            if (SqlPart[SqlPartEnum.Where].Count > 0)
                SqlPart[SqlPartEnum.Where].Add(" AND ");
        }

        public void Or()
        {
            if (SqlPart[SqlPartEnum.Where].Count > 0)
                SqlPart[SqlPartEnum.Where].Add(" OR ");
        }

        public void Not()
        {
            SqlPart[SqlPartEnum.Where].Add(" NOT ");
        }

        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} {1} {2}",
                Adapter.Field(tableName, fieldName),
                op,
                Adapter.Parameter(paramId));

            SqlPart[SqlPartEnum.Where].Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} LIKE {1}",
                Adapter.Field(tableName, fieldName),
                Adapter.Parameter(paramId));

            SqlPart[SqlPartEnum.Where].Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldNull(string tableName, string fieldName)
        {
            SqlPart[SqlPartEnum.Where].Add(string.Format("{0} IS NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            SqlPart[SqlPartEnum.Where].Add(string.Format("{0} IS NOT NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            string newCondition = string.Format("{0} {1} {2}",
            Adapter.Field(leftTableName, leftFieldName),
            op,
            Adapter.Field(rightTableName, rightFieldName));

            SqlPart[SqlPartEnum.Where].Add(newCondition);
        }



        public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
        {
            var joinString = string.Format("JOIN {0} ON {1} = {2}",
                                           Adapter.Table(joinTableName),
                                           Adapter.Field(originalTableName, leftField),
                                           Adapter.Field(joinTableName, rightField));
            SqlPart[SqlPartEnum.From].Add(joinTableName);
            SqlPart[SqlPartEnum.Join].Add(joinString);
            SplitColumns.Add(rightField);
        }

        public void OrderBy(string tableName, string fieldName, bool desc = false)
        {
            var order = Adapter.Field(tableName, fieldName);
            if (desc)
                order += " DESC";

            SqlPart[SqlPartEnum.Order].Add(order);
        }

        public void Select(string tableName)
        {
            var selectionString = string.Format("{0}.*", Adapter.Table(tableName));
            SqlPart[SqlPartEnum.Select].Add(selectionString);
        }

        /// <summary>
        /// 常数字段。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="alias"></param>
        public void Select(object value, string alias)
        {
            var paramId = NextParamId();
            AddParameter(paramId, value);
            var selectionString = $"{Adapter.Parameter(paramId)} AS {alias}";
            SqlPart[SqlPartEnum.Select].Add(selectionString);
        }

        public void Select(string tableName, string fieldName, string alias = null)
        {
            SqlPart[SqlPartEnum.Select].Add(Adapter.Field(tableName, fieldName, alias));
        }

        /// <summary>
        /// 包含方法的字段。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="selectFunction"></param>
        /// <param name="alias"></param>
        public void Select(string tableName, string fieldName, string selectFunction, string alias)
        {
            SqlPart[SqlPartEnum.Select].Add(Adapter.SelectFunction(tableName, fieldName, selectFunction, alias));
        }

        public void GroupBy(string tableName, string fieldName)
        {
            SqlPart[SqlPartEnum.Group].Add(Adapter.Field(tableName, fieldName));
        }

        #region helpers
        private string NextParamId()
        {
            ++CurrentParamIndex;
            return PARAMETER_PREFIX + CurrentParamIndex.ToString(CultureInfo.InvariantCulture);
        }

        private void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
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

            SqlPart[SqlPartEnum.Where].Add(newCondition);
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
            SqlPart[SqlPartEnum.Where].Add(newCondition);
        }
    }
}
