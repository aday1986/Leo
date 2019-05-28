using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Leo.Data.Expressions
{
    //Core
    public partial class LambdaResolver
    {
        public LambdaResolver()
        {
            var e = Enum.GetValues(typeof(SqlPartEnum));
            foreach (SqlPartEnum item in e)
            {
                SqlPart.Add(item, new List<string>());
            }
            this.Adapter = new SqlServerSqlAdapter();
        }

        #region Resolver

        //private string Resolver(ParameterExpression exp, MemberInfo member)
        //{

        //}

        /// <summary>
        /// 常数表达式。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(ConstantExpression exp, MemberInfo member)
        {
            return AddParameter(exp.Value, member?.Name) + ",";
        }

        private string Resolver(MemberExpression exp, MemberInfo member)
        {
            string result = string.Empty;

            if (exp != null && (exp.Expression == null || exp.Expression.NodeType != ExpressionType.Parameter))//静态值
            {
                object value = GetExpressionValue(exp);
                result += AddParameter(value, member?.Name) + ",";
            }
            else
            {
                result += Field(GetTableName(exp.Member.ReflectedType), GetColumnName(exp), member?.Name) + ",";
            }
            return result;
        }

        private string Resolver(NewExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            for (int i = 0; i < exp.Members.Count; i++)
            {
                result += Resolver((dynamic)exp.Arguments[i], exp.Members[i]);
            }
            result = result.TrimEnd(',');
            return result;
        }

        private string Resolver(MethodCallExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            if (Adapter.IsFunc(exp.Method))
            {
                string parms = string.Empty;
                foreach (var arg in exp.Arguments)
                {
                    parms += Resolver((dynamic)arg, null);
                }
                result += Func(exp.Method.Name, parms.TrimEnd(','), member?.Name) + ",";

            }
            else if (exp.Method.IsStatic)
            {
                result += AddParameter(GetExpressionValue(exp), member?.Name) + ",";
            }
            return result;
        }

        private string Resolver(UnaryExpression exp, MemberInfo member)
        {
            string result = string.Empty;
          
            result +=operationDictionary[exp.NodeType]+ Resolver((dynamic)exp.Operand, member);
            return result;
        }

        private string Resolver(BinaryExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            result += "(";
            result += Resolver((dynamic)exp.Left, null).TrimEnd(',');

            if ( exp.Right is ConstantExpression && ((ConstantExpression)exp.Right).Value==null)
            {
                    switch (exp.NodeType)
                    {
                        case ExpressionType.Equal:
                            result += " IS NULL";
                            break;
                        case ExpressionType.NotEqual:
                            result += " IS NOT NULL";
                            break;
                        default:
                            throw new Exception($"不支持{exp.NodeType.ToString()}。");
                    }
            }
            else
            {
                result += operationDictionary[exp.NodeType];
                result += Resolver((dynamic)exp.Right, null).TrimEnd(',');
            }
           
            result += ")";
            return result;
        }

        #endregion

        private Dictionary<ExpressionType, string> operationDictionary
            = new Dictionary<ExpressionType, string>(){
                { ExpressionType.Equal, "="},
                { ExpressionType.NotEqual, "!="},
                { ExpressionType.GreaterThan, ">"},
                { ExpressionType.LessThan, "<"},
                { ExpressionType.GreaterThanOrEqual, ">="},
                { ExpressionType.LessThanOrEqual, "<="},
                { ExpressionType.And," AND "},
                { ExpressionType.AndAlso," AND "},
                { ExpressionType.Or," OR "},
                { ExpressionType.OrElse," OR "},
                 { ExpressionType.Not,"NOT "},
                  { ExpressionType.Negate,"-"}
            };

        private string AddParameter(object value, string alias = null)
        {
            ++CurrentParamIndex;
            string id = PARAMETER_PREFIX + CurrentParamIndex.ToString(CultureInfo.InvariantCulture);
            this.Parameters.Add(id, value);
            return Adapter.Parameter(id, alias);
        }

        private string Field(string tableName, string fieldName, string alias = null)
        {
            return Adapter.Field(tableName, fieldName, alias);
        }

        private string Func(string funcName, string parms, string alias = null)
        {
            return Adapter.Func(funcName, parms, alias);
        }

    }

    //Helper
    public partial class LambdaResolver
    {
       
        private static string GetColumnName(MemberExpression exp)
        {
            var column = exp.Member.GetCustomAttributes<ColumnAttribute>(false).FirstOrDefault();
            return column?.ColumnName ?? exp.Member.Name;
        }

        private static string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        private static string GetTableName(Type type)
        {
            var column = type.GetCustomAttributes<TableAttribute>(false).FirstOrDefault();
            return column?.TableName ?? type.Name;
        }

        private static string GetTableName(MemberExpression exp)
        {
            return GetTableName(exp.Member.DeclaringType);
        }

       

        private object GetExpressionValue(Expression exp)
        {
            if (exp == null)
                return null;
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
            var obj = callExpression.Object != null ? GetExpressionValue(callExpression.Object) : arguments.FirstOrDefault();
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

        private void AddTableName(params string[] tables)
        {
            foreach (var table in tables)
            {
                if (!SqlPart[SqlPartEnum.From].Contains(table))
                {
                    SqlPart[SqlPartEnum.From].Add(table);
                }
            }

        }
    }

    //Private
    public partial class LambdaResolver
    {
        private ISqlAdapter Adapter { get; set; }

        private const string PARAMETER_PREFIX = "p";

        private Dictionary<SqlPartEnum, List<string>> SqlPart { get; } = new Dictionary<SqlPartEnum, List<string>>();

        private int CurrentParamIndex { get; set; }

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

        private string OrderText
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

        private string HavingText
        {
            get
            {
                if (SqlPart[SqlPartEnum.Having].Count == 0)
                    return "";
                else
                    return "HAVING " + string.Join(" ", SqlPart[SqlPartEnum.Having]);
            }
        }

    }

    //Public
    public partial class LambdaResolver
    {
        #region Public

        public string QueryString
        {
            get { return Adapter.QueryString(Selection, Source, Conditions, Grouping, HavingText, OrderText); }
        }

        public IDictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

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
            //匿名类中的常数字段，不能是变量。
            AddTableName(GetTableName<T1>());
            SqlPart[SqlPartEnum.Select].Add(Resolver((dynamic)selector.Body, null));
        }

        public void Select<T1, T2>(Expression<Func<T1, T2, object>> selector)
        {
            AddTableName(new string[] { GetTableName<T1>(), GetTableName<T2>() });
            SqlPart[SqlPartEnum.Select].Add(Resolver((dynamic)selector.Body, null));
        }

        public void Select<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
        {
            AddTableName(new string[] { GetTableName<T1>(), GetTableName<T2>(), GetTableName<T3>() });
            SqlPart[SqlPartEnum.Select].Add(Resolver((dynamic)selector.Body, null));
        }

        public void Where<T1>(Expression<Func<T1, bool>> conditions)
        {
            AddTableName(GetTableName<T1>());
            SqlPart[SqlPartEnum.Where].Add(Resolver((dynamic)conditions.Body, null));
        }

        public void Where<T1, T2>(Expression<Func<T1, T2, bool>> conditions)
        {
            AddTableName(new string[] { GetTableName<T1>(), GetTableName<T2>() });
            SqlPart[SqlPartEnum.Where].Add(Resolver((dynamic)conditions.Body, null));
        }

        public void Where<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            AddTableName(new string[] { GetTableName<T1>(), GetTableName<T2>(), GetTableName<T3>() });
            SqlPart[SqlPartEnum.Where].Add(Resolver((dynamic)conditions.Body, null));
        }

        #endregion
    }
}
