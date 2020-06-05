//Contributor:  Nicholas Mayne

using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Plugins;
using SmartStore.Services.Configuration;
using SmartStore.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Authentication.External
{
    public partial class OpenAuthenticationService : IOpenAuthenticationService
    {
        #region Private Fields

        private readonly ICustomerService _customerService;

        private readonly IRepository<ExternalAuthenticationRecord> _externalAuthenticationRecordRepository;

        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;

        private readonly IPluginFinder _pluginFinder;

        private readonly IProviderManager _providerManager;

        private readonly ISettingService _settingService;

        #endregion Private Fields

        #region Public Constructors

        public OpenAuthenticationService(
            IRepository<ExternalAuthenticationRecord> externalAuthenticationRecordRepository,
            IPluginFinder pluginFinder,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            ICustomerService customerService,
            ISettingService settingService,
            IProviderManager providerManager)
        {
            this._externalAuthenticationRecordRepository = externalAuthenticationRecordRepository;
            this._pluginFinder = pluginFinder;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._customerService = customerService;
            this._settingService = settingService;
            this._providerManager = providerManager;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual bool AccountExists(OpenAuthenticationParameters parameters)
        {
            return GetUser(parameters) != null;
        }

        public virtual void AssociateExternalAccountWithUser(Customer customer, OpenAuthenticationParameters parameters)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            //find email
            string email = null;
            if (parameters.UserClaims != null)
                foreach (var userClaim in parameters.UserClaims
                    .Where(x => x.Contact != null && !String.IsNullOrEmpty(x.Contact.Email)))
                {
                    //found
                    email = userClaim.Contact.Email;
                    break;
                }

            var externalAuthenticationRecord = new ExternalAuthenticationRecord()
            {
                CustomerId = customer.Id,
                Email = email,
                ExternalIdentifier = parameters.ExternalIdentifier,
                ExternalDisplayIdentifier = parameters.ExternalDisplayIdentifier,
                OAuthToken = parameters.OAuthToken,
                OAuthAccessToken = parameters.OAuthAccessToken,
                ProviderSystemName = parameters.ProviderSystemName,
            };

            _externalAuthenticationRecordRepository.Insert(externalAuthenticationRecord);
        }

        public virtual IList<ExternalAuthenticationRecord> GetExternalIdentifiersFor(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            return customer.ExternalAuthenticationRecords.ToList();
        }

        public virtual Customer GetUser(OpenAuthenticationParameters parameters)
        {
            var record = _externalAuthenticationRecordRepository.Table
                .Where(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.ProviderSystemName == parameters.ProviderSystemName)
                .FirstOrDefault();

            if (record != null)
                return _customerService.GetCustomerById(record.CustomerId);

            return null;
        }

        /// <summary>
        /// Load active external authentication methods
        /// </summary>
		/// <param name="storeId">Load records allows only in specified store; pass 0 to load all records</param>
        /// <returns>Payment methods</returns>
		public virtual IEnumerable<Provider<IExternalAuthenticationMethod>> LoadActiveExternalAuthenticationMethods(int storeId = 0)
        {
            var allMethods = LoadAllExternalAuthenticationMethods(storeId);
            var activeMethods = allMethods
                   .Where(p => _externalAuthenticationSettings.ActiveAuthenticationMethodSystemNames.Contains(p.Metadata.SystemName, StringComparer.InvariantCultureIgnoreCase));

            return activeMethods;
        }

        /// <summary>
        /// Load all external authentication methods
        /// </summary>
        /// <param name="storeId">Load records allows only in specified store; pass 0 to load all records</param>
        /// <returns>External authentication methods</returns>
        public virtual IEnumerable<Provider<IExternalAuthenticationMethod>> LoadAllExternalAuthenticationMethods(int storeId = 0)
        {
            return _providerManager.GetAllProviders<IExternalAuthenticationMethod>(storeId);
        }

        /// <summary>
        /// Load external authentication method by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found external authentication method</returns>
		public virtual Provider<IExternalAuthenticationMethod> LoadExternalAuthenticationMethodBySystemName(string systemName, int storeId = 0)
        {
            return _providerManager.GetProvider<IExternalAuthenticationMethod>(systemName, storeId);
        }

        public virtual void RemoveAssociation(OpenAuthenticationParameters parameters)
        {
            var record = _externalAuthenticationRecordRepository.Table
                .Where(o => o.ExternalIdentifier == parameters.ExternalIdentifier && o.ProviderSystemName == parameters.ProviderSystemName)
                .FirstOrDefault();

            if (record != null)
                _externalAuthenticationRecordRepository.Delete(record);
        }

        #endregion Public Methods
    }
}