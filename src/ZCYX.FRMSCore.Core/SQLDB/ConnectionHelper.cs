using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class ConnectionHelper 
    {
        #region DB Type Converters

        public static byte[] ConvertToBinary(params int[] intArray)
        {
            byte[] data = new byte[intArray.Length * 4];

            int j = 0;
            foreach (int val in intArray)
            {
                data[j++] = (byte)((val & 0xFF000000) >> 24);
                data[j++] = (byte)((val & 0x00FF0000) >> 16);
                data[j++] = (byte)((val & 0x0000FF00) >> 8);
                data[j++] = (byte)(val & 0x000000FF);
            }

            return data;
        }

        public static byte[] ConvertToBinary(params long[] longArray)
        {
            byte[] data = new byte[longArray.Length * 8];

            int j = 0;
            foreach (long val in longArray)
            {
                data[j++] = 0; // (byte)((val & 0xFF00000000000000) >> 56);
                data[j++] = (byte)((val & 0x00FF000000000000) >> 48);
                data[j++] = (byte)((val & 0x0000FF0000000000) >> 40);
                data[j++] = (byte)((val & 0x000000FF00000000) >> 32);
                data[j++] = (byte)((val & 0x00000000FF000000) >> 24);
                data[j++] = (byte)((val & 0x0000000000FF0000) >> 16);
                data[j++] = (byte)((val & 0x000000000000FF00) >> 8);
                data[j++] = (byte)(val & 0x00000000000000FF);
            }

            return data;
        }

        public static DateTime CreateSqlSafeDate(DateTime dateTimeValue)
        {
            if (dateTimeValue < SqlDateTime.MinValue.Value)
                return SqlDateTime.MinValue.Value;
            else if (dateTimeValue > SqlDateTime.MaxValue.Value)
                return SqlDateTime.MaxValue.Value;
            else
                return dateTimeValue;
        }

        public static DataTable ConvertToIntegerTableType(List<int> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Value", typeof(int));
            foreach (int item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }


        public static DataTable ConvertToLongTableType(List<long> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Value", typeof(long));
            foreach (int item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }

        #endregion
    }
}
