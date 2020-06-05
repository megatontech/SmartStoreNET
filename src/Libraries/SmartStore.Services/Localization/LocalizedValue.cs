using Newtonsoft.Json;
using SmartStore.Core.Domain.Localization;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartStore.Services.Localization
{
    public class LocalizedValue
    {
        #region Private Fields

        // Regex for all types of brackets which need to be "swapped": ({[]})
        private readonly static Regex _rgBrackets = new Regex(@"\(|\{|\[|\]|\}|\)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        #endregion Private Fields



        #region Public Methods

        /// <summary>
        /// Fixes the flow of brackets within a text if the current page language has RTL flow.
        /// </summary>
        /// <param name="str">The test to fix.</param>
        /// <param name="currentLanguage">Current language</param>
        /// <returns></returns>
        public static string FixBrackets(string str, Language currentLanguage)
        {
            if (!currentLanguage.Rtl || str.IsEmpty())
            {
                return str;
            }

            var controlChar = "&lrm;";
            return _rgBrackets.Replace(str, m =>
            {
                return controlChar + m.Value + controlChar;
            });
        }

        #endregion Public Methods
    }

    [Serializable]
    public class LocalizedValue<T> : IHtmlString, IEquatable<LocalizedValue<T>>, IComparable, IComparable<LocalizedValue<T>>
    {
        #region Private Fields

        private readonly Language _currentLanguage;

        private readonly Language _requestLanguage;

        private T _value;

        #endregion Private Fields

        #region Public Constructors

        public LocalizedValue(T value, Language requestLanguage, Language currentLanguage)
        {
            _value = value;
            _requestLanguage = requestLanguage;
            _currentLanguage = currentLanguage;
        }

        #endregion Public Constructors



        #region Public Properties

        public bool BidiOverride
        {
            get { return _requestLanguage != _currentLanguage && _requestLanguage.Rtl != _currentLanguage.Rtl; }
        }

        [JsonIgnore]
        public Language CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        public bool IsFallback
        {
            get { return _requestLanguage != _currentLanguage; }
        }

        [JsonIgnore]
        public Language RequestLanguage
        {
            get { return _requestLanguage; }
        }

        public T Value
        {
            get { return _value; }
        }

        #endregion Public Properties



        #region Public Methods

        public static implicit operator T(LocalizedValue<T> obj)
        {
            if (obj == null)
            {
                return default;
            }

            return obj.Value;
        }

        public void ChangeValue(T value)
        {
            _value = value;
        }

        public int CompareTo(object other)
        {
            return CompareTo(other as LocalizedValue<T>);
        }

        public int CompareTo(LocalizedValue<T> other)
        {
            if (other == null)
            {
                return 1;
            }

            if (Value is IComparable<T> val1)
            {
                return val1.CompareTo(other.Value);
            }

            if (Value is IComparable val2)
            {
                return val2.CompareTo(other.Value);
            }

            return 0;
        }

        public override bool Equals(object other)
        {
            return this.Equals(other as LocalizedValue<T>);
        }

        public bool Equals(LocalizedValue<T> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return object.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (_value != null)
                hashCode ^= _value.GetHashCode();
            return hashCode;
        }

        public string ToHtmlString()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            if (_value == null)
            {
                return null;
            }

            if (typeof(T) == typeof(string))
            {
                return _value as string;
            }

            return _value.Convert<string>(CultureInfo.GetCultureInfo(_currentLanguage.LanguageCulture));
        }

        #endregion Public Methods
    }
}