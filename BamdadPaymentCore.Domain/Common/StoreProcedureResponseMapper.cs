using BamdadPaymentCore.Domain.Models.StoreProceduresModels;
using System.Data;
using System.Reflection;

namespace BamdadPaymentCore.Domain.Common
{
    public static class StoreProcedureResponseMapper
    {
        public static DataTable ToDataTable<T>(this T item) where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "The input item cannot be null.");

            var dataTable = new DataTable();

            // Get the properties of the class
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add columns to the DataTable
            foreach (var prop in properties)
            {
                var columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dataTable.Columns.Add(prop.Name, columnType);
            }

            // Add a single row to the DataTable
            var row = dataTable.NewRow();
            foreach (var prop in properties)
            {
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            }
            dataTable.Rows.Add(row);

            return dataTable;
        }

    }
}
