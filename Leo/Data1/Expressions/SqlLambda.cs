using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Leo.Data1.Expressions.Resolver;
using Leo.Data1.Expressions.ValueObjects;

namespace Leo.Data1.Expressions
{
    /// <summary>
    /// The single most important LambdaSqlBuilder class. Encapsulates the whole SQL building and lambda expression resolving logic. 
    /// Serves as a proxy to the underlying SQL builder and the lambda expression resolver. It should be used to continually build the SQL query
    /// and then request the QueryString as well as the QueryParameters at the end.
    /// </summary>
    /// <typeparam name="T">Entity type required for lambda expressions as well as for proper resolution of the table name and the column names</typeparam>
    public class SqlLambda<T> : SqlLambdaBase
    {
        public SqlLambda()
        {
            _builder = new SqlQueryBuilder(LambdaResolver.GetTableName<T>(), _defaultAdapter);
            _resolver = new LambdaResolver(_builder);
        }

        public SqlLambda(Expression<Func<T, bool>> expression) : this()
        {
            Where(expression);
        }

        internal SqlLambda(SqlQueryBuilder builder, LambdaResolver resolver)
        {
            _builder = builder;
            _resolver = resolver;
        }

        public SqlLambda<T> Where(Expression<Func<T, bool>> expression)
        {
            return And(expression);
        }

        public SqlLambda<T> And(Expression<Func<T, bool>> expression)
        {
            _builder.And();
            _resolver.Where(expression);
            return this;
        }

        public SqlLambda<T> Or(Expression<Func<T, bool>> expression)
        {
            _builder.Or();
            _resolver.Where(expression);
            return this;
        }

     

        public SqlLambda<T> OrderBy(Expression<Func<T, object>> expression)
        {
            _resolver.OrderBy(expression);
            return this;
        }

        public SqlLambda<T> OrderByDescending(Expression<Func<T, object>> expression)
        {
            _resolver.OrderBy(expression, true);
            return this;
        }

        public SqlLambda<T> Select(params Expression<Func<T, object>>[] expressions)
        {
            foreach (var expression in expressions)
                _resolver.Select(expression);
            return this;
        }

        public SqlLambda<TResult> Join<T2, TKey, TResult>(SqlLambda<T2> joinQuery,  
            Expression<Func<T, TKey>> primaryKeySelector, 
            Expression<Func<T, TKey>> foreignKeySelector,
            Func<T, T2, TResult> selection)
        {
            var query = new SqlLambda<TResult>(_builder, _resolver);
            _resolver.Join<T, T2, TKey>(primaryKeySelector, foreignKeySelector);
            return query;
        }

        public SqlLambda<T2> Join<T2>(Expression<Func<T, T2, bool>> expression)
        {
            var joinQuery = new SqlLambda<T2>(_builder, _resolver);
            _resolver.Join(expression);
            return joinQuery;
        }

        public SqlLambda<T> GroupBy(Expression<Func<T, object>> expression)
        {
            _resolver.GroupBy(expression);
            return this;
        }


        //public SqlLambda<T> WhereIsIn(Expression<Func<T, object>> expression, SqlLambdaBase sqlQuery)
        //{
        //    _builder.And();
        //    _resolver.QueryByIsIn(expression, sqlQuery);
        //    return this;
        //}

        //public SqlLambda<T> WhereIsIn(Expression<Func<T, object>> expression, IEnumerable<object> values)
        //{
        //    _builder.And();
        //    _resolver.QueryByIsIn(expression, values);
        //    return this;
        //}

        //public SqlLambda<T> WhereNotIn(Expression<Func<T, object>> expression, SqlLambdaBase sqlQuery)
        //{
        //    _builder.And();
        //    _resolver.QueryByNotIn(expression, sqlQuery);
        //    return this;
        //}

        //public SqlLambda<T> WhereNotIn(Expression<Func<T, object>> expression, IEnumerable<object> values)
        //{
        //    _builder.And();
        //    _resolver.QueryByNotIn(expression, values);
        //    return this;
        //}
    }
}
