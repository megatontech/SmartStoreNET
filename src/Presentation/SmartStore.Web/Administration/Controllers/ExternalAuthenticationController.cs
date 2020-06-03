using SmartStore.Admin.Models.ExternalAuthentication;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Authentication.External;
using SmartStore.Services.Configuration;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Plugins;
using SmartStore.Web.Framework.Security;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class ExternalAuthenticationController : AdminControllerBase
    {
        #region Fields

        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IPermissionService _permissionService;
        private readonly PluginMediator _pluginMediator;
        private readonly ISettingService _settingService;

        #endregion Fields

        #region Constructors

        public ExternalAuthenticationController(
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            ISettingService settingService,
            IPermissionService permissionService,
            PluginMediator pluginMediator)
        {
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._pluginMediator = pluginMediator;
        }

        #endregion Constructors

        #region Methods

        public ActionResult ActivateProvider(string systemName, bool activate)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var method = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(systemName);
            bool dirty = method.IsMethodActive(_externalAuthenticationSettings) != activate;
            if (dirty)
            {
                if (!activate)
                    _externalAuthenticationSettings.ActiveAuthenticationMethodSystemNames.Remove(method.Metadata.SystemName);
                else
                    _externalAuthenticationSettings.ActiveAuthenticationMethodSystemNames.Add(method.Metadata.SystemName);

                _settingService.SaveSetting(_externalAuthenticationSettings);
                _pluginMediator.ActivateDependentWidgets(method.Metadata, activate);
            }

            return RedirectToAction("Providers");
        }

        public ActionResult Providers()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var methodsModel = new List<AuthenticationMethodModel>();
            var methods = _openAuthenticationService.LoadAllExternalAuthenticationMethods();
            foreach (var method in methods)
            {
                var model = _pluginMediator.ToProviderModel<IExternalAuthenticationMethod, AuthenticationMethodModel>(method);
                model.IsActive = method.IsMethodActive(_externalAuthenticationSettings);
                methodsModel.Add(model);
            }

            return View(methodsModel);
        }

        #endregion Methods
    }
}