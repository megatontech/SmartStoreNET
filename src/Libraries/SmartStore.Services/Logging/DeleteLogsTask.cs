using SmartStore.Core.Logging;
using SmartStore.Services.Tasks;
using System;

namespace SmartStore.Services.Logging
{
    /// <summary>
    /// Represents a task for deleting log entries
    /// </summary>
    public partial class DeleteLogsTask : ITask
    {
        #region Private Fields

        private readonly ILogService _logService;

        #endregion Private Fields

        #region Public Constructors

        public DeleteLogsTask(ILogService logService)
        {
            _logService = logService;
        }

        #endregion Public Constructors



        #region Public Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute(TaskExecutionContext ctx)
        {
            var olderThanDays = 7; // TODO: move to settings
            var toUtc = DateTime.Now.AddDays(-olderThanDays);

            _logService.ClearLog(toUtc, LogLevel.Error);
        }

        #endregion Public Methods
    }
}