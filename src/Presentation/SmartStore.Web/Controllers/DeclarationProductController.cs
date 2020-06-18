using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using SmartStore;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Seo;
using SmartStore.Core.Domain.Tax;
using SmartStore.Services;
using SmartStore.Services.Catalog;
using SmartStore.Services.Catalog.Modelling;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Orders;
using SmartStore.Services.Security;
using SmartStore.Services.Seo;
using SmartStore.Services.Stores;
using SmartStore.Services.Tax;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Framework.UI;
using SmartStore.Web.Infrastructure.Cache;
using SmartStore.Web.Models.Catalog;

namespace SmartStore.Web.Controllers
{
    public partial class DeclarationProductController : PublicControllerBase
	{
		private readonly ICommonServices _services;
		private readonly IManufacturerService _manufacturerService;
		private readonly IProductService _productService;
		private readonly IDeclarationProductService _dproductService;
		private readonly IProductAttributeService _productAttributeService;
		private readonly ITaxService _taxService;
		private readonly IPictureService _pictureService;
		private readonly ICustomerContentService _customerContentService;
		private readonly ICustomerService _customerService;
		private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
		private readonly IProductTagService _productTagService;
		private readonly IOrderReportService _orderReportService;
		private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
		private readonly IAclService _aclService;
		private readonly IStoreMappingService _storeMappingService;
		private readonly MediaSettings _mediaSettings;
		private readonly SeoSettings _seoSettings;
		private readonly CatalogSettings _catalogSettings;
		private readonly ShoppingCartSettings _shoppingCartSettings;
		private readonly LocalizationSettings _localizationSettings;
		private readonly CaptchaSettings _captchaSettings;
		private readonly CatalogHelper _helper;
		private readonly IBreadcrumb _breadcrumb;
		private readonly Lazy<PrivacySettings> _privacySettings;
		private readonly Lazy<TaxSettings> _taxSettings;

		public DeclarationProductController(
			ICommonServices services,
			IManufacturerService manufacturerService,
			IProductService productService,
			IDeclarationProductService dproductService,
			IProductAttributeService productAttributeService,
			ITaxService taxService,
			IPictureService pictureService,
			ICustomerContentService customerContentService, 
			ICustomerService customerService,
			IRecentlyViewedProductsService recentlyViewedProductsService, 
			IProductTagService productTagService,
			IOrderReportService orderReportService,
			IBackInStockSubscriptionService backInStockSubscriptionService, 
			IAclService aclService,
			IStoreMappingService storeMappingService,
			MediaSettings mediaSettings,
			SeoSettings seoSettings,
			CatalogSettings catalogSettings,
			ShoppingCartSettings shoppingCartSettings,
			LocalizationSettings localizationSettings, 
			CaptchaSettings captchaSettings,
			CatalogHelper helper,
			IBreadcrumb breadcrumb,
			Lazy<PrivacySettings> privacySettings,
            Lazy<TaxSettings> taxSettings)
        {
			_services = services;
			_manufacturerService = manufacturerService;
			_productService = productService;
			_productAttributeService = productAttributeService;
			_taxService = taxService;
			_pictureService = pictureService;
			_customerContentService = customerContentService;
			_customerService = customerService;
			_recentlyViewedProductsService = recentlyViewedProductsService;
			_productTagService = productTagService;
			_orderReportService = orderReportService;
			_backInStockSubscriptionService = backInStockSubscriptionService;
			_aclService = aclService;
			_storeMappingService = storeMappingService;
			_mediaSettings = mediaSettings;
			_seoSettings = seoSettings;
			_catalogSettings = catalogSettings;
			_shoppingCartSettings = shoppingCartSettings;
			_localizationSettings = localizationSettings;
			_captchaSettings = captchaSettings;
			_helper = helper;
			_breadcrumb = breadcrumb;
			_privacySettings = privacySettings;
			_taxSettings = taxSettings;
			_dproductService = dproductService;
		}

		#region Products

