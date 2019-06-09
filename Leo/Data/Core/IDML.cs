using System;
using System.Linq.Expressions;

namespace Leo.Data
{
    public interface IDML
    {

        void Add<T>(params T[] entities);

        void Remove<T>(params T[] entities);

        void Remove<T>(Expression<Func<T, bool>> conditions);

        void Update<T>(params T[] entities);

        void Update<T>(T model, Expression<Func<T, bool>> conditions);

        int SaveChanges();
    }

}
