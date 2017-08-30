using System;
using System.Data;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public static class ObjectExtensions
    {
        public static object ParseDbNull(this object x)
        {
            return x == DBNull.Value ? null : x;
        }

        public static int? ToNullableInt(this object x)
        {
            return x == DBNull.Value || x == null ? (int?)null : Convert.ToInt32(x);
        }

        public static object GetNullableValue(this DataRow dr, string columnName)
        {
            if (!dr.Table.Columns.Contains(columnName))
                return null;
            else
            {
                if (dr[columnName] == DBNull.Value)
                    return null;

                return dr[columnName];
            }
        }

        public static void AddMissingColumns(this DataTable dt, Type columnsEnum)
        {
            string[] columns = Enum.GetNames(columnsEnum);

            foreach (string column in columns)
            {
                if (!dt.Columns.Contains(column))
                {
                    dt.Columns.Add(column);
                }
            }
        }

    }
}
