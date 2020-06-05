using SmartStore.Core.Domain.DataExchange;
using System;

namespace SmartStore.Services.DataExchange.Export
{
    /// <summary>
    /// Declares data processing types supported by an export provider.
    /// Projection type controls whether to display corresponding projection fields while editing an export profile.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ExportFeaturesAttribute : Attribute
    {
        #region Public Properties

        public ExportFeatures Features { get; set; }

        #endregion Public Properties
    }
}