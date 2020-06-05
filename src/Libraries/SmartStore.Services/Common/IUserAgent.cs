using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Common
{
    public interface IUserAgent
    {
        #region Public Properties

        DeviceInfo Device { get; }

        bool IsBot { get; }

        bool IsMobileDevice { get; }

        bool IsPdfConverter { get; }

        bool IsTablet { get; }

        OSInfo OS { get; }

        string RawValue { get; set; }

        UserAgentInfo UserAgent { get; }

        #endregion Public Properties
    }

    public sealed class DeviceInfo
    {
        #region Public Constructors

        public DeviceInfo(string family, bool isBot)
        {
            this.Family = family;
            this.IsBot = isBot;
        }

        #endregion Public Constructors



        #region Public Properties

        public string Family { get; private set; }

        public bool IsBot { get; private set; }

        #endregion Public Properties



        #region Public Methods

        public override string ToString()
        {
            return this.Family;
        }

        #endregion Public Methods
    }

    public sealed class OSInfo
    {
        #region Public Constructors

        public OSInfo(string family, string major, string minor, string patch, string patchMinor)
        {
            this.Family = family;
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.PatchMinor = patchMinor;
        }

        #endregion Public Constructors



        #region Public Properties

        public string Family { get; private set; }

        public string Major { get; private set; }

        public string Minor { get; private set; }

        public string Patch { get; private set; }

        public string PatchMinor { get; private set; }

        #endregion Public Properties



        #region Public Methods

        public override string ToString()
        {
            var str = VersionString.Format(Major, Minor, Patch, PatchMinor);
            return (this.Family + (!string.IsNullOrEmpty(str) ? (" " + str) : null));
        }

        #endregion Public Methods



        #region Private Methods

        private static string FormatVersionString(params string[] parts)
        {
            return string.Join(".", (from v in parts
                                     where !string.IsNullOrEmpty(v)
                                     select v).ToArray<string>());
        }

        #endregion Private Methods
    }

    public sealed class UserAgentInfo
    {
        #region Private Fields

        private static readonly HashSet<string> s_Bots = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "BingPreview"
        };

        private bool? _isBot;

        private bool? _supportsWebP;

        #endregion Private Fields

        #region Public Constructors

        public UserAgentInfo(string family, string major, string minor, string patch)
        {
            this.Family = family;
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
        }

        #endregion Public Constructors



        #region Public Properties

        public string Family { get; private set; }

        public bool IsBot
        {
            get
            {
                if (!_isBot.HasValue)
                {
                    _isBot = s_Bots.Contains(Family);
                }
                return _isBot.Value;
            }
        }

        public string Major { get; private set; }

        public string Minor { get; private set; }

        public string Patch { get; private set; }

        public bool SupportsWebP
        {
            get
            {
                if (_supportsWebP == null)
                {
                    if (Family == "Chrome")
                    {
                        _supportsWebP = Major.ToInt() >= 49;
                    }
                    else if (Family.StartsWith("Chrome Mobile"))
                    {
                        _supportsWebP = Major.ToInt() >= 61;
                    }
                    else if (Family == "Opera")
                    {
                        _supportsWebP = Major.ToInt() >= 48;
                    }
                    else if (Family == "Opera Mini")
                    {
                        _supportsWebP = true;
                    }
                    else if (Family == "Android")
                    {
                        _supportsWebP = Major.ToInt() >= 5 || (Major.ToInt() == 4 && Minor.ToInt() >= 4);
                    }
                    else
                    {
                        _supportsWebP = false;
                    }
                }

                return _supportsWebP.Value;
            }
        }

        #endregion Public Properties



        #region Public Methods

        public override string ToString()
        {
            var str = VersionString.Format(Major, Minor, Patch);
            return (this.Family + (!string.IsNullOrEmpty(str) ? (" " + str) : null));
        }

        #endregion Public Methods
    }

    internal static class VersionString
    {
        #region Public Methods

        public static string Format(params string[] parts)
        {
            return string.Join(".", (from v in parts
                                     where !string.IsNullOrEmpty(v)
                                     select v).ToArray<string>());
        }

        #endregion Public Methods
    }
}