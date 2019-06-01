using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Leo.Data
{
    public static class EntityConvert
    {

        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {
            if (entities == null || entities.Count() == 0)
                return null;
            DataTable result = new DataTable();
            var infos = typeof(T).GetProperties().ToDictionary(p => p.Name);
            if (TableAttribute.TryGetTableAttribute<T>(out TableAttribute tableAttribute))
            {
                result.TableName = tableAttribute.TableName;
            }
            else
            {
                result.TableName = typeof(T).Name;
            }

            if (ColumnAttribute.TryGetColumnAttributes<T>(out Dictionary<PropertyInfo, ColumnAttribute> columnAttributes))
            {
                //设置表头
                List<DataColumn> keys = new List<DataColumn>();
                foreach (var att in columnAttributes)
                {
                    var value = att.Value;
                    DataColumn column = new DataColumn()
                    {
                        ColumnName = value.ColumnName ?? att.Key.Name,
                        Caption = value.Description,
                        AllowDBNull = value.Nullable,
                        AutoIncrement = value.IsIdentity,
                        DataType = infos[att.Key.Name].PropertyType,
                        Unique = value.Unique,
                    };
                    if (column.DataType == typeof(string) && value.Length>0)
                        column.MaxLength = value.Length;
                    if (Leo.Util.Converter.TryParse(value.DefaultVal, infos[att.Key.Name].PropertyType, out object obj))
                        column.DefaultValue = obj;
                    if (value.IsPrimaryKey)
                        keys.Add(column);
                    result.Columns.Add(column);
                }
                result.PrimaryKey = keys.ToArray();

                //设置值

                foreach (var entity in entities)
                {
                    var row = result.NewRow();
                    foreach (var att in columnAttributes)
                    {
                        var value = att.Value;
                        row[value.ColumnName ?? att.Key.Name] = infos[att.Key.Name].GetValue(entity);

                    }
                    result.Rows.Add(row);
                }
            }
            return result;
        }

    }

   


}
