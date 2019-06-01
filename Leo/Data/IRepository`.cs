using Leo.Data;
using Leo.Data.Expressions;
using Leo.DI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Leo.Data
{


    public static class Comman
    {
        public static int ExecuteNonQuery(this IDbCommand command, string sql, Dictionary<string, object> parms)
        {
            command.CommandText = sql;
            foreach (var item in parms)
            {
                var p = command.CreateParameter();
                p.ParameterName = item.Key;
                p.Value = item.Value;
                command.Parameters.Add(p);
            }
            return command.ExecuteNonQuery();
        }
    }

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

    public class Repository<T> : IRepository<T>
    {
        private readonly IUnitOfWork unit;
        private readonly LambdaResolver resolver;

        public Repository(IUnitOfWork unit, LambdaResolver resolver)
        {
            this.unit = unit;
            this.resolver = resolver;
        }

        public void Add(params T[] entities)
        {
            foreach (var entity in entities)
            {
                var sql = resolver.InsertSql<T>(entity);
                unit.Execute(cmd => cmd.ExecuteNonQuery(sql, resolver.Parameters));
            }
        }

        public T Get(params object[] keyvalues)
        {
            throw new NotImplementedException();
        }

        public IQuery<T> Query()
        {
            return unit.Query<T>(c=>new Query<T>(new QueryContext(resolver,c)));
        }

        public void Remove(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<T, bool>> conditions)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
           return unit.SaveChanges();
        }

        public void Update(params T[] entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T model, Expression<Func<T, bool>> conditions)
        {
            throw new NotImplementedException();
        }
    }

}
