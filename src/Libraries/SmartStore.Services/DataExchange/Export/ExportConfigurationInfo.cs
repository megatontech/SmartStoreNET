﻿using System;

namespace SmartStore.Services.DataExchange.Export
{
    /// <summary>
    /// Serves information about export provider specific configuration
    /// </summary>
    public class ExportConfigurationInfo
    {
        #region Public Properties

        /// <summary>
        /// Callback to initialize the view model. Can be <c>null</c>.
        /// </summary>
        public Action<object> Initialize { get; set; }

        /// <summary>
        /// Type of the view model
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// The partial view name for the configuration
        /// </summary>
        public string PartialViewName { get; set; }

        #endregion Public Properties
    }
}