		[RewriteUrl(SslRequirement.No)]
		public ActionResult ProductDetails(int productId)
		{
			var product = _dproductService.GetProductById(productId);
			if (product == null || product.Deleted || product.IsSystemProduct)
				return HttpNotFound();

			// Is published? Check whether the current user has a "Manage catalog" permission.
			// It allows him to preview a product before publishing.
			if (!product.Published && !_services.Permissions.Authorize(StandardPermissionProvider.ManageCatalog))
				return HttpNotFound();

			// ACL (access control list)
			if (!_aclService.Authorize(product))
				return HttpNotFound();

			// Store mapping
			if (!_storeMappingService.Authorize(product))
				return HttpNotFound();

			// Is product individually visible?
			if (!product.VisibleIndividually)
			{
				// Find parent grouped product.
				var parentGroupedProduct = _dproductService.GetProductById(product.ParentGroupedProductId);
				if (parentGroupedProduct == null)
					return HttpNotFound();

                var seName = parentGroupedProduct.GetSeName();
                if (seName.IsEmpty())
                    return HttpNotFound();

                var routeValues = new RouteValueDictionary();
				routeValues.Add("SeName", seName);

				// Add query string parameters.
				Request.QueryString.AllKeys.Each(x => routeValues.Add(x, Request.QueryString[x]));

				return RedirectToRoute("Product", routeValues);
			}

			// Prepare the view model
			var model = _helper.PrepareProductDetailsPageModel(product, new ProductVariantQuery() {  });

			// Some cargo data
			model.PictureSize = _mediaSettings.ProductDetailsPictureSize;
			model.CanonicalUrlsEnabled = _seoSettings.CanonicalUrlsEnabled;

			// Save as recently viewed
			//_recentlyViewedProductsService.AddProductToRecentlyViewedList(product.Id);

			// Activity log
			//_services.CustomerActivity.InsertActivity("PublicStore.ViewProduct", T("ActivityLog.PublicStore.ViewProduct"), product.Name);

			// Breadcrumb
			//if (_catalogSettings.CategoryBreadcrumbEnabled)
			//{
   //             _helper.GetCategoryBreadcrumbD(_breadcrumb, ControllerContext, product);

			//	_breadcrumb.Track(new MenuItem
			//	{
			//		Text = model.Name,
			//		Rtl = model.Name.CurrentLanguage.Rtl,
			//		EntityId = product.Id,
			//		Url = Url.RouteUrl("Product", new { model.SeName })
			//	});
			//}

			return View(model.ProductTemplateViewPath, model);
		}

		[ChildActionOnly]
		public ActionResult ProductManufacturers(int productId, bool preparePictureModel = false)
		{
			var cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_MANUFACTURERS_MODEL_KEY,
				productId,
				!_catalogSettings.HideManufacturerDefaultPictures,
				_services.WorkContext.WorkingLanguage.Id,
				_services.StoreContext.CurrentStore.Id);

			var cacheModel = _services.Cache.Get(cacheKey, () =>
			{
				var model = _manufacturerService.GetProductManufacturersByProductId(productId)
					.Select(x =>
					{
						var m = x.Manufacturer.ToModel();
						if (preparePictureModel)
						{
							m.PictureModel.ImageUrl = _pictureService.GetUrl(x.Manufacturer.PictureId.GetValueOrDefault(), 0, !_catalogSettings.HideManufacturerDefaultPictures);
							var pictureUrl = _pictureService.GetUrl(x.Manufacturer.PictureId.GetValueOrDefault());
							if (pictureUrl != null)
							{
								m.PictureModel.PictureId = x.Manufacturer.PictureId.GetValueOrDefault();
								m.PictureModel.Title = string.Format(T("Media.Product.ImageLinkTitleFormat"), m.Name);
								m.PictureModel.AlternateText = string.Format(T("Media.Product.ImageAlternateTextFormat"), m.Name);
							}
						}
						return m;
					})
					.ToList();
				return model;
			}, TimeSpan.FromHours(6));

