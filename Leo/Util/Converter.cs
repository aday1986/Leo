using Leo.Util.IniConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Leo.Util
{
    public class Converter
    {
        /// <summary>
        /// 序列化实例为Base64。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string SerializeBase64(object target)
        {
            //  序列化
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(stream, target);
                return System.Convert.ToBase64String(stream.ToArray());
                //return Encoding.UTF8.GetString(stream.ToArray()); 
            }
        }

        /// <summary>
        /// 返序列化Base64为指定实例<see cref="T"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T DeserializeBase64<T>(string target)
        {
            //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(target)))
            using (MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(target)))
            {
                BinaryFormatter format = new BinaryFormatter();
                return (T)format.Deserialize(ms);
            }
        }

        /// <summary>
        /// 尝试将实例<see cref="object"/>转换为指定<see cref="Type"/>。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(object obj, Type type, out object result)
        {
            result = null;
            if (obj == null)
                return false;
            var tryParse = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder
               , new Type[] { obj.GetType(), type.MakeByRefType() }
               , new ParameterModifier[] { new ParameterModifier(2) });
            if (tryParse != null)//先尝试反射调用类的TryParse静态方法。
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

        /// <summary>
        /// 尝试将实例<see cref="object"/>转换为指定类型<see cref="T"/>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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
