using Autofac;
using AutoMapper;
using NuGet;
using SmartStore.Admin.Models.Catalog;
using SmartStore.Collections;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Seo;
using SmartStore.Core.Events;
using SmartStore.Core.Logging;
using SmartStore.Core.Search;
using SmartStore.Data.Utilities;
using SmartStore.Services;
using SmartStore.Services.Catalog;
using SmartStore.Services.Catalog.Extensions;
using SmartStore.Services.Catalog.Modelling;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using SmartStore.Services.Discounts;
using SmartStore.Services.Helpers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Orders;
using SmartStore.Services.Search;
using SmartStore.Services.Security;
using SmartStore.Services.Seo;
using SmartStore.Services.Stores;
using SmartStore.Services.Tax;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Modelling;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单系统管理/报单产品管理
    /// </summary>
    [AdminAuthorize]
    public partial class DeclarationProductController : AdminControllerBase
    {
        #region Fields

        private readonly IAclService _aclService;
        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly CatalogSettings _catalogSettings;
        private readonly ICategoryService _categoryService;
        private readonly ICopyProductService _copyProductService;
        private readonly ICountryService _countryService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDbContext _dbContext;
        private readonly IDeliveryTimeService _deliveryTimesService;
        private readonly IDiscountService _discountService;
        private readonly IDownloadService _downloadService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IDeclarationProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ProductUrlHelper _productUrlHelper;
        private readonly IRepository<ProductVariantAttributeCombination> _pvacRepository;
        private readonly IQuantityUnitService _quantityUnitService;
        private readonly SearchSettings _searchSettings;
        private readonly SeoSettings _seoSettings;
        private readonly ICommonServices _services;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Constructors

        public DeclarationProductController(
            IDeclarationProductService productService,
            IProductTemplateService productTemplateService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ICustomerService customerService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ISpecificationAttributeService specificationAttributeService,
            IPictureService pictureService,
            ITaxCategoryService taxCategoryService,
            IProductTagService productTagService,
            ICopyProductService copyProductService,
            ICustomerActivityService customerActivityService,
            IPermissionService permissionService,
            IAclService aclService,
            IStoreService storeService,
            IStoreMappingService storeMappingService,
            AdminAreaSettings adminAreaSettings,
            IDateTimeHelper dateTimeHelper,
            IDiscountService discountService,
            IProductAttributeService productAttributeService,
            IRepository<ProductVariantAttributeCombination> pvacRepository,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IShoppingCartService shoppingCartService,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser,
            CatalogSettings catalogSettings,
            IDownloadService downloadService,
            IDeliveryTimeService deliveryTimesService,
            IQuantityUnitService quantityUnitService,
            IMeasureService measureService,
            MeasureSettings measureSettings,
            IPriceFormatter priceFormatter,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            ICommonServices services,
            ICountryService countryService,
            ICatalogSearchService catalogSearchService,
            ProductUrlHelper productUrlHelper,
            SeoSettings seoSettings,
            MediaSettings mediaSettings,
            SearchSettings searchSettings)
        {
            _productService = productService;
            _productTemplateService = productTemplateService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _customerService = customerService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
            _languageService = languageService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _specificationAttributeService = specificationAttributeService;
            _pictureService = pictureService;
            _taxCategoryService = taxCategoryService;
            _productTagService = productTagService;
            _copyProductService = copyProductService;
            _customerActivityService = customerActivityService;
            _permissionService = permissionService;
            _aclService = aclService;
            _storeService = storeService;
            _storeMappingService = storeMappingService;
            _adminAreaSettings = adminAreaSettings;
            _dateTimeHelper = dateTimeHelper;
            _discountService = discountService;
            _productAttributeService = productAttributeService;
            _pvacRepository = pvacRepository;
            _backInStockSubscriptionService = backInStockSubscriptionService;
            _shoppingCartService = shoppingCartService;
            _productAttributeFormatter = productAttributeFormatter;
            _productAttributeParser = productAttributeParser;
            _catalogSettings = catalogSettings;
            _downloadService = downloadService;
            _deliveryTimesService = deliveryTimesService;
            _quantityUnitService = quantityUnitService;
            _measureService = measureService;
            _measureSettings = measureSettings;
            _priceFormatter = priceFormatter;
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
            _genericAttributeService = genericAttributeService;
            _services = services;
            _countryService = countryService;
            _catalogSearchService = catalogSearchService;
            _productUrlHelper = productUrlHelper;
            _seoSettings = seoSettings;
            _mediaSettings = mediaSettings;
            _searchSettings = searchSettings;
            //Mapper.Initialize(cfg => cfg.CreateMap<DeclarationProduct, DeclarationProduct>());
        }

        #endregion Constructors

        #region Update[...]

        [NonAction]
        protected void UpdateProductBundleItems(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.BundleTitleText = m.BundleTitleText;
            p.BundlePerItemPricing = m.BundlePerItemPricing;
            p.BundlePerItemShipping = m.BundlePerItemShipping;
            p.BundlePerItemShoppingCart = m.BundlePerItemShoppingCart;

            // SEO
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(p, x => x.BundleTitleText, localized.BundleTitleText, localized.LanguageId);
            }
        }

        [NonAction]
        protected void UpdateProductDownloads(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.IsDownload = m.IsDownload;
            //p.DownloadId = m.DownloadId ?? 0;
            p.UnlimitedDownloads = m.UnlimitedDownloads;
            p.MaxNumberOfDownloads = m.MaxNumberOfDownloads;
            p.DownloadExpirationDays = m.DownloadExpirationDays;
            p.DownloadActivationTypeId = m.DownloadActivationTypeId;
            p.HasUserAgreement = m.HasUserAgreement;
            p.UserAgreementText = m.UserAgreementText;
            p.HasSampleDownload = m.HasSampleDownload;
            p.SampleDownloadId = m.SampleDownloadId == 0 ? (int?)null : m.SampleDownloadId;

            if (m.NewVersionDownloadId != 0 && m.NewVersionDownloadId != null)
            {
                // set version info & product id for new download
                var newVersion = _downloadService.GetDownloadById((int)m.NewVersionDownloadId);
                newVersion.FileVersion = m.NewVersion != null ? m.NewVersion : String.Empty;
                newVersion.EntityId = p.Id;
                newVersion.IsTransient = false;
                _downloadService.UpdateDownload(newVersion, newVersion.MediaStorage.Data);
            }
            else if (m.DownloadFileVersion.HasValue())
            {
                var download = _downloadService.GetDownloadsFor(p).FirstOrDefault();
                if (download != null)
                {
                    download.FileVersion = new SemanticVersion(m.DownloadFileVersion).ToString();
                    download.EntityId = p.Id;
                    download.IsTransient = false;
                    _downloadService.UpdateDownload(download);
                }
            }
        }

        [NonAction]
        protected void UpdateProductGeneralInfo(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.ProductTypeId = m.ProductTypeId;
            p.VisibleIndividually = m.VisibleIndividually;
            p.ProductTemplateId = m.ProductTemplateId;

            p.Name = m.Name;
            p.ShortDescription = m.ShortDescription;
            p.FullDescription = m.FullDescription;
            p.Sku = m.Sku;
            p.ManufacturerPartNumber = m.ManufacturerPartNumber;
            p.Gtin = m.Gtin;
            p.AdminComment = m.AdminComment;
            p.AvailableStartDateTimeUtc = m.AvailableStartDateTimeUtc;
            p.AvailableEndDateTimeUtc = m.AvailableEndDateTimeUtc;

            p.AllowCustomerReviews = m.AllowCustomerReviews;
            p.ShowOnHomePage = m.ShowOnHomePage;
            p.HomePageDisplayOrder = m.HomePageDisplayOrder;
            p.Published = m.Published;
            p.RequireOtherProducts = m.RequireOtherProducts;
            p.RequiredProductIds = m.RequiredProductIds;
            p.AutomaticallyAddRequiredProducts = m.AutomaticallyAddRequiredProducts;

            p.IsGiftCard = m.IsGiftCard;
            p.GiftCardTypeId = m.GiftCardTypeId;

            p.IsRecurring = m.IsRecurring;
            p.RecurringCycleLength = m.RecurringCycleLength;
            p.RecurringCyclePeriodId = m.RecurringCyclePeriodId;
            p.RecurringTotalCycles = m.RecurringTotalCycles;

            p.IsShipEnabled = m.IsShipEnabled;
            p.DeliveryTimeId = m.DeliveryTimeId == 0 ? (int?)null : m.DeliveryTimeId;
            p.QuantityUnitId = m.QuantityUnitId == 0 ? (int?)null : m.QuantityUnitId;
            p.IsFreeShipping = m.IsFreeShipping;
            p.AdditionalShippingCharge = m.AdditionalShippingCharge;
            p.Weight = m.Weight;
            p.Length = m.Length;
            p.Width = m.Width;
            p.Height = m.Height;

            p.IsEsd = m.IsEsd;
            p.IsTaxExempt = m.IsTaxExempt;
            p.TaxCategoryId = m.TaxCategoryId ?? 0;
            p.CustomsTariffNumber = m.CustomsTariffNumber;
            p.CountryOfOriginId = m.CountryOfOriginId == 0 ? (int?)null : m.CountryOfOriginId;

            p.AvailableEndDateTimeUtc = p.AvailableEndDateTimeUtc.ToEndOfTheDay();
            p.SpecialPriceEndDateTimeUtc = p.SpecialPriceEndDateTimeUtc.ToEndOfTheDay();
        }

        [NonAction]
        protected void UpdateProductInventory(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            var prevStockQuantity = product.StockQuantity;

            p.ManageInventoryMethodId = m.ManageInventoryMethodId;
            p.StockQuantity = m.StockQuantity;
            p.DisplayStockAvailability = m.DisplayStockAvailability;
            p.DisplayStockQuantity = m.DisplayStockQuantity;
            p.MinStockQuantity = m.MinStockQuantity;
            p.LowStockActivityId = m.LowStockActivityId;
            p.NotifyAdminForQuantityBelow = m.NotifyAdminForQuantityBelow;
            p.BackorderModeId = m.BackorderModeId;
            p.AllowBackInStockSubscriptions = m.AllowBackInStockSubscriptions;
            p.OrderMinimumQuantity = m.OrderMinimumQuantity;
            p.OrderMaximumQuantity = m.OrderMaximumQuantity;
            p.QuantityStep = m.QuantityStep;
            p.HideQuantityControl = m.HideQuantityControl;

            // back in stock notifications
            if (p.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                p.BackorderMode == BackorderMode.NoBackorders &&
                p.AllowBackInStockSubscriptions &&
                p.StockQuantity > 0 &&
                prevStockQuantity <= 0 &&
                p.Published &&
                !p.Deleted &&
                !p.IsSystemProduct)
            {
                //_backInStockSubscriptionService.SendNotificationsToSubscribers(p);
            }

            if (p.StockQuantity != prevStockQuantity && p.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
            {
                _productService.AdjustInventory(p, true, 0, string.Empty);
            }
        }

        [NonAction]
        protected void UpdateProductPictures(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.HasPreviewPicture = m.HasPreviewPicture;
        }

        //        if (!product.ProductTagExists(productTag.Id))
        //        {
        //            product.ProductTags.Add(productTag);
        //            _productService.UpdateProduct(product);
        //            _services.DbContext.ChangeState(product, System.Data.Entity.EntityState.Modified);
        //        }
        //    }
        //}
        [NonAction]
        protected void UpdateProductPrice(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.Price = m.Price;
            p.OldPrice = m.OldPrice;
            p.ProductCost = m.ProductCost;
            p.SpecialPrice = m.SpecialPrice;
            p.SpecialPriceStartDateTimeUtc = m.SpecialPriceStartDateTimeUtc;
            p.SpecialPriceEndDateTimeUtc = m.SpecialPriceEndDateTimeUtc;
            p.DisableBuyButton = m.DisableBuyButton;
            p.DisableWishlistButton = m.DisableWishlistButton;
            p.AvailableForPreOrder = m.AvailableForPreOrder;
            p.CallForPrice = m.CallForPrice;
            p.CustomerEntersPrice = m.CustomerEntersPrice;
            p.MinimumCustomerEnteredPrice = m.MinimumCustomerEnteredPrice;
            p.MaximumCustomerEnteredPrice = m.MaximumCustomerEnteredPrice;

            p.BasePriceEnabled = m.BasePriceEnabled;
            p.BasePriceBaseAmount = m.BasePriceBaseAmount;
            p.BasePriceAmount = m.BasePriceAmount;
            p.BasePriceMeasureUnit = m.BasePriceMeasureUnit;
        }

        //        if (productTag2 == null)
        //        {
        //            // Add new product tag
        //            productTag = new ProductTag { Name = productTagName };
        //            _productTagService.InsertProductTag(productTag);
        //        }
        //        else
        //        {
        //            productTag = productTag2;
        //        }
        [NonAction]
        protected void UpdateProductSeo(DeclarationProduct product, DeclarationProductModel model)
        {
            var p = product;
            var m = model;

            p.MetaKeywords = m.MetaKeywords;
            p.MetaDescription = m.MetaDescription;
            p.MetaTitle = m.MetaTitle;

            // SEO
            var service = _localizedEntityService;
            foreach (var localized in model.Locales)
            {
                service.SaveLocalizedValue(p, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                service.SaveLocalizedValue(p, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                service.SaveLocalizedValue(p, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
            }
        }

        private DeclarationProduct convertDeclaration(DeclarationProduct dproduct)
        {
            DeclarationProduct dto = Mapper.Map<DeclarationProduct>(dproduct);
            return dto;
        }

        //private Product convertDeclaration(DeclarationProduct dproduct)
        //{
        //    Product dto = Mapper.Map<DeclarationProduct>(dproduct);
        //    return dto;
        //}

        //[NonAction]
        //protected void UpdateProductTags(DeclarationProduct product, params string[] rawProductTags)
        //{
        //    Guard.NotNull(product, nameof(product));

        //    if (rawProductTags == null || rawProductTags.Length == 0)
        //    {
        //        product.ProductTags.Clear();
        //        _productService.UpdateProduct(product);
        //        return;
        //    }

        //    var productTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        //    foreach (string str in rawProductTags)
        //    {
        //        string tag = str.TrimSafe();
        //        if (tag.HasValue())
        //            productTags.Add(tag);
        //    }

        //    var existingProductTags = product.ProductTags;
        //    var productTagsToRemove = new List<ProductTag>();

        //    foreach (var existingProductTag in existingProductTags)
        //    {
        //        bool found = false;
        //        foreach (string newProductTag in productTags)
        //        {
        //            if (existingProductTag.Name.Equals(newProductTag, StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                found = true;
        //                break;
        //            }
        //        }
        //        if (!found)
        //        {
        //            productTagsToRemove.Add(existingProductTag);
        //        }
        //    }

        //    foreach (var productTag in productTagsToRemove)
        //    {
        //        product.ProductTags.Remove(productTag);
        //        _productService.UpdateProduct(product);
        //        _services.DbContext.ChangeState(product, System.Data.Entity.EntityState.Modified);
        //    }

        [NonAction]
        private void UpdateDataOfExistingProduct(DeclarationProduct product, DeclarationProductModel model, bool editMode)
        {
            var p = product;
            var m = model;

            var nameChanged = editMode ? _dbContext.IsPropertyModified(p, x => x.Name) : false;
            var seoTabLoaded = m.LoadedTabs.Contains("SEO", StringComparer.OrdinalIgnoreCase);

            // Handle Download transiency
            var download = _downloadService.GetDownloadsFor(p).FirstOrDefault();
            if (download != null)
            {
                MediaHelper.UpdateDownloadTransientStateFor(p, x => download.Id);
            }
            MediaHelper.UpdateDownloadTransientStateFor(p, x => x.SampleDownloadId);

            // SEO
            m.SeName = p.ValidateSeName(m.SeName, p.Name, true, _urlRecordService, _seoSettings);
            _urlRecordService.SaveSlug(p, m.SeName, 0);

            if (editMode)
            {
                // we need the event to be fired
                _productService.UpdateProduct(p);
            }

            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(product, x => x.Name, localized.Name, localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product, x => x.ShortDescription, localized.ShortDescription, localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product, x => x.FullDescription, localized.FullDescription, localized.LanguageId);

                // search engine name
                var localizedSeName = p.ValidateSeName(localized.SeName, localized.Name, false, _urlRecordService, _seoSettings, localized.LanguageId);
                _urlRecordService.SaveSlug(p, localizedSeName, localized.LanguageId);
            }

            // picture seo names
            if (nameChanged)
            {
                UpdatePictureSeoNames(p);
            }

            // product tags
            //UpdateProductTags(p, m.ProductTags);
        }

        //    foreach (string productTagName in productTags)
        //    {
        //        ProductTag productTag = null;
        //        var productTag2 = _productTagService.GetProductTagByName(productTagName);
        [NonAction]
        private void UpdateLocales(ProductTag productTag, ProductTagModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(productTag, x => x.Name, localized.Name, localized.LanguageId);
            }
        }

        [NonAction]
        private void UpdateLocales(ProductVariantAttributeValue pvav, DeclarationProductModel.ProductVariantAttributeValueModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(pvav, x => x.Name, localized.Name, localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(pvav, x => x.Alias, localized.Alias, localized.LanguageId);
            }
        }

        [NonAction]
        private void UpdatePictureSeoNames(DeclarationProduct product)
        {
            foreach (var pp in product.ProductPictures)
            {
                _pictureService.SetSeoFilename(pp.PictureId, _pictureService.GetPictureSeName(product.Name));
            }
        }

        #endregion Update[...]

        #region Utitilies

        [NonAction]
        protected void PrepareProductModel(DeclarationProductModel model, DeclarationProduct product, bool setPredefinedValues, bool excludeProperties)
        {
            Guard.NotNull(model, nameof(model));

            if (product != null)
            {
                var parentGroupedProduct = _productService.GetProductById(product.ParentGroupedProductId);
                if (parentGroupedProduct != null)
                {
                    model.AssociatedToProductId = product.ParentGroupedProductId;
                    model.AssociatedToProductName = parentGroupedProduct.Name;
                }

                model.CreatedOn = _dateTimeHelper.ConvertToUserTime(product.CreatedOnUtc, DateTimeKind.Utc);
                model.UpdatedOn = _dateTimeHelper.ConvertToUserTime(product.UpdatedOnUtc, DateTimeKind.Utc);

                if (product.LimitedToStores)
                {
                    var storeMappings = _storeMappingService.GetStoreMappings(product);
                    if (storeMappings.FirstOrDefault(x => x.StoreId == _services.StoreContext.CurrentStore.Id) == null)
                    {
                        var storeMapping = storeMappings.FirstOrDefault();
                        if (storeMapping != null)
                        {
                            var store = _services.StoreService.GetStoreById(storeMapping.StoreId);
                            if (store != null)
                                model.ProductUrl = store.Url.EnsureEndsWith("/") + product.GetSeName();
                        }
                    }
                }

                if (model.ProductUrl.IsEmpty())
                {
                    model.ProductUrl = Url.RouteUrl("DeclarationProduct", new { SeName = product.GetSeName() }, Request.Url.Scheme);
                }

                // downloads
                model.DownloadVersions = _downloadService.GetDownloadsFor(product)
                    .Select(x => new DownloadVersion
                    {
                        FileVersion = x.FileVersion,
                        DownloadId = x.Id,
                        FileName = string.Concat(x.Filename, x.Extension),
                        DownloadUrl = Url.Action("DownloadFile", "Download", new { downloadId = x.Id })
                    })
                    .ToList();

                var currentDownload = _downloadService.GetDownloadsFor(product).FirstOrDefault();

                model.DownloadId = currentDownload?.Id;
                model.DownloadFileVersion = (currentDownload?.FileVersion).EmptyNull();
            }

            model.PrimaryStoreCurrencyCode = _services.StoreContext.CurrentStore.PrimaryStoreCurrency.CurrencyCode;
            model.BaseWeightIn = _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId)?.Name;
            model.BaseDimensionIn = _measureService.GetMeasureDimensionById(_measureSettings.BaseDimensionId)?.Name;

            model.NumberOfAvailableProductAttributes = _productAttributeService.GetAllProductAttributes().Count;
            model.NumberOfAvailableManufacturers = _manufacturerService.GetAllManufacturers("", pageIndex: 0, pageSize: 1, showHidden: true).TotalCount;
            model.NumberOfAvailableCategories = _categoryService.GetAllCategories(pageIndex: 0, pageSize: 1, showHidden: true).TotalCount;

            // copy product
            if (product != null)
            {
                model.CopyProductModel.Id = product.Id;
                model.CopyProductModel.Name = T("Admin.Common.CopyOf", product.Name);
                model.CopyProductModel.Published = true;
                model.CopyProductModel.CopyImages = true;
            }

            // templates
            var templates = _productTemplateService.GetAllProductTemplates();
            foreach (var template in templates)
            {
                model.AvailableProductTemplates.Add(new SelectListItem
                {
                    Text = template.Name,
                    Value = template.Id.ToString()
                });
            }

            // DeclarationProduct tags
            if (product != null)
            {
                //model.ProductTags = product.ProductTags.Select(x => x.Name).ToArray();
            }

            var allTags = _productTagService.GetAllProductTagNames();
            model.AvailableProductTags = new MultiSelectList(allTags, model.ProductTags);

            // tax categories
            var taxCategories = _taxCategoryService.GetAllTaxCategories();
            foreach (var tc in taxCategories)
            {
                model.AvailableTaxCategories.Add(new SelectListItem
                {
                    Text = tc.Name,
                    Value = tc.Id.ToString(),
                    Selected = product != null && !setPredefinedValues && tc.Id == product.TaxCategoryId
                });
            }

            // Do not pre-select a tax category that is not stored.
            if (product != null && product.TaxCategoryId == 0)
            {
                model.AvailableTaxCategories.Insert(0, new SelectListItem { Text = T("Common.PleaseSelect"), Value = "", Selected = true });
            }

            // delivery times
            var defaultDeliveryTime = _deliveryTimesService.GetDefaultDeliveryTime();
            var deliveryTimes = _deliveryTimesService.GetAllDeliveryTimes();
            foreach (var dt in deliveryTimes)
            {
                var isSelected = false;
                if (setPredefinedValues)
                {
                    isSelected = (defaultDeliveryTime != null && dt.Id == defaultDeliveryTime.Id);
                }
                else
                {
                    isSelected = (product != null && dt.Id == product.DeliveryTimeId.GetValueOrDefault());
                }

                model.AvailableDeliveryTimes.Add(new SelectListItem
                {
                    Text = dt.Name,
                    Value = dt.Id.ToString(),
                    Selected = isSelected
                });
            }

            // quantity units
            var quantityUnits = _quantityUnitService.GetAllQuantityUnits();
            foreach (var mu in quantityUnits)
            {
                model.AvailableQuantityUnits.Add(new SelectListItem
                {
                    Text = mu.Name,
                    Value = mu.Id.ToString(),
                    Selected = product != null && !setPredefinedValues && mu.Id == product.QuantityUnitId.GetValueOrDefault()
                });
            }

            // BasePrice aka PAnGV
            var measureUnits = _measureService.GetAllMeasureWeights()
                .Select(x => x.SystemKeyword).Concat(_measureService.GetAllMeasureDimensions().Select(x => x.SystemKeyword)).ToList();

            // don't forget biz import!
            if (product != null && !setPredefinedValues && product.BasePriceMeasureUnit.HasValue() && !measureUnits.Exists(u => u.IsCaseInsensitiveEqual(product.BasePriceMeasureUnit)))
            {
                measureUnits.Add(product.BasePriceMeasureUnit);
            }

            foreach (var mu in measureUnits)
            {
                model.AvailableMeasureUnits.Add(new SelectListItem
                {
                    Text = mu,
                    Value = mu,
                    Selected = product != null && !setPredefinedValues && mu.Equals(product.BasePriceMeasureUnit, StringComparison.OrdinalIgnoreCase)
                });
            }

            // specification attributes
            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes().ToList();
            for (int i = 0; i < specificationAttributes.Count; i++)
            {
                var sa = specificationAttributes[i];
                model.AddSpecificationAttributeModel.AvailableAttributes.Add(new SelectListItem { Text = sa.Name, Value = sa.Id.ToString() });
                if (i == 0)
                {
                    //attribute options
                    foreach (var sao in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(sa.Id))
                    {
                        model.AddSpecificationAttributeModel.AvailableOptions.Add(new SelectListItem { Text = sao.Name, Value = sao.Id.ToString() });
                    }
                }
            }

            // discounts
            var discounts = _discountService.GetAllDiscounts(DiscountType.AssignedToSkus, null, true);
            model.AvailableDiscounts = discounts.ToList();
            if (product != null && !excludeProperties)
            {
                //model.SelectedDiscountIds = product.AppliedDiscounts.Select(d => d.Id).ToArray();
            }

            var inventoryMethods = ((ManageInventoryMethod[])Enum.GetValues(typeof(ManageInventoryMethod))).Where(
                x => (model.ProductTypeId == (int)ProductType.BundledProduct && x == ManageInventoryMethod.ManageStockByAttributes) ? false : true
            );

            foreach (var inventoryMethod in inventoryMethods)
            {
                model.AvailableManageInventoryMethods.Add(new SelectListItem
                {
                    Value = ((int)inventoryMethod).ToString(),
                    Text = inventoryMethod.GetLocalizedEnum(_localizationService, _workContext),
                    Selected = ((int)inventoryMethod == model.ManageInventoryMethodId)
                });
            }

            model.AvailableCountries = _countryService.GetAllCountries(true)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = product != null && x.Id == product.CountryOfOriginId
                })
                .ToList();

            if (setPredefinedValues)
            {
                model.MaximumCustomerEnteredPrice = 1000;
                model.MaxNumberOfDownloads = 10;
                model.RecurringCycleLength = 100;
                model.RecurringTotalCycles = 10;
                model.StockQuantity = 10000;
                model.NotifyAdminForQuantityBelow = 1;
                model.OrderMinimumQuantity = 1;
                model.OrderMaximumQuantity = 100;
                model.QuantityStep = 1;
                model.HideQuantityControl = false;
                model.UnlimitedDownloads = true;
                model.IsShipEnabled = true;
                model.AllowCustomerReviews = true;
                model.Published = true;
                model.VisibleIndividually = true;
                model.HasPreviewPicture = false;
            }
        }

        private IQueryable<DeclarationProduct> ApplySorting(IQueryable<DeclarationProduct> query, GridCommand command)
        {
            if (command.SortDescriptors != null && command.SortDescriptors.Count > 0)
            {
                var sort = command.SortDescriptors.First();
                if (sort.Member == "Name")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.Name);
                    else
                        query = query.OrderByDescending(x => x.Name);
                }
                else if (sort.Member == "Sku")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.Sku);
                    else
                        query = query.OrderByDescending(x => x.Sku);
                }
                else if (sort.Member == "Price")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.Price);
                    else
                        query = query.OrderByDescending(x => x.Price);
                }
                else if (sort.Member == "OldPrice")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.OldPrice);
                    else
                        query = query.OrderByDescending(x => x.OldPrice);
                }
                else if (sort.Member == "StockQuantity")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.StockQuantity);
                    else
                        query = query.OrderByDescending(x => x.StockQuantity);
                }
                else if (sort.Member == "CreatedOn")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.CreatedOnUtc);
                    else
                        query = query.OrderByDescending(x => x.CreatedOnUtc);
                }
                else if (sort.Member == "UpdatedOn")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.UpdatedOnUtc);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOnUtc);
                }
                else if (sort.Member == "Published")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.Published);
                    else
                        query = query.OrderByDescending(x => x.Published);
                }
                else if (sort.Member == "LimitedToStores")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.LimitedToStores);
                    else
                        query = query.OrderByDescending(x => x.LimitedToStores);
                }
                else if (sort.Member == "ManageInventoryMethod")
                {
                    if (sort.SortDirection == ListSortDirection.Ascending)
                        query = query.OrderBy(x => x.ManageInventoryMethodId);
                    else
                        query = query.OrderByDescending(x => x.ManageInventoryMethodId);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }

            return query;
        }

        [NonAction]
        private void PrepareAclModel(DeclarationProductModel model, DeclarationProduct product, bool excludeProperties)
        {
            Guard.NotNull(model, nameof(model));

            if (!excludeProperties)
            {
                if (product != null)
                {
                    model.SelectedCustomerRoleIds = _aclService.GetCustomerRoleIdsWithAccessTo(product);
                }
                else
                {
                    model.SelectedCustomerRoleIds = new int[0];
                }
            }

            model.AvailableCustomerRoles = _customerService.GetAllCustomerRoles(true).ToSelectListItems(model.SelectedCustomerRoleIds);
        }

        [NonAction]
        private void PrepareProductPictureThumbnailModel(DeclarationProductModel model, DeclarationProduct product, PictureInfo defaultPicture)
        {
            Guard.NotNull(model, nameof(model));

            model.PictureThumbnailUrl = _pictureService.GetUrl(defaultPicture, _mediaSettings.CartThumbPictureSize, true);
            model.NoThumb = defaultPicture == null;
        }

        [NonAction]
        private void PrepareStoresMappingModel(DeclarationProductModel model, DeclarationProduct product, bool excludeProperties)
        {
            Guard.NotNull(model, nameof(model));

            if (!excludeProperties)
            {
                model.SelectedStoreIds = _storeMappingService.GetStoresIdsWithAccess(product);
            }

            model.AvailableStores = _storeService.GetAllStores().ToSelectListItems(model.SelectedStoreIds);
        }

        #endregion Utitilies

        #region Misc

        [HttpPost]
        public ActionResult GetBasePrice(int productId, string basePriceMeasureUnit, decimal basePriceAmount, int basePriceBaseAmount)
        {
            var product = _productService.GetProductById(productId);
            string basePrice = "";

            if (basePriceAmount != Decimal.Zero)
            {
                decimal basePriceValue = Convert.ToDecimal((product.Price / basePriceAmount) * basePriceBaseAmount);

                string basePriceFormatted = _priceFormatter.FormatPrice(basePriceValue, true, false);
                string unit = "{0} {1}".FormatWith(basePriceBaseAmount, basePriceMeasureUnit);

                basePrice = _localizationService.GetResource("Admin.Catalog.Products.Fields.BasePriceInfo").FormatWith(
                    basePriceAmount.ToString("G29") + " " + basePriceMeasureUnit, basePriceFormatted, unit);
            }

            return Json(new { Result = true, BasePrice = basePrice });
        }

        #endregion Misc

        #region DeclarationProduct list / create / edit / delete

        [HttpPost]
        public ActionResult CopyProduct(DeclarationProductModel model)
        {
            //
            //    

            var copyModel = model.CopyProductModel;
            try
            {
                DeclarationProduct newProduct = null;
                var product = _productService.GetProductById(copyModel.Id);

                for (var i = 1; i <= copyModel.NumberOfCopies; ++i)
                {
                    var newName = copyModel.NumberOfCopies > 1 ? $"{copyModel.Name} {i}" : copyModel.Name;
                    //newProduct = _copyProductService.CopyProduct((product), newName, copyModel.Published, copyModel.CopyImages);
                }

                if (newProduct != null)
                {
                    NotifySuccess(T("Admin.Common.TaskSuccessfullyProcessed"));

                    return RedirectToAction("Edit", new { id = newProduct.Id });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                NotifyError(ex.ToAllMessages());
            }

            return RedirectToAction("Edit", new { id = copyModel.Id });
        }

        //create product
        public ActionResult Create()
        {
            //
            //    

            var model = new DeclarationProductModel();
            model.IsTaxExempt = true;
            PrepareProductModel(model, null, true, true);

            //product
            //AddLocales(_languageService, model.Locales);
            PrepareAclModel(model, null, false);
            PrepareStoresMappingModel(model, null, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public ActionResult Create(DeclarationProductModel model, bool continueEditing, FormCollection form)
        {
            //
            //    

            if (model.DownloadFileVersion.HasValue() && model.DownloadId != null)
            {
                try
                {
                    var test = new SemanticVersion(model.DownloadFileVersion).ToString();
                }
                catch
                {
                    ModelState.AddModelError("FileVersion", T("Admin.Catalog.Products.Download.SemanticVersion.NotValid"));
                }
            }
            //var category = new Category();

            //if (_categoryService.GetProductCategoryById(28) != null) { category = _categoryService.GetCategoryById(28); }
            //else {
            //    category.Id = 28;
            //    category.Name = "报单产品";
            //    category.ParentCategoryId = 0;
            //    category.Deleted = false;
            //    category.UpdatedOnUtc = DateTime.Now;
            //    category.Published = true;
            //    //category.PictureId = 999999;
            //    category.CreatedOnUtc = DateTime.Now;
            //    category.ShowOnHomePage = true;
            //    category.DisplayOrder = 0;
            //    category.DefaultViewMode = "grid";
            //    MediaHelper.UpdatePictureTransientStateFor(category, c => c.PictureId);
            //    _categoryService.InsertCategory(category);
            //    category = _categoryService.GetCategoryById(28); }

            if (ModelState.IsValid)
            {
                var product = new DeclarationProduct();

                MapModelToProduct(model, product, form);

                product.StockQuantity = 10000;
                product.OrderMinimumQuantity = 1;
                product.OrderMaximumQuantity = 100;
                product.HideQuantityControl = false;
                product.IsShipEnabled = true;
                product.AllowCustomerReviews = true;
                product.Published = true;
                product.VisibleIndividually = true;
                product.MaximumCustomerEnteredPrice = 1000;
                
                if (product.ProductType == ProductType.BundledProduct)
                {
                    product.BundleTitleText = _localizationService.GetResource("Products.Bundle.BundleIncludes");
                }
                //Mapper.Initialize(cfg => cfg.CreateMap<DeclarationProduct, DeclarationProduct>());
                //DeclarationProduct dto = Mapper.Map<DeclarationProduct>(product);
                _productService.InsertProduct(product);

                UpdateDataOfExistingProduct(product, model, false);

                //activity log
                _customerActivityService.InsertActivity("AddNewProduct", _localizationService.GetResource("ActivityLog.AddNewProduct"), product.Name);

                NotifySuccess(_localizationService.GetResource("Admin.Catalog.Products.Added"));

                if (continueEditing)
                {
                    // ensure that the same tab gets selected in edit view
                    var selectedTab = TempData["SelectedTab.product-edit"] as SmartStore.Web.Framework.UI.SelectedTabInfo;
                    if (selectedTab != null)
                    {
                        selectedTab.Path = Url.Action("Edit", new System.Web.Routing.RouteValueDictionary { { "id", product.Id } });
                    }
                }

                return continueEditing ? RedirectToAction("Edit", new { id = product.Id }) : RedirectToAction("List");
            }

            // If we got this far, something failed, redisplay form
            PrepareProductModel(model, null, false, true);
            PrepareAclModel(model, null, true);
            PrepareStoresMappingModel(model, null, true);
            return View(model);
        }

        //delete product
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //
            //    

            var product = _productService.GetProductById(id);
            _productService.DeleteProduct(product);

            //activity log
            _customerActivityService.InsertActivity("DeleteProduct", _localizationService.GetResource("ActivityLog.DeleteProduct"), product.Name);

            NotifySuccess(_localizationService.GetResource("Admin.Catalog.Products.Deleted"));
            return RedirectToAction("List");
        }

        public ActionResult DeleteSelected(string selectedIds)
        {
            //
            //    

            var products = new List<DeclarationProduct>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                products.AddRange(_productService.GetProductsByIds(ids));

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    _productService.DeleteProduct(product);
                }
            }

            return RedirectToAction("List");
        }

        //edit product
        public ActionResult Edit(int id)
        {
            //
            //    

            var product = _productService.GetProductById(id);

            if (product == null)
            {
                NotifyWarning(T("Products.NotFound", id));
                return RedirectToAction("List");
            }

            if (product.Deleted)
            {
                NotifyWarning(T("Products.Deleted", id));
                return RedirectToAction("List");
            }

            var model = product.ToModel();
            model.IsTaxExempt = true;
            PrepareProductModel(model, product, false, false);

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = product.GetLocalized(x => x.Name, languageId, false, false);
                locale.ShortDescription = product.GetLocalized(x => x.ShortDescription, languageId, false, false);
                locale.FullDescription = product.GetLocalized(x => x.FullDescription, languageId, false, false);
                locale.MetaKeywords = product.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                locale.MetaDescription = product.GetLocalized(x => x.MetaDescription, languageId, false, false);
                locale.MetaTitle = product.GetLocalized(x => x.MetaTitle, languageId, false, false);
                locale.SeName = product.GetSeName(languageId, false, false);
                locale.BundleTitleText = product.GetLocalized(x => x.BundleTitleText, languageId, false, false);
            });

            PrepareProductPictureThumbnailModel(model, product, _pictureService.GetPictureInfo(product.MainPictureId));
            PrepareAclModel(model, product, false);
            PrepareStoresMappingModel(model, product, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public ActionResult Edit(DeclarationProductModel model, bool continueEditing, FormCollection form)
        {
            //
            //{
            //    
            //}

            var product = _productService.GetProductById(model.Id);
            if (product == null)
            {
                NotifyWarning(T("Products.NotFound", model.Id));
                return RedirectToAction("List");
            }

            if (product.Deleted)
            {
                NotifyWarning(T("Products.Deleted", model.Id));
                return RedirectToAction("List");
            }

            if (model.DownloadFileVersion.HasValue() && model.DownloadId != null)
            {
                try
                {
                    var test = new SemanticVersion(model.DownloadFileVersion).ToString();
                }
                catch
                {
                    ModelState.AddModelError("FileVersion", T("Admin.Catalog.Products.Download.SemanticVersion.NotValid"));
                }
            }

            if (ModelState.IsValid)
            {
                MapModelToProduct(model, product, form);
                UpdateDataOfExistingProduct(product, model, true);

                // activity log
                _customerActivityService.InsertActivity("EditProduct", _localizationService.GetResource("ActivityLog.EditProduct"), product.Name);

                NotifySuccess(_localizationService.GetResource("Admin.Catalog.Products.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = product.Id }) : RedirectToAction("List");
            }

            // If we got this far, something failed, redisplay form
            PrepareProductModel(model, product, false, true);

            PrepareProductPictureThumbnailModel(model, product, _pictureService.GetPictureInfo(product.MainPictureId));

            PrepareAclModel(model, product, true);

            PrepareStoresMappingModel(model, product, true);

            return View(model);
        }

        //list products
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List(DeclarationProductListModel model)
        {
            //
            //    

            var allStores = _storeService.GetAllStores();

            model.DisplayProductPictures = _adminAreaSettings.DisplayProductPictures;
            model.GridPageSize = _adminAreaSettings.GridPageSize;

            foreach (var c in _categoryService.GetCategoryTree(includeHidden: true).FlattenNodes(false))
            {
                model.AvailableCategories.Add(new SelectListItem { Text = c.GetCategoryNameIndented(), Value = c.Id.ToString() });
            }

            foreach (var m in _manufacturerService.GetAllManufacturers(true))
            {
                model.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            }

            foreach (var s in allStores)
            {
                model.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            }

            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();

            return View(model);
        }

        public ActionResult LoadEditTab(int id, string tabName, string viewPath = null)
        {
            //
            //    return Content("Error while loading template: Access denied.");

            try
            {
                if (id == 0)
                {
                    // is Create mode
                    return PartialView("_Create.SaveFirst");
                }

                if (tabName.IsEmpty())
                {
                    return Content("A unique tab name has to specified (route parameter: tabName)");
                }

                var product = _productService.GetProductById(id);

                var model = product.ToModel();

                PrepareProductModel(model, product, false, false);

                AddLocales(_languageService, model.Locales, (locale, languageId) =>
                {
                    locale.Name = product.GetLocalized(x => x.Name, languageId, false, false);
                    locale.ShortDescription = product.GetLocalized(x => x.ShortDescription, languageId, false, false);
                    locale.FullDescription = product.GetLocalized(x => x.FullDescription, languageId, false, false);
                    locale.MetaKeywords = product.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = product.GetLocalized(x => x.MetaDescription, languageId, false, false);
                    locale.MetaTitle = product.GetLocalized(x => x.MetaTitle, languageId, false, false);
                    locale.SeName = product.GetSeName(languageId, false, false);
                    locale.BundleTitleText = product.GetLocalized(x => x.BundleTitleText, languageId, false, false);
                });

                PrepareProductPictureThumbnailModel(model, product, _pictureService.GetPictureInfo(product.MainPictureId));

                PrepareAclModel(model, product, false);

                PrepareStoresMappingModel(model, product, false);

                return PartialView(viewPath.NullEmpty() ?? "_CreateOrUpdate." + tabName, model);
            }
            catch (Exception ex)
            {
                return Content("Error while loading template: " + ex.Message);
            }
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductList(GridCommand command, DeclarationProductListModel model)
        {
            var gridModel = new GridModel<DeclarationProductModel>();

            //
            //{
            //    gridModel.Data = Enumerable.Empty<DeclarationProductModel>();
            //    NotifyAccessDenied();
            //}
            //else
            {
                var fields = new List<string> { "name" };
                if (_searchSettings.SearchFields.Contains("sku"))
                    fields.Add("sku");
                if (_searchSettings.SearchFields.Contains("shortdescription"))
                    fields.Add("shortdescription");

                var searchQuery = new CatalogSearchQuery(fields.ToArray(), model.SearchProductName)
                    .HasStoreId(model.SearchStoreId)
                    .WithLanguage(_workContext.WorkingLanguage);

                if (model.SearchIsPublished.HasValue)
                    searchQuery = searchQuery.PublishedOnly(model.SearchIsPublished.Value);

                if (model.SearchHomePageProducts.HasValue)
                    searchQuery = searchQuery.HomePageProductsOnly(model.SearchHomePageProducts.Value);

                if (model.SearchProductTypeId > 0)
                    searchQuery = searchQuery.IsProductType((ProductType)model.SearchProductTypeId);

                if (model.SearchWithoutManufacturers.HasValue)
                    searchQuery = searchQuery.HasAnyManufacturer(!model.SearchWithoutManufacturers.Value);
                else if (model.SearchManufacturerId != 0)
                    searchQuery = searchQuery.WithManufacturerIds(null, model.SearchManufacturerId);

                if (model.SearchWithoutCategories.HasValue)
                    searchQuery = searchQuery.HasAnyCategory(!model.SearchWithoutCategories.Value);
                else if (model.SearchCategoryId != 0)
                    searchQuery = searchQuery.WithCategoryIds(null, model.SearchCategoryId);

                var query = _catalogSearchService.PrepareDeclarationQuery(searchQuery);
                //query = ApplySorting(query, command);

                var products = new PagedList<DeclarationProduct>(query, command.Page - 1, command.PageSize);
               var pictureInfos = _pictureService.GetPictureInfos(products.Select(x=>x.Id).ToArray());
                //_mediaSettings.CartThumbPictureSize
                gridModel.Data = products.Select(x =>
                {
                    var productModel = new DeclarationProductModel
                    {
                        Sku = x.Sku,
                        Published = x.Published,
                        ProductTypeLabelHint = x.ProductTypeLabelHint,
                        Name = x.Name,
                        Id = x.Id,
                        StockQuantity = x.StockQuantity,
                        Price = x.Price,
                        LimitedToStores = x.LimitedToStores
                    };

                    var defaultPicture = pictureInfos.Get(x.MainPictureId.GetValueOrDefault());
                    productModel.PictureThumbnailUrl = _pictureService.GetUrl(defaultPicture, _mediaSettings.CartThumbPictureSize, true);
                    productModel.NoThumb = defaultPicture == null;

                    productModel.ProductTypeName = x.GetProductTypeLabel(_localizationService);
                    productModel.UpdatedOn = _dateTimeHelper.ConvertToUserTime(x.UpdatedOnUtc, DateTimeKind.Utc);
                    productModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);

                    return productModel;
                });

                gridModel.Total = products.TotalCount;
            }

            return new JsonResult
            {
                Data = gridModel
            };
        }

        [NonAction]
        protected void MapModelToProduct(DeclarationProductModel model, DeclarationProduct product, FormCollection form)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<DeclarationProduct, DeclarationProduct>());
            //DeclarationProduct dproduct = Mapper.Map<DeclarationProduct>(product);
            if (model.LoadedTabs == null || model.LoadedTabs.Length == 0)
            {
                model.LoadedTabs = new string[] { "Info" };
            }

            foreach (var tab in model.LoadedTabs)
            {
                switch (tab.ToLowerInvariant())
                {
                    case "info":
                        UpdateProductGeneralInfo(product, model);
                        break;

                    case "inventory":
                        UpdateProductInventory(product, model);
                        break;

                    case "bundleitems":
                        UpdateProductBundleItems(product, model);
                        break;

                    case "price":
                        UpdateProductPrice(product, model);
                        break;

                    case "discounts":
                        //UpdateProductDiscounts((product), model);
                        break;

                    case "downloads":
                        UpdateProductDownloads(product, model);
                        break;

                    case "pictures":
                        UpdateProductPictures(product, model);
                        break;

                    case "seo":
                        UpdateProductSeo(product, model);
                        break;

                    case "acl":
                        SaveAclMappings(product, model);
                        break;

                    case "stores":
                        SaveStoreMappings(product, model);
                        break;
                }
            }

            _eventPublisher.Publish(new ModelBoundEvent(model, product, form));
        }

        #endregion DeclarationProduct list / create / edit / delete

        #region DeclarationProduct categories

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryDelete(int id, GridCommand command)
        {
            var productCategory = _categoryService.GetProductCategoryById(id);
            var productId = productCategory.ProductId;

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                _categoryService.DeleteProductCategory(productCategory);
            }

            return ProductCategoryList(command, productId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryInsert(GridCommand command, DeclarationProductModel.ProductCategoryModel model)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productCategory = new ProductCategory
                {
                    ProductId = model.ProductId,
                    CategoryId = Int32.Parse(model.Category), //use Category property (not CategoryId) because appropriate property is stored in it
                    IsFeaturedProduct = model.IsFeaturedProduct,
                    DisplayOrder = model.DisplayOrder
                };

                _categoryService.InsertProductCategory(productCategory);

                var mru = new TrimmedBuffer<string>(
                    _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.MostRecentlyUsedCategories),
                    model.Category,
                    _catalogSettings.MostRecentlyUsedCategoriesMaxSize);

                _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.MostRecentlyUsedCategories, mru.ToString());
            }

            return ProductCategoryList(command, model.ProductId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryList(GridCommand command, int productId)
        {
            var model = new GridModel<DeclarationProductModel.ProductCategoryModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productCategories = _categoryService.GetProductCategoriesByProductId(productId, true);
                var productCategoriesModel = productCategories
                    .Select(x =>
                    {
                        var node = _categoryService.GetCategoryTree(x.CategoryId, true);
                        return new DeclarationProductModel.ProductCategoryModel
                        {
                            Id = x.Id,
                            Category = node != null ? _categoryService.GetCategoryPath(node, aliasPattern: "<span class='badge badge-secondary'>{0}</span>") : string.Empty,
                            ProductId = x.ProductId,
                            CategoryId = x.CategoryId,
                            IsFeaturedProduct = x.IsFeaturedProduct,
                            DisplayOrder = x.DisplayOrder
                        };
                    })
                    .ToList();

                model.Data = productCategoriesModel;
                model.Total = productCategoriesModel.Count;
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<DeclarationProductModel.ProductCategoryModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductCategoryUpdate(GridCommand command, DeclarationProductModel.ProductCategoryModel model)
        {
            var productCategory = _categoryService.GetProductCategoryById(model.Id);

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var categoryChanged = (Int32.Parse(model.Category) != productCategory.CategoryId);

                //use Category property (not CategoryId) because appropriate property is stored in it
                productCategory.CategoryId = Int32.Parse(model.Category);
                productCategory.IsFeaturedProduct = model.IsFeaturedProduct;
                productCategory.DisplayOrder = model.DisplayOrder;
                _categoryService.UpdateProductCategory(productCategory);

                if (categoryChanged)
                {
                    var mru = new TrimmedBuffer<string>(
                        _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.MostRecentlyUsedCategories),
                        model.Category,
                        _catalogSettings.MostRecentlyUsedCategoriesMaxSize);

                    _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.MostRecentlyUsedCategories, mru.ToString());
                }
            }

            return ProductCategoryList(command, productCategory.ProductId);
        }

        #endregion DeclarationProduct categories

        #region DeclarationProduct manufacturers

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductManufacturerDelete(int id, GridCommand command)
        {
            var productManufacturer = _manufacturerService.GetProductManufacturerById(id);
            var productId = productManufacturer.ProductId;

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                _manufacturerService.DeleteProductManufacturer(productManufacturer);
            }

            return ProductManufacturerList(command, productId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductManufacturerInsert(GridCommand command, DeclarationProductModel.ProductManufacturerModel model)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productManufacturer = new ProductManufacturer
                {
                    ProductId = model.ProductId,
                    ManufacturerId = Int32.Parse(model.Manufacturer), //use Manufacturer property (not ManufacturerId) because appropriate property is stored in it
                    IsFeaturedProduct = model.IsFeaturedProduct,
                    DisplayOrder = model.DisplayOrder
                };

                _manufacturerService.InsertProductManufacturer(productManufacturer);

                var mru = new TrimmedBuffer<string>(
                    _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.MostRecentlyUsedManufacturers),
                    model.Manufacturer,
                    _catalogSettings.MostRecentlyUsedManufacturersMaxSize);

                _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.MostRecentlyUsedManufacturers, mru.ToString());
            }

            return ProductManufacturerList(command, model.ProductId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductManufacturerList(GridCommand command, int productId)
        {
            var model = new GridModel<DeclarationProductModel.ProductManufacturerModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productManufacturers = _manufacturerService.GetProductManufacturersByProductId(productId, true);
                var productManufacturersModel = productManufacturers
                    .Select(x =>
                    {
                        return new DeclarationProductModel.ProductManufacturerModel
                        {
                            Id = x.Id,
                            Manufacturer = _manufacturerService.GetManufacturerById(x.ManufacturerId).Name,
                            ProductId = x.ProductId,
                            ManufacturerId = x.ManufacturerId,
                            IsFeaturedProduct = x.IsFeaturedProduct,
                            DisplayOrder = x.DisplayOrder
                        };
                    })
                    .ToList();

                model.Data = productManufacturersModel;
                model.Total = productManufacturersModel.Count;
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<DeclarationProductModel.ProductManufacturerModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductManufacturerUpdate(GridCommand command, DeclarationProductModel.ProductManufacturerModel model)
        {
            var productManufacturer = _manufacturerService.GetProductManufacturerById(model.Id);

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var manufacturerChanged = (Int32.Parse(model.Manufacturer) != productManufacturer.ManufacturerId);

                //use Manufacturer property (not ManufacturerId) because appropriate property is stored in it
                productManufacturer.ManufacturerId = Int32.Parse(model.Manufacturer);
                productManufacturer.IsFeaturedProduct = model.IsFeaturedProduct;
                productManufacturer.DisplayOrder = model.DisplayOrder;

                _manufacturerService.UpdateProductManufacturer(productManufacturer);

                if (manufacturerChanged)
                {
                    var mru = new TrimmedBuffer<string>(
                        _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.MostRecentlyUsedManufacturers),
                        model.Manufacturer,
                        _catalogSettings.MostRecentlyUsedManufacturersMaxSize);

                    _genericAttributeService.SaveAttribute<string>(_workContext.CurrentCustomer, SystemCustomerAttributeNames.MostRecentlyUsedManufacturers, mru.ToString());
                }
            }

            return ProductManufacturerList(command, productManufacturer.ProductId);
        }

        #endregion DeclarationProduct manufacturers

        #region DeclarationProduct pictures

        public ActionResult ProductPictureAdd(int pictureId, int displayOrder, int productId)
        {
            //
            //    

            if (pictureId == 0)
                throw new ArgumentException();

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException(T("Products.NotFound", productId));

            var productPicture = new DeclarationProductPicture
            {
                PictureId = pictureId,
                ProductId = productId,
                DisplayOrder = displayOrder,
            };

            MediaHelper.UpdatePictureTransientStateFor(productPicture, pp => pp.PictureId);

            _productService.InsertProductPicture(productPicture);

            _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(product.Name));

            return Json(new { Result = true, message = T("Admin.DeclarationProduct.Picture.Added").JsText.ToString() }, JsonRequestBehavior.AllowGet);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictureDelete(int id, int productId, GridCommand command)
        {
            if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productPicture = _productService.GetProductPictureById(id);
                if (productPicture != null)
                {
                    _productService.DeleteProductPicture(productPicture);
                }

                var picture = _pictureService.GetPictureById(productPicture.PictureId);
                if (picture != null)
                {
                    _pictureService.DeletePicture(picture);
                }
            }

            return ProductPictureList(command, productId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictureList(GridCommand command, int productId)
        {
            var model = new GridModel<DeclarationProductModel.ProductPictureModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productPictures = _productService.GetProductPicturesByProductId(productId);

                var productPicturesModel = productPictures
                    .Select(x =>
                    {
                        var pictureModel = new DeclarationProductModel.ProductPictureModel
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            PictureId = x.PictureId,
                            DisplayOrder = x.DisplayOrder
                        };

                        try
                        {
                            pictureModel.PictureUrl = _pictureService.GetUrl(x.PictureId);
                        }
                        catch (Exception ex)
                        {
                            // The user must always have the possibility to delete faulty images.
                            Logger.Error(ex);
                        }

                        return pictureModel;
                    })
                    .ToList();

                model.Data = productPicturesModel;
                model.Total = productPicturesModel.Count;
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<DeclarationProductModel.ProductPictureModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductPictureUpdate(DeclarationProductModel.ProductPictureModel model, GridCommand command)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productPicture = _productService.GetProductPictureById(model.Id);
                if (productPicture != null)
                {
                    productPicture.DisplayOrder = model.DisplayOrder;

                    _productService.UpdateProductPicture(productPicture);
                }
            }

            return ProductPictureList(command, model.ProductId);
        }

        #endregion DeclarationProduct pictures

        #region DeclarationProduct specification attributes

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductSpecAttrDelete(int psaId, GridCommand command)
        {
            var psa = _specificationAttributeService.GetProductSpecificationAttributeById(psaId);
            var productId = psa.ProductId;

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                _specificationAttributeService.DeleteProductSpecificationAttribute(psa);
            }

            return ProductSpecAttrList(command, productId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductSpecAttrList(GridCommand command, int productId)
        {
            var model = new GridModel<ProductSpecificationAttributeModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productSpecAttributes = _specificationAttributeService.GetProductSpecificationAttributesByProductId(productId);
                var specAttributeIds = productSpecAttributes.Select(x => x.SpecificationAttributeOption.SpecificationAttributeId).ToArray();
                var specOptions = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeIds(specAttributeIds);

                var productSpecModel = productSpecAttributes
                    .Select(x =>
                    {
                        var attributeId = x.SpecificationAttributeOption.SpecificationAttributeId;
                        var psaModel = new ProductSpecificationAttributeModel
                        {
                            Id = x.Id,
                            SpecificationAttributeName = x.SpecificationAttributeOption.SpecificationAttribute.Name,
                            SpecificationAttributeOptionName = x.SpecificationAttributeOption.Name,
                            SpecificationAttributeId = attributeId,
                            SpecificationAttributeOptionId = x.SpecificationAttributeOptionId,
                            AllowFiltering = x.AllowFiltering,
                            ShowOnProductPage = x.ShowOnProductPage,
                            DisplayOrder = x.DisplayOrder
                        };

                        if (specOptions.ContainsKey(attributeId))
                        {
                            psaModel.SpecificationAttributeOptions = specOptions[attributeId]
                                .Select(y => new ProductSpecificationAttributeModel.SpecificationAttributeOption
                                {
                                    id = y.Id,
                                    name = y.Name,
                                    text = y.Name
                                })
                                .ToList();

                            psaModel.SpecificationAttributeOptionsUrl = Url.Action("GetOptionsByAttributeId", "SpecificationAttribute", new { attributeId = attributeId });
                        }

                        return psaModel;
                    })
                    .OrderBy(x => x.DisplayOrder)
                    .ThenBy(x => x.SpecificationAttributeId)
                    .ToList();

                model.Data = productSpecModel;
                model.Total = productSpecModel.Count;
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<ProductSpecificationAttributeModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductSpecAttrUpdate(int psaId, ProductSpecificationAttributeModel model, GridCommand command)
        {
            var psa = _specificationAttributeService.GetProductSpecificationAttributeById(psaId);

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                psa.AllowFiltering = model.AllowFiltering;
                psa.ShowOnProductPage = model.ShowOnProductPage;
                psa.DisplayOrder = model.DisplayOrder;

                _specificationAttributeService.UpdateProductSpecificationAttribute(psa);
            }

            return ProductSpecAttrList(command, psa.ProductId);
        }

        public ActionResult ProductSpecificationAttributeAdd(
                                    int specificationAttributeOptionId,
            bool? allowFiltering,
            bool? showOnProductPage,
            int displayOrder,
            int productId)
        {
            var success = false;
            var message = string.Empty;

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var psa = new ProductSpecificationAttribute
                {
                    SpecificationAttributeOptionId = specificationAttributeOptionId,
                    ProductId = productId,
                    AllowFiltering = allowFiltering,
                    ShowOnProductPage = showOnProductPage,
                    DisplayOrder = displayOrder,
                };

                try
                {
                    _specificationAttributeService.InsertProductSpecificationAttribute(psa);
                    success = true;
                }
                catch (Exception exception)
                {
                    message = exception.Message;
                }
            }
            //else
            //{
            //    NotifyAccessDenied();
            //}

            return Json(new { success = success, message = message });
        }

        #endregion DeclarationProduct specification attributes

        #region DeclarationProduct tags

        //edit
        public ActionResult EditProductTag(int id)
        {
            //
            //    

            var productTag = _productTagService.GetProductTagById(id);
            if (productTag == null)
                //No product tag found with the specified id
                return RedirectToAction("List");

            var model = new ProductTagModel()
            {
                Id = productTag.Id,
                Name = productTag.Name,
                ProductCount = _productTagService.GetProductCount(productTag.Id, 0)
            };
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = productTag.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProductTag(string btnId, string formId, ProductTagModel model)
        {
            //
            //    

            var productTag = _productTagService.GetProductTagById(model.Id);
            if (productTag == null)
                //No product tag found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                productTag.Name = model.Name;
                _productTagService.UpdateProductTag(productTag);
                //locales
                UpdateLocales(productTag, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                ViewBag.formId = formId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductTagDelete(int id, GridCommand command)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var tag = _productTagService.GetProductTagById(id);

                _productTagService.DeleteProductTag(tag);
            }

            return ProductTags(command);
        }

        public ActionResult ProductTags()
        {
            //
            //    

            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductTags(GridCommand command)
        {
            var model = new GridModel<ProductTagModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var tags = _productTagService.GetAllProductTags()
                    .OrderByDescending(x => _productTagService.GetProductCount(x.Id, 0))
                    .Select(x =>
                    {
                        return new ProductTagModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProductCount = _productTagService.GetProductCount(x.Id, 0)
                        };
                    })
                    .ForCommand(command);

                model.Data = tags.PagedForCommand(command);
                model.Total = tags.Count();
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<ProductTagModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        #endregion DeclarationProduct tags

        #region Low stock reports

        public ActionResult LowStockReport()
        {
            //
            //    

            return View();
        }

        #endregion Low stock reports

        #region Bulk editing

        public ActionResult BulkEdit()
        {
            //
            //    

            var allStores = _services.StoreService.GetAllStores();

            var model = new BulkEditListModel();
            model.GridPageSize = _adminAreaSettings.GridPageSize;

            foreach (var c in _categoryService.GetCategoryTree(includeHidden: true).FlattenNodes(false))
            {
                model.AvailableCategories.Add(new SelectListItem { Text = c.GetCategoryNameIndented(), Value = c.Id.ToString() });
            }

            foreach (var m in _manufacturerService.GetAllManufacturers(true))
            {
                model.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            }

            foreach (var s in allStores)
            {
                model.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            }

            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult BulkEditSave(GridCommand command,
            [Bind(Prefix = "updated")]IEnumerable<BulkEditProductModel> updatedProducts,
            [Bind(Prefix = "deleted")]IEnumerable<BulkEditProductModel> deletedProducts,
            BulkEditListModel model)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                if (updatedProducts != null)
                {
                    foreach (var pModel in updatedProducts)
                    {
                        var product = _productService.GetProductById(pModel.Id);
                        if (product != null)
                        {
                            var prevStockQuantity = product.StockQuantity;

                            product.Sku = pModel.Sku;
                            product.Price = pModel.Price;
                            product.OldPrice = pModel.OldPrice;
                            product.StockQuantity = pModel.StockQuantity;
                            product.Published = pModel.Published;

                            _productService.UpdateProduct(product);

                            // back in stock notifications
                            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                                product.BackorderMode == BackorderMode.NoBackorders &&
                                product.AllowBackInStockSubscriptions &&
                                product.StockQuantity > 0 &&
                                prevStockQuantity <= 0 &&
                                product.Published &&
                                !product.Deleted &&
                                !product.IsSystemProduct)
                            {
                               // _backInStockSubscriptionService.SendNotificationsToSubscribers((product));
                            }

                            if (product.StockQuantity != prevStockQuantity && product.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
                            {
                                _productService.AdjustInventory(product, true, 0, string.Empty);
                            }
                        }
                    }
                }

                if (deletedProducts != null)
                {
                    foreach (var pModel in deletedProducts)
                    {
                        var product = _productService.GetProductById(pModel.Id);
                        if (product != null)
                        {
                            _productService.DeleteProduct(product);
                        }
                    }
                }
            }

            return BulkEditSelect(command, model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult BulkEditSelect(GridCommand command, BulkEditListModel model)
        {
            var gridModel = new GridModel<BulkEditProductModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var fields = new List<string> { "name" };
                if (_searchSettings.SearchFields.Contains("sku"))
                    fields.Add("sku");
                if (_searchSettings.SearchFields.Contains("shortdescription"))
                    fields.Add("shortdescription");

                var searchQuery = new CatalogSearchQuery(fields.ToArray(), model.SearchProductName)
                    .HasStoreId(model.SearchStoreId)
                    .WithLanguage(_workContext.WorkingLanguage);

                if (model.SearchProductTypeId > 0)
                    searchQuery = searchQuery.IsProductType((ProductType)model.SearchProductTypeId);

                if (model.SearchManufacturerId != 0)
                    searchQuery = searchQuery.WithManufacturerIds(null, model.SearchManufacturerId);

                if (model.SearchCategoryId != 0)
                    searchQuery = searchQuery.WithCategoryIds(null, model.SearchCategoryId);

                var query = _catalogSearchService.PrepareDeclarationQuery(searchQuery);
                query = ApplySorting(query, command);

                var products = new PagedList<DeclarationProduct>(query, command.Page - 1, command.PageSize);

                gridModel.Data = products.Select(x =>
                {
                    var productModel = new BulkEditProductModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProductTypeName = x.GetProductTypeLabel(_localizationService),
                        ProductTypeLabelHint = x.ProductTypeLabelHint,
                        Sku = x.Sku,
                        OldPrice = x.OldPrice,
                        Price = x.Price,
                        ManageInventoryMethod = x.ManageInventoryMethod.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
                        StockQuantity = x.StockQuantity,
                        Published = x.Published
                    };

                    return productModel;
                });

                gridModel.Total = products.TotalCount;
            }
            //else
            //{
            //    gridModel.Data = Enumerable.Empty<BulkEditProductModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion Bulk editing

        #region Tier prices

        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult TierPriceDelete(int id, GridCommand command)
        //{
        //    var tierPrice = _productService.GetTierPriceById(id);
        //    var productId = tierPrice.ProductId;

        //    if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
        //    {
        //        var product = _productService.GetProductById(productId);

        //        _productService.DeleteTierPrice(tierPrice);

        //        //update "HasTierPrices" property
        //        _productService.UpdateHasTierPricesProperty(product);
        //    }

        //    return TierPriceList(command, productId);
        //}

        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult TierPriceInsert(GridCommand command, DeclarationProductModel.TierPriceModel model)
        //{
        //    if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
        //    {
        //        var product = _productService.GetProductById(model.ProductId);

        //        var tierPrice = new TierPrice
        //        {
        //            ProductId = model.ProductId,
        //            // use Store property (not Store propertyId) because appropriate property is stored in it
        //            StoreId = model.Store.ToInt(),
        //            // use CustomerRole property (not CustomerRoleId) because appropriate property is stored in it
        //            CustomerRoleId = model.CustomerRole.IsNumeric() && Int32.Parse(model.CustomerRole) != 0 ? Int32.Parse(model.CustomerRole) : (int?)null,
        //            Quantity = model.Quantity,
        //            Price = model.Price1,
        //            CalculationMethod = model.CalculationMethod == null ? TierPriceCalculationMethod.Fixed : (TierPriceCalculationMethod)(Int32.Parse(model.CalculationMethod))
        //        };

        //        _productService.InsertTierPrice(tierPrice);

        //        //update "HasTierPrices" property
        //        _productService.UpdateHasTierPricesProperty(product);
        //    }

        //    return TierPriceList(command, model.ProductId);
        //}

        //[HttpPost, GridAction(EnableCustomBinding = true)]
        //public ActionResult TierPriceList(GridCommand command, int productId)
        //{
        //    var model = new GridModel<DeclarationProductModel.TierPriceModel>();

        //    if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
        //    {
        //        var product = _productService.GetProductById(productId);

        //        var allStores = _services.StoreService.GetAllStores();
        //        var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
        //        string allRolesString = T("Admin.Catalog.Products.TierPrices.Fields.CustomerRole.AllRoles");
        //        string allStoresString = T("Admin.Common.StoresAll");
        //        string deletedString = "[{0}]".FormatInvariant(T("Admin.Common.Deleted"));

        //        var tierPricesModel = product.TierPrices
        //            .OrderBy(x => x.StoreId)
        //            .ThenBy(x => x.Quantity)
        //            .ThenBy(x => x.CustomerRoleId)
        //            .Select(x =>
        //            {
        //                var tierPriceModel = new DeclarationProductModel.TierPriceModel
        //                {
        //                    Id = x.Id,
        //                    StoreId = x.StoreId,
        //                    CustomerRoleId = x.CustomerRoleId ?? 0,
        //                    ProductId = x.ProductId,
        //                    Quantity = x.Quantity,
        //                    CalculationMethodId = (int)x.CalculationMethod,
        //                    Price1 = x.Price
        //                };

        //                switch (x.CalculationMethod)
        //                {
        //                    case TierPriceCalculationMethod.Fixed:
        //                        tierPriceModel.CalculationMethod = T("Admin.DeclarationProduct.Price.Tierprices.Fixed").Text;
        //                        break;

        //                    case TierPriceCalculationMethod.Adjustment:
        //                        tierPriceModel.CalculationMethod = T("Admin.DeclarationProduct.Price.Tierprices.Adjustment").Text;
        //                        break;

        //                    case TierPriceCalculationMethod.Percental:
        //                        tierPriceModel.CalculationMethod = T("Admin.DeclarationProduct.Price.Tierprices.Percental").Text;
        //                        break;

        //                    default:
        //                        tierPriceModel.CalculationMethod = x.CalculationMethod.ToString();
        //                        break;
        //                }

        //                if (x.CustomerRoleId.HasValue)
        //                {
        //                    var role = allCustomerRoles.FirstOrDefault(r => r.Id == x.CustomerRoleId.Value);
        //                    tierPriceModel.CustomerRole = (role == null ? allRolesString : role.Name);
        //                }
        //                else
        //                {
        //                    tierPriceModel.CustomerRole = allRolesString;
        //                }

        //                if (x.StoreId > 0)
        //                {
        //                    var store = allStores.FirstOrDefault(s => s.Id == x.StoreId);
        //                    tierPriceModel.Store = (store == null ? deletedString : store.Name);
        //                }
        //                else
        //                {
        //                    tierPriceModel.Store = allStoresString;
        //                }

        //                return tierPriceModel;
        //            })
        //            .ToList();

        //        model.Data = tierPricesModel;
        //        model.Total = tierPricesModel.Count();
        //    }
        //    else
        //    {
        //        model.Data = Enumerable.Empty<DeclarationProductModel.TierPriceModel>();

        //        NotifyAccessDenied();
        //    }

        //    return new JsonResult
        //    {
        //        Data = model
        //    };
        //}

        //[GridAction(EnableCustomBinding = true)]
        //public ActionResult TierPriceUpdate(GridCommand command, DeclarationProductModel.TierPriceModel model)
        //{
        //    var tierPrice = _productService.GetTierPriceById(model.Id);

        //    if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
        //    {
        //        //use Store property (not Store propertyId) because appropriate property is stored in it
        //        tierPrice.StoreId = model.Store.ToInt();
        //        //use CustomerRole property (not CustomerRoleId) because appropriate property is stored in it
        //        tierPrice.CustomerRoleId = model.CustomerRole.IsNumeric() && Int32.Parse(model.CustomerRole) != 0 ? Int32.Parse(model.CustomerRole) : (int?)null;
        //        tierPrice.Quantity = model.Quantity;
        //        tierPrice.Price = model.Price1;
        //        tierPrice.CalculationMethod = model.CalculationMethod == null ? TierPriceCalculationMethod.Fixed : (TierPriceCalculationMethod)(Int32.Parse(model.CalculationMethod));
        //        _productService.UpdateTierPrice(tierPrice);
        //    }

        //    return TierPriceList(command, tierPrice.ProductId);
        //}

        #endregion Tier prices

        #region DeclarationProduct variant attributes

        public ActionResult AllProductVariantAttributes(string label, int selectedId)
        {
            var attributes = _productAttributeService.GetAllProductAttributes();

            if (label.HasValue())
            {
                attributes.Insert(0, new ProductAttribute { Name = label, Id = 0 });
            }

            var query =
                from attr in attributes
                select new
                {
                    id = attr.Id.ToString(),
                    text = attr.Name,
                    selected = attr.Id == selectedId
                };

            return new JsonResult { Data = query.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult CopyAttributeOptions(int productVariantAttributeId, int optionsSetId, bool deleteExistingValues)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);
                if (pva == null)
                {
                    NotifyError(T("Products.Variants.NotFound", productVariantAttributeId));
                }
                else
                {
                    try
                    {
                        var numberOfCopiedOptions = _productAttributeService.CopyAttributeOptions(pva, optionsSetId, deleteExistingValues);

                        NotifySuccess(string.Concat(T("Admin.Common.TaskSuccessfullyProcessed"), " ",
                            T("Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.NumberOfCopiedOptions", numberOfCopiedOptions)));
                    }
                    catch (Exception exception)
                    {
                        NotifyError(exception.Message);
                    }
                }
            }
            //else
            //{
            //    NotifyAccessDenied();
            //}

            return new JsonResult { Data = string.Empty };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeDelete(int id, GridCommand command)
        {
            var pva = _productAttributeService.GetProductVariantAttributeById(id);
            var productId = pva.ProductId;

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                _productAttributeService.DeleteProductVariantAttribute(pva);
            }

            return ProductVariantAttributeList(command, productId);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeInsert(GridCommand command, DeclarationProductModel.ProductVariantAttributeModel model)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var pva = new ProductVariantAttribute
                {
                    ProductId = model.ProductId,
                    ProductAttributeId = Int32.Parse(model.ProductAttribute), //use ProductAttribute property (not ProductAttributeId) because appropriate property is stored in it
                    TextPrompt = model.TextPrompt,
                    IsRequired = model.IsRequired,
                    AttributeControlTypeId = Int32.Parse(model.AttributeControlType), //use AttributeControlType property (not AttributeControlTypeId) because appropriate property is stored in it
                    DisplayOrder = model.DisplayOrder1
                };

                try
                {
                    _productAttributeService.InsertProductVariantAttribute(pva);
                }
                catch (Exception exception)
                {
                    Services.Notifier.Error(exception.Message);
                }
            }
            //else
            //{
            //    NotifyAccessDenied();
            //}

            return ProductVariantAttributeList(command, model.ProductId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeList(GridCommand command, int productId)
        {
            var model = new GridModel<DeclarationProductModel.ProductVariantAttributeModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var productVariantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(productId);
                var productVariantAttributesModel = productVariantAttributes
                    .Select(x =>
                    {
                        var pvaModel = new DeclarationProductModel.ProductVariantAttributeModel
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            ProductAttribute = _productAttributeService.GetProductAttributeById(x.ProductAttributeId).Name,
                            ProductAttributeId = x.ProductAttributeId,
                            TextPrompt = x.TextPrompt,
                            IsRequired = x.IsRequired,
                            AttributeControlType = x.AttributeControlType.GetLocalizedEnum(_localizationService, _workContext),
                            AttributeControlTypeId = x.AttributeControlTypeId,
                            DisplayOrder1 = x.DisplayOrder
                        };

                        if (x.ShouldHaveValues())
                        {
                            pvaModel.ValueCount = x.ProductVariantAttributeValues != null ? x.ProductVariantAttributeValues.Count : 0;
                            pvaModel.ViewEditUrl = Url.Action("EditAttributeValues", "DeclarationProduct", new { productVariantAttributeId = x.Id });
                            pvaModel.ViewEditText = T("Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.ViewLink", pvaModel.ValueCount);

                            if (x.ProductAttribute.ProductAttributeOptionsSets.Any())
                            {
                                var optionsSets = new StringBuilder($"<option>{T("Admin.Catalog.Products.ProductVariantAttributes.Attributes.Values.CopyOptions")}</option>");
                                x.ProductAttribute.ProductAttributeOptionsSets.Each(set => optionsSets.Append($"<option value=\"{set.Id}\">{set.Name}</option>"));
                                pvaModel.OptionsSets = optionsSets.ToString();
                            }
                        }

                        return pvaModel;
                    })
                    .ToList();

                model.Data = productVariantAttributesModel;
                model.Total = productVariantAttributesModel.Count;
            }
            //else
            //{
            //    model.Data = Enumerable.Empty<DeclarationProductModel.ProductVariantAttributeModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = model
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeUpdate(GridCommand command, DeclarationProductModel.ProductVariantAttributeModel model)
        {
            var pva = _productAttributeService.GetProductVariantAttributeById(model.Id);

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                //use ProductAttribute property (not ProductAttributeId) because appropriate property is stored in it
                pva.ProductAttributeId = Int32.Parse(model.ProductAttribute);
                pva.TextPrompt = model.TextPrompt;
                pva.IsRequired = model.IsRequired;
                //use AttributeControlType property (not AttributeControlTypeId) because appropriate property is stored in it
                pva.AttributeControlTypeId = Int32.Parse(model.AttributeControlType);
                pva.DisplayOrder = model.DisplayOrder1;

                try
                {
                    _productAttributeService.UpdateProductVariantAttribute(pva);
                }
                catch (Exception exception)
                {
                    NotifyError(exception.Message);
                }
            }
            //else
            //{
            //    NotifyAccessDenied();
            //}

            return ProductVariantAttributeList(command, pva.ProductId);
        }

        #endregion DeclarationProduct variant attributes

        #region DeclarationProduct variant attribute values

        public ActionResult EditAttributeValues(int productVariantAttributeId)
        {
            //
            //    

            var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);
            if (pva == null)
                throw new ArgumentException(T("Products.Variants.NotFound", productVariantAttributeId));

            var product = _productService.GetProductById(pva.ProductId);
            if (product == null)
                throw new ArgumentException(T("Products.NotFound", pva.ProductId));

            var model = new DeclarationProductModel.ProductVariantAttributeValueListModel
            {
                ProductName = product.Name,
                ProductId = pva.ProductId,
                ProductVariantAttributeName = pva.ProductAttribute.Name,
                ProductVariantAttributeId = pva.Id
            };

            return View(model);
        }

        public ActionResult ProductAttributeValueCreatePopup(int productVariantAttributeId)
        {
            //
            //    

            var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);
            if (pva == null)
                throw new ArgumentException(T("Products.Variants.NotFound", productVariantAttributeId));

            var model = new DeclarationProductModel.ProductVariantAttributeValueModel
            {
                ProductId = pva.ProductId,
                ProductVariantAttributeId = productVariantAttributeId,
                IsListTypeAttribute = pva.IsListTypeAttribute(),
                Color = "",
                Quantity = 1
            };

            AddLocales(_languageService, model.Locales);

            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductAttributeValueDelete(int pvavId, int productVariantAttributeId, GridCommand command)
        {
            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var pvav = _productAttributeService.GetProductVariantAttributeValueById(pvavId);

                _productAttributeService.DeleteProductVariantAttributeValue(pvav);
            }

            return ProductAttributeValueList(productVariantAttributeId, command);
        }

        public ActionResult ProductAttributeValueEditPopup(int id)
        {
            //
            //    

            var pvav = _productAttributeService.GetProductVariantAttributeValueById(id);
            if (pvav == null)
                return RedirectToAction("List", "DeclarationProduct");

            var linkedProduct = _productService.GetProductById(pvav.LinkedProductId);

            var model = new DeclarationProductModel.ProductVariantAttributeValueModel
            {
                ProductId = pvav.ProductVariantAttribute.ProductId,
                ProductVariantAttributeId = pvav.ProductVariantAttributeId,
                Name = pvav.Name,
                Alias = pvav.Alias,
                Color = pvav.Color,
                PictureId = pvav.PictureId,
                IsListTypeAttribute = pvav.ProductVariantAttribute.IsListTypeAttribute(),
                PriceAdjustment = pvav.PriceAdjustment,
                WeightAdjustment = pvav.WeightAdjustment,
                IsPreSelected = pvav.IsPreSelected,
                DisplayOrder = pvav.DisplayOrder,
                ValueTypeId = pvav.ValueTypeId,
                TypeName = pvav.ValueType.GetLocalizedEnum(_localizationService, _workContext),
                TypeNameClass = (pvav.ValueType == ProductVariantAttributeValueType.ProductLinkage ? "fa fa-link mr-2" : "d-none hide hidden-xs-up"),
                LinkedProductId = pvav.LinkedProductId,
                Quantity = pvav.Quantity
            };

            if (linkedProduct != null)
            {
                model.LinkedProductName = linkedProduct.GetLocalized(p => p.Name);
                model.LinkedProductTypeName = linkedProduct.GetProductTypeLabel(_localizationService);
                model.LinkedProductTypeLabelHint = linkedProduct.ProductTypeLabelHint;

                if (model.Quantity > 1)
                    model.QuantityInfo = " × {0}".FormatWith(model.Quantity);
            }

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = pvav.GetLocalized(x => x.Name, languageId, false, false);
                locale.Alias = pvav.GetLocalized(x => x.Alias, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductAttributeValueList(int productVariantAttributeId, GridCommand command)
        {
            var gridModel = new GridModel<DeclarationProductModel.ProductVariantAttributeValueModel>();

            //if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var pva = _productAttributeService.GetProductVariantAttributeById(productVariantAttributeId);

                var values = _productAttributeService.GetProductVariantAttributeValues(productVariantAttributeId);

                gridModel.Data = values.Select(x =>
                {
                    var linkedProduct = _productService.GetProductById(x.LinkedProductId);

                    var model = new DeclarationProductModel.ProductVariantAttributeValueModel
                    {
                        Id = x.Id,
                        ProductVariantAttributeId = x.ProductVariantAttributeId,
                        Name = x.Name,
                        NameString = Server.HtmlEncode(x.Color.IsEmpty() ? x.Name : string.Format("{0} - {1}", x.Name, x.Color)),
                        Alias = x.Alias,
                        Color = x.Color,
                        PictureId = x.PictureId,
                        PriceAdjustment = x.PriceAdjustment,
                        PriceAdjustmentString = (x.ValueType == ProductVariantAttributeValueType.Simple ? x.PriceAdjustment.ToString("G29") : ""),
                        WeightAdjustment = x.WeightAdjustment,
                        WeightAdjustmentString = (x.ValueType == ProductVariantAttributeValueType.Simple ? x.WeightAdjustment.ToString("G29") : ""),
                        IsPreSelected = x.IsPreSelected,
                        DisplayOrder = x.DisplayOrder,
                        ValueTypeId = x.ValueTypeId,
                        TypeName = x.ValueType.GetLocalizedEnum(_localizationService, _workContext),
                        TypeNameClass = (x.ValueType == ProductVariantAttributeValueType.ProductLinkage ? "fa fa-link mr-2" : "d-none hide hidden-xs-up"),
                        LinkedProductId = x.LinkedProductId,
                        Quantity = x.Quantity
                    };

                    if (linkedProduct != null)
                    {
                        model.LinkedProductName = linkedProduct.GetLocalized(p => p.Name);
                        model.LinkedProductTypeName = linkedProduct.GetProductTypeLabel(_localizationService);
                        model.LinkedProductTypeLabelHint = linkedProduct.ProductTypeLabelHint;

                        if (model.Quantity > 1)
                            model.QuantityInfo = " × {0}".FormatWith(model.Quantity);
                    }

                    return model;
                });

                gridModel.Total = values.Count();
            }
            //else
            //{
            //    gridModel.Data = Enumerable.Empty<DeclarationProductModel.ProductVariantAttributeValueModel>();

            //    NotifyAccessDenied();
            //}

            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion DeclarationProduct variant attribute values

        #region DeclarationProduct variant attribute combinations

        public ActionResult AttributeCombinationCreatePopup(string btnId, string formId, int productId)
        {
            
                

            var product = _productService.GetProductById(productId);
            if (product == null)
                return RedirectToAction("List", "DeclarationProduct");

            var model = new ProductVariantAttributeCombinationModel();

            PrepareProductAttributeCombinationModel(model, null, (product));
            PrepareVariantCombinationAttributes(model, (product));
            PrepareVariantCombinationPictures(model, (product));
            PrepareDeliveryTimes(model);
            PrepareViewBag(btnId, formId, false, false);

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AttributeCombinationCreatePopup(
            string btnId,
            string formId,
            int productId,
            ProductVariantAttributeCombinationModel model,
            ProductVariantQuery query)
        {
            
                

            var product = _productService.GetProductById(productId);
            if (product == null)
                return RedirectToAction("List", "DeclarationProduct");

            var warnings = new List<string>();
            var variantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(product.Id);

            var attributeXml = query.CreateSelectedAttributesXml(product.Id, 0, variantAttributes, _productAttributeParser, _localizationService,
                _downloadService, _catalogSettings, this.Request, warnings);

            //warnings.AddRange(_shoppingCartService.GetShoppingCartItemAttributeWarnings(_workContext.CurrentCustomer, ShoppingCartType.ShoppingCart, (product), attributeXml));

            if (_productAttributeParser.FindProductVariantAttributeCombination(product.Id, attributeXml) != null)
            {
                warnings.Add(_localizationService.GetResource("Admin.Catalog.Products.ProductVariantAttributes.AttributeCombinations.CombiExists"));
            }

            if (warnings.Count == 0)
            {
                var combination = model.ToEntity();
                combination.AttributesXml = attributeXml;
                combination.SetAssignedPictureIds(model.AssignedPictureIds);

                _productAttributeService.InsertProductVariantAttributeCombination(combination);

                _productService.UpdateLowestAttributeCombinationPriceProperty(product);
            }

            PrepareProductAttributeCombinationModel(model, null, (product));
            PrepareVariantCombinationAttributes(model, (product));
            PrepareVariantCombinationPictures(model, (product));
            PrepareDeliveryTimes(model);
            PrepareViewBag(btnId, formId, warnings.Count == 0, false);

            if (warnings.Count > 0)
                model.Warnings = warnings;

            return View(model);
        }

        public ActionResult AttributeCombinationEditPopup(int id, string btnId, string formId)
        {
            
                

            var combination = _productAttributeService.GetProductVariantAttributeCombinationById(id);
            if (combination == null)
            {
                return RedirectToAction("List", "DeclarationProduct");
            }

            var product = _productService.GetProductById(combination.ProductId);
            if (product == null)
                return RedirectToAction("List", "DeclarationProduct");

            var model = combination.ToModel();

            PrepareProductAttributeCombinationModel(model, combination, (product), true);
            PrepareVariantCombinationAttributes(model, (product));
            PrepareVariantCombinationPictures(model, (product));
            PrepareDeliveryTimes(model, model.DeliveryTimeId);
            PrepareViewBag(btnId, formId);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AttributeCombinationEditPopup(string btnId, string formId, ProductVariantAttributeCombinationModel model, FormCollection form)
        {
            
                

            if (ModelState.IsValid)
            {
                var combination = _productAttributeService.GetProductVariantAttributeCombinationById(model.Id);
                if (combination == null)
                    return RedirectToAction("List", "DeclarationProduct");

                string attributeXml = combination.AttributesXml;
                combination = model.ToEntity(combination);
                combination.AttributesXml = attributeXml;
                combination.SetAssignedPictureIds(model.AssignedPictureIds);

                _productAttributeService.UpdateProductVariantAttributeCombination(combination);

               // _productService.UpdateLowestAttributeCombinationPriceProperty((combination.DeclarationProduct));

                PrepareViewBag(btnId, formId, true);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CombinationExistenceNote(int productId, ProductVariantQuery query)
        {
            // No further authorization here.
            var warnings = new List<string>();
            var attributes = _productAttributeService.GetProductVariantAttributesByProductId(productId);

            var attributeXml = query.CreateSelectedAttributesXml(productId, 0, attributes, _productAttributeParser,
                _localizationService, _downloadService, _catalogSettings, this.Request, warnings);

            var exists = (_productAttributeParser.FindProductVariantAttributeCombination(productId, attributeXml) != null);
            if (!exists)
            {
                var product = _productService.GetProductById(productId);
                //if (product != null)
                   // warnings.AddRange(_shoppingCartService.GetShoppingCartItemAttributeWarnings(_workContext.CurrentCustomer, ShoppingCartType.ShoppingCart, (product), attributeXml));
            }

            if (warnings.Count > 0)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        Message = warnings[0],
                        HasWarning = true
                    }
                };
            }

            return new JsonResult
            {
                Data = new
                {
                    Message = _localizationService.GetResource(exists ?
                        "Admin.Catalog.Products.ProductVariantAttributes.AttributeCombinations.CombiExists" :
                        "Admin.Catalog.Products.Variants.ProductVariantAttributes.AttributeCombinations.CombiNotExists"
                    ),
                    HasWarning = exists
                }
            };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateAllAttributeCombinations(ProductVariantAttributeCombinationModel model, int productId)
        {
            
                

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException(T("Products.NotFound", productId));

            //_productAttributeService.CreateAllProductVariantAttributeCombinations((product));

            _productService.UpdateLowestAttributeCombinationPriceProperty(product);

            return new JsonResult { Data = "" };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteAllAttributeCombinations(ProductVariantAttributeCombinationModel model, int productId)
        {
            
                

            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new ArgumentException(T("Products.NotFound", productId));

            _pvacRepository.DeleteAll(x => x.ProductId == product.Id);

            _productService.UpdateLowestAttributeCombinationPriceProperty(product);

            return new JsonResult { Data = "" };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeCombinationDelete(int id, GridCommand command)
        {
            var pvac = _productAttributeService.GetProductVariantAttributeCombinationById(id);
            var productId = pvac.ProductId;

            if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                _productAttributeService.DeleteProductVariantAttributeCombination(pvac);

                var product = _productService.GetProductById(productId);
                _productService.UpdateLowestAttributeCombinationPriceProperty(product);
            }

            return ProductVariantAttributeCombinationList(command, productId);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult ProductVariantAttributeCombinationList(GridCommand command, int productId)
        {
            var model = new GridModel<ProductVariantAttributeCombinationModel>();

            if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
            {
                var customer = _workContext.CurrentCustomer;
                var product = _productService.GetProductById(productId);
                var allCombinations = _productAttributeService.GetAllProductVariantAttributeCombinations(product.Id, command.Page - 1, command.PageSize);
                var productUrlTitle = T("Common.OpenInShop");
                var productSeName = product.GetSeName();

                _productAttributeParser.PrefetchProductVariantAttributes(allCombinations.Select(x => x.AttributesXml));

                var productVariantAttributesModel = allCombinations.Select(x =>
                {
                    var pvacModel = x.ToModel();
                    pvacModel.ProductId = product.Id;
                    pvacModel.ProductUrlTitle = productUrlTitle;
                    pvacModel.ProductUrl = _productUrlHelper.GetProductUrl(product.Id, productSeName, x.AttributesXml);
                    //pvacModel.AttributesXml = _productAttributeFormatter.FormatAttributes((product), x.AttributesXml, customer, "<br />", true, true, true, false);

                    // Not really necessary here:
                    //var warnings = _shoppingCartService.GetShoppingCartItemAttributeWarnings(
                    //		customer,
                    //		ShoppingCartType.ShoppingCart,
                    //		x.DeclarationProduct,
                    //		x.AttributesXml,
                    //		combination: x);

                    //pvacModel.Warnings.AddRange(warnings);

                    return pvacModel;
                })
                .ToList();

                model.Data = productVariantAttributesModel;
                model.Total = allCombinations.TotalCount;
            }
            else
            {
                model.Data = Enumerable.Empty<ProductVariantAttributeCombinationModel>();

                NotifyAccessDenied();
            }

            return new JsonResult
            {
                Data = model
            };
        }

        private void PrepareDeliveryTimes(ProductVariantAttributeCombinationModel model, int? selectId = null)
        {
            var deliveryTimes = _deliveryTimesService.GetAllDeliveryTimes();

            foreach (var dt in deliveryTimes)
            {
                model.AvailableDeliveryTimes.Add(new SelectListItem()
                {
                    Text = dt.Name,
                    Value = dt.Id.ToString(),
                    Selected = (selectId == dt.Id)
                });
            }
        }

        private void PrepareProductAttributeCombinationModel(
                                                                                            ProductVariantAttributeCombinationModel model,
            ProductVariantAttributeCombination entity,
            DeclarationProduct product, bool formatAttributes = false)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (product == null)
                throw new ArgumentNullException("variant");

            model.ProductId = product.Id;
            model.PrimaryStoreCurrencyCode = _services.StoreContext.CurrentStore.PrimaryStoreCurrency.CurrencyCode;
            model.BaseDimensionIn = _measureService.GetMeasureDimensionById(_measureSettings.BaseDimensionId)?.Name;

            if (entity == null)
            {
                // It's a new entity, so initialize it properly.
                model.StockQuantity = 10000;
                model.IsActive = true;
                model.AllowOutOfStockOrders = true;
            }

            if (formatAttributes && entity != null)
            {
                //model.AttributesXml = _productAttributeFormatter.FormatAttributes(product, entity.AttributesXml, _workContext.CurrentCustomer, "<br />", true, true, true, false);
            }
        }

        private void PrepareVariantCombinationAttributes(ProductVariantAttributeCombinationModel model, DeclarationProduct product)
        {
            var productVariantAttributes = _productAttributeService.GetProductVariantAttributesByProductId(product.Id);
            foreach (var attribute in productVariantAttributes)
            {
                var pvaModel = new ProductVariantAttributeCombinationModel.ProductVariantAttributeModel()
                {
                    Id = attribute.Id,
                    ProductAttributeId = attribute.ProductAttributeId,
                    Name = attribute.ProductAttribute.Name,
                    TextPrompt = attribute.TextPrompt,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var pvaValues = _productAttributeService.GetProductVariantAttributeValues(attribute.Id);
                    foreach (var pvaValue in pvaValues)
                    {
                        var pvaValueModel = new ProductVariantAttributeCombinationModel.ProductVariantAttributeValueModel()
                        {
                            Id = pvaValue.Id,
                            Name = pvaValue.Name,
                            IsPreSelected = pvaValue.IsPreSelected
                        };
                        pvaModel.Values.Add(pvaValueModel);
                    }
                }

                model.ProductVariantAttributes.Add(pvaModel);
            }
        }

        private void PrepareVariantCombinationPictures(ProductVariantAttributeCombinationModel model, DeclarationProduct product)
        {
            var pictures = _pictureService.GetPicturesByProductId(product.Id);
            foreach (var picture in pictures)
            {
                var assignablePicture = new ProductVariantAttributeCombinationModel.PictureSelectItemModel();
                assignablePicture.Id = picture.Id;
                assignablePicture.IsAssigned = model.AssignedPictureIds.Contains(picture.Id);
                assignablePicture.PictureUrl = _pictureService.GetUrl(picture, 125, FallbackPictureType.NoFallback);
                model.AssignablePictures.Add(assignablePicture);
            }
        }

        private void PrepareViewBag(string btnId, string formId, bool refreshPage = false, bool isEdit = true)
        {
            ViewBag.btnId = btnId;
            ViewBag.formId = formId;
            ViewBag.RefreshPage = refreshPage;
            ViewBag.IsEdit = isEdit;
        }

        #endregion DeclarationProduct variant attribute combinations

        #region downloads

        public ActionResult DeleteDownloadVersion(int downloadId, int productId)
        {
            var download = _downloadService.GetDownloadById(downloadId);
            if (download == null)
            {
                return HttpNotFound();
            }

            _downloadService.DeleteDownload(download);

            NotifySuccess(T("Admin.Common.TaskSuccessfullyProcessed"));

            return RedirectToAction("Edit", new { id = productId });
        }

        #endregion downloads

        #region Hidden normalizers

        public ActionResult FixProductMainPictureIds(DateTime? ifModifiedSinceUtc = null)
        {
            
                

            var count = DataMigrator.FixProductMainPictureIds(_dbContext, ifModifiedSinceUtc);

            return Content("Fixed {0} ids.".FormatInvariant(count));
        }

        #endregion Hidden normalizers
    }
}