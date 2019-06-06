using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace Leo.Data
{

    internal class QueryContext
    {
        public QueryContext(LambdaResolver resolver, IDbCommand command)
        {
            Resolver = resolver;
            Command = command;
        }

        public LambdaResolver Resolver { get; }
        private IDbCommand Command { get; }

        public IDbCommand GetCommand()
        {
            Command.CommandText = Resolver.QueryString();
            foreach (var item in Resolver.Parameters)
            {
                var p = Command.CreateParameter();
                p.ParameterName = item.Key;
                p.Value = item.Value;
                Command.Parameters.Add(p);
            }
            return Command;
        }
    }

    public class Query
    {
        internal readonly LambdaResolver resolver;
        internal readonly QueryContext context;
        internal Query(QueryContext context)
        {
            this.resolver = context.Resolver;
            this.context = context;
        }

        internal Query(IDbProvider provider)
        {
            this.resolver = provider.CreateResolver();
            var cnn = provider.CreateConnection();
            var cmd = provider.CreateCommand();
            cmd.Connection = cnn;
            this.context = new QueryContext(resolver, cmd);
        }

        public override string ToString()
        {
            return resolver.ToString();
        }
    }

    public class Query<T1> : Query
    {
        public Query(IDbProvider provider) : base(provider)
        {

        }
        internal Query(QueryContext context) : base(context)
        { }

        public T1 Get(params object[] keyvalues)
        {
            if (keyvalues == null || keyvalues.Length == 0)
                return default;
            var type = typeof(T1);
            if (ColumnAttribute.TryGetKeyColumns<T1>(out Dictionary<PropertyInfo, ColumnAttribute> keys))
            {
                BinaryExpression binary = null;
                int i = 0;
                foreach (var key in keys)
                {
                    var left = Expression.MakeMemberAccess(Expression.Parameter(type), key.Key as MemberInfo);
                    var right = Expression.Constant(keyvalues[i]);
                    if (binary == null)
                    {
                        binary = Expression.Equal(left, right);
                    }
                    else
                    {
                        binary = Expression.AndAlso(binary, Expression.Equal(left, right));
                    }
                    i++;
                }
                var lambda = Expression.Lambda(binary, Expression.Parameter(typeof(T1)));
                return this.Where((Expression<Func<T1, bool>>)lambda).ToArray().FirstOrDefault();
            }
            else
            {
                throw new Exception($"{type.Name}不包含任何主键字段。");
            }
        }

        public Query<T1, TJoin> Join<TJoin>(Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, TJoin>(context);
        }

        public Query<T1, TJoin> Join<TJoin>(Query<TJoin> query, Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join(query, on);
            return new Query<T1, TJoin>(context);
        }

        public Query<TResult> Select<TResult>(Expression<Func<T1, TResult>> selector)
        {
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public IEnumerable<T1> ToArray()
        {
            IEnumerable<T1> results = null;
            results = SqlCore.Query<T1>(this, this.context.GetCommand());
            resolver.Init();
            return results;
        }

        public Query<T1> Where(Expression<Func<T1, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public Query<T1> Order(Expression<Func<T1, object>> selector)
        {
            resolver.Order(selector);
            return this;
        }

        public Query<T1> Group(Expression<Func<T1, object>> selector)
        {
            resolver.Group(selector);
            return this;
        }

        public Query<T1> Having(Expression<Func<T1, bool>> conditions)
        {
            resolver.Having(conditions);
            return this;
        }

        public Query<T1> Skip(int count)
        {
            resolver.Skip = count;
            return this;
        }

        public Query<T1> Take(int count)
        {
            resolver.Size = count;
            return this;
        }
    }

    public class Query<T1, T2> : Query
    {
        internal Query(QueryContext context) : base(context)
        {
        }

        public Query<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public Query<T1, T2> Where(Expression<Func<T1, T2, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public Query<T1, T2, TJoin> Join<TJoin>(Expression<Func<T1, T2, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, T2, TJoin>(context);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            resolver.Select(selector);
            var results = SqlCore.Query<TResult>(this, this.context.GetCommand());
            return results;

        }

        public Query<T1, T2> Order(Expression<Func<T1, T2, object>> selector)
        {
            resolver.Order(selector);
            return this;
        }

        public Query<T1, T2> Group(Expression<Func<T1, T2, object>> selector)
        {
            resolver.Group(selector);
            return this;
        }

        public Query<T1, T2> Having(Expression<Func<T1, T2, bool>> conditions)
        {
            resolver.Having(conditions);
            return this;
        }

        public Query<T1, T2> Skip(int count)
        {
            resolver.Skip = count;
            return this;
        }

        public Query<T1, T2> Take(int count)
        {
            resolver.Size = count;
            return this;
        }


    }

    public class Query<T1, T2, T3> : Query
    {
        internal Query(QueryContext context) : base(context)
        {
        }

        public Query<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public Query<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            resolver.Select(selector);
            var results = SqlCore.Query<TResult>(this, this.context.GetCommand());
            return results;
        }

        public Query<T1, T2, T3> Order(Expression<Func<T1, T2, T3, object>> selector)
        {
            resolver.Order(selector);
            return this;
        }

        public Query<T1, T2, T3> Group(Expression<Func<T1, T2, T3, object>> selector)
        {
            resolver.Group(selector);
            return this;
        }

        public Query<T1, T2, T3> Having(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            resolver.Having(conditions);
            return this;
        }

        public Query<T1, T2, T3> Skip(int count)
        {
            resolver.Skip = count;
            return this;
        }

        public Query<T1, T2, T3> Take(int count)
        {
            resolver.Size = count;
            return this;
        }


    }

}
