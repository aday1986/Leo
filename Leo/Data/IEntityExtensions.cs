using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Leo.Data
{
    public static class IEntityExtensions
    {
       

        public static DataTable ToDataTable<T>(this IEnumerable<T> entities) where T : IEntity
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

            if (ColumnAttribute.TryGetColumnAttributes<T>(out Dictionary<string, ColumnAttribute> columnAttributes))
            {
                //设置表头
                List<DataColumn> keys = new List<DataColumn>();
                foreach (var att in columnAttributes)
                {
                    var value = att.Value;
                    DataColumn column = new DataColumn()
                    {
                        ColumnName = value.ColumnName ?? att.Key,
                        Caption = value.Description,
                        AllowDBNull = value.Nullable,
                        AutoIncrement = value.IsIdentity,
                        DataType = infos[att.Key].PropertyType,
                        Unique = value.Unique,
                    };
                    if (column.DataType == typeof(string) && value.MaxLength.HasValue)
                        column.MaxLength = value.MaxLength.Value;
                    if (Leo.Util.Convert.TryParse(value.DefaultValue,infos[att.Key].PropertyType,out object obj))
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
                        row[value.ColumnName ?? att.Key] = infos[att.Key].GetValue(entity);

                    }
                    result.Rows.Add(row);
                }
            }
            return result;
        } 
    }

    public class TestModel : IEntity
    {
        [Key]
        [Column(IsPrimaryKey =true,DefaultValue ="9")]
        public int Id { get; set; }
        [Column()]
        public string Name { get; set; }
    }


}
