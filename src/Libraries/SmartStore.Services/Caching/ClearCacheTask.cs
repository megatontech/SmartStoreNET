using SmartStore.Core.Caching;
using SmartStore.Services.Tasks;

namespace SmartStore.Services.Caching
{
    /// <summary>
    /// Clear cache scheduled task implementation
    /// </summary>
    public partial class ClearCacheTask : ITask
    {
        #region Private Fields

        private readonly ICacheManager _cacheManager;

        #endregion Private Fields

        #region Public Constructors

        public ClearCacheTask(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        #endregion Public Constructors



        #region Public Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute(TaskExecutionContext ctx)
        {
            _cacheManager.Clear();
        }

        #endregion Public Methods
    }
}