using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Events;
using SmartStore.Core.Localization;
using SmartStore.Core.Plugins;
using SmartStore.Data.Caching;
using SmartStore.Services.Catalog;
using SmartStore.Services.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Directory
{
    public partial class DeliveryTimeService : IDeliveryTimeService
    {
        #region Private Fields

        private readonly IRepository<ProductVariantAttributeCombination> _attributeCombinationRepository;

        private readonly CatalogSettings _catalogSettings;

        private readonly ICustomerService _customerService;

        private readonly IRepository<DeliveryTime> _deliveryTimeRepository;

        private readonly IEventPublisher _eventPublisher;

        private readonly IPluginFinder _pluginFinder;

        private readonly IRepository<Product> _productRepository;

        #endregion Private Fields

        #region Public Constructors

        public DeliveryTimeService(
            IRepository<DeliveryTime> deliveryTimeRepository,
            IRepository<Product> productRepository,
            IRepository<ProductVariantAttributeCombination> attributeCombinationRepository,
            ICustomerService customerService,
            IPluginFinder pluginFinder,
            IEventPublisher eventPublisher,
            CatalogSettings catalogSettings)
        {
            this._deliveryTimeRepository = deliveryTimeRepository;
            this._customerService = customerService;
            this._pluginFinder = pluginFinder;
            this._eventPublisher = eventPublisher;
            this._productRepository = productRepository;
            this._attributeCombinationRepository = attributeCombinationRepository;
            this._catalogSettings = catalogSettings;

            T = NullLocalizer.Instance;
        }

        #endregion Public Constructors



        #region Public Properties

        public Localizer T { get; set; }

        #endregion Public Properties



        #region Public Methods

        public virtual void DeleteDeliveryTime(DeliveryTime deliveryTime)
        {
            if (deliveryTime == null)
                throw new ArgumentNullException("deliveryTime");

            if (this.IsAssociated(deliveryTime.Id))
                throw new SmartException(T("Admin.Configuration.DeliveryTimes.CannotDeleteAssignedProducts"));

            _deliveryTimeRepository.Delete(deliveryTime);
        }

        public virtual IList<DeliveryTime> GetAllDeliveryTimes()
        {
            var query = _deliveryTimeRepository.Table.OrderBy(c => c.DisplayOrder);
            var deliveryTimes = query.ToListCached("db.delivtimes.all");
            return deliveryTimes;
        }

        public virtual DeliveryTime GetDefaultDeliveryTime()
        {
            return _deliveryTimeRepository.Table.Where(x => x.IsDefault == true).FirstOrDefault();
        }

        public virtual DeliveryTime GetDeliveryTime(Product product)
        {
            var deliveryTimeId = product.GetDeliveryTimeIdAccordingToStock(_catalogSettings);
            return GetDeliveryTimeById(deliveryTimeId ?? 0);
        }

        public virtual DeliveryTime GetDeliveryTimeById(int deliveryTimeId)
        {
            if (deliveryTimeId == 0)
            {
                if (_catalogSettings.ShowDefaultDeliveryTime)
                {
                    return GetDefaultDeliveryTime();
                }
                else
                {
                    return null;
                }
            }

            return _deliveryTimeRepository.GetByIdCached(deliveryTimeId, "deliverytime-{0}".FormatInvariant(deliveryTimeId));
        }

        public virtual void InsertDeliveryTime(DeliveryTime deliveryTime)
        {
            if (deliveryTime == null)
                throw new ArgumentNullException("deliveryTime");

            _deliveryTimeRepository.Insert(deliveryTime);
        }

        public virtual bool IsAssociated(int deliveryTimeId)
        {
            if (deliveryTimeId == 0)
                return false;

            var query =
                from p in _productRepository.Table
                where p.DeliveryTimeId == deliveryTimeId || p.ProductVariantAttributeCombinations.Any(c => c.DeliveryTimeId == deliveryTimeId)
                select p.Id;

            return query.Count() > 0;
        }

        public virtual void SetToDefault(DeliveryTime deliveryTime)
        {
            if (deliveryTime == null)
                throw new ArgumentNullException("deliveryTime");

            var deliveryTimes = GetAllDeliveryTimes();

            foreach (var time in deliveryTimes)
            {
                time.IsDefault = time.Equals(deliveryTime) ? true : false;
                _deliveryTimeRepository.Update(time);
            }
        }

        public virtual void UpdateDeliveryTime(DeliveryTime deliveryTime)
        {
            if (deliveryTime == null)
                throw new ArgumentNullException("deliveryTime");

            _deliveryTimeRepository.Update(deliveryTime);
        }

        #endregion Public Methods
    }
}