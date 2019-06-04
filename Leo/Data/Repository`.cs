using Leo.Data;
using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Leo.Data
{
    public class Repository<T> : IRepository<T>
    {
        private readonly IUnitOfWork unit;
        private readonly LambdaResolver resolver;

        public Repository(IUnitOfWork unit, IDbProvider dbProvider)
        {
            this.unit = unit;
            this.resolver = new LambdaResolver(dbProvider);
        }

        public void Add(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
              sql  = resolver.InsertSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            unit.Execute(cmd => cmd.ExecuteNonQuery(this, sql, paramList));
        }

        public T Get(params object[] keyvalues)
        {
            var type = typeof(T);
            if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<PropertyInfo, ColumnAttribute> keys))
            {
                BinaryExpression binary=null;
                int i = 0;
                foreach (var key in keys)
                {
                    var left = Expression.MakeMemberAccess(Expression.Parameter(type), key.Key as MemberInfo);
                    var right = Expression.Constant(keyvalues[i]);
                    if (binary==null)
                    {
                        binary = Expression.Equal(left, right);
                    }
                    else
                    {
                        binary = Expression.AndAlso(binary, Expression.Equal(left, right));
                    }
                    i++;
                }
                var lambda = Expression.Lambda(binary, Expression.Parameter(typeof(T)));
              return  Query().Where((Expression<Func<T, bool>>)lambda).ToArray().FirstOrDefault();   
            }
            else
            {
                throw new Exception($"{type.Name}不包含任何主键字段。");
            }
        }

        public Query<T> Query()
        {
            resolver.Init();
            return unit.Query<T>(c=>new Query<T>(new QueryContext(resolver,c)));
        }

        public void Remove(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
                sql = resolver.DeleteSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            unit.Execute(cmd => cmd.ExecuteNonQuery(this, sql, paramList));
        }

        public void Remove(Expression<Func<T, bool>> conditions)
        {
            unit.Execute(c => c.ExecuteNonQuery(this, resolver.DeleteSql(conditions), resolver.Parameters));
        }

        public int SaveChanges()
        {
           return unit.SaveChanges();
        }

        public void Update(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
                sql = resolver.UpdateSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            unit.Execute(cmd => cmd.ExecuteNonQuery(this, sql, paramList));
        }

        public void Update(T model, Expression<Func<T, bool>> conditions)
        {
            unit.Execute(c => c.ExecuteNonQuery(this, resolver.UpdateSql(model,conditions), resolver.Parameters));
        }
    }

}
