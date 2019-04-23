using System;
using System.Collections.Generic;
using System.Text;
using Leo.ThirdParty.Dapper;

namespace Leo.Data.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISqlBulider sqlBulider;

        public DapperRepository(IUnitOfWork unitOfWork, ISqlBulider sqlBulider)
        {
            this.unitOfWork = unitOfWork;
            this.sqlBulider = sqlBulider;
        }

        public void Add(T entity)
        {
            unitOfWork.Execute(
                tran => tran.Connection.Execute(sqlBulider.GetInsertSql<T>(), entity, tran)
                );
        }

        public void AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public T Get(params object[] keyvalues)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query(IEnumerable<Condition> conditions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> QueryPage(IEnumerable<Condition> conditions, int index, int pagesize)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 999;
            // throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
