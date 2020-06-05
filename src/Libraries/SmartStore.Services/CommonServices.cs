using Autofac;
using SmartStore.Core;
using SmartStore.Core.Caching;
using SmartStore.Core.Data;
using SmartStore.Core.Events;
using SmartStore.Core.Logging;
using SmartStore.Services.Configuration;
using SmartStore.Services.Helpers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Messages;
using SmartStore.Services.Security;
using SmartStore.Services.Stores;
using System;

namespace SmartStore.Services
{
    public class CommonServices : ICommonServices
    {
        #region Private Fields

        private readonly Lazy<ICacheManager> _cacheManager;

        private readonly Lazy<IChronometer> _chronometer;

        private readonly IComponentContext _container;

        private readonly Lazy<ICustomerActivityService> _customerActivity;

        private readonly Lazy<IDateTimeHelper> _dateTimeHelper;

        private readonly Lazy<IDbContext> _dbContext;

        private readonly Lazy<IDisplayControl> _displayControl;

        private readonly Lazy<IApplicationEnvironment> _env;

        private readonly Lazy<IEventPublisher> _eventPublisher;

        private readonly Lazy<ILocalizationService> _localization;

        private readonly Lazy<IMessageFactory> _messageFactory;

        private readonly Lazy<INotifier> _notifier;

        private readonly Lazy<IPermissionService> _permissions;

        private readonly Lazy<IPictureService> _pictureService;

        private readonly Lazy<IRequestCache> _requestCache;

        private readonly Lazy<ISettingService> _settings;

        private readonly Lazy<IStoreContext> _storeContext;

        private readonly Lazy<IStoreService> _storeService;

        private readonly Lazy<IWebHelper> _webHelper;

        private readonly Lazy<IWorkContext> _workContext;

        #endregion Private Fields

        #region Public Constructors

        public CommonServices(
            IComponentContext container,
            Lazy<IApplicationEnvironment> env,
            Lazy<ICacheManager> cacheManager,
            Lazy<IRequestCache> requestCache,
            Lazy<IDbContext> dbContext,
            Lazy<IStoreContext> storeContext,
            Lazy<IWebHelper> webHelper,
            Lazy<IWorkContext> workContext,
            Lazy<IEventPublisher> eventPublisher,
            Lazy<ILocalizationService> localization,
            Lazy<ICustomerActivityService> customerActivity,
            Lazy<IPictureService> pictureService,
            Lazy<INotifier> notifier,
            Lazy<IPermissionService> permissions,
            Lazy<ISettingService> settings,
            Lazy<IStoreService> storeService,
            Lazy<IDateTimeHelper> dateTimeHelper,
            Lazy<IDisplayControl> displayControl,
            Lazy<IChronometer> chronometer,
            Lazy<IMessageFactory> messageFactory)
        {
            this._container = container;
            this._env = env;
            this._cacheManager = cacheManager;
            this._requestCache = requestCache;
            this._dbContext = dbContext;
            this._storeContext = storeContext;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._eventPublisher = eventPublisher;
            this._localization = localization;
            this._customerActivity = customerActivity;
            this._pictureService = pictureService;
            this._notifier = notifier;
            this._permissions = permissions;
            this._settings = settings;
            this._storeService = storeService;
            this._dateTimeHelper = dateTimeHelper;
            this._displayControl = displayControl;
            this._chronometer = chronometer;
            this._messageFactory = messageFactory;
        }

        #endregion Public Constructors



        #region Public Properties

        public IApplicationEnvironment ApplicationEnvironment => _env.Value;

        public ICacheManager Cache => _cacheManager.Value;

        public IChronometer Chronometer => _chronometer.Value;

        public IComponentContext Container => _container;

        public ICustomerActivityService CustomerActivity => _customerActivity.Value;

        public IDateTimeHelper DateTimeHelper => _dateTimeHelper.Value;

        public IDbContext DbContext => _dbContext.Value;

        public IDisplayControl DisplayControl => _displayControl.Value;

        public IEventPublisher EventPublisher => _eventPublisher.Value;

        public ILocalizationService Localization => _localization.Value;

        public IMessageFactory MessageFactory => _messageFactory.Value;

        public INotifier Notifier => _notifier.Value;

        public IPermissionService Permissions => _permissions.Value;

        public IPictureService PictureService => _pictureService.Value;

        public IRequestCache RequestCache => _requestCache.Value;

        public ISettingService Settings => _settings.Value;

        public IStoreContext StoreContext => _storeContext.Value;

        public IStoreService StoreService => _storeService.Value;

        public IWebHelper WebHelper => _webHelper.Value;

        public IWorkContext WorkContext => _workContext.Value;

        #endregion Public Properties
    }
}