			if (cacheModel.Count == 0)
				return Content("");

			return PartialView(cacheModel);
		}

		[ChildActionOnly]
		public ActionResult ReviewSummary(int id /* productId */)
		{
			var product = _dproductService.GetProductById(id);
			if (product == null)
				throw new ArgumentException(T("Products.NotFound", id));

			var model = new ProductReviewOverviewModel
			{
				ProductId = product.Id,
				RatingSum = product.ApprovedRatingSum,
				TotalReviews = product.ApprovedTotalReviews,
				AllowCustomerReviews = product.AllowCustomerReviews
			};

			return PartialView("Product.ReviewSummary", model);
		}

		[ChildActionOnly]
		public ActionResult ProductSpecifications(int productId)
		{
			return Content("");
			//var product = _dproductService.GetProductById(productId);
			//if (product == null)
			//{
			//	throw new ArgumentException(T("Products.NotFound", productId));
			//}			

			//var model = _helper.PrepareProductSpecificationModel(product);

			//if (model.Count == 0)
			//{
			//	return Content("");
			//}		

			//return PartialView("Product.Specs", model);
		}

		[ChildActionOnly]
		public ActionResult ProductDetailReviews(int productId)
		{
			return Content("");
			//var product = _dproductService.GetProductById(productId);
			//if (product == null || !product.AllowCustomerReviews)
			//{
			//	return Content("");
			//}
				
			//var model = new ProductReviewsModel();
			//_helper.PrepareProductReviewsModel(model, product, 10);

			//return PartialView("Product.Reviews", model);
		}

		[ChildActionOnly]
		public ActionResult ProductTierPrices(int productId)
		{
			return Content("");
			//if (!_services.Permissions.Authorize(StandardPermissionProvider.DisplayPrices))
			//{
			//	return Content("");
			//}	

			//var product = _dproductService.GetProductById(productId);
			//if (product == null)
			//{
			//	throw new ArgumentException(T("Products.NotFound", productId));
			//}
			
			//if (!product.HasTierPrices)
			//{
			//	// No tier prices
			//	return Content(""); 
			//}

   //         var model = _helper.CreateTierPriceModel(product);
            
   //         return PartialView("Product.TierPrices", model);
		}

		[ChildActionOnly]
		public ActionResult RelatedProducts(int productId, int? productThumbPictureSize)
		{
			return Content("");
			//var products = new List<Product>();
			//var relatedProducts = _dproductService.GetRelatedProductsByProductId1(productId);

			//foreach (var product in _dproductService.GetProductsByIds(relatedProducts.Select(x => x.ProductId2).ToArray()))
			//{
			//	// Ensure has ACL permission and appropriate store mapping
			//	if (_aclService.Authorize(product) && _storeMappingService.Authorize(product))
			//		products.Add(product);
			//}

			//if (products.Count == 0)
			//{
			//	return Content("");
			//}

			//var settings = _helper.GetBestFitProductSummaryMappingSettings(ProductSummaryViewMode.Grid, x =>
			//{
			//	x.ThumbnailSize = productThumbPictureSize;
			//	x.MapDeliveryTimes = false;
			//});		

			//var model = _helper.MapProductSummaryModel(products, settings);
			//model.ShowBasePrice = false;

			//return PartialView("Product.RelatedProducts", model);
		}

		[ChildActionOnly]
		public ActionResult ProductsAlsoPurchased(int productId, int? productThumbPictureSize)
		{
			if (!_catalogSettings.ProductsAlsoPurchasedEnabled)
			{
				return Content("");
			}				

			// load and cache report
			var productIds = _services.Cache.Get(string.Format(ModelCacheEventConsumer.PRODUCTS_ALSO_PURCHASED_IDS_KEY, productId, _services.StoreContext.CurrentStore.Id), () => 
			{
				return _orderReportService.GetAlsoPurchasedProductsIds(_services.StoreContext.CurrentStore.Id, productId, _catalogSettings.ProductsAlsoPurchasedNumber);
			});

			// Load products
			var products = _dproductService.GetProductsByIds(productIds);

			// ACL and store mapping
			products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

			if (products.Count == 0)
			{
				return Content("");
			}			

			// Prepare model
			var settings = _helper.GetBestFitProductSummaryMappingSettings(ProductSummaryViewMode.Mini, x =>
			{
				x.ThumbnailSize = productThumbPictureSize;
			});

			var model = _helper.MapProductSummaryModel(products, settings);

			return PartialView("Product.AlsoPurchased", model);
		}

