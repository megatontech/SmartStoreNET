using SmartStore.Services.Calc;
using SmartStore.Services.Customers;
using SmartStore.Services.Tasks;
using System;
using System.Threading.Tasks;

namespace SmartStore.Services.Calc
{
    /// <summary>
    /// Represents a task for deleting guest customers
    /// </summary>
    public partial class CalcRewardThreeTask : ITask
    {
        #region Private Fields

        private readonly ICustomerService _customerService;
        private readonly ICalcRewardService _calcService;
        
        #endregion Private Fields

        #region Public Constructors

        public CalcRewardThreeTask(ICustomerService customerService, ICalcRewardService calcService)
        {
            _customerService = customerService;
            _calcService = calcService;
        }

        public void Execute(TaskExecutionContext ctx)
        {
            var total = 0M;
             _calcService.CalcRewardThreeAsync(total);
        }

        #endregion Public Constructors



        #region Public Methods

        //public override async Task ExecuteAsync(TaskExecutionContext ctx)
        //{
        //    //60*24 = 1 day
        //    var olderThanMinutes = 1440; // TODO: move to settings
        //    var total = 0M;
        //    await _calcService.CalcRewardThreeAsync(total);
        //}

        #endregion Public Methods
    }
}