using System;
using System.Collections.Generic;

namespace Leo.Data.EF
{
    /// <summary>
    /// 表示用于数据库实体对象类型动态调用的一组类。
    /// </summary>
    public interface IEntityTypeProvider
    {
        IEnumerable<Type> GetTypes();
    }
}
