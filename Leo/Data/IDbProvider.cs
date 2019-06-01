using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Leo.Data
{
    /// <summary>
    /// 表示用于创建数据库连接等相关组件及Sql语句的一组类。
    /// </summary>
    public interface IDbProvider
    {
        IDbConnection CreateConnection();
        IDbDataAdapter CreateAdapter();
        IDbCommand CreateCommand();
        IDbDataParameter CreateDataParameter(string key, object value);
        ISqlAdapter CreateSqlAdapter();
    }
}
