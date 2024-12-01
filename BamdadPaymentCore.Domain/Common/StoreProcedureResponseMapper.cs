using BamdadPaymentCore.Domain.StoreProceduresModels;
using System.Data;
using System.Reflection;

namespace BamdadPaymentCore.Domain.Common
{
    public static class StoreProcedureResponseMapper
    {
        public static DataTable ListToDataTable<T>(this List<T> items) where T : StoreProcedureResponseModel
        {
            var dataTable = new DataTable();

            if (items == null || !items.Any()) return dataTable;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var row = dataTable.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

    }
}
