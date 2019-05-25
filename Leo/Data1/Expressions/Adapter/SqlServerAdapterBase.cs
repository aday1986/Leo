using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leo.Data1.Expressions.Adapter
{
    /// <summary>
    /// Provides functionality common to all supported SQL Server versions
    /// </summary>
    class SqlServerAdapterBase : SqlAdapterBase
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize)
        {
            return string.Format("SELECT TOP({4}) {0} FROM {1} {2} {3}",
                    selection, source, conditions, order, pageSize);
        }


        public string Table(string tableName)
        {
            return string.Format("[{0}]", tableName);
        }

        public string Field(string tableName, string fieldName,string alias=null)
        {
            return $"[{tableName}].[{fieldName}]{(string.IsNullOrEmpty(alias) || alias==fieldName?"":$" AS {alias}")}";
        }

        /// <summary>
        /// 待改进。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="selectFunction"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string SelectFunction(string tableName, string fieldName,string selectFunction, string alias )
        {
            return $"{ selectFunction} ({ Field(tableName,fieldName)}) AS {alias}";
        }

        public string Parameter(string parameterId)
        {
            return "@" + parameterId;
        }
    }
}
