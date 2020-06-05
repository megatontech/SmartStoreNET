using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Plugins;
using System.Collections.Generic;

namespace SmartStore.Services.Directory
{
    /// <summary>
    /// Exchange rate provider interface
    /// </summary>
    public partial interface IExchangeRateProvider : IProvider
    {
        #region Public Methods

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode);

        #endregion Public Methods
    }
}