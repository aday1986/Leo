using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Leo.Util
{
  public  class Convert
    {
      public static bool TryParse(object obj, Type type, out object result)
        {
            result = null;
            if (obj == null)
                return false;
            var tryParse = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder
               , new Type[] { obj.GetType(), type.MakeByRefType() }
               , new ParameterModifier[] { new ParameterModifier(2) });
            if (tryParse != null)
            {
                var parameters = new object[] { obj, Activator.CreateInstance(type) };
                bool success = (bool)tryParse.Invoke(null, parameters);
                if (success)
                    result = parameters[1];
                return success;
            }
            try
            {
                result = System.Convert.ChangeType(obj, type);
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

      public static bool TryParse<T>(object obj, out T result)
        {
            result = default(T);
            if (TryParse(obj, typeof(T), out object resultObj))
            {
                result = (T)resultObj;
                return true;
            }
            return false;
        }
    }
}
