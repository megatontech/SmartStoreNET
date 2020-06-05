//Contributor:  Nicholas Mayne

namespace SmartStore.Services.Authentication.External
{
    public partial interface IExternalAuthorizer
    {
        #region Public Methods

        AuthorizationResult Authorize(OpenAuthenticationParameters parameters);

        #endregion Public Methods
    }
}