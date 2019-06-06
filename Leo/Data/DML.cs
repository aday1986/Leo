using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Leo.Data
{
    public class DML:IDML
    {
        private readonly IDbProvider dbProvider;
        private readonly LambdaResolver resolver;
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

        private void Execute(string sql, params Dictionary<string, object>[] param)
        {
            using (var command = dbProvider.CreateCommand())
            {
                command.CommandText = sql;
                command.Transaction = CurrentTran;
                rowCount += SqlCore.ExecuteNonQuery(this, new CommandCollections(command ,param));
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

        public DML(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
            this.resolver = dbProvider.CreateResolver();
        }

        public void Add<T>(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
                sql = resolver.InsertSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            Execute(sql,paramList.ToArray());
        }


        public void Remove<T>(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
                sql = resolver.DeleteSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            Execute(sql, paramList.ToArray());
        }

        public void Remove<T>(Expression<Func<T, bool>> conditions)
        {
            Execute(resolver.DeleteSql(conditions), resolver.Parameters);
        }



        public void Update<T>(params T[] entities)
        {
            List<Dictionary<string, object>> paramList = new List<Dictionary<string, object>>();
            var sql = string.Empty;
            foreach (var entity in entities)
            {
                sql = resolver.UpdateSql<T>(entity);
                paramList.Add(resolver.Parameters);
            }
            Execute(sql, paramList.ToArray());
        }

        public void Update<T>(T model, Expression<Func<T, bool>> conditions)
        {
            Execute(resolver.UpdateSql(model, conditions), resolver.Parameters);
        }
    }

}
