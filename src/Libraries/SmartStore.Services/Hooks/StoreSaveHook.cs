using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Stores;
using SmartStore.Services.Media;
using SmartStore.Services.Stores;
using SmartStore.Services.Tasks;
using SmartStore.Utilities;
using System.Web;

namespace SmartStore.Services.Hooks
{
    public class StoreSaveHook : DbSaveHook<Store>
    {
        #region Private Fields

        private readonly HttpContextBase _httpContext;

        private readonly IPictureService _pictureService;

        private readonly IStoreService _storeService;

        private readonly ITaskScheduler _taskScheduler;

        #endregion Private Fields

        #region Public Constructors

        public StoreSaveHook(IPictureService pictureService, ITaskScheduler taskScheduler, IStoreService storeService, HttpContextBase httpContext)
        {
            _pictureService = pictureService;
            _taskScheduler = taskScheduler;
            _storeService = storeService;
            _httpContext = httpContext;
        }

        #endregion Public Constructors



        #region Protected Methods

        protected override void OnDeleted(Store entity, IHookedEntity entry)
        {
            _pictureService.ClearUrlCache();
            TryChangeSchedulerBaseUrl();
        }

        protected override void OnInserted(Store entity, IHookedEntity entry)
        {
            TryChangeSchedulerBaseUrl();
        }

        protected override void OnUpdated(Store entity, IHookedEntity entry)
        {
            TryChangeSchedulerBaseUrl();
        }

        protected override void OnUpdating(Store entity, IHookedEntity entry)
        {
            if (entry.IsPropertyModified(nameof(entity.ContentDeliveryNetwork)))
            {
                _pictureService.ClearUrlCache();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void TryChangeSchedulerBaseUrl()
        {
            if (CommonHelper.GetAppSetting<string>("sm:TaskSchedulerBaseUrl").IsWebUrl() == false)
            {
                _taskScheduler.SetBaseUrl(_storeService, _httpContext);
            }
        }

        #endregion Private Methods
    }
}