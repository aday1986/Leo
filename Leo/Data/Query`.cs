using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

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
        public string ToSql()
        {
            return resolver.QueryString();
        }
    }

    public class Query<T> :Query
    {



        internal Query(QueryContext context):base(context)
        { }
       


        public Query<T, TJoin> Join<TJoin>(Expression<Func<T, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T, TJoin>(context);
        }

        public Query<T, TJoin> Join<TJoin>(Query<TJoin> query, Expression<Func<T, TJoin, bool>> on)
        {
            resolver.Join(query.resolver, query, on);
            return new Query<T, TJoin>(context);
        }

        public Query<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T));
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public IEnumerable<T> ToArray()
        {
            IEnumerable<T> results = null;
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                results = reader.ToList<T>();
            }
            return results;
        }

      

        public Query<T> Where(Expression<Func<T, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public override string ToString()
        {
            return resolver.ToString();
        }
    }

    public class Query<T1, T2> 
    {
        private readonly LambdaResolver resolver;
        private readonly QueryContext context;

        internal Query(QueryContext context)
        {
            this.resolver = context.Resolver;
            this.context = context;
        }



        public Query<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T1), typeof(T2));
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public Query<T1, T2> Where(Expression<Func<T1, T2, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public Query<T1, T2, TJoin> Join<TJoin>(Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, T2, TJoin>(context);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            IEnumerable<TResult> results = null;
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                results = reader.ToList<TResult>();
            }
            return results;
        }
    }

    public class Query<T1, T2, T3> 
    {

        private readonly LambdaResolver resolver;
        private readonly QueryContext context;

        internal Query(QueryContext context)
        {
            this.resolver = context.Resolver;
            this.context = context;
        }

        public Query<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T1), typeof(T2), typeof(T3));
            resolver.Select(selector);
            return new Query<TResult>(context);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            IEnumerable<TResult> results = null;
            using (var reader = SqlCore.ExecuteReader(this, this.context.GetCommand()))
            {
                results = reader.ToList<TResult>();
            }
            return results;
        }

        public Query<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }
    }

}
