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

    public class QueryContext
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

        public override string ToString()
        {
            return resolver.ToString();
        }
    }

    public class Query<T1> :Query
    {
        internal Query(QueryContext context):base(context)
        { }

        public Query<T1, TJoin> Join<TJoin>(Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, TJoin>(context);
        }

        public Query<T1, TJoin> Join<TJoin>(Query<TJoin> query, Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join( query, on);
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
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                results = reader.ToList<T1>();
            }
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

    public class Query<T1, T2> :Query
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

        public Query<T1, T2, TJoin> Join<TJoin>(Expression<Func<T1,T2, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, T2, TJoin>(context);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            resolver.Select(selector); 
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                IEnumerable<TResult> results = null;
                results = reader.ToList<TResult>();
                return results;
            }
        }

        public Query<T1,T2> Order(Expression<Func<T1,T2, object>> selector)
        {
            resolver.Order(selector);
            return this;
        }

        public Query<T1,T2> Group(Expression<Func<T1,T2, object>> selector)
        {
            resolver.Group(selector);
            return this;
        }

        public Query<T1,T2> Having(Expression<Func<T1,T2, bool>> conditions)
        {
            resolver.Having(conditions);
            return this;
        }

        public Query<T1,T2> Skip(int count)
        {
            resolver.Skip = count;
            return this;
        }

        public Query<T1,T2> Take(int count)
        {
            resolver.Size = count;
            return this;
        }


    }

    public class Query<T1, T2, T3> :Query
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
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                IEnumerable<TResult> results = null;
                results = reader.ToList<TResult>();
                return results;
            }
        }

        public Query<T1, T2,T3> Order(Expression<Func<T1, T2,T3, object>> selector)
        {
            resolver.Order(selector);
            return this;
        }

        public Query<T1, T2,T3> Group(Expression<Func<T1, T2,T3, object>> selector)
        {
            resolver.Group(selector);
            return this;
        }

        public Query<T1, T2,T3> Having(Expression<Func<T1, T2,T3, bool>> conditions)
        {
            resolver.Having(conditions);
            return this;
        }

        public Query<T1, T2,T3> Skip(int count)
        {
            resolver.Skip = count;
            return this;
        }

        public Query<T1, T2,T3> Take(int count)
        {
            resolver.Size = count;
            return this;
        }


    }

}
