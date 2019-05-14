using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Leo.Data
{
    public delegate int DbTranAction(IDbTransaction db);
    public delegate IEnumerable<T> DbQueryAction<T>(IDbConnection db);
    public interface IUnitOfWork
    {
        void Execute(DbTranAction dbTranAction);

       IEnumerable<T> Query<T>(DbQueryAction<T> dbQueryAction);

        int SaveChanges();
    }

}
