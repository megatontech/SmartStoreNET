//Contributor:  Nicholas Mayne

namespace SmartStore.Services.Authentication.External
{
    public partial interface IClaimsTranslator<T>
    {
        #region Public Methods

        UserClaims Translate(T response);

        #endregion Public Methods
    }
}