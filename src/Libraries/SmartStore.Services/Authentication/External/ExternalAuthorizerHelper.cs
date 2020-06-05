//Contributor:  Nicholas Mayne

using SmartStore.Core.Infrastructure;
using System.Web;

namespace SmartStore.Services.Authentication.External
{
    public static partial class ExternalAuthorizerHelper
    {
        #region Public Methods

        public static void RemoveParameters()
        {
            var session = GetSession();
            session.Remove("sm.externalauth.parameters");
        }

        public static OpenAuthenticationParameters RetrieveParametersFromRoundTrip(bool removeOnRetrieval)
        {
            var session = GetSession();
            var parameters = session["sm.externalauth.parameters"];
            if (parameters != null && removeOnRetrieval)
                RemoveParameters();

            return parameters as OpenAuthenticationParameters;
        }

        public static void StoreParametersForRoundTrip(OpenAuthenticationParameters parameters)
        {
            var session = GetSession();
            session["sm.externalauth.parameters"] = parameters;
        }

        #endregion Public Methods



        #region Private Methods

        private static HttpSessionStateBase GetSession()
        {
            var session = EngineContext.Current.Resolve<HttpSessionStateBase>();
            return session;
        }

        #endregion Private Methods
    }
}