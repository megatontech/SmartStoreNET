using SmartStore.Core.Domain.Customers;
using System.Collections.Generic;

namespace SmartStore.Services.Customers
{
    public class CustomerExportedEvent
    {
        #region Public Constructors

        public CustomerExportedEvent(Customer customer, IDictionary<string, object> result)
        {
            Guard.NotNull(customer, nameof(customer));
            Guard.NotNull(result, nameof(result));

            Customer = customer;
            Result = result;
        }

        #endregion Public Constructors



        #region Public Properties

        public Customer Customer { get; private set; }

        public IDictionary<string, object> Result { get; private set; }

        #endregion Public Properties
    }
}