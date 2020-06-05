using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Customers
{
    /// <summary>
    /// An event message, which will be published after customer has registered
    /// </summary>
    public class CustomerRegisteredEvent
    {
        #region Public Properties

        public Customer Customer
        {
            get;
            set;
        }

        #endregion Public Properties
    }
}