		[ChildActionOnly]
		public ActionResult CrossSellProducts(int? productThumbPictureSize)
		{
			return PartialView(ProductSummaryModel.Empty);
			//var cart = _services.WorkContext.CurrentCustomer.GetCartItems(ShoppingCartType.ShoppingCart, _services.StoreContext.CurrentStore.Id);

			//var products = _dproductService.GetCrosssellProductsByShoppingCart(cart, _shoppingCartSettings.CrossSellsNumber);

			//// ACL and store mapping
			//products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

			//if (products.Any())
			//{
			//	// Cross-sell products are dispalyed on the shopping cart page.
			//	// We know that the entire shopping cart page is not refresh
			//	// even if "ShoppingCartSettings.DisplayCartAfterAddingProduct" setting  is enabled.
			//	// That's why we force page refresh (redirect) in this case
			//	var settings = _helper.GetBestFitProductSummaryMappingSettings(ProductSummaryViewMode.Grid, x =>
			//	{
			//		x.ThumbnailSize = productThumbPictureSize;
			//		x.ForceRedirectionAfterAddingToCart = true;
			//	});

			//	var model = _helper.MapProductSummaryModel(products, settings);

			//	return PartialView(model);
			//}

			//return PartialView(ProductSummaryModel.Empty);
		}

		[ActionName("BackInStockSubscribe")]
		public ActionResult BackInStockSubscribePopup(int id /* productId */)
		{
			var product = _dproductService.GetProductById(id);
			if (product == null || product.Deleted || product.IsSystemProduct)
			{
				throw new ArgumentException(T("Products.NotFound", id));
			}

			var customer = _services.WorkContext.CurrentCustomer;
			var store = _services.StoreContext.CurrentStore;

			var model = new BackInStockSubscribeModel();
			model.ProductId = product.Id;
			model.ProductName = product.GetLocalized(x => x.Name);
			model.ProductSeName = product.GetSeName();
			model.IsCurrentCustomerRegistered = customer.IsRegistered();
			model.MaximumBackInStockSubscriptions = _catalogSettings.MaximumBackInStockSubscriptions;
			model.CurrentNumberOfBackInStockSubscriptions = _backInStockSubscriptionService
				 .GetAllSubscriptionsByCustomerId(customer.Id, store.Id, 0, 1)
				 .TotalCount;

			if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
				product.BackorderMode == BackorderMode.NoBackorders &&
				product.AllowBackInStockSubscriptions &&
				product.StockQuantity <= 0)
			{
				// Out of stock.
				model.SubscriptionAllowed = true;
				model.AlreadySubscribed = _backInStockSubscriptionService.FindSubscription(customer.Id, product.Id, store.Id) != null;
			}

