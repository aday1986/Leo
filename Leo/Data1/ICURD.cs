using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data1
{
    public interface IQuery
    {
        bool Any();
        string ToSql();
    }

    public interface IQuery<T>:IQuery
    {
      
        IQuery<T> Where(Func<T,bool> func);
        IQuery<T> OrderBy<TKey>(Func<T,TKey> func,bool desc=false);
        IQuery<T> GroupBy<TKey>(Func<T, TKey> func);
        IQuery<T> Skip(int count);
        IQuery<T> Take(int count);

        IQuery<T, TJoin> Join<TJoin>(Func<T, TJoin, bool> on);

        /// <summary>
        /// 可以使用聚合函数等。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="mapping"></param>
        /// <returns></returns>
        IEnumerable<TResult> ToList<TResult>(Func<T, TResult> mapping );
        IEnumerable<T> ToList();
    }

    public interface IQuery<T1, T2>
    {
        IQuery<T1,T2> Where(Func<T1, T2, bool> func);
        IQuery<T1, T2> OrderBy<TKey>(Func<T1, T2, TKey> func, bool desc = false);
        IQuery<T1, T2> GroupBy<TKey>(Func<T1, T2, TKey> func);
        IQuery<T1, T2> Skip(int count);
        IQuery<T1, T2> Take(int count);

        IQuery<T1, T2, TJoin> Join<TJoin>(Func<T1, T2, TJoin, bool> on);

        /// <summary>
        /// 可以使用聚合函数等。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="mapping"></param>
        /// <returns></returns>
        IEnumerable<TResult> ToList<TResult>(Func<T1,T2, TResult> mapping);
    }

    public interface IQuery<T1, T2,T3>
    {
        IQuery<T1, T2, T3> Where(Func<T1, T2, T3, bool> func);
        IQuery<T1, T2, T3> OrderBy<TKey>(Func<T1, T2, T3, TKey> func, bool desc = false);
        IQuery<T1, T2, T3> GroupBy<TKey>(Func<T1, T2, T3, TKey> func);
        IQuery<T1, T2, T3> Skip(int count);
        IQuery<T1, T2, T3> Take(int count);

        /// <summary>
        /// 可以使用聚合函数等。
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="mapping"></param>
        /// <returns></returns>
        IEnumerable<TResult> ToList<TResult>(Func<T1, T2, T3, TResult> mapping);
    }

    public interface IDML
    {
        void Add<T>(params T[] entities);
        void Remove<T>(params T[] entities);
        void Update<T>(params T[] entities);
        int SaveChanges();
    }

    public interface IDDL
    {
        void CreateDataBase();
        void DropDataBase();
        void CreateTable<T>();
        void DropTable(string name);
        void AlterTable<T>(string name);
    }

    public interface IExpressionParser
    {
      
    }
}
