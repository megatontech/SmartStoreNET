using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Plugins;
using System;
using System.Linq;

namespace SmartStore.Services.Authentication.External
{
    public static class OpenAuthenticationExtentions
    {
        #region Public Methods

        public static bool IsMethodActive(this Provider<IExternalAuthenticationMethod> method, ExternalAuthenticationSettings settings)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (settings == null)
                throw new ArgumentNullException("settings");

            if (settings.ActiveAuthenticationMethodSystemNames == null)
                return false;

            return settings.ActiveAuthenticationMethodSystemNames.Contains(method.Metadata.SystemName, StringComparer.OrdinalIgnoreCase);
        }

        #endregion Public Methods
    }
}