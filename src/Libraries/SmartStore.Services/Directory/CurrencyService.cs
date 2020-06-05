using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Stores;
using SmartStore.Core.Events;
using SmartStore.Core.Plugins;
using SmartStore.Data.Caching;
using SmartStore.Services.Stores;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Directory
{
    public partial class CurrencyService : ICurrencyService
    {
        #region Private Fields

        private readonly IRepository<Currency> _currencyRepository;

        private readonly CurrencySettings _currencySettings;

        private readonly IEventPublisher _eventPublisher;

        private readonly IPluginFinder _pluginFinder;

        private readonly IProviderManager _providerManager;

        private readonly IStoreContext _storeContext;

        private readonly IStoreMappingService _storeMappingService;

        #endregion Private Fields

        #region Public Constructors

        public CurrencyService(
            IRepository<Currency> currencyRepository,
            IStoreMappingService storeMappingService,
            CurrencySettings currencySettings,
            IPluginFinder pluginFinder,
            IEventPublisher eventPublisher,
            IProviderManager providerManager,
            IStoreContext storeContext)
        {
            _currencyRepository = currencyRepository;
            _storeMappingService = storeMappingService;
            _currencySettings = currencySettings;
            _pluginFinder = pluginFinder;
            _eventPublisher = eventPublisher;
            _providerManager = providerManager;
            _storeContext = storeContext;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;

            return decimal.Zero;
        }

        public virtual decimal ConvertCurrency(decimal amount, Currency sourceCurrency, Currency targetCurrency, Store store = null)
        {
            decimal result = amount;
            if (sourceCurrency.Id == targetCurrency.Id)
                return result;

            if (result != decimal.Zero && sourceCurrency.Id != targetCurrency.Id)
            {
                result = ConvertToPrimaryExchangeRateCurrency(result, sourceCurrency, store);
                result = ConvertFromPrimaryExchangeRateCurrency(result, targetCurrency, store);
            }

            return result;
        }

        public virtual decimal ConvertFromPrimaryExchangeRateCurrency(decimal amount, Currency targetCurrency, Store store = null)
        {
            decimal result = amount;
            var primaryExchangeRateCurrency = (store == null ? _storeContext.CurrentStore.PrimaryExchangeRateCurrency : store.PrimaryExchangeRateCurrency);

            if (result != decimal.Zero && targetCurrency.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = targetCurrency.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new SmartException(string.Format("Exchange rate not found for currency [{0}]", targetCurrency.Name));
                result = result * exchangeRate;
            }
            return result;
        }

        public virtual decimal ConvertFromPrimaryStoreCurrency(decimal amount, Currency targetCurrency, Store store = null)
        {
            decimal result = amount;
            var primaryStoreCurrency = (store == null ? _storeContext.CurrentStore.PrimaryStoreCurrency : store.PrimaryStoreCurrency);
            result = ConvertCurrency(amount, primaryStoreCurrency, targetCurrency, store);
            return result;
        }

        public virtual decimal ConvertToPrimaryExchangeRateCurrency(decimal amount, Currency sourceCurrency, Store store = null)
        {
            decimal result = amount;
            var primaryExchangeRateCurrency = (store == null ? _storeContext.CurrentStore.PrimaryExchangeRateCurrency : store.PrimaryExchangeRateCurrency);

            if (result != decimal.Zero && sourceCurrency.Id != primaryExchangeRateCurrency.Id)
            {
                decimal exchangeRate = sourceCurrency.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new SmartException(string.Format("Exchange rate not found for currency [{0}]", sourceCurrency.Name));
                result = result / exchangeRate;
            }
            return result;
        }

        public virtual decimal ConvertToPrimaryStoreCurrency(decimal amount, Currency sourceCurrency, Store store = null)
        {
            decimal result = amount;
            var primaryStoreCurrency = (store == null ? _storeContext.CurrentStore.PrimaryStoreCurrency : store.PrimaryStoreCurrency);

            if (result != decimal.Zero && sourceCurrency.Id != primaryStoreCurrency.Id)
            {
                decimal exchangeRate = sourceCurrency.Rate;
                if (exchangeRate == decimal.Zero)
                    throw new SmartException(string.Format("Exchange rate not found for currency [{0}]", sourceCurrency.Name));
                result = result / exchangeRate;
            }
            return result;
        }

        public virtual void DeleteCurrency(Currency currency)
        {
            Guard.NotNull(currency, nameof(currency));

            _currencyRepository.Delete(currency);
        }

        public virtual IList<Currency> GetAllCurrencies(bool showHidden = false, int storeId = 0)
        {
            var query = _currencyRepository.Table;

            if (!showHidden)
                query = query.Where(c => c.Published);

            query = query.OrderBy(c => c.DisplayOrder);

            var currencies = query.ToListCached();

            // store mapping
            if (storeId > 0)
            {
                currencies = currencies
                    .Where(c => _storeMappingService.Authorize(c, storeId))
                    .ToList();
            }
            return currencies;
        }

        public virtual Currency GetCurrencyByCode(string currencyCode)
        {
            Guard.NotNull(currencyCode, nameof(currencyCode));

            return GetAllCurrencies(true).FirstOrDefault(c => c.CurrencyCode.ToLower() == currencyCode.ToLower());
        }

        public virtual Currency GetCurrencyById(int currencyId)
        {
            if (currencyId == 0)
                return null;

            return _currencyRepository.GetByIdCached(currencyId, "db.cur.id-" + currencyId);
        }

        public virtual IList<ExchangeRate> GetCurrencyLiveRates(string exchangeRateCurrencyCode)
        {
            var exchangeRateProvider = LoadActiveExchangeRateProvider();
            if (exchangeRateProvider != null)
            {
                return exchangeRateProvider.Value.GetCurrencyLiveRates(exchangeRateCurrencyCode);
            }
            return new List<ExchangeRate>();
        }

        public virtual void InsertCurrency(Currency currency)
        {
            Guard.NotNull(currency, nameof(currency));

            _currencyRepository.Insert(currency);
        }

        public virtual Provider<IExchangeRateProvider> LoadActiveExchangeRateProvider()
        {
            return LoadExchangeRateProviderBySystemName(_currencySettings.ActiveExchangeRateProviderSystemName) ?? LoadAllExchangeRateProviders().FirstOrDefault();
        }

        public virtual IEnumerable<Provider<IExchangeRateProvider>> LoadAllExchangeRateProviders()
        {
            return _providerManager.GetAllProviders<IExchangeRateProvider>();
        }

        public virtual Provider<IExchangeRateProvider> LoadExchangeRateProviderBySystemName(string systemName)
        {
            return _providerManager.GetProvider<IExchangeRateProvider>(systemName);
        }

        public virtual void UpdateCurrency(Currency currency)
        {
            Guard.NotNull(currency, nameof(currency));

            _currencyRepository.Update(currency);
        }

        #endregion Public Methods
    }
}