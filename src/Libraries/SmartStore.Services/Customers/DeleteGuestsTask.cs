using SmartStore.Services.Tasks;
using System;
using System.Threading.Tasks;

namespace SmartStore.Services.Customers
{
    /// <summary>
    /// Represents a task for deleting guest customers
    /// </summary>
    public partial class DeleteGuestsTask : AsyncTask
    {
        #region Private Fields

        private readonly ICustomerService _customerService;

        #endregion Private Fields

        #region Public Constructors

        public DeleteGuestsTask(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion Public Constructors



        #region Public Methods

        public override async Task ExecuteAsync(TaskExecutionContext ctx)
        {
            //60*24 = 1 day
            var olderThanMinutes = 1440; // TODO: move to settings
            await _customerService.DeleteGuestCustomersAsync(null, DateTime.UtcNow.AddMinutes(-olderThanMinutes), true);
        }

        #endregion Public Methods
    }
}