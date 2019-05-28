using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Leo.Data1.Expressions.Resolver.ExpressionTree;

namespace Leo.Data1.Expressions.Resolver
{
    /// <summary>
    /// <see cref="LambdaExpression"/>解析器。
    /// </summary>
    partial class LambdaResolver
    {
        private Dictionary<ExpressionType, string> _operationDictionary
            = new Dictionary<ExpressionType, string>(){
                { ExpressionType.Equal, "="},
                { ExpressionType.NotEqual, "!="},
                { ExpressionType.GreaterThan, ">"},
                { ExpressionType.LessThan, "<"},
                { ExpressionType.GreaterThanOrEqual, ">="},
                { ExpressionType.LessThanOrEqual, "<="}};

        private SqlQueryBuilder _builder { get; set; }

        public LambdaResolver(SqlQueryBuilder builder)
        {
            _builder = builder;
        }


        #region Join
        public void Join<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            var joinExpression = GetBinaryExpression(exp.Body);
            var leftExpression = GetMemberExpression(joinExpression.Left);
            var rightExpression = GetMemberExpression(joinExpression.Right);

            Join<T1, T2>(leftExpression, rightExpression);
        }

        public void Join<T1, T2, TKey>(Expression<Func<T1, TKey>> leftExpression, Expression<Func<T1, TKey>> rightExpression)
        {
            Join<T1, T2>(GetMemberExpression(leftExpression.Body), GetMemberExpression(rightExpression.Body));
        }

        public void Join<T1, T2>(MemberExpression leftExpression, MemberExpression rightExpression)
        {
            _builder.Join(GetTableName<T1>(), GetTableName<T2>(), GetColumnName(leftExpression), GetColumnName(rightExpression));
        }
        #endregion

        #region Select

        private static Type aggFunc = typeof(AggFunc);

        public void Select<T>(Expression<Func<T, object>> exp)
        {
            Select<T>(exp.Body, null);
        }

