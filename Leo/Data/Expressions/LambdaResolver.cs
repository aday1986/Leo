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
    //Core
    public partial class LambdaResolver
    {
        private static Dictionary<ExpressionType, string> operationDictionary
         = new Dictionary<ExpressionType, string>(){
                { ExpressionType.Equal, "="},
                { ExpressionType.NotEqual, "<>"},
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

        private const string TABLE = "t";

        private const string QUERY = "q";

        private int CurrentParamIndex { get; set; }

        private int SourceIndex { get; set; }

        private ISqlAdapter Adapter { get; set; }

        private Dictionary<SqlPartEnum, List<string>> SqlPart { get; set; }

        private Dictionary<object, SourceInfo> SourceInfos { get; set; }

       private Dictionary<MemberInfo, ColumnInfo> ColumnInfos { get; set; }

        internal Dictionary<SourceInfo, List<JoinInfo>> JoinInfos { get; set; }

        /// <summary>
        /// 匿名类属性映射。
        /// </summary>
        private Dictionary<MemberInfo, MemberInfo> AnonymousMemberMapper { get; set; }

        /// <summary>
        /// 匿名类映射。
        /// </summary>
        private Dictionary<Type, List<Type>> AnonymousTypeMapper { get; set; }

        internal void Init()
        {
            this.Parameters = new Dictionary<string, object>();
            this.SourceInfos = new Dictionary<object, SourceInfo>();
            this.ColumnInfos = new Dictionary<MemberInfo, ColumnInfo>();
            this.JoinInfos = new Dictionary<SourceInfo, List<JoinInfo>>();
            this.AnonymousMemberMapper = new Dictionary<MemberInfo, MemberInfo>();
            this.AnonymousTypeMapper = new Dictionary<Type, List<Type>>();
            this.CurrentParamIndex = 0;
            this.SourceIndex = 0;
            this.Size = null;
            this.Skip = null;
            this.SqlPart = new Dictionary<SqlPartEnum, List<string>>();
            var e = Enum.GetValues(typeof(SqlPartEnum));
            foreach (SqlPartEnum item in e)
            {
                SqlPart.Add(item, new List<string>());
            }
        }



        private string Source
        {
            get
            {
                string result = string.Empty;
                foreach (var sourceInfo in SourceInfos.Where(s => !s.Value.IsQuery))
                {
                    result += $"{Adapter.Source(sourceInfo.Value)}";
                    if (JoinInfos.TryGetValue(sourceInfo.Value, out List<JoinInfo> values))
                    {
                        foreach (var value in values)
                        {
                            string right = Adapter.Source(value.Right);
                            result += $" {value.JoinEnum.ToString().ToUpper()} JOIN {right} ON {value.OnText}";
                        }
                    }
                    result += ",";
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
                var source = AddSource(exp.Type);
                return Adapter.GetSourceName(source) + ".*,";
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
                if (value is Array)//数组解析为p1,p2,p3,
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
                if (member != null && IsAnonymousType(member.DeclaringType))
                {
                    AnonymousMemberMapper[member] = exp.Member;
                }
                var realMember = GetRealMemberInfo(exp.Member);//递归获取。
                if (!ColumnInfos.ContainsKey(realMember))
                {
                    ColumnInfos[realMember] = new ColumnInfo()
                    {
                        Alias = member?.Name,
                        Source = SourceInfos[realMember.ReflectedType],
                        ColumnText = ColumnAttribute.GetColumnName(realMember)
                    };
                }
                result += Adapter.Field(ColumnInfos[realMember], !(member == null)) + ",";
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

    //Public
    public partial class LambdaResolver
    {
        public int? Size { get; set; }

        public int? Skip { get; set; }

        public Dictionary<string, object> Parameters { get; private set; }

        public LambdaResolver(ISqlAdapter adapter)
        {
            Init();
            Adapter = adapter;
        }

        public string QueryString()
        {
            string result = Adapter.QuerySql(Selection, Source, Conditions, Grouping, HavingText, OrderText
                , Size, Skip);
            return result;
        }

        public string InsertSql<T>(T entity)
        {
            Init();
            var modelType = typeof(T);
            string tableName = TableAttribute.GetTableName(modelType);
            AddSource(modelType);
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
            AddSource(modelType);
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
                    BinaryExpression binary = null;
                    foreach (var key in keys)
                    {
                        var left = Expression.MakeMemberAccess(Expression.Parameter(modelType), key.Key as MemberInfo);
                        var right = Expression.Constant(key.Key.GetValue(entity));
                        if (binary == null)
                        {
                            binary = Expression.Equal(left, right);
                        }
                        else
                        {
                            binary = Expression.AndAlso(binary, Expression.Equal(left, right));
                        }
                    }
                    conditions = Expression.Lambda(binary, Expression.Parameter(typeof(T))) as Expression<Func<T, bool>>;

                }
                else
                {
                    throw new Exception($"实体{modelType.Name}没有可被匹配的主键，无法更新。");
                }
            }
            Where(conditions);
            return Adapter.UpdateSql(fields, Source, Conditions);
        }

        public string DeleteSql<T>(T entity)
        {
            var type = typeof(T);
            if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<PropertyInfo, ColumnAttribute> keys))
            {
                BinaryExpression binary = null;
                foreach (var key in keys)
                {
                    var left = Expression.MakeMemberAccess(Expression.Parameter(type), key.Key as MemberInfo);
                    var right = Expression.Constant(key.Key.GetValue(entity));
                    if (binary == null)
                    {
                        binary = Expression.Equal(left, right);
                    }
                    else
                    {
                        binary = Expression.AndAlso(binary, Expression.Equal(left, right));
                    }
                }
                var lambda = Expression.Lambda(binary, Expression.Parameter(typeof(T))) as Expression<Func<T, bool>>;
                return DeleteSql(lambda);
            }
            else
            {
                throw new Exception($"{type.Name}不包含任何主键字段。");
            }
        }

        public string DeleteSql<T>(Expression<Func<T, bool>> conditions)
        {
            Init();
            var modelType = typeof(T);
            string tableName = TableAttribute.GetTableName(modelType);
            AddSource(modelType);
            SqlPart[SqlPartEnum.Where].Add(Resolver(conditions));
            return Adapter.DeleteSql(Source, Conditions);
        }

        public void Join<T, TJoin>(Expression<Func<T, TJoin, bool>> on, JoinEnum joinEnum = JoinEnum.INNER)
      => JoinType<T, TJoin>(on, joinEnum);

        public void Join<T1, T2, TJoin>(Expression<Func<T1, T2, TJoin, bool>> on, JoinEnum joinEnum = JoinEnum.INNER)
     => JoinType<T1, TJoin>(on, joinEnum);

        public void Join<T, TJoin>(Query<TJoin> query, Expression<Func<T, TJoin, bool>> on, JoinEnum joinEnum = JoinEnum.INNER)
       => JoinQuery<T, TJoin>(query, on, joinEnum);

        public void Join<T1, T2, TJoin>(Query<TJoin> query, Expression<Func<T1, T2, TJoin, bool>> on, JoinEnum joinEnum = JoinEnum.INNER)
     => JoinQuery<T1, TJoin>(query, on, joinEnum);

        public void Select<T1, TResult>(Expression<Func<T1, TResult>> selector)
        {
            var typeResult = typeof(TResult);
            if (!AnonymousTypeMapper.ContainsKey(typeResult))
                AnonymousTypeMapper[typeResult] = new List<Type>();
            AnonymousTypeMapper[typeResult].AddRange(new Type[] { typeof(T1) });
            Resolver(selector, SqlPartEnum.Select);
        }

        public void Select<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            var typeResult = typeof(TResult);
            if (!AnonymousTypeMapper.ContainsKey(typeResult))
                AnonymousTypeMapper[typeResult] = new List<Type>();
            AnonymousTypeMapper[typeResult].AddRange(new Type[] { typeof(T1), typeof(T2) });
            Resolver(selector, SqlPartEnum.Select);
        }

        public void Select<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            var typeResult = typeof(TResult);
            if (!AnonymousTypeMapper.ContainsKey(typeResult))
                AnonymousTypeMapper[typeResult] = new List<Type>();
            AnonymousTypeMapper[typeResult].AddRange(new Type[] { typeof(T1), typeof(T2), typeof(T3) });
            Resolver(selector, SqlPartEnum.Select);
        }

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

        public void Having<T1>(Expression<Func<T1, bool>> conditions)
              => Resolver(conditions, SqlPartEnum.Having);

        public void Having<T1, T2>(Expression<Func<T1, T2, bool>> conditions)
              => Resolver(conditions, SqlPartEnum.Having);

        public void Having<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> conditions)
          => Resolver(conditions, SqlPartEnum.Having);

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
        private void JoinType<T, TJoin>(Expression on, JoinEnum joinEnum = JoinEnum.INNER)
        {
            string onText = Resolver(on, null);
            var right = SourceInfos[typeof(TJoin)];
            SourceInfo left = null;
            if (IsAnonymousType(typeof(T)))
                left = SourceInfos[AnonymousTypeMapper[typeof(T)].FirstOrDefault()];
            else
                left = SourceInfos[typeof(T)];
            SourceInfos.Remove(right);
            JoinInfo joinInfo = new JoinInfo
            {
                JoinEnum = joinEnum,
                OnText = onText,
                Left = left,
                Right = right
            };
            if (!JoinInfos.ContainsKey(joinInfo.Left))
            {
                JoinInfos[joinInfo.Left] = new List<JoinInfo>();
            }
            JoinInfos[joinInfo.Left].Add(joinInfo);
        }

        private void JoinQuery<T, TJoin>(Query<TJoin> query, Expression on, JoinEnum joinEnum = JoinEnum.INNER)
        {
            if (query.resolver == this) throw new Exception("不能是自己的子查询。");
            var right = AddSource(query);
            string onText = Resolver(on, null);
            SourceInfo left = null;
            if (IsAnonymousType(typeof(T)))
                left = SourceInfos[AnonymousTypeMapper[typeof(T)].FirstOrDefault()];
            else
                left = SourceInfos[typeof(T)];
            SourceInfos.Remove(right);
            JoinInfo joinInfo = new JoinInfo
            {
                JoinEnum = joinEnum,
                OnText = onText,
                Left = left,
                Right = right
            };
            if (!JoinInfos.ContainsKey(joinInfo.Left))
            {
                JoinInfos[joinInfo.Left] = new List<JoinInfo>();
            }
            JoinInfos[joinInfo.Left].Add(joinInfo);
        }

        /// <summary>
        /// 递归获取源类型信息。
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private MemberInfo GetRealMemberInfo(MemberInfo member)
        {
            if (IsAnonymousType(member.DeclaringType))
            {
                member = AnonymousMemberMapper[member];
                return GetRealMemberInfo(member);
            }
            else
            {
                return member;
            }
        }

        /// <summary>
        /// 判断是否匿名类。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsAnonymousType(Type type)
        {
            return !type.IsVisible;
        }

        /// <summary>
        /// 解析<see cref="Expression"/>为常数值。
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 添加且返回结果，已存在对象则只返回。可以是类型，或者<see cref="Query"/>子查询。
        /// 如果是子查询，会将里面的Params添加到当前查询。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
      private SourceInfo AddSource(object source)
        {
            if (SourceInfos.ContainsKey(source))
                return SourceInfos[source];
            SourceInfo sourceInfo = new SourceInfo();
           
            if (source is Type)
            {
                var type = source as Type;
                sourceInfo.Alias = $"{TABLE}{SourceIndex++}";
                sourceInfo.IsQuery = false;
                sourceInfo.SourceText = TableAttribute.GetTableName(type);
                sourceInfo.SourceType = type;
            }
            else if (source is Query)
            {
                var query = source as Query;
                sourceInfo.Alias = $"{QUERY}{SourceIndex++}";
                sourceInfo.IsQuery = true;
                sourceInfo.SourceText = query.resolver.QueryString();
                sourceInfo.SourceType = typeof(Query);
                sourceInfo.Parameters = query.resolver.Parameters;
                foreach (var item in sourceInfo.Parameters)
                {
                    string newKey = AddParameter(item.Value);
                    sourceInfo.SourceText = sourceInfo.SourceText.Replace(item.Key, newKey);
                }
                foreach (var item in query.resolver.AnonymousMemberMapper)
                {
                    this.AnonymousMemberMapper[item.Key] = item.Value;
                }
                foreach (var item in query.resolver.AnonymousTypeMapper)
                {
                    this.AnonymousTypeMapper[item.Key] = item.Value;
                }
                foreach (var item in query.resolver.ColumnInfos)
                {
                    item.Value.Source = sourceInfo;
                    this.ColumnInfos[item.Key] = item.Value;
                }
            }
            SourceInfos[source] = sourceInfo;
            return sourceInfo;
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

            string id = $"{ PARAM}{CurrentParamIndex++}";
            this.Parameters.Add(Adapter.Parameter(id), value);
            return Adapter.Parameter(id, alias);
        }
    }

}
