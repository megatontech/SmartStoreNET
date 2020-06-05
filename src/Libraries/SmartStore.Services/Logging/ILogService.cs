using SmartStore.Core;
using SmartStore.Core.Domain.Logging;
using SmartStore.Core.Logging;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Logging
{
    public interface ILogService
    {
        #region Public Methods

        /// <summary>
        /// Clears a log
        /// </summary>
        void ClearLog();

        void ClearLog(DateTime toUtc, LogLevel logLevel);

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        void DeleteLog(Log log);

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logger">Logger name</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Log item collection</returns>
        IPagedList<Log> GetAllLogs(
            DateTime? fromUtc,
            DateTime? toUtc,
            string logger,
            string message,
            LogLevel? logLevel,
            int pageIndex,
            int pageSize);

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        Log GetLogById(int logId);

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        IList<Log> GetLogByIds(int[] logIds);

        #endregion Public Methods
    }
}