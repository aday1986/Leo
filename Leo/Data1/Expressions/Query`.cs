using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Leo.Data.Expressions
{
    public class Query<T> : IQuery<T>
    {
        public readonly LambdaResolver resolver;

        internal Query(LambdaResolver resolver)
        {
            this.resolver = resolver;
        }

        public Query()
        {
            resolver = new LambdaResolver();
        }


        public IQuery<T, TJoin> Join<TJoin>(Expression<Func<T, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T, TJoin>(resolver);
        }

        public IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T));
            resolver.Select(selector);
            return new Query<TResult>(resolver);
        }

        public IEnumerable<T> ToArray()
        {
           SqlConnection conn = new System.Data.SqlClient.SqlConnection("Server=192.168.45.249;DataBase=nzerp" +
                    ";Max Pool Size=1024;uid=sa;pwd=fordoo2001");
            SqlCommand command = new SqlCommand(resolver.QueryString(), conn);
            foreach (var item in resolver.Parameters)
            {
                command.Parameters.Add(new SqlParameter(item.Key, item.Value));
            }
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table=new DataTable();
            adapter.Fill(table);
            return null;
        }

        public IQuery<T> Where(Expression<Func<T, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }
    }

    public class Query<T1, T2> : IQuery<T1, T2>
    {
        protected readonly LambdaResolver resolver;

        internal Query(LambdaResolver resolver)
        {
            this.resolver = resolver;
        }



        public IQuery<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T1), typeof(T2));
            resolver.Select(selector);
            return new Query<TResult>(resolver);
        }

        public IQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }

        public IQuery<T1, T2, TJoin> Join<TJoin>(Expression<Func<T1, TJoin, bool>> on)
        {
            resolver.Join(on);
            return new Query<T1, T2, TJoin>(resolver);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, TResult>> selector)
        {
            throw new NotImplementedException();
        }
    }

    public class Query<T1, T2, T3> : IQuery<T1, T2, T3>
    {

        public readonly LambdaResolver resolver;

        internal Query(LambdaResolver resolver)
        {
            this.resolver = resolver;
        }



        public IQuery<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            //resolver.SignAni(typeof(TResult), typeof(T1), typeof(T2), typeof(T3));
            resolver.Select(selector);
            return new Query<TResult>(resolver);
        }

        public IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, T3, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> conditions)
        {
            resolver.Where(conditions);
            return this;
        }
    }

}
