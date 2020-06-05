using SmartStore.Services.Tasks;
using SmartStore.Utilities;

namespace SmartStore.Services.Common
{
    /// <summary>
    /// Task to cleanup temporary files
    /// </summary>
    public partial class TempFileCleanupTask : ITask
    {
        #region Public Methods

        public void Execute(TaskExecutionContext ctx)
        {
            FileSystemHelper.ClearTempDirectories();
        }

        #endregion Public Methods
    }
}