        private void Select<T>(Expression exp, MemberInfo member)
        {
            switch (exp.NodeType)//可以在继续递归更深层。
            {
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.Constant:
                    Select<T>((dynamic)exp, member);
                    break;
                case ExpressionType.Parameter:
                    _builder.Select(GetTableName(exp.Type));
                    break;
                case ExpressionType.New:
                    var newExpression = exp as NewExpression;
                    for (int i = 0; i < newExpression.Members.Count; i++)
                    {
                        Select<T>((dynamic)newExpression.Arguments[i], newExpression.Members[i]);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid exp");
            }
        }

        private void Select<T>(MemberExpression exp, MemberInfo member)
        {
            if (exp.Expression == null && exp.Member.MemberType == MemberTypes.Property)//静态属性
            {
                var pro = exp.Member.ReflectedType.GetProperty(exp.Member.Name);
                if (pro.GetMethod.IsStatic)
                {  
                object value = pro.GetValue(null, null);
                    _builder.Select(value, member.Name);
                }
                else
                {
                    throw new Exception($"{pro.Name}没有静态Get方法。");
                }
            }
            else
                _builder.Select(GetTableName<T>(), GetColumnName(exp), member.Name);
        }


        private void Select<T>(MethodCallExpression exp, MemberInfo member)
        {
            if (exp.Method.ReflectedType == aggFunc)
            {
              
                foreach (var item in exp.Arguments)
                {
                    Select<T>((dynamic)item, null);
                }
                _builder.Select(GetTableName<T>(), GetColumnName(exp.Arguments.FirstOrDefault()), exp.Method.Name, member.Name);
            }
            else
            {
                throw new ArgumentNullException($"{exp.Method.ReflectedType.Name}.{exp.Method.Name} not find.");
            }
        }

        private void Select<T>(ConstantExpression exp, MemberInfo member)
        {
            _builder.Select(exp.Value, member.Name);
        }
        #endregion

        #region OrderGroup
        public void OrderBy<T>(Expression<Func<T, object>> exp, bool desc = false)
        {
            var fieldName = GetColumnName(GetMemberExpression(exp.Body));
            _builder.OrderBy(GetTableName<T>(), fieldName, desc);
        }

        public void GroupBy<T>(Expression<Func<T, object>> exp)
        {
            GroupBy<T>(GetMemberExpression(exp.Body));
        }

        private void GroupBy<T>(MemberExpression exp)
        {
            var fieldName = GetColumnName(GetMemberExpression(exp));
            _builder.GroupBy(GetTableName<T>(), fieldName);
        }
        #endregion

        #region Where
        public void Where<T>(Expression<Func<T, bool>> exp)
        {
            var expressionTree = ResolveQuery((dynamic)exp.Body);
            BuildSql(expressionTree);
        }

        private Node ResolveQuery(ConstantExpression constantExpression)
        {
            return new ValueNode() { Value = constantExpression.Value };
        }

        private Node ResolveQuery(UnaryExpression unaryExpression)
        {
            return new SingleOperationNode()
            {
                Operator = unaryExpression.NodeType,
                Child = ResolveQuery((dynamic)unaryExpression.Operand)
            };
        }

        private Node ResolveQuery(BinaryExpression binaryExpression)
        {
            return new OperationNode
            {
                Left = ResolveQuery((dynamic)binaryExpression.Left),
                Operator = binaryExpression.NodeType,
                Right = ResolveQuery((dynamic)binaryExpression.Right)
            };
        }

        private Node ResolveQuery(MethodCallExpression callExpression)
        {
            LikeMethod callFunction;
            if (Enum.TryParse(callExpression.Method.Name, true, out callFunction))
            {
                var member = callExpression.Object as MemberExpression;
                var fieldValue = (string)GetExpressionValue(callExpression.Arguments.First());

                return new LikeNode()
                {
                    MemberNode = new MemberNode()
                    {
                        TableName = GetTableName(member),
                        FieldName = GetColumnName(callExpression.Object)
                    },
                    Method = callFunction,
                    Value = fieldValue
                };
            }
            else
            {
                var value = ResolveMethodCall(callExpression);
                return new ValueNode() { Value = value };
            }
        }

        private Node ResolveQuery(MemberExpression memberExpression, MemberExpression rootExpression = null)
        {
            rootExpression = rootExpression ?? memberExpression;
            switch (memberExpression.Expression.NodeType)
            {
                case ExpressionType.Parameter:
                    return new MemberNode()
                    { TableName = GetTableName(rootExpression), FieldName = GetColumnName(rootExpression) };
                case ExpressionType.MemberAccess:
                    return ResolveQuery(memberExpression.Expression as MemberExpression, rootExpression);
                case ExpressionType.Call:
                case ExpressionType.Constant:
                    return new ValueNode() { Value = GetExpressionValue(rootExpression) };
                default:
                    throw new ArgumentException("Expected member exp");
            }
        }
        #endregion

     

        void BuildSql(Node node)
        {
            BuildSql((dynamic)node);
        }

        void BuildSql(LikeNode node)
        {
            if (node.Method == LikeMethod.Equals)
            {
                _builder.QueryByField(node.MemberNode.TableName, node.MemberNode.FieldName,
                    _operationDictionary[ExpressionType.Equal], node.Value);
            }
            else
            {
                string value = node.Value;
                switch (node.Method)
                {
                    case LikeMethod.StartsWith:
                        value = node.Value + "%";
                        break;
                    case LikeMethod.EndsWith:
                        value = "%" + node.Value;
                        break;
                    case LikeMethod.Contains:
                        value = "%" + node.Value + "%";
                        break;
                }
                _builder.QueryByFieldLike(node.MemberNode.TableName, node.MemberNode.FieldName, value);
            }
        }

        void BuildSql(OperationNode node)
        {
            BuildSql((dynamic)node.Left, (dynamic)node.Right, node.Operator);
        }

        void BuildSql(MemberNode memberNode)
        {
            _builder.QueryByField(memberNode.TableName, memberNode.FieldName, _operationDictionary[ExpressionType.Equal], true);
        }

        void BuildSql(SingleOperationNode node)
        {
            if (node.Operator == ExpressionType.Not)
                _builder.Not();
            BuildSql(node.Child);
        }

        void BuildSql(MemberNode memberNode, ValueNode valueNode, ExpressionType op)
        {
            if (valueNode.Value == null)
            {
                ResolveNullValue(memberNode, op);
            }
            else
            {
                _builder.QueryByField(memberNode.TableName, memberNode.FieldName, _operationDictionary[op], valueNode.Value);
            }
        }

        void BuildSql(ValueNode valueNode, MemberNode memberNode, ExpressionType op)
        {
            BuildSql(memberNode, valueNode, op);
        }

        void BuildSql(MemberNode leftMember, MemberNode rightMember, ExpressionType op)
        {
            _builder.QueryByFieldComparison(leftMember.TableName, leftMember.FieldName, _operationDictionary[op], rightMember.TableName, rightMember.FieldName);
        }

        void BuildSql(SingleOperationNode leftMember, Node rightMember, ExpressionType op)
        {
            if (leftMember.Operator == ExpressionType.Not)
                BuildSql(leftMember as Node, rightMember, op);
            else
                BuildSql((dynamic)leftMember.Child, (dynamic)rightMember, op);
        }

        void BuildSql(Node leftMember, SingleOperationNode rightMember, ExpressionType op)
        {
            BuildSql(rightMember, leftMember, op);
        }

        void BuildSql(Node leftNode, Node rightNode, ExpressionType op)
        {
            _builder.BeginExpression();
            BuildSql((dynamic)leftNode);
            ResolveOperation(op);
            BuildSql((dynamic)rightNode);
            _builder.EndExpression();
        }

        void ResolveNullValue(MemberNode memberNode, ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Equal:
                    _builder.QueryByFieldNull(memberNode.TableName, memberNode.FieldName);
                    break;
                case ExpressionType.NotEqual:
                    _builder.QueryByFieldNotNull(memberNode.TableName, memberNode.FieldName);
                    break;
            }
        }

        void ResolveSingleOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.Not:
                    _builder.Not();
                    break;
            }
        }

        void ResolveOperation(ExpressionType op)
        {
            switch (op)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _builder.And();
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _builder.Or();
                    break;
                default:
                    throw new ArgumentException(string.Format("Unrecognized binary exp operation '{0}'", op.ToString()));
            }
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

        #region Fail functions

        private void ResolveQuery(Expression exp)
        {
            throw new ArgumentException(string.Format("The provided exp '{0}' is currently not supported", exp.NodeType));
        }

        #endregion


        //public void QueryByIsIn<T>(Expression<Func<T, object>> exp, SqlLambdaBase sqlQuery)
        //{
        //    var fieldName = GetColumnName(exp);
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        //}

        //public void QueryByIsIn<T>(Expression<Func<T, object>> exp, IEnumerable<object> values)
        //{
        //    var fieldName = GetColumnName(exp);
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        //}

        //public void QueryByNotIn<T>(Expression<Func<T, object>> exp, SqlLambdaBase sqlQuery)
        //{
        //    var fieldName = GetColumnName(exp);
        //    _builder.Not();
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        //}

        //public void QueryByNotIn<T>(Expression<Func<T, object>> exp, IEnumerable<object> values)
        //{
        //    var fieldName = GetColumnName(exp);
        //    _builder.Not();
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        //}
    }
}
