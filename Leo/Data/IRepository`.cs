﻿using Leo.Data.Expressions;
using Leo.DI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Leo.Data
{

    /// <summary>
    /// 表示一组数据库CURD的基础功能。
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// 添加一个实体。
        /// </summary>
        /// <param name="entity"></param>
        void Add(params T[] entities);

        void Remove(params T[] entities);

        void Remove(Expression<Func<T, bool>> conditions);

        void Update(params T[] entities);

        void Update(T model, Expression<Func<T, bool>> conditions);

        IQuery<T> Query();

        T Get(params object[] keyvalues);

        int SaveChanges();

    }

}
