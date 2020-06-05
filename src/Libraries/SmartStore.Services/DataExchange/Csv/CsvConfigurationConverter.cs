using Newtonsoft.Json;
using SmartStore.ComponentModel;
using System;
using System.Globalization;

namespace SmartStore.Services.DataExchange.Csv
{
    public class CsvConfigurationConverter : TypeConverterBase
    {
        #region Public Constructors

        public CsvConfigurationConverter()
            : base(typeof(object))
        {
        }

        #endregion Public Constructors



        #region Public Methods

        public override bool CanConvertFrom(Type type)
        {
            return type == typeof(string);
        }

        public override bool CanConvertTo(Type type)
        {
            return type == typeof(string);
        }

        public override object ConvertFrom(CultureInfo culture, object value)
        {
            if (value is string)
            {
                return JsonConvert.DeserializeObject<CsvConfiguration>((string)value);
            }

            return base.ConvertFrom(culture, value);
        }

        public T ConvertFrom<T>(string value)
        {
            if (value.HasValue())
                return (T)ConvertFrom(CultureInfo.InvariantCulture, value);

            return default(T);
        }

        public override object ConvertTo(CultureInfo culture, string format, object value, Type to)
        {
            if (to == typeof(string))
            {
                if (value is CsvConfiguration)
                {
                    return JsonConvert.SerializeObject(value);
                }
                else
                {
                    return string.Empty;
                }
            }

            return base.ConvertTo(culture, format, value, to);
        }

        public string ConvertTo(object value)
        {
            return (string)ConvertTo(CultureInfo.InvariantCulture, null, value, typeof(string));
        }

        #endregion Public Methods
    }
}