using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Leo.ThirdParty.Dapper;

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
                if (currentTran == null)
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
            try
            {
                rowCount += dbTranAction.Invoke(CurrentTran);
            }
            catch (Exception)
            {
                rowCount = -1;
                throw;
            }
        }

        public T Query<T>(DbQueryAction<T> dbQueryAction)
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
                currentTran.Dispose();
                rowCount = 0;
            }
        }
    }
}
