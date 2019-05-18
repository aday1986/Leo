using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leo.DI;
using Leo.ThirdParty.Dapper;
using Microsoft.Extensions.Logging;

namespace Leo.Data.Dapper
{
    [Service]
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbProvider dbProvider;
        private readonly ILogger logger;

        public DapperRepository(IUnitOfWork unitOfWork, IDbProvider dbProvider, ILogger<DapperRepository<T>> logger = null)
        {
            this.unitOfWork = unitOfWork;
            this.dbProvider = dbProvider;
            this.logger = logger;
            RegisterLogEvent();
        }

        private void RegisterLogEvent()
        {
            if (logger != null)
            {
                AfterAdd += (s, e) => { logger.LogInformation($"After add {e.Item}."); };
                BeforeAdd += (s, e) => { logger.LogInformation($"Before add {e.Item}."); };
                AfterRemove += (s, e) => { logger.LogInformation($"After remove {e.Item}."); };
                BeforeRemove += (s, e) => { logger.LogInformation($"Before remove {e.Item}."); };
                AfterUpdate += (s, e) => { logger.LogInformation($"After update {e.Item}."); };
                BeforeUpdate += (s, e) => { logger.LogInformation($"Before Update {e.Item}."); };
                AfterAddRange += (s, e) => { logger.LogInformation($"After add range {e.Item},ItemCount={e.Item.Count()}."); };
                BeforeAddRange += (s, e) => { logger.LogInformation($"Before add range {e.Item},ItemCount={e.Item.Count()}."); };
                AfterRemoveRange += (s, e) => { logger.LogInformation($"After remove range {e.Item},ItemCount={e.Item.Count()}."); };
                BeforeRemoveRange += (s, e) => { logger.LogInformation($"Before remove range {e.Item},ItemCount={e.Item.Count()}."); };
                AfterUpdateRange += (s, e) => { logger.LogInformation($"After update range {e.Item},ItemCount={e.Item.Count()}."); };
                BeforeUpdateRange += (s, e) => { logger.LogInformation($"Before update range {e.Item},ItemCount={e.Item.Count()}."); };
                BeforeQuery += (s, e) => { logger.LogInformation($"Before Query,Sql={e.Item}."); };
                AfterQuery += (s, e) => { logger.LogInformation($"After query,ItemCount={e.Item.Count()}."); };
                AfterSaveChanges += (s, e) => { logger.LogInformation($"After save changes,RowCount={e.Item}."); };
            }
        }

        public event EventHandler<T> AfterAdd;
        public event EventHandler<T> BeforeAdd;
        public event EventHandler<T> AfterRemove;
        public event EventHandler<T> BeforeRemove;
        public event EventHandler<T> AfterUpdate;
        public event EventHandler<T> BeforeUpdate;
        public event EventHandler<IEnumerable<T>> AfterAddRange;
        public event EventHandler<IEnumerable<T>> BeforeAddRange;
        public event EventHandler<IEnumerable<T>> AfterRemoveRange;
        public event EventHandler<IEnumerable<T>> BeforeRemoveRange;
        public event EventHandler<IEnumerable<T>> AfterUpdateRange;
        public event EventHandler<IEnumerable<T>> BeforeUpdateRange;
        public event EventHandler<string> BeforeQuery;
        public event EventHandler<IEnumerable<T>> AfterQuery;
        public event EventHandler<int> AfterSaveChanges;


        public void Add(T entity)
        {
            BeforeAdd?.Invoke(this, new EventArgs<T>(entity));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetInsertSql<T>(), entity, tran));
            AfterAdd?.Invoke(this, new EventArgs<T>(entity));
        }

        public void AddRange(IEnumerable<T> entities)
        {
            BeforeAddRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetInsertSql<T>(), entities, tran));
            AfterAddRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
        }

        public T Get(params object[] keyvalues)
        {
            if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<string, ColumnAttribute> keys))
            {
                List<Condition> conditions = new List<Condition>();
                int i = 0;
                foreach (var key in keys)
                {
                    conditions.Add(new Condition()
                    {
                        ConditionType = ConditionEnum.Equal,
                        Key = key.Value.ColumnName ?? key.Key,
                        Value = keyvalues[i++]
                    });
                }
                return Query(conditions).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<T> Query(IEnumerable<Condition> conditions)
        {
            var sql = dbProvider.GetSelectSql<T>(conditions, out Dictionary<string, object> parameters);
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("Sql语句拼装失败。");
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters.Any())
            {
                foreach (var param in parameters)
                {
                    dynamicParameters.Add(param.Key, param.Value);
                }
            }
            BeforeQuery?.Invoke(this, new EventArgs<string>(sql));
            var result = unitOfWork.Query(d => d.Query<T>(sql, dynamicParameters));
            AfterQuery?.Invoke(this, new EventArgs<IEnumerable<T>>(result));
            return result;
        }

        public IEnumerable<T> QueryPage(IEnumerable<Condition> conditions, int index, int pagesize)
        {
            var sql = dbProvider.GetSelectSql<T>(conditions,
                out Dictionary<string, object> parameters, pagesize, index);
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("Sql语句拼装失败。");
            DynamicParameters dynamicParameters = new DynamicParameters();
            if (parameters.Any())
            {
                foreach (var param in parameters)
                {
                    dynamicParameters.Add(param.Key, param.Value);
                }
            }
            BeforeQuery?.Invoke(this, new EventArgs<string>(sql));
            var result = unitOfWork.Query(d => d.Query<T>(sql, dynamicParameters));
            AfterQuery?.Invoke(this, new EventArgs<IEnumerable<T>>(result));
            return result;
        }

        public void Remove(T entity)
        {
            BeforeRemove?.Invoke(this, new EventArgs<T>(entity));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetDeleteSql<T>(), entity, tran));
            AfterRemove?.Invoke(this, new EventArgs<T>(entity));
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            BeforeRemoveRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetDeleteSql<T>(), entities, tran));
            AfterRemoveRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
        }

        public int SaveChanges()
        {
            var result = unitOfWork.SaveChanges();
            AfterSaveChanges?.Invoke(this, new EventArgs<int>(result));
            return result;
        }

        public void Update(T entity)
        {
            BeforeUpdate?.Invoke(this, new EventArgs<T>(entity));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetUpdateSql<T>(), entity, tran));
            AfterUpdate?.Invoke(this, new EventArgs<T>(entity));
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            BeforeUpdateRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
            unitOfWork.Execute(tran => tran.Connection.Execute(dbProvider.GetUpdateSql<T>(), entities, tran));
            AfterUpdateRange?.Invoke(this, new EventArgs<IEnumerable<T>>(entities));
        }
    }
}
