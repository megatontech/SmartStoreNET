using System;
using System.Collections.Generic;

namespace SmartStore.Services.DataExchange.Import
{
    public interface IDataColumn
    {
        #region Public Properties

        string Name { get; }

        Type Type { get; }

        #endregion Public Properties
    }

    public interface IDataRow
    {
        #region Public Properties

        IDataTable Table { get; }

        object[] Values { get; }

        #endregion Public Properties



        #region Public Indexers

        object this[int index] { get; set; }

        object this[string name] { get; set; }

        #endregion Public Indexers
    }

    public interface IDataTable
    {
        #region Public Properties

        IList<IDataColumn> Columns { get; }

        IList<IDataRow> Rows { get; }

        #endregion Public Properties



        #region Public Methods

        int GetColumnIndex(string name);

        bool HasColumn(string name);

        #endregion Public Methods
    }

    public static class IDataRowExtensions
    {
        #region Public Methods

        public static object GetValue(this IDataRow row, int index)
        {
            return row[index];
        }

        public static object GetValue(this IDataRow row, string name)
        {
            return row[name];
        }

        public static void SetValue(this IDataRow row, int index, object value)
        {
            row[index] = value;
        }

        public static void SetValue(this IDataRow row, string name, object value)
        {
            row[name] = value;
        }

        public static bool TryGetValue(this IDataRow row, string name, out object value)
        {
            value = null;

            var index = row.Table.GetColumnIndex(name);
            if (index < 0)
                return false;

            value = row[index];
            return true;
        }

        public static bool TrySetValue(this IDataRow row, string name, object value)
        {
            var index = row.Table.GetColumnIndex(name);
            if (index < 0)
                return false;

            row[index] = value;
            return true;
        }

        #endregion Public Methods
    }
}