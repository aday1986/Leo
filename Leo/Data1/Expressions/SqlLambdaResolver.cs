using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Leo.Data1.Expressions
{
    public interface ISqlLambdaResolver
    {
        void Select<T1>(Expression<Func<T1, object>> selector);
        void Select<T1, T2>(Expression<Func<T1, T2, object>> selector);
        void Select<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector);
        void Join<T1, T2>(Expression<Func<T1, T2, bool>> on);
        void Join<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> on);
        void Where<T1>(Expression<Func<T1, bool>> conditions);
        void Where<T1,T2>(Expression<Func<T1,T2, bool>> conditions);
        void Where<T1,T2,T3>(Expression<Func<T1,T2,T3, bool>> conditions);
        void Order<T1>(Expression<Func<T1, object>> selector);
        void Order<T1,T2>(Expression<Func<T1,T2, object>> selector);
        void Order<T1,T2,T3>(Expression<Func<T1,T2,T3, object>> selector);
        void Group<T1>(Expression<Func<T1, object>> selector);
        void Group<T1, T2>(Expression<Func<T1, T2, object>> selector);
        void Group<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector);
        void Having<T1>(Expression<Func<T1, object>> selector);
        void Having<T1, T2>(Expression<Func<T1, T2, object>> selector);
        void Having<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector);
    }


    public class SqlLambdaResolver : ISqlLambdaResolver
    {
        #region SqlBulider
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

        private string OrderText
        {
            get
            {
                if (OrderByList.Count == 0)
                    return "";
                else
                    return "ORDER BY " + string.Join(", ", OrderByList);
            }
        }

        private string GroupText
        {
            get
            {
                if (GroupByList.Count == 0)
                    return "";
                else
                    return "GROUP BY " + string.Join(", ", GroupByList);
            }
        }

        private string HavingText
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
            get { return Adapter.QueryString(Selection, Source, Conditions,OrderText, GroupText, HavingText); }
        }

        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            if (pageNumber.HasValue)
            {
                if (OrderByList.Count == 0)
                    throw new Exception("Pagination requires the ORDER BY statement to be specified");

                return Adapter.QueryStringPage(Source, Selection, Conditions, OrderText, pageSize, pageNumber.Value);
            }

            return Adapter.QueryStringPage(Source, Selection, Conditions, OrderText, pageSize);
        }

        internal SqlLambdaResolver(string tableName, ISqlAdapter adapter)
        {
            TableNames.Add(tableName);
            Adapter = adapter;
            Parameters = new ExpandoObject();
            CurrentParamIndex = 0;
        }
        #endregion

        #region Tree
        private void Resolver(Expression exp, MemberInfo member)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Constant:
                case ExpressionType.Call:
                   Resolver((dynamic)exp, member);
                    break;
                case ExpressionType.Parameter:
                    var selectionString = string.Format("{0}.*", Adapter.Table(GetTableName(exp.Type)));
                    SelectionList.Add(selectionString);
                    break;
                case ExpressionType.New:
                    var newExpression = exp as NewExpression;
                    for (int i = 0; i < newExpression.Members.Count; i++)
                    {
                        Resolver((dynamic)newExpression.Arguments[i], newExpression.Members[i]);
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion


        public void Group<T1>(Expression<Func<T1, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Group<T1, T2>(Expression<Func<T1, T2, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Group<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Having<T1>(Expression<Func<T1, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Having<T1, T2>(Expression<Func<T1, T2, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Having<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Join<T1, T2>(Expression<Func<T1, T2, bool>> on)
        {
            throw new NotImplementedException();
        }

        public void Join<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> on)
        {
            throw new NotImplementedException();
        }

        public void Order<T1>(Expression<Func<T1, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Order<T1, T2>(Expression<Func<T1, T2, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Order<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Select<T1>(Expression<Func<T1, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Select<T1, T2>(Expression<Func<T1, T2, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Select<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
        {
            throw new NotImplementedException();
        }

        public void Where<T1>(Expression<Func<T1, bool>> conditions)
        {
            throw new NotImplementedException();
        }

        public void Where<T1, T2>(Expression<Func<T1, T2, bool>> conditions)
        {
            throw new NotImplementedException();
        }

        public void Where<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        public static string GetColumnName<T>(Expression<Func<T, object>> selector)
        {
            return GetColumnName(GetMemberExpression(selector.Body));
        }

        public static string GetColumnName(Expression exp)
        {
            var member = GetMemberExpression(exp);
            var column = member.Member.GetCustomAttributes(false).OfType<ColumnAttribute>().FirstOrDefault();
            if (column != null)
                return column.ColumnName;
            else
                return member.Member.Name;
        }

        public static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        public static string GetTableName(Type type)
        {
            var column = type.GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (column != null)
                return column.TableName;
            else
                return type.Name;
        }

        private static string GetTableName(MemberExpression exp)
        {
            return GetTableName(exp.Member.DeclaringType);
        }

        private static BinaryExpression GetBinaryExpression(Expression exp)
        {
            if (exp is BinaryExpression)
                return exp as BinaryExpression;

            throw new ArgumentException("Binary exp expected");
        }

        private static MemberExpression GetMemberExpression(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return exp as MemberExpression;
                case ExpressionType.Convert:
                    return GetMemberExpression((exp as UnaryExpression).Operand);
            }

            throw new ArgumentException("Member exp expected");
        }


        private object GetExpressionValue(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Constant:
                    return (exp as ConstantExpression).Value;
                case ExpressionType.Call:
                    return ResolveMethodCall(exp as MethodCallExpression);
                case ExpressionType.MemberAccess:
                    var memberExpr = (exp as MemberExpression);
                    var obj = GetExpressionValue(memberExpr.Expression);
                    return ResolveValue((dynamic)memberExpr.Member, obj);
                default:
                    throw new ArgumentException("Expected constant exp");
            }
        }

        private object ResolveMethodCall(MethodCallExpression callExpression)
        {
            var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
            var obj = callExpression.Object != null ? GetExpressionValue(callExpression.Object) : arguments.First();

            return callExpression.Method.Invoke(obj, arguments);
        }

        private object ResolveValue(PropertyInfo property, object obj)
        {
            return property.GetValue(obj, null);
        }

        private object ResolveValue(FieldInfo field, object obj)
        {
            return field.GetValue(obj);
        }

        #endregion
    }

}
