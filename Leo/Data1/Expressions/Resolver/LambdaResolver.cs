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
        public void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression)
        {
            var joinExpression = GetBinaryExpression(expression.Body);
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

        public void Select<T>(Expression<Func<T, object>> expression)
        {
            Select<T>(expression.Body, null);
        }

        private void Select<T>(Expression expression, MemberInfo member)
        {
            switch (expression.NodeType)//可以在继续递归更深层。
            {
                case ExpressionType.Parameter:
                    _builder.Select(GetTableName(expression.Type));
                    break;
                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    Select<T>(GetMemberExpression(expression), member);
                    break;
                case ExpressionType.Call:
                    Select<T>(expression as MethodCallExpression, member);
                    break;
                case ExpressionType.New:
                    var newExpression = expression as NewExpression;
                    for (int i = 0; i < newExpression.Members.Count; i++)
                    {
                        Select<T>(newExpression.Arguments[i], newExpression.Members[i]);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select<T>(MemberExpression expression, MemberInfo member)
        {
            if (expression.Type.IsClass && expression.Type != typeof(String))
                _builder.Select(GetTableName(expression.Type));
            else
                _builder.Select(GetTableName<T>(), GetColumnName(expression), member.Name);
        }

       
        private void Select<T>(MethodCallExpression expression, MemberInfo member)
        {
            if (expression.Method.ReflectedType == aggFunc)
            {
                _builder.Select(GetTableName<T>(), GetColumnName(expression.Arguments.FirstOrDefault()), expression.Method.Name, member.Name);
            }
            else
            {
                throw new ArgumentNullException($"{expression.Method.ReflectedType.Name}.{expression.Method.Name} not find.");
            }

        }
        #endregion

        #region OrderGroup
        public void OrderBy<T>(Expression<Func<T, object>> expression, bool desc = false)
        {
            var fieldName = GetColumnName(GetMemberExpression(expression.Body));
            _builder.OrderBy(GetTableName<T>(), fieldName, desc);
        }

        public void GroupBy<T>(Expression<Func<T, object>> expression)
        {
            GroupBy<T>(GetMemberExpression(expression.Body));
        }

        private void GroupBy<T>(MemberExpression expression)
        {
            var fieldName = GetColumnName(GetMemberExpression(expression));
            _builder.GroupBy(GetTableName<T>(), fieldName);
        }
        #endregion

        #region Where
        public void Where<T>(Expression<Func<T, bool>> expression)
        {
            var expressionTree = ResolveQuery((dynamic)expression.Body);
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
                    throw new ArgumentException("Expected member expression");
            }
        }
        #endregion

        #region Helpers

        public static string GetColumnName<T>(Expression<Func<T, object>> selector)
        {
            return GetColumnName(GetMemberExpression(selector.Body));
        }

        public static string GetColumnName(Expression expression)
        {
            var member = GetMemberExpression(expression);
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

        private static string GetTableName(MemberExpression expression)
        {
            return GetTableName(expression.Member.DeclaringType);
        }

        private static BinaryExpression GetBinaryExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return expression as BinaryExpression;

            throw new ArgumentException("Binary expression expected");
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return expression as MemberExpression;
                case ExpressionType.Convert:
                    return GetMemberExpression((expression as UnaryExpression).Operand);
            }

            throw new ArgumentException("Member expression expected");
        }


        private object GetExpressionValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return (expression as ConstantExpression).Value;
                case ExpressionType.Call:
                    return ResolveMethodCall(expression as MethodCallExpression);
                case ExpressionType.MemberAccess:
                    var memberExpr = (expression as MemberExpression);
                    var obj = GetExpressionValue(memberExpr.Expression);
                    return ResolveValue((dynamic)memberExpr.Member, obj);
                default:
                    throw new ArgumentException("Expected constant expression");
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

        private void ResolveQuery(Expression expression)
        {
            throw new ArgumentException(string.Format("The provided expression '{0}' is currently not supported", expression.NodeType));
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
                    throw new ArgumentException(string.Format("Unrecognized binary expression operation '{0}'", op.ToString()));
            }
        }


        //public void QueryByIsIn<T>(Expression<Func<T, object>> expression, SqlLambdaBase sqlQuery)
        //{
        //    var fieldName = GetColumnName(expression);
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        //}

        //public void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        //{
        //    var fieldName = GetColumnName(expression);
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        //}

        //public void QueryByNotIn<T>(Expression<Func<T, object>> expression, SqlLambdaBase sqlQuery)
        //{
        //    var fieldName = GetColumnName(expression);
        //    _builder.Not();
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, sqlQuery);
        //}

        //public void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        //{
        //    var fieldName = GetColumnName(expression);
        //    _builder.Not();
        //    _builder.QueryByIsIn(GetTableName<T>(), fieldName, values);
        //}
    }
}
