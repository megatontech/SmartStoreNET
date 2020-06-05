using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService
    {
        #region Public Methods

        Customer GetAuthenticatedCustomer();

        void SignIn(Customer customer, bool createPersistentCookie);

        void SignOut();

        #endregion Public Methods
    }
}