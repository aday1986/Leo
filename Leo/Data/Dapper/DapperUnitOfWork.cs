using System;
using System.Collections.Generic;
using System.Data;

namespace Leo.Data.Dapper
{

    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbProvider dbProvider;

        public DapperUnitOfWork(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }
        private int rowCount;
        private IDbTransaction currentTran;
        private IDbTransaction CurrentTran
        {
            get
            {
                if (currentTran == null|| currentTran.Connection==null)
                {
                    var db = dbProvider.CreateConnection();
                    db.Open();
                    currentTran = db.BeginTransaction();
                    rowCount = 0;
                }
                return currentTran;
            }
        }
        public void Execute(DbTranAction dbTranAction)
        {
                rowCount += dbTranAction.Invoke(CurrentTran);
        }

        public IEnumerable<T> Query<T>(DbQueryAction<T> dbQueryAction)
        {
            try
            {
                using (var db = dbProvider.CreateConnection())
                {
                    return dbQueryAction.Invoke(db);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveChanges()
        {
            try
            {
                if (currentTran == null)
                    return 0;
                currentTran.Commit();
                return rowCount;
            }
            catch (Exception)
            {
                currentTran.Rollback();
                throw;
            }
            finally
            {
                if (currentTran!=null) currentTran=null;
                rowCount = 0;
            }
        }
    }
}
