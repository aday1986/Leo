using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Leo.Data
{
   public interface IMapper
    {
        T Map<T>(IDataRecord record);
        T Map<T>(object source);
        T Map<TSource, T>(TSource source,params Action<TSource,T>[] profiles);
        Hashtable Map(object source);

    }
}
