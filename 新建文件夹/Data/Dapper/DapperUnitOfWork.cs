using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Leo.ThirdParty.Dapper;
using Microsoft.Extensions.Logging;

namespace Leo.Data.Dapper
{

    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbProvider dbProvider;
        private readonly ILogger<DapperUnitOfWork> logger;

        public DapperUnitOfWork(IDbProvider dbProvider,ILogger<DapperUnitOfWork> logger)
        {
            this.dbProvider = dbProvider;
            this.logger = logger;
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
                logger.LogInformation("aaa");
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
                if (currentTran!=null) currentTran.Dispose();
               
                rowCount = 0;
            }
        }
    }
}
