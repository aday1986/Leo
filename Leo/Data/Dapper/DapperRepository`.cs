using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leo.Fac;
using Leo.ThirdParty.Dapper;
using Microsoft.Extensions.Logging;

namespace Leo.Data.Dapper
{
   [Service]
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISqlBulider sqlBulider;
        private readonly ILogger logger;

        public DapperRepository(IUnitOfWork unitOfWork, ISqlBulider sqlBulider,ILogger<DapperRepository<T>> logger=null)
        {
            this.unitOfWork = unitOfWork;
            this.sqlBulider = sqlBulider;
            this.logger = logger;
        }

        public void Add(T entity)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetInsertSql<T>(), entity, tran));
        }

       

        public void AddRange(IEnumerable<T> entities)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetInsertSql<T>(), entities, tran));
        }

        public T Get(params object[] keyvalues)
        {
            if (ColumnAttribute.TryGetKeyColumns<T>(out Dictionary<string,ColumnAttribute> keys))
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
            var sql = sqlBulider.GetSelectSql<T>(conditions, out Dictionary<string, object> parameters);
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
          return unitOfWork.Query(d => d.Query<T>(sql, dynamicParameters));
        }

        public IEnumerable<T> QueryPage(IEnumerable<Condition> conditions, int index, int pagesize)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetDeleteSql<T>(), entity, tran));
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetDeleteSql<T>(), entities, tran));
        }

        public int SaveChanges()
        {
           return unitOfWork.SaveChanges();
        }

        public void Update(T entity)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetUpdateSql<T>(), entity, tran));
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            unitOfWork.Execute(tran => tran.Connection.Execute(sqlBulider.GetUpdateSql<T>(), entities, tran));
        }
    }
}
