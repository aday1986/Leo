using Leo.Data;
using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Leo.Data
{

    public interface IUnitOfWork
    {
        void Execute(Func<IDbCommand, int> func);

        Query<T> Query<T>(Func<IDbCommand,Query<T>> func);

        int SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbProvider dbProvider;

        public UnitOfWork(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        private int rowCount;
        private IDbTransaction currentTran;
        private IDbTransaction CurrentTran
        {
            get
            {
                if (currentTran == null || currentTran.Connection == null)
                {
                    var db = dbProvider.CreateConnection();
                    db.Open();
                    currentTran = db.BeginTransaction();
                    rowCount = 0;
                }
                return currentTran;
            }
        }

      

        public void Execute(Func<IDbCommand, int> func)
        {
            using (var command= dbProvider.CreateCommand())
            {
                command.Transaction = CurrentTran;
                rowCount += func.Invoke(command);
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
                if (currentTran != null) currentTran = null;
                rowCount = 0;
            }

        }

        public Query<T> Query<T>(Func<IDbCommand, Query<T>> func)
        {
            var command = dbProvider.CreateCommand();
            command.Connection = dbProvider.CreateConnection();
            return func.Invoke(command);
        }
    }

}
