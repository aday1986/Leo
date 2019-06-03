using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Leo.Data.Expressions
{
    public class SourceInfo
    {
        public Type Type { get; set; }

        public string SourceText { get; set; }

        public string Alias { get; set; }

        public bool IsQuery { get; set; }
    }

    public class JoinInfo
    {
      public  SourceInfo Left { get; set; }
       public SourceInfo Right { get; set; }
        public string OnText { get; set; }
        public JoinEnum JoinEnum { get; set; }

    }

    public class ColumnInfo
    {
        public SourceInfo Source { get; set; }

        public string ColumnText { get; set; }

        public string Alias { get; set; }
    }
    //Core
    public partial class LambdaResolver
    {

        private List<SourceInfo> sourceInfos { get; set; }

        private List<ColumnInfo> ColumnInfos { get; set; }

        private Dictionary<SourceInfo, JoinInfo> JoinInfos { get; set; }


        /// <summary>
        /// 解析，并添加指定<see cref="SqlPartEnum"/>的表达式，外部调用递归函数集的入口。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="sqlPart"></param>
        private void Resolver(Expression exp, SqlPartEnum sqlPart)
        {
            SqlPart[sqlPart].Add(Resolver(exp));
        }

        /// <summary>
        /// 通过dynamic分配调用函数。
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private string Resolver(Expression exp, MemberInfo member = null)
        {
            return Resolver((dynamic)exp, member);
        }

        /// <summary>
        /// 解析Lambda表达式。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(LambdaExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            foreach (var item in exp.Parameters)
            {
                Resolver(item, null);
            }
            result = Resolver(exp.Body, member).TrimEnd(',');
            return result;
        }

        /// <summary>
        /// 解析表达式参数。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(ParameterExpression exp, MemberInfo member)
        {
            if (IsAnonymousType(exp.Type))
            {
                return "*";
            }
            else
            {
                AddTableName(exp.Type);
                return Adapter.Table(TableAttribute.GetTableName(exp.Type)) + ".*,";
            }
        }

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

        /// <summary>
        /// 解析属性字段参数。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(MemberExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            if (exp != null && (exp.Expression == null || exp.Expression.NodeType != ExpressionType.Parameter))//静态值
            {
                object value = GetExpressionValue(exp);
                if (value is Array)//数组
                {
                    var arr = value as Array;
                    foreach (var item in arr)
                    {
                        result += AddParameter(item) + ",";
                    }
                }
                else
                {
                    result += AddParameter(value, member?.Name) + ",";
                }
            }
            else
            {
                if (member!=null && IsAnonymousType(member.DeclaringType))//添加匿名类映射。
                {
                    AnonymousTypeMapper.Add(member,exp.Member);
                }
                  if (IsAnonymousType(exp.Member.DeclaringType))
                {
                    result += Adapter.Field(TableAttribute.GetTableName(AnonymousTypeMapper[exp.Member].DeclaringType),ColumnAttribute.GetColumnName(AnonymousTypeMapper[exp.Member]), member?.Name) + ",";
                }
                else
                {
                    result += Adapter.Field(TableAttribute.GetTableName(exp.Member.ReflectedType),ColumnAttribute.GetColumnName(exp.Member), member?.Name) + ",";
                }
            }
            return result;
        }

        /// <summary>
        /// 解析新构建的匿名类。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(NewExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            for (int i = 0; i < exp.Members.Count; i++)
            {
                result += Resolver(exp.Arguments[i], exp.Members[i]);
            }
            return result;
        }

        /// <summary>
        /// 解析新构建的非匿名类。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(MemberInitExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            foreach (MemberAssignment item in exp.Bindings)
            {
                result += Resolver(item.Expression, item.Member);
            }
            return result;
        }

        /// <summary>
        /// 解析方法函数。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(MethodCallExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            if (Adapter.IsFunc(exp.Method))//系统函数，解析为对应Sql语句。
            {
                //判断是否为扩展方法，如like,in。
                if (exp.Method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                {
                    string parms = string.Empty;
                    for (int i = 1; i < exp.Arguments.Count; i++)
                    {
                        parms += Resolver(exp.Arguments[i]);
                    }
                    result += $"{Resolver(exp.Arguments[0]).TrimEnd(',')} " +
                           $"{Adapter.Func(exp.Method.Name, parms.TrimEnd(','))}";
                }
                else
                {
                    string parms = string.Empty;
                    foreach (var arg in exp.Arguments)
                    {
                        parms += Resolver(arg);
                    }
                    result += Adapter.Func(exp.Method.Name, parms.TrimEnd(','), member?.Name) + ",";
                }
            }
            else//非系统函数，直接获取值，并添加为参数。
            {
                result += AddParameter(GetExpressionValue(exp), member?.Name) + ",";
            }
            return result;
        }

        /// <summary>
        /// 解析新建的数组对象。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(NewArrayExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            foreach (var item in exp.Expressions)
            {
                result += Resolver(item);
            }
            return result.TrimEnd(',');
        }

        /// <summary>
        /// 解析一元表达式。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(UnaryExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                    result += Resolver(exp.Operand);
                    break;
                default:
                    result += operationDictionary[exp.NodeType] + Resolver(exp.Operand, member);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 解析二元表达式。
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private string Resolver(BinaryExpression exp, MemberInfo member)
        {
            string result = string.Empty;
            string left = Resolver(exp.Left).TrimEnd(',');
            if (exp.Left is BinaryExpression && HasChildBinary((BinaryExpression)exp.Left))
                result += $"({left})";
            else
                result += left;

            if (exp.Right is ConstantExpression && ((ConstantExpression)exp.Right).Value == null)
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
                string right = Resolver(exp.Right).TrimEnd(',');
                if (exp.Right is BinaryExpression && HasChildBinary((BinaryExpression)exp.Right))
                    result += $"({right})";
                else
                    result += right;
            }
            return result;
        }
    }

    //Private
    public partial class LambdaResolver
    {
        private static Dictionary<ExpressionType, string> operationDictionary
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

        private const string PARAM = "p";

        private int CurrentParamIndex { get; set; }

        private ISqlAdapter Adapter { get; set; }

        private Dictionary<SqlPartEnum, List<string>> SqlPart { get; set; }

        /// <summary>
        /// 匿名类属性映射。
        /// </summary>
        private Dictionary<MemberInfo, MemberInfo> AnonymousTypeMapper { get; } = new Dictionary<MemberInfo, MemberInfo>();

        private Dictionary<string, List<Tuple<string, string>>> OnJoins { get; set; }

        private string Source
        {
            get
            {
                string result = string.Empty;
                foreach (var onJoin in OnJoins)
                {
                    SqlPart[SqlPartEnum.From].Remove(onJoin.Key);
                    result += $"{Adapter.Table(onJoin.Key)} ";
                    foreach (var item in onJoin.Value)
                    {
                        SqlPart[SqlPartEnum.From].Remove(item.Item1);
                        result += $"{item.Item2} ";
                    }
                    result += ",";
                }
                foreach (var table in SqlPart[SqlPartEnum.From])
                {
                    result += $"{Adapter.Table(table)},";
                }
                if (string.IsNullOrEmpty(result))
                {
                    throw new ArgumentNullException(nameof(Source));
                }
                return result.TrimEnd(',');
            }
        }

        private string Selection
        {
            get
            {
                if (SqlPart[SqlPartEnum.Select].Any())
                    return string.Join(",", SqlPart[SqlPartEnum.Select]);
                else
                    return "*";
            }
        }

        private string Conditions
        {
            get
            {
                string result = string.Empty;
                if (SqlPart[SqlPartEnum.Where].Any())
                {
                    if (SqlPart[SqlPartEnum.Where].Count == 1)//单条件集合。
                    {
                        result = $"WHERE {SqlPart[SqlPartEnum.Where].FirstOrDefault()}";
                    }
                    else//多条件集合。
                    {
                        result += "WHERE ";
                        foreach (var item in SqlPart[SqlPartEnum.Where])
                        {
                            if (item.Count(c => c == ' ') >= 3)//判断是否加括号，不严谨。
                            {
                                result += $"({item}) AND ";
                            }
                            else
                            {
                                result += $"{item} AND ";
                            }
                        }
                        result = result.Remove(result.Length - 5, 5);
                    }
                }
                return result;
            }
        }

        private string OrderText
        {
            get
            {
                if (SqlPart[SqlPartEnum.Order].Any())
                    return "ORDER BY " + string.Join(",", SqlPart[SqlPartEnum.Order]);
                else
                    return string.Empty;
            }
        }

        private string Grouping
        {
            get
            {
                if (SqlPart[SqlPartEnum.Group].Any())
                    return "GROUP BY " + string.Join(",", SqlPart[SqlPartEnum.Group]);
                else
                    return string.Empty;
            }
        }

        private string HavingText
        {
            get
            {
                if (SqlPart[SqlPartEnum.Having].Any())
                    return "HAVING " + string.Join(" ", SqlPart[SqlPartEnum.Having]);
                else
                    return string.Empty;
            }
        }
    }

    //Public
    public partial class LambdaResolver
    {
        public Dictionary<string, object> Parameters { get; private set; }

        public LambdaResolver(ISqlAdapter adapter)
        {
            Init();
            Adapter = adapter;
        }

        public void Init()
        {
            Parameters = new Dictionary<string, object>();
            this.SqlPart = new Dictionary<SqlPartEnum, List<string>>();
            this.OnJoins = new Dictionary<string, List<Tuple<string, string>>>();
            this.CurrentParamIndex = 0;
            var e = Enum.GetValues(typeof(SqlPartEnum));
            foreach (SqlPartEnum item in e)
            {
                SqlPart.Add(item, new List<string>());
            }
        }

        public string QueryString()
        {
            string result = Adapter.QuerySql(Selection, Source, Conditions, Grouping, HavingText, OrderText);
            return result;
        }

        public string InsertSql<T>(T entity)
        {
            Init();
            var modelType = typeof(T);
            string tableName = TableAttribute.GetTableName(modelType);
            AddTableName(modelType);
            var pros = modelType.GetProperties();
            foreach (var pro in pros)
            {
                var att = pro.GetCustomAttribute<ColumnAttribute>();
                if (att != null && (att.IsIdentity))
                    continue;
                this.Parameters.Add(ColumnAttribute.GetColumnName(pro), pro.GetValue(entity));
            }
            var fields = Parameters.Select(p => p.Key).ToArray();
            return Adapter.InsertSql(fields, Source);
        }

        public string UpdateSql<T>(T entity, Expression<Func<T, bool>> conditions = null)
        {
            Init();
            var modelType = typeof(T);
            string tableName = TableAttribute.GetTableName(modelType);
            AddTableName(modelType);
            var pros = modelType.GetProperties();
            foreach (var pro in pros)
            {
                var att = pro.GetCustomAttribute<ColumnAttribute>();
                if (att != null && (att.IsPrimaryKey || att.IsIdentity || att.NoUpdate))
                    continue;
                this.Parameters.Add(ColumnAttribute.GetColumnName(pro), pro.GetValue(entity));
            }
            var fields = Parameters.Select(p => p.Key).ToArray();

            if (conditions == null)//条件
            {
                if (modelType.TryGetKeyColumns(out Dictionary<PropertyInfo, ColumnAttribute> keys))
                {
                    string strConditions = string.Empty;
                    foreach (var key in keys)
                    {
                        string columnNmae = key.Value.ColumnName ?? key.Key.Name;
                        strConditions += $"{Adapter.Field(tableName, columnNmae)}={Adapter.Parameter(columnNmae)} AND ";//条件
                    }
                    SqlPart[SqlPartEnum.Where].Add(strConditions.Remove(strConditions.Length - 5, 5));
                }
                else
                {
                    throw new Exception($"实体{modelType.Name}没有可被匹配的主键，无法更新。");
                }
            }
            else
            {
                SqlPart[SqlPartEnum.Where].Add(Resolver(conditions));
            }
            return Adapter.UpdateSql(fields, Source, Conditions);
        }

        public string DeleteSql<T>(Expression<Func<T, bool>> conditions)
        {
            Init();
            var modelType = typeof(T);
            string tableName = TableAttribute.GetTableName(modelType);
            AddTableName(modelType);
            SqlPart[SqlPartEnum.Where].Add(Resolver(conditions));
            return Adapter.DeleteSql(Source, Conditions);
        }

        public void Join<T, TJoin>(Expression<Func<T, TJoin, bool>> on, JoinEnum joinEnum = JoinEnum.INNER)
        {
            string table1 = IsAnonymousType(typeof(T)) ? OnJoins.Last().Key : TableAttribute.GetTableName<T>();
            string table2 = TableAttribute.GetTableName<TJoin>();
            if (!OnJoins.ContainsKey(table1))
                OnJoins[table1] = new List<Tuple<string, string>>();
            OnJoins[table1].Add(
                new Tuple<string, string>(
                    table2
                , $"{joinEnum.ToString()} JOIN {Adapter.Table(TableAttribute.GetTableName<TJoin>())} ON {Resolver((dynamic)on.Body, null)}"));
        }

        public void Select<T1, TResult>(Expression<Func<T1, TResult>> selector)
         => Resolver(selector, SqlPartEnum.Select);

        public void Select<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selector)
         => Resolver(selector, SqlPartEnum.Select);

        public void Select<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
            => Resolver(selector, SqlPartEnum.Select);

        public void Order<T1>(Expression<Func<T1, object>> selector)
            => Resolver(selector, SqlPartEnum.Order);

        public void Order<T1, T2>(Expression<Func<T1, T2, object>> selector)
            => Resolver(selector, SqlPartEnum.Order);

        public void Order<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
            => Resolver(selector, SqlPartEnum.Order);

        public void Group<T1>(Expression<Func<T1, object>> selector)
       => Resolver(selector, SqlPartEnum.Group);

        public void Group<T1, T2>(Expression<Func<T1, T2, object>> selector)
          => Resolver(selector, SqlPartEnum.Group);

        public void Group<T1, T2, T3>(Expression<Func<T1, T2, T3, object>> selector)
         => Resolver(selector, SqlPartEnum.Group);

        public void Having<T1>(Expression<Func<T1, bool>> selector)
              => Resolver(selector, SqlPartEnum.Having);

        public void Having<T1, T2>(Expression<Func<T1, T2, bool>> selector)
              => Resolver(selector, SqlPartEnum.Having);

        public void Having<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> selector)
          => Resolver(selector, SqlPartEnum.Having);

        public void Where<T1>(Expression<Func<T1, bool>> conditions)
       => Resolver(conditions, SqlPartEnum.Where);

        public void Where<T1, T2>(Expression<Func<T1, T2, bool>> conditions)
        => Resolver(conditions, SqlPartEnum.Where);

        public void Where<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> conditions)
        => Resolver(conditions, SqlPartEnum.Where);

        public override string ToString()
        {
            return this.QueryString();
        }
    }

    //Helper
    public partial class LambdaResolver
    {
        /// <summary>
        /// 判断是否匿名类。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsAnonymousType(Type type)
        {
            return !type.IsVisible;
        }

       

      

        private object GetExpressionValue(Expression exp)
        {
            if (exp == null)
                return null;
            switch (exp.NodeType)
            {
                case ExpressionType.Constant://常数。
                    return (exp as ConstantExpression).Value;
                case ExpressionType.Call://静态方法。
                    var callExp = (exp as MethodCallExpression);
                    var arguments = callExp.Arguments.Select(GetExpressionValue).ToArray();
                    var instance = callExp.Object != null ? GetExpressionValue(callExp.Object) : arguments.FirstOrDefault();
                    return callExp.Method.Invoke(instance, arguments);
                case ExpressionType.MemberAccess://静态属性或字段。
                    var memberExp = (exp as MemberExpression);
                    var parentObj = GetExpressionValue(memberExp.Expression);
                    switch (memberExp.Member.MemberType)
                    {
                        case MemberTypes.Property:
                            return (memberExp.Member as PropertyInfo).GetValue(parentObj, null);
                        case MemberTypes.Field:
                            return (memberExp.Member as FieldInfo).GetValue(parentObj);
                    }
                    break;      
            }
            throw new ArgumentException($"{exp.NodeType}类型无法被编译成静态值。");
        }

       internal void AddTableName(params Type[] types)
        {
            foreach (var type in types)
            {
                if (!IsAnonymousType(type))
                {
                    string table = TableAttribute.GetTableName(type);
                    if (!SqlPart[SqlPartEnum.From].Contains(table))
                    {
                        SqlPart[SqlPartEnum.From].Add(table);
                    }
                }
              
            }
        }

        /// <summary>
        /// 判断是否有子二元计算节点。
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private bool HasChildBinary(BinaryExpression exp)
        {
            return exp.Right is BinaryExpression || exp.Left is BinaryExpression;
        }

        private string AddParameter(object value, string alias = null)
        {
            ++CurrentParamIndex;
            string id = PARAM + CurrentParamIndex.ToString(CultureInfo.InvariantCulture);
            this.Parameters.Add(id, value);
            return Adapter.Parameter(id, alias);
        }
    }

}
