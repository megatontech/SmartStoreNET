﻿using SmartStore.Core.Domain.DataExchange;

namespace SmartStore.Services.DataExchange.Export
{
    public abstract class ExportProviderBase : IExportProvider
    {
        #region Public Properties

        /// <summary>
        /// Get provider specific configuration information. Return <c>null</c> when no provider specific configuration is required.
        /// </summary>
        public virtual ExportConfigurationInfo ConfigurationInfo
        {
            get { return null; }
        }

        /// <summary>
        /// The exported entity type
        /// </summary>
        public virtual ExportEntityType EntityType
        {
            get { return ExportEntityType.Product; }
        }

        /// <summary>
        /// File extension of the export files (without dot). Return <c>null</c> for a non file based, on-the-fly export.
        /// </summary>
        public virtual string FileExtension
        {
            get { return null; }
        }

        #endregion Public Properties



        #region Public Methods

        public void Execute(ExportExecuteContext context)
        {
            Export(context);
        }

        /// <summary>
        /// Called once per store when export execution ended
        /// </summary>
        /// <param name="context">Export execution context</param>
        public virtual void OnExecuted(ExportExecuteContext context)
        {
        }

        #endregion Public Methods



        #region Protected Methods

        /// <summary>
        /// Export data to a file
        /// </summary>
        /// <param name="context">Export execution context</param>
        protected abstract void Export(ExportExecuteContext context);

        #endregion Protected Methods
    }
}