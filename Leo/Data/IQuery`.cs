using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Leo.Data
{
    public interface IQuery<T>
    {
        IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQuery<T, TJoin> Join<TJoin>(Expression<Func<T, TJoin, bool>> on);
        IQuery<T> Where(Expression<Func<T, bool>> conditions);

        IEnumerable<T> ToArray();
    }

    public interface IQuery<T1, T2>
    {
        IQuery<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> selector);
        IQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> conditions);
        IQuery<T1, T2, TJoin> Join<TJoin>(Expression<Func<T1, TJoin, bool>> on);
        IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, TResult>> selector);
    }

    public interface IQuery<T1, T2, T3>
    {
        IQuery<TResult> Select<TResult>(Expression<Func<T1, T2, T3, TResult>> selector);
        IQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> conditions);
        IEnumerable<TResult> ToArray<TResult>(Expression<Func<T1, T2, T3, TResult>> selector);
    }

}
