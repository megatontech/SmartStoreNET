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
    public interface ICommonServices
    {
        #region Public Properties

        IApplicationEnvironment ApplicationEnvironment { get; }

        ICacheManager Cache { get; }

        IChronometer Chronometer { get; }

        IComponentContext Container { get; }

        ICustomerActivityService CustomerActivity { get; }

        IDateTimeHelper DateTimeHelper { get; }

        IDbContext DbContext { get; }

        IDisplayControl DisplayControl { get; }

        IEventPublisher EventPublisher { get; }

        ILocalizationService Localization { get; }

        IMessageFactory MessageFactory { get; }

        INotifier Notifier { get; }

        IPermissionService Permissions { get; }

        IPictureService PictureService { get; }

        IRequestCache RequestCache { get; }

        ISettingService Settings { get; }

        IStoreContext StoreContext { get; }

        IStoreService StoreService { get; }

        IWebHelper WebHelper { get; }

        IWorkContext WorkContext { get; }

        #endregion Public Properties
    }

    public static class ICommonServicesExtensions
    {
        #region Public Methods

        public static TService Resolve<TService>(this ICommonServices services)
        {
            return services.Container.Resolve<TService>();
        }

        public static TService Resolve<TService>(this ICommonServices services, object serviceKey)
        {
            return services.Container.ResolveKeyed<TService>(serviceKey);
        }

        public static object Resolve(this ICommonServices services, Type serviceType)
        {
            return services.Resolve(null, serviceType);
        }

        public static object Resolve(this ICommonServices services, object serviceKey, Type serviceType)
        {
            return services.Container.ResolveKeyed(serviceKey, serviceType);
        }

        public static TService ResolveNamed<TService>(this ICommonServices services, string serviceName)
        {
            return services.Container.ResolveNamed<TService>(serviceName);
        }

        public static object ResolveNamed(this ICommonServices services, string serviceName, Type serviceType)
        {
            return services.Container.ResolveNamed(serviceName, serviceType);
        }

        #endregion Public Methods
    }
}