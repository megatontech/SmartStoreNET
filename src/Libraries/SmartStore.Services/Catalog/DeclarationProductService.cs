using SmartStore.Collections;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Orders;
using SmartStore.Data.Caching;
using SmartStore.Services.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace SmartStore.Services.Catalog
{
    public partial class DeclarationProductService : IDeclarationProductService
    {
        #region Private Fields

        private readonly IDbContext _dbContext;

        private readonly LocalizationSettings _localizationSettings;

        private readonly IProductAttributeParser _productAttributeParser;

        private readonly IProductAttributeService _productAttributeService;

        private readonly IRepository<DeclarationProductPicture> _productPictureRepository;

        private readonly IRepository<DeclarationProduct> _productRepository;

        private readonly IRepository<ProductVariantAttributeCombination> _productVariantAttributeCombinationRepository;

        private readonly ICommonServices _services;

        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        private readonly IRepository<TierPrice> _tierPriceRepository;

        #endregion Private Fields

        #region Public Constructors

        public DeclarationProductService(
            IRepository<DeclarationProduct> productRepository,
            IRepository<TierPrice> tierPriceRepository,
            IRepository<DeclarationProductPicture> productPictureRepository,
            IRepository<ProductVariantAttributeCombination> productVariantAttributeCombinationRepository,
            IRepository<ShoppingCartItem> shoppingCartItemRepository,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            IDbContext dbContext,
            LocalizationSettings localizationSettings,
            ICommonServices services)
        {
            _productRepository = productRepository;
            _tierPriceRepository = tierPriceRepository;
            _productPictureRepository = productPictureRepository;
            _productVariantAttributeCombinationRepository = productVariantAttributeCombinationRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productAttributeService = productAttributeService;
            _productAttributeParser = productAttributeParser;
            _dbContext = dbContext;
            _localizationSettings = localizationSettings;
            _services = services;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual AdjustInventoryResult AdjustInventory(OrganizedShoppingCartItem sci, bool decrease)
        {
            if (sci == null)
                throw new ArgumentNullException("cartItem");

            if (sci.Item.Product.ProductType == ProductType.BundledProduct && sci.Item.Product.BundlePerItemShoppingCart)
            {
                if (sci.ChildItems != null)
                {
                    foreach (var child in sci.ChildItems.Where(x => x.Item.Id != sci.Item.Id))
                    {
                        AdjustInventory(child.Item.Product.convertProduct(), decrease, sci.Item.Quantity * child.Item.Quantity, child.Item.AttributesXml);
                    }
                }
                return new AdjustInventoryResult();
            }
            else
            {
                return AdjustInventory(sci.Item.Product.convertProduct(), decrease, sci.Item.Quantity, sci.Item.AttributesXml);
            }
        }

        public virtual AdjustInventoryResult AdjustInventory(OrderItem orderItem, bool decrease, int quantity)
        {
            Guard.NotNull(orderItem, nameof(orderItem));

            if (orderItem.Product.ProductType == ProductType.BundledProduct && orderItem.Product.BundlePerItemShoppingCart)
            {
                if (orderItem.BundleData.HasValue())
                {
                    var bundleData = orderItem.GetBundleData();
                    if (bundleData.Count > 0)
                    {
                        var products = GetProductsByIds(bundleData.Select(x => x.ProductId).ToArray());

                        foreach (var item in bundleData)
                        {
                            var product = products.FirstOrDefault(x => x.Id == item.ProductId);
                            if (product != null)
                                AdjustInventory(product, decrease, quantity * item.Quantity, item.AttributesXml);
                        }
                    }
                }
                return new AdjustInventoryResult();
            }
            else
            {
                return AdjustInventory(orderItem.Product.convertProduct(), decrease, quantity, orderItem.AttributesXml);
            }
        }

        public virtual AdjustInventoryResult AdjustInventory(DeclarationProduct product, bool decrease, int quantity, string attributesXml)
        {
            Guard.NotNull(product, nameof(product));

            var result = new AdjustInventoryResult();

            switch (product.ManageInventoryMethod)
            {
                case ManageInventoryMethod.DontManageStock:
                    {
                        //do nothing
                    }
                    break;

                case ManageInventoryMethod.ManageStock:
                    {
                        result.StockQuantityOld = product.StockQuantity;
                        if (decrease)
                            result.StockQuantityNew = product.StockQuantity - quantity;
                        else
                            result.StockQuantityNew = product.StockQuantity + quantity;

                        bool newPublished = product.Published;
                        bool newDisableBuyButton = product.DisableBuyButton;
                        bool newDisableWishlistButton = product.DisableWishlistButton;

                        //check if minimum quantity is reached
                        switch (product.LowStockActivity)
                        {
                            case LowStockActivity.DisableBuyButton:
                                newDisableBuyButton = product.MinStockQuantity >= result.StockQuantityNew;
                                newDisableWishlistButton = product.MinStockQuantity >= result.StockQuantityNew;
                                break;

                            case LowStockActivity.Unpublish:
                                newPublished = product.MinStockQuantity <= result.StockQuantityNew;
                                break;
                        }

                        product.StockQuantity = result.StockQuantityNew;
                        product.DisableBuyButton = newDisableBuyButton;
                        product.DisableWishlistButton = newDisableWishlistButton;
                        product.Published = newPublished;

                        UpdateProduct(product);

                        //send email notification
                        //if (decrease && product.NotifyAdminForQuantityBelow > result.StockQuantityNew)
                        //    _services.MessageFactory.SendQuantityBelowStoreOwnerNotification(product, _localizationSettings.DefaultAdminLanguageId);
                    }
                    break;

                case ManageInventoryMethod.ManageStockByAttributes:
                    {
                        var combination = _productAttributeParser.FindProductVariantAttributeCombination(product.Id, attributesXml);
                        if (combination != null)
                        {
                            result.StockQuantityOld = combination.StockQuantity;
                            if (decrease)
                                result.StockQuantityNew = combination.StockQuantity - quantity;
                            else
                                result.StockQuantityNew = combination.StockQuantity + quantity;

                            combination.StockQuantity = result.StockQuantityNew;
                            _productAttributeService.UpdateProductVariantAttributeCombination(combination);
                        }
                    }
                    break;

                default:
                    break;
            }

            var attributeValues = _productAttributeParser.ParseProductVariantAttributeValues(attributesXml);

            attributeValues
                .Where(x => x.ValueType == ProductVariantAttributeValueType.ProductLinkage)
                .ToList()
                .Each(x =>
            {
                var linkedProduct = GetProductById(x.LinkedProductId);
                if (linkedProduct != null)
                    AdjustInventory(linkedProduct, decrease, quantity * x.Quantity, "");
            });

            return result;
        }

        public virtual void DeleteProduct(DeclarationProduct product)
        {
            Guard.NotNull(product, nameof(product));

            product.Deleted = true;
            product.DeliveryTimeId = null;
            product.QuantityUnitId = null;
            product.CountryOfOriginId = null;

            UpdateProduct(product);

            if (product.ProductType == ProductType.GroupedProduct)
            {
                var associatedProducts = _productRepository.Table
                    .Where(x => x.ParentGroupedProductId == product.Id)
                    .ToList();

                associatedProducts.ForEach(x => x.ParentGroupedProductId = 0);

                _dbContext.SaveChanges();
            }
        }

        public virtual void DeleteProductPicture(DeclarationProductPicture productPicture)
        {
            Guard.NotNull(productPicture, nameof(productPicture));

            UnassignDeletedPictureFromVariantCombinations(productPicture);

            _productPictureRepository.Delete(productPicture);
        }

        public virtual void DeleteTierPrice(TierPrice tierPrice)
        {
            Guard.NotNull(tierPrice, nameof(tierPrice));

            _tierPriceRepository.Delete(tierPrice);
        }

        public virtual IList<DeclarationProduct> GetAllProductsDisplayedOnHomePage()
        {
            var query =
                from p in _productRepository.Table
                orderby p.HomePageDisplayOrder
                where p.Published && !p.Deleted
                select p;

            var products = query.ToList();
            return products;
        }

        public Multimap<int, Discount> GetAppliedDiscountsByProductIds(int[] productIds)
        {
            throw new NotImplementedException();
        }

        public virtual Multimap<int, DeclarationProduct> GetAssociatedProductsByProductIds(int[] productIds, bool showHidden = false)
        {
            Guard.NotNull(productIds, nameof(productIds));

            if (!productIds.Any())
            {
                return new Multimap<int, DeclarationProduct>();
            }

            // Ignore multistore. Expect multistore setting for associated products is the same as for parent grouped product.
            var query = _productRepository.TableUntracked
                .Where(x => productIds.Contains(x.ParentGroupedProductId) && !x.Deleted && (showHidden || x.Published))
                .OrderBy(x => x.ParentGroupedProductId)
                .ThenBy(x => x.DisplayOrder);

            var associatedProducts = query.ToList();

            var map = associatedProducts
                .ToMultimap(x => x.ParentGroupedProductId, x => x);

            return map;
        }

        public virtual DeclarationProduct GetProductByGtin(string gtin)
        {
            if (String.IsNullOrEmpty(gtin))
                return null;

            gtin = gtin.Trim();

            var query = from p in _productRepository.Table
                        orderby p.Id
                        where !p.Deleted && p.Gtin == gtin
                        select p;
            var product = query.FirstOrDefault();
            return product;
        }

        public virtual DeclarationProduct GetProductById(int productId)
        {
            if (productId == 0)
                return null;

            return _productRepository.GetByIdCached(productId, "db.product.id-" + productId);
        }

        public virtual DeclarationProduct GetProductByManufacturerPartNumber(string manufacturerPartNumber)
        {
            if (manufacturerPartNumber.IsEmpty())
                return null;

            manufacturerPartNumber = manufacturerPartNumber.Trim();

            var product = _productRepository.Table
                .Where(x => !x.Deleted && x.ManufacturerPartNumber == manufacturerPartNumber)
                .OrderBy(x => x.Id)
                .FirstOrDefault();

            return product;
        }

        public virtual DeclarationProduct GetProductByName(string name)
        {
            if (name.IsEmpty())
                return null;

            name = name.Trim();

            var product = _productRepository.Table
                .Where(x => !x.Deleted && x.Name == name)
                .OrderBy(x => x.Id)
                .FirstOrDefault();

            return product;
        }

        public virtual DeclarationProduct GetProductBySku(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();

            var query = from p in _productRepository.Table
                        orderby p.DisplayOrder, p.Id
                        where !p.Deleted && p.Sku == sku
                        select p;
            var product = query.FirstOrDefault();
            return product;
        }

        public virtual DeclarationProduct GetProductBySystemName(string systemName)
        {
            if (systemName.IsEmpty())
            {
                return null;
            }

            var product = _productRepository.Table.FirstOrDefault(x => x.SystemName == systemName && x.IsSystemProduct);
            return product;
        }

        public virtual DeclarationProductPicture GetProductPictureById(int productPictureId)
        {
            if (productPictureId == 0)
                return null;

            var pp = _productPictureRepository.GetById(productPictureId);
            return pp;
        }

        public virtual IList<DeclarationProductPicture> GetProductPicturesByProductId(int productId)
        {
            var query = from pp in _productPictureRepository.Table
                        where pp.ProductId == productId
                        orderby pp.DisplayOrder
                        select pp;
            var productPictures = query.ToList();
            return productPictures;
        }

        public virtual Multimap<int, DeclarationProductPicture> GetProductPicturesByProductIds(int[] productIds, bool onlyFirstPicture = false)
        {
            Guard.NotNull(productIds, nameof(productIds));

            if (!productIds.Any())
            {
                return new Multimap<int, DeclarationProductPicture>();
            }

            var query =
                from pp in _productPictureRepository.TableUntracked.Expand(x => x.Picture)
                where productIds.Contains(pp.ProductId)
                orderby pp.ProductId, pp.DisplayOrder
                select pp;

            if (onlyFirstPicture)
            {
                var map = query.GroupBy(x => x.ProductId, x => x)
                    .Select(x => x.FirstOrDefault())
                    .ToList()
                    .ToMultimap(x => x.ProductId, x => x);

                return map;
            }
            else
            {
                var map = query
                    .ToList()
                    .ToMultimap(x => x.ProductId, x => x);

                return map;
            }
        }

        public virtual IList<DeclarationProduct> GetProductsByIds(int[] productIds, ProductLoadFlags flags = ProductLoadFlags.None)
        {
            if (productIds == null || !productIds.Any())
            {
                return new List<DeclarationProduct>();
            }

            var query = from p in _productRepository.Table
                        where productIds.Contains(p.Id)
                        select p;

            if (flags > ProductLoadFlags.None)
            {
                query = ApplyLoadFlags(query, flags);
            }

            var products = query.ToList();

            // Sort by passed identifier sequence.
            return products.OrderBySequence(productIds).ToList();
        }

        public virtual TierPrice GetTierPriceById(int tierPriceId)
        {
            if (tierPriceId == 0)
                return null;

            var tierPrice = _tierPriceRepository.GetById(tierPriceId);
            return tierPrice;
        }

        public virtual Multimap<int, TierPrice> GetTierPricesByProductIds(int[] productIds, Customer customer = null, int storeId = 0)
        {
            Guard.NotNull(productIds, nameof(productIds));

            if (!productIds.Any())
            {
                return new Multimap<int, TierPrice>();
            }

            var query =
                from x in _tierPriceRepository.TableUntracked
                where productIds.Contains(x.ProductId)
                select x;

            if (storeId != 0)
                query = query.Where(x => x.StoreId == 0 || x.StoreId == storeId);

            query = query.OrderBy(x => x.ProductId).ThenBy(x => x.Quantity);

            var list = query.ToList();

            if (customer != null)
                list = list.FilterForCustomer(customer).ToList();

            var map = list
                .ToMultimap(x => x.ProductId, x => x);

            return map;
        }

        public virtual void InsertProduct(DeclarationProduct product)
        {
            Guard.NotNull(product, nameof(product));

            _productRepository.Insert(product);
        }

        public virtual void InsertProductPicture(DeclarationProductPicture productPicture)
        {
            Guard.NotNull(productPicture, nameof(productPicture));

            _productPictureRepository.Insert(productPicture);
        }

        public virtual void InsertTierPrice(TierPrice tierPrice)
        {
            Guard.NotNull(tierPrice, nameof(tierPrice));

            _tierPriceRepository.Insert(tierPrice);
        }

        //    var result = new List<DeclarationProduct>();
        //    result.AddRange(products1);
        //    result.AddRange(products2);
        //    return result;
        ////}
        //public virtual void UpdateHasTierPricesProperty(DeclarationProduct product)
        //{
        //    Guard.NotNull(product, nameof(product));

        //    var prevValue = product.HasTierPrices;
        //    product.HasTierPrices = product.TierPrices.Count > 0;
        //    if (prevValue != product.HasTierPrices)
        //        UpdateProduct(product);
        //}

        //    // only distinct products (group by ID)
        //    // if we use standard Distinct() method, then all fields will be compared (low performance)
        //    query2 = from p in query2
        //             group p by p.Id into pGroup
        //             orderby pGroup.Key
        //             select pGroup.FirstOrDefault();
        //    var products2 = query2.ToList();
        public virtual void UpdateLowestAttributeCombinationPriceProperty(DeclarationProduct product)
        {
            Guard.NotNull(product, nameof(product));

            var prevValue = product.LowestAttributeCombinationPrice;

            product.LowestAttributeCombinationPrice = _productAttributeService.GetLowestCombinationPrice(product.Id);

            if (prevValue != product.LowestAttributeCombinationPrice)
                UpdateProduct(product);
        }

        public virtual void UpdateProduct(DeclarationProduct product)
        {
            Guard.NotNull(product, nameof(product));

            _productRepository.Update(product);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual void UpdateProductPicture(DeclarationProductPicture productPicture)
        {
            Guard.NotNull(productPicture, nameof(productPicture));

            _productPictureRepository.Update(productPicture);
        }

        //    // Track inventory for product by product attributes
        //    var query2 = from p in _productRepository.Table
        //                 from pvac in p.ProductVariantAttributeCombinations
        //                 where !p.Deleted &&
        //                    p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStockByAttributes &&
        //                    pvac.StockQuantity <= 0
        //                 select p;
        public virtual void UpdateTierPrice(TierPrice tierPrice)
        {
            Guard.NotNull(tierPrice, nameof(tierPrice));

            _tierPriceRepository.Update(tierPrice);
        }

        #endregion Public Methods



        #region Private Methods

        private IQueryable<DeclarationProduct> ApplyLoadFlags(IQueryable<DeclarationProduct> query, ProductLoadFlags flags)
        {
            //if (flags.HasFlag(ProductLoadFlags.WithAttributeCombinations))
            //{
            //    query = query.Include(x => x.ProductVariantAttributeCombinations);
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithBundleItems))
            //{
            //    query = query.Include(x => x.ProductBundleItems.Select(y => y.Product));
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithCategories))
            //{
            //    query = query.Include(x => x.ProductCategories.Select(y => y.Category));
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithDiscounts))
            //{
            //    query = query.Include(x => x.AppliedDiscounts);
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithManufacturers))
            //{
            //    query = query.Include(x => x.ProductManufacturers.Select(y => y.Manufacturer));
            //}

            if (flags.HasFlag(ProductLoadFlags.WithPictures))
            {
                query = query.Include(x => x.ProductPictures);
            }

            //if (flags.HasFlag(ProductLoadFlags.WithReviews))
            //{
            //    query = query.Include(x => x.ProductReviews);
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithSpecificationAttributes))
            //{
            //    query = query.Include(x => x.ProductSpecificationAttributes.Select(y => y.SpecificationAttributeOption));
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithTags))
            //{
            //    query = query.Include(x => x.ProductTags);
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithTierPrices))
            //{
            //    query = query.Include(x => x.TierPrices);
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithAttributes))
            //{
            //    query = query.Include(x => x.ProductVariantAttributes.Select(y => y.ProductAttribute));
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithAttributeValues))
            //{
            //    query = query.Include(x => x.ProductVariantAttributes.Select(y => y.ProductVariantAttributeValues));
            //}

            //if (flags.HasFlag(ProductLoadFlags.WithDeliveryTime))
            //{
            //    query = query.Include(x => x.DeliveryTime);
            //}

            return query;
        }

        //public virtual void UpdateProductReviewTotals(DeclarationProduct product)
        //{
        //    Guard.NotNull(product, nameof(product));

        //    int approvedRatingSum = 0;
        //    int notApprovedRatingSum = 0;
        //    int approvedTotalReviews = 0;
        //    int notApprovedTotalReviews = 0;
        //    var reviews = product.ProductReviews;
        //    foreach (var pr in reviews)
        //    {
        //        if (pr.IsApproved)
        //        {
        //            approvedRatingSum += pr.Rating;
        //            approvedTotalReviews++;
        //        }
        //        else
        //        {
        //            notApprovedRatingSum += pr.Rating;
        //            notApprovedTotalReviews++;
        //        }
        //    }

        //    product.ApprovedRatingSum = approvedRatingSum;
        //    product.NotApprovedRatingSum = notApprovedRatingSum;
        //    product.ApprovedTotalReviews = approvedTotalReviews;
        //    product.NotApprovedTotalReviews = notApprovedTotalReviews;
        //    UpdateProduct(product);
        //}

        //public virtual IList<DeclarationProduct> GetLowStockProducts()
        //{
        //    // Track inventory for product
        //    var query1 = from p in _productRepository.Table
        //                 orderby p.MinStockQuantity
        //                 where !p.Deleted &&
        //                    p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStock &&
        //                    p.MinStockQuantity >= p.StockQuantity
        //                 select p;
        //    var products1 = query1.ToList();
        private void UnassignDeletedPictureFromVariantCombinations(DeclarationProductPicture productPicture)
        {
            var picId = productPicture.Id;
            bool touched = false;

            var combinations =
                from c in this._productVariantAttributeCombinationRepository.Table
                where c.ProductId == productPicture.Product.Id && !String.IsNullOrEmpty(c.AssignedPictureIds)
                select c;

            foreach (var c in combinations)
            {
                var ids = c.GetAssignedPictureIds().ToList();
                if (ids.Contains(picId))
                {
                    ids.Remove(picId);

                    //c.AssignedPictureIds = ids.Count > 0 ? String.Join<int>(",", ids) : null;
                    c.SetAssignedPictureIds(ids.ToArray());
                    touched = true;

                    // we will save after we're done. It's faster.
                }
            }

            // save in one shot!
            if (touched)
            {
                _dbContext.SaveChanges();
            }
        }

        #endregion Private Methods
    }
}