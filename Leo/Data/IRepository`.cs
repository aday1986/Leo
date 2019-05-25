using Leo.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data
{
   
    /// <summary>
    /// 表示一组数据库CURD的基础功能。
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 添加一个实体。
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// 添加一组实体。
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        IEnumerable<T> Query(IEnumerable<Condition> conditions);

        IEnumerable<T> QueryPage(IEnumerable<Condition> conditions, int index, int pagesize);

        T Get(params object[] keyvalues);

        int SaveChanges();

    }

}
