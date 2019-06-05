using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Leo.Data
{
    /// <summary>
    /// DataReader Extensions
    /// </summary>
    public static class DataReaderExtensions
    {

        #region Public Static Methods


        /// <summary>
        /// 将SqlDataReader转成T类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ToModel<T>(this IDataReader reader)
        {
            var cache = GetCache<T>();
            return (T)cache.Invoke(reader);
        }

        /// <summary>
        /// 将SqlDataReader转成List<T>类型。
        /// 匿名对象使用反射构造函数构建，常规类使用emit效率更高。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            Type type = typeof(T);
            List<T> res;
            if (type.IsVisible)
            {
                res = reader.Select<T>();
            }
            else
            {
                res = new List<T>();
                while (reader.Read())
                {
                    res.Add(reader.ToModel<T>());
                }
            }
            return res;
        }


        #endregion

        #region Static Readonly Fields
        private static readonly MethodInfo DataRecord_ItemGetter_String =
            typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(string) });
        private static readonly MethodInfo DataRecord_ItemGetter_Int =
            typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo DataRecord_GetOrdinal =
            typeof(IDataRecord).GetMethod("GetOrdinal");
        private static readonly MethodInfo DataReader_Read =
            typeof(IDataReader).GetMethod("Read");
        private static readonly MethodInfo Convert_IsDBNull =
            typeof(Convert).GetMethod("IsDBNull");
        private static readonly MethodInfo DataRecord_GetDateTime =
            typeof(IDataRecord).GetMethod("GetDateTime");
        private static readonly MethodInfo DataRecord_GetDecimal =
            typeof(IDataRecord).GetMethod("GetDecimal");
        private static readonly MethodInfo DataRecord_GetDouble =
            typeof(IDataRecord).GetMethod("GetDouble");
        private static readonly MethodInfo DataRecord_GetInt32 =
            typeof(IDataRecord).GetMethod("GetInt32");
        private static readonly MethodInfo DataRecord_GetInt64 =
            typeof(IDataRecord).GetMethod("GetInt64");
        private static readonly MethodInfo DataRecord_GetString =
            typeof(IDataRecord).GetMethod("GetString");
        private static readonly MethodInfo DataRecord_IsDBNull =
            typeof(IDataRecord).GetMethod("IsDBNull");

        /// <summary>
        /// 属性反射信息缓存 key:类型的hashCode,value属性信息
        /// </summary>
        private static Dictionary<Type, Func<IDataReader, object>> cache
            = new Dictionary<Type, Func<IDataReader, object>>();

        private static Func<IDataReader, object> GetCache<T>()
        {
            Type type = typeof(T);
            if (!cache.TryGetValue(type, out Func<IDataReader, object> value))
            {
                if (type.IsVisible)
                {
                    value = reader =>
                    {
                        var res = Activator.CreateInstance(type);
                        var propInfos = type.GetProperties().ToDictionary(p => p.Name);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var n = reader.GetName(i);
                            if (propInfos.ContainsKey(n))
                            {
                                var v = GetValue(reader, i, propInfos[n].PropertyType);
                                propInfos[n].SetValue(res, v);
                            }
                        }
                        return res;
                    };
                }
                else
                {
                    value = reader =>
                    {
                        var constructor = typeof(T).GetConstructors()
                          .OrderBy(c => c.GetParameters().Length).First();
                        //取当前构造函数的参数
                        var parameters = constructor.GetParameters();
                        var values = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var v = GetValue(reader, i, parameters[i].ParameterType);
                            values[i] = v;
                        }
                        return (T)constructor.Invoke(values);
                    };
                }
                cache.Add(type, value);
            }
            return cache[type];
        }

        private static object GetValue(IDataReader reader, int i, Type type)
        {
            var v = reader.GetValue(i);
            if (type.IsEnum || type == typeof(Int32))
            {
                v = Convert.ToInt32(v);
            }
            else if (Leo.Util.Converter.TryParse(v, type, out object result))
            {
                v = result;
            }
            else
            {
                throw new Exception($"{v.GetType().Name}类型无法被解析为{type.Name}。");
            }
            return v;
        }

        /// <summary>
        /// 把结果集流转换成数据实体列表
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="reader">结果集流</param>
        /// <returns>数据实体列表</returns>
        private static List<T> Select<T>(this IDataReader reader)
        {
            return EntityConverter<T>.Select(reader);
        }

        /// <summary>
        /// 把结果集流转换成数据实体序列（延迟）
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="reader">结果集流</param>
        /// <returns>数据实体序列（延迟）</returns>
        private static IEnumerable<T> SelectLazy<T>(this IDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            return EntityConverter<T>.SelectDelay(reader);
        }
        #endregion

      

        #region Class: EntityConverter<T>

        /// <summary>
        /// 实体转换器
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        private class EntityConverter<T>
        {
            #region Struct: DbColumnInfo
            private struct DbColumnInfo
            {
                public readonly string PropertyName;
                public readonly string ColumnName;
                public readonly Type Type;
                public readonly MethodInfo SetMethod;

                public DbColumnInfo(PropertyInfo prop, ColumnAttribute attr)
                {
                    PropertyName = prop.Name;
                    ColumnName = attr?.ColumnName ?? prop.Name;
                    Type = prop.PropertyType;
                    SetMethod = prop.GetSetMethod(false);
                }
            }

            #endregion

            #region Fields
            private static Func<IDataReader, T> dataLoader;
            private static Func<IDataReader, List<T>> batchDataLoader;
            #endregion

            #region Properties

            private static Func<IDataReader, T> DataLoader
            {
                get
                {
                    if (dataLoader == null)
                        dataLoader = CreateDataLoader(new List<DbColumnInfo>(GetProperties()));
                    return dataLoader;
                }
            }

            private static Func<IDataReader, List<T>> BatchDataLoader
            {
                get
                {
                    if (batchDataLoader == null)
                        batchDataLoader = CreateBatchDataLoader(new List<DbColumnInfo>(GetProperties()));
                    return batchDataLoader;
                }
            }

            #endregion

            #region Init Methods

            private static IEnumerable<DbColumnInfo> GetProperties()
            {
               
                foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (prop.GetIndexParameters().Length > 0)
                        continue;
                    var setMethod = prop.GetSetMethod(false);
                    if (setMethod == null)
                        continue;

                    var attr = Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute), true) as ColumnAttribute;
                   
                    yield return new DbColumnInfo(prop, attr);
                }
            }

            private static Func<IDataReader, T> CreateDataLoader(List<DbColumnInfo> columnInfoes)
            {
                DynamicMethod dm = new DynamicMethod(string.Empty, typeof(T),
                    new Type[] { typeof(IDataReader) }, typeof(EntityConverter<T>));
                ILGenerator il = dm.GetILGenerator();
                LocalBuilder item = il.DeclareLocal(typeof(T));
                // [ int %index% = arg.GetOrdinal(%ColumnName%); ]
                LocalBuilder[] colIndices = GetColumnIndices(il, columnInfoes);
                // T item = new T { %Property% = ... };
                BuildItem(il, columnInfoes, item, colIndices);
                // return item;
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ret);
                return (Func<IDataReader, T>)dm.CreateDelegate(typeof(Func<IDataReader, T>));
            }

            private static Func<IDataReader, List<T>> CreateBatchDataLoader(List<DbColumnInfo> columnInfoes)
            {
                DynamicMethod dm = new DynamicMethod(string.Empty, typeof(List<T>),
                    new Type[] { typeof(IDataReader) }, typeof(EntityConverter<T>));
                ILGenerator il = dm.GetILGenerator();
                LocalBuilder list = il.DeclareLocal(typeof(List<T>));
                LocalBuilder item = il.DeclareLocal(typeof(T));
                Label exit = il.DefineLabel();
                Label loop = il.DefineLabel();
                // List<T> list = new List<T>();
                il.Emit(OpCodes.Newobj, typeof(List<T>).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc_S, list);
                // [ int %index% = arg.GetOrdinal(%ColumnName%); ]
                LocalBuilder[] colIndices = GetColumnIndices(il, columnInfoes);
                // while (arg.Read()) {
                il.MarkLabel(loop);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, DataReader_Read);
                il.Emit(OpCodes.Brfalse, exit);
                //      T item = new T { %Property% = ... };
                BuildItem(il, columnInfoes, item, colIndices);
                //      list.Add(item);
                il.Emit(OpCodes.Ldloc_S, list);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Callvirt, typeof(List<T>).GetMethod("Add"));
                // }
                il.Emit(OpCodes.Br, loop);
                il.MarkLabel(exit);
                // return list;
                il.Emit(OpCodes.Ldloc_S, list);
                il.Emit(OpCodes.Ret);
                return (Func<IDataReader, List<T>>)dm.CreateDelegate(typeof(Func<IDataReader, List<T>>));
            }

            private static LocalBuilder[] GetColumnIndices(ILGenerator il, List<DbColumnInfo> columnInfoes)
            {
                LocalBuilder[] colIndices = new LocalBuilder[columnInfoes.Count];
                for (int i = 0; i < colIndices.Length; i++)
                {
                    // int %index% = arg.GetOrdinal(%ColumnName%);
                    colIndices[i] = il.DeclareLocal(typeof(int));
                    //if (columnInfoes[i].IsOptional)
                    //{
                    //    // try {
                    //    il.BeginExceptionBlock();
                    //}
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, columnInfoes[i].ColumnName);
                    il.Emit(OpCodes.Callvirt, DataRecord_GetOrdinal);
                    il.Emit(OpCodes.Stloc_S, colIndices[i]);
                    //if (columnInfoes[i].IsOptional)
                    //{
                    //    Label exit = il.DefineLabel();
                    //    il.Emit(OpCodes.Leave_S, exit);
                    //    // } catch (IndexOutOfRangeException) {
                    //    il.BeginCatchBlock(typeof(IndexOutOfRangeException));
                    //    // //forget the exception
                    //    il.Emit(OpCodes.Pop);
                    //    // int %index% = -1; // if not found, -1
                    //    il.Emit(OpCodes.Ldc_I4_M1);
                    //    il.Emit(OpCodes.Stloc_S, colIndices[i]);
                    //    il.Emit(OpCodes.Leave_S, exit);
                    //    // } catch (ArgumentException) {
                    //    il.BeginCatchBlock(typeof(ArgumentException));
                    //    // forget the exception
                    //    il.Emit(OpCodes.Pop);
                    //    // int %index% = -1; // if not found, -1
                    //    il.Emit(OpCodes.Ldc_I4_M1);
                    //    il.Emit(OpCodes.Stloc_S, colIndices[i]);
                    //    il.Emit(OpCodes.Leave_S, exit);
                    //    // }
                    //    il.EndExceptionBlock();
                    //    il.MarkLabel(exit);
                    //}
                }
                return colIndices;
            }

            private static void BuildItem(ILGenerator il, List<DbColumnInfo> columnInfoes,
                LocalBuilder item, LocalBuilder[] colIndices)
            {
                // T item = new T();
                il.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
                //var ctor = typeof(T).GetConstructors().OrderBy(c => c.GetParameters().Length).First();
                //il.Emit(OpCodes.Newobj,ctor );
               
                il.Emit(OpCodes.Stloc_S, item);
                for (int i = 0; i < colIndices.Length; i++)
                {
                    //if (columnInfoes[i].IsOptional)
                    //{
                    //    // if %index% == -1 then goto skip;
                    //    skip = il.DefineLabel();
                    //    il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                    //    il.Emit(OpCodes.Ldc_I4_M1);
                    //    il.Emit(OpCodes.Beq, skip);
                    //}
                    if (IsCompatibleType(columnInfoes[i].Type, typeof(int)))
                    {
                        // item.%Property% = arg.GetInt32(%index%);
                        ReadInt32(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].Type, typeof(int?)))
                    {
                        // item.%Property% = arg.IsDBNull ? default(int?) : (int?)arg.GetInt32(%index%);
                        ReadNullableInt32(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].Type, typeof(long)))
                    {
                        // item.%Property% = arg.GetInt64(%index%);
                        ReadInt64(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].Type, typeof(long?)))
                    {
                        // item.%Property% = arg.IsDBNull ? default(long?) : (long?)arg.GetInt64(%index%);
                        ReadNullableInt64(il, item, columnInfoes, colIndices, i);
                    }
                    else if (IsCompatibleType(columnInfoes[i].Type, typeof(decimal)))
                    {
                        // item.%Property% = arg.GetDecimal(%index%);
                        ReadDecimal(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].Type == typeof(decimal?))
                    {
                        // item.%Property% = arg.IsDBNull ? default(decimal?) : (int?)arg.GetDecimal(%index%);
                        ReadNullableDecimal(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].Type == typeof(DateTime))
                    {
                        // item.%Property% = arg.GetDateTime(%index%);
                        ReadDateTime(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else if (columnInfoes[i].Type == typeof(DateTime?))
                    {
                        // item.%Property% = arg.IsDBNull ? default(DateTime?) : (int?)arg.GetDateTime(%index%);
                        ReadNullableDateTime(il, item, columnInfoes[i].SetMethod, colIndices[i]);
                    }
                    else
                    {
                        // item.%Property% = (%PropertyType%)arg[%index%];
                        ReadObject(il, item, columnInfoes, colIndices, i);
                    }
                    //if (columnInfoes[i].IsOptional)
                    //{
                    //    // :skip
                    //    il.MarkLabel(skip);
                    //}
                }
            }

            private static bool IsCompatibleType(Type t1, Type t2)
            {
                if (t1 == t2)
                    return true;
                if (t1.IsEnum && Enum.GetUnderlyingType(t1) == t2)
                    return true;
                var u1 = Nullable.GetUnderlyingType(t1);
                var u2 = Nullable.GetUnderlyingType(t2);
                if (u1 != null && u2 != null)
                    return IsCompatibleType(u1, u2);
                return false;
            }

            private static void ReadInt32(ILGenerator il, LocalBuilder item,
                List<DbColumnInfo> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt32);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadNullableInt32(ILGenerator il, LocalBuilder item,
                List<DbColumnInfo> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                var local = il.DeclareLocal(columnInfoes[i].Type);
                Label intNull = il.DefineLabel();
                Label intCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, intNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt32);
                il.Emit(OpCodes.Call, columnInfoes[i].Type.GetConstructor(
                    new Type[] { Nullable.GetUnderlyingType(columnInfoes[i].Type) }));
                il.Emit(OpCodes.Br_S, intCommon);
                il.MarkLabel(intNull);
                il.Emit(OpCodes.Initobj, columnInfoes[i].Type);
                il.MarkLabel(intCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadInt64(ILGenerator il, LocalBuilder item,
                List<DbColumnInfo> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt64);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadNullableInt64(ILGenerator il, LocalBuilder item,
                List<DbColumnInfo> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                var local = il.DeclareLocal(columnInfoes[i].Type);
                Label intNull = il.DefineLabel();
                Label intCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, intNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_GetInt64);
                il.Emit(OpCodes.Call, columnInfoes[i].Type.GetConstructor(
                    new Type[] { Nullable.GetUnderlyingType(columnInfoes[i].Type) }));
                il.Emit(OpCodes.Br_S, intCommon);
                il.MarkLabel(intNull);
                il.Emit(OpCodes.Initobj, columnInfoes[i].Type);
                il.MarkLabel(intCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            private static void ReadDecimal(ILGenerator il, LocalBuilder item,
                MethodInfo setMethod, LocalBuilder colIndex)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDecimal);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadNullableDecimal(ILGenerator il, LocalBuilder item,
                MethodInfo setMethod, LocalBuilder colIndex)
            {
                var local = il.DeclareLocal(typeof(decimal?));
                Label decimalNull = il.DefineLabel();
                Label decimalCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, decimalNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDecimal);
                il.Emit(OpCodes.Call, typeof(decimal?).GetConstructor(new Type[] { typeof(decimal) }));
                il.Emit(OpCodes.Br_S, decimalCommon);
                il.MarkLabel(decimalNull);
                il.Emit(OpCodes.Initobj, typeof(decimal?));
                il.MarkLabel(decimalCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadDateTime(ILGenerator il, LocalBuilder item,
                MethodInfo setMethod, LocalBuilder colIndex)
            {
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDateTime);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadNullableDateTime(ILGenerator il, LocalBuilder item,
                MethodInfo setMethod, LocalBuilder colIndex)
            {
                var local = il.DeclareLocal(typeof(DateTime?));
                Label dtNull = il.DefineLabel();
                Label dtCommon = il.DefineLabel();
                il.Emit(OpCodes.Ldloca, local);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_IsDBNull);
                il.Emit(OpCodes.Brtrue_S, dtNull);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndex);
                il.Emit(OpCodes.Callvirt, DataRecord_GetDateTime);
                il.Emit(OpCodes.Call, typeof(DateTime?).GetConstructor(new Type[] { typeof(DateTime) }));
                il.Emit(OpCodes.Br_S, dtCommon);
                il.MarkLabel(dtNull);
                il.Emit(OpCodes.Initobj, typeof(DateTime?));
                il.MarkLabel(dtCommon);
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Callvirt, setMethod);
            }

            private static void ReadObject(ILGenerator il, LocalBuilder item,
                List<DbColumnInfo> columnInfoes, LocalBuilder[] colIndices, int i)
            {
                Label common = il.DefineLabel();
                il.Emit(OpCodes.Ldloc_S, item);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc_S, colIndices[i]);
                il.Emit(OpCodes.Callvirt, DataRecord_ItemGetter_Int);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Call, Convert_IsDBNull);
                il.Emit(OpCodes.Brfalse_S, common);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldnull);
                il.MarkLabel(common);
                il.Emit(OpCodes.Unbox_Any, columnInfoes[i].Type);
                il.Emit(OpCodes.Callvirt, columnInfoes[i].SetMethod);
            }

            #endregion

            #region Internal Methods

            internal static IEnumerable<T> SelectDelay(IDataReader reader)
            {
                while (reader.Read())
                    yield return DataLoader(reader);
            }

            internal static List<T> Select(IDataReader reader)
            {
                return BatchDataLoader(reader);
            }
            #endregion
        }

        #endregion

    }

}
