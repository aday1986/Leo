using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data.Dapper
{
    public class DapperRepositoryBuilder : IRepositoryBuilder
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISqlBulider sqlBulider;

        public DapperRepositoryBuilder(IUnitOfWork unitOfWork, ISqlBulider sqlBulider)
        {
            this.unitOfWork = unitOfWork;
            this.sqlBulider = sqlBulider;
        }

        public IRepository<T> Create<T>() where T : class
        {
            return new DapperRepository<T>(unitOfWork, sqlBulider);
        }
    }
}