			return View("BackInStockSubscribePopup", model);
		}

		[HttpPost]
		public ActionResult BackInStockSubscribePopup(int id /* productId */, FormCollection form)
		{
			var product = _dproductService.GetProductById(id);
			if (product == null || product.Deleted || product.IsSystemProduct)
			{
				throw new ArgumentException(T("Products.NotFound", id));
			}

			if (!_services.WorkContext.CurrentCustomer.IsRegistered())
			{
				return Content(T("BackInStockSubscriptions.OnlyRegistered"));
			}

			if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
				product.BackorderMode == BackorderMode.NoBackorders &&
				product.AllowBackInStockSubscriptions &&
				product.StockQuantity <= 0)
			{
				var customer = _services.WorkContext.CurrentCustomer;
				var store = _services.StoreContext.CurrentStore;

				// Out of stock.
				var subscription = _backInStockSubscriptionService.FindSubscription(customer.Id, product.Id, store.Id);
				if (subscription != null)
				{
					// Unsubscribe.
					_backInStockSubscriptionService.DeleteSubscription(subscription);
					return Content("Unsubscribed");
				}
				else
				{
					if (_backInStockSubscriptionService.GetAllSubscriptionsByCustomerId(customer.Id, store.Id, 0, 1).TotalCount >= _catalogSettings.MaximumBackInStockSubscriptions)
					{
						return Content(string.Format(T("BackInStockSubscriptions.MaxSubscriptions"), _catalogSettings.MaximumBackInStockSubscriptions));
					}

					// Subscribe.
					//subscription = new BackInStockSubscription
					//{
					//	Customer = customer,
					//	Product = product,
					//	StoreId = store.Id,
					//	CreatedOnUtc = DateTime.UtcNow
					//};

					_backInStockSubscriptionService.InsertSubscription(subscription);
					return Content("Subscribed");
				}
			}
			else
			{
				return Content(T("BackInStockSubscriptions.NotAllowed"));
			}
		}

		//[HttpPost]
  //      [ValidateInput(false)]
  //      public ActionResult UpdateProductDetails(int productId, string itemType, int bundleItemId, ProductVariantQuery query, FormCollection form)
		//{
		//	int quantity = 1;
		//	int galleryStartIndex = -1;
		//	string galleryHtml = null;
		//	string dynamicThumbUrl = null;
		//	var isAssociated = itemType.IsCaseInsensitiveEqual("associateditem");
		//	var pictureModel = new ProductDetailsPictureModel();
		//	var m = new ProductDetailsModel();
		//	var product = _dproductService.GetProductById(productId);
		//	//var bItem = _dproductService.GetBundleItemById(bundleItemId);
		//	IList<ProductBundleItemData> bundleItems = null;
		//	ProductBundleItemData bundleItem = (bItem == null ? null : new ProductBundleItemData(bItem));

		//	// Quantity required for tier prices.
		//	string quantityKey = form.AllKeys.FirstOrDefault(k => k.EndsWith("EnteredQuantity"));
		//	if (quantityKey.HasValue())
		//	{
		//		int.TryParse(form[quantityKey], out quantity);
		//	}

		//	if (product.ProductType == ProductType.BundledProduct && product.BundlePerItemPricing)
		//	{
		//		//bundleItems = _dproductService.GetBundleItems(product.Id);
		//		//if (query.Variants.Count > 0)
		//		//{
		//		//	// May add elements to query object if they are preselected by bundle item filter.
		//		//	foreach (var itemData in bundleItems)
		//		//	{
		//		//		_helper.PrepareProductDetailsPageModel(itemData.Item.Product, query, false, itemData, null);
		//		//	}
		//		//}
		//	}

		//	// Get merged model data.
		//	_helper.PrepareProductDetailModel(m, product, query, isAssociated, bundleItem, bundleItems, quantity);

		//	if (bundleItem != null)
		//	{
		//		// Update bundle item thumbnail.
		//		if (!bundleItem.Item.HideThumbnail)
		//		{
		//			var picture = m.GetAssignedPicture(_pictureService, null, bundleItem.Item.ProductId);
		//			dynamicThumbUrl = _pictureService.GetUrl(picture, _mediaSettings.BundledProductPictureSize, false);
		//		}
		//	}
		//	else if (isAssociated)
		//	{
		//		// Update associated product thumbnail.
		//		var picture = m.GetAssignedPicture(_pictureService, null, productId);
		//		dynamicThumbUrl = _pictureService.GetUrl(picture, _mediaSettings.AssociatedProductPictureSize, false);
		//	}
		//	else if (product.ProductType != ProductType.BundledProduct)
		//	{
		//		// Update image gallery.
		//		var pictures = _pictureService.GetPicturesByProductId(productId);

  //              if (product.HasPreviewPicture && pictures.Count > 1)
  //              {
  //                  pictures.RemoveAt(0);
  //              }

  //              if (pictures.Count <= _catalogSettings.DisplayAllImagesNumber)
		//		{
		//			// All pictures rendered... only index is required.
		//			var picture = m.GetAssignedPicture(_pictureService, pictures);
		//			galleryStartIndex = (picture == null ? 0 : pictures.IndexOf(picture));
		//		}
		//		else
		//		{
		//			var allCombinationPictureIds = _productAttributeService.GetAllProductVariantAttributeCombinationPictureIds(product.Id);

		//			_helper.PrepareProductDetailsPictureModel(
		//				pictureModel,
		//				pictures,
		//				product.GetLocalized(x => x.Name),
		//				allCombinationPictureIds,
		//				false,
		//				bundleItem,
		//				m.SelectedCombination);

		//			galleryStartIndex = pictureModel.GalleryStartIndex;
		//			galleryHtml = this.RenderPartialViewToString("Product.Picture", pictureModel);
		//		}

  //              m.PriceDisplayStyle = _catalogSettings.PriceDisplayStyle;
  //              m.DisplayTextForZeroPrices = _catalogSettings.DisplayTextForZeroPrices;
  //          }

		//	object partials = null;
			
		//	if (m.IsBundlePart)
		//	{
		//		partials = new
		//		{
		//			BundleItemPrice = this.RenderPartialViewToString("Product.Offer.Price", m),
		//			BundleItemStock = this.RenderPartialViewToString("Product.StockInfo", m)
		//		};
		//	}
		//	else
		//	{
		//		var dataDictAddToCart = new ViewDataDictionary();
		//		dataDictAddToCart.TemplateInfo.HtmlFieldPrefix = string.Format("addtocart_{0}", m.Id);

		//		decimal adjustment = decimal.Zero;
		//		decimal taxRate = decimal.Zero;
		//		var finalPriceWithDiscountBase = 0M;
		//			//_taxService.GetProductPrice(product, product.Price, _services.WorkContext.CurrentCustomer, out taxRate);
				
		//		if (!_taxSettings.Value.PricesIncludeTax && _services.WorkContext.TaxDisplayType == TaxDisplayType.IncludingTax)
		//		{
		//			adjustment = (m.ProductPrice.PriceValue - finalPriceWithDiscountBase) / (taxRate / 100 + 1);
		//		}
		//		else if(_taxSettings.Value.PricesIncludeTax && _services.WorkContext.TaxDisplayType == TaxDisplayType.ExcludingTax)
		//		{
		//			adjustment = (m.ProductPrice.PriceValue - finalPriceWithDiscountBase) * (taxRate / 100 + 1);
		//		}
		//		else
		//		{
		//			adjustment = m.ProductPrice.PriceValue - finalPriceWithDiscountBase;
		//		}

		//		partials = new
		//		{
		//			Attrs = this.RenderPartialViewToString("Product.Attrs", m),
		//			Price = this.RenderPartialViewToString("Product.Offer.Price", m),
		//			Stock = this.RenderPartialViewToString("Product.StockInfo", m),
		//			OfferActions = this.RenderPartialViewToString("Product.Offer.Actions", m, dataDictAddToCart),
		//			//TierPrices = this.RenderPartialViewToString("Product.TierPrices", _helper.CreateTierPriceModel(product, adjustment)),
  //                  BundlePrice = product.ProductType == ProductType.BundledProduct ? this.RenderPartialViewToString("Product.Bundle.Price", m) : (string)null
		//		};
		//	}

		//	object data = new
		//	{
		//		Partials = partials,
		//		DynamicThumblUrl = dynamicThumbUrl,
		//		GalleryStartIndex = galleryStartIndex,
		//		GalleryHtml = galleryHtml
		//	};

		//	return new JsonResult { Data = data };
		//}

		#endregion

	}
}
