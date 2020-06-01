using System;
using System.Collections.Generic;
using SmartStore.Collections;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Orders;

namespace SmartStore.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial interface IDeclarationProductService
    {
        #region Products

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="product">Product</param>
        void DeleteProduct(DeclarationProduct product);

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        IList<DeclarationProduct> GetAllProductsDisplayedOnHomePage();

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductById(int productId);

        /// <summary>
        /// Gets products by identifier
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
		/// <param name="flags">Which navigation properties to eager load</param>
        /// <returns>Products</returns>
        IList<DeclarationProduct> GetProductsByIds(int[] productIds, ProductLoadFlags flags = ProductLoadFlags.None);

        /// <summary>
        /// Get product by system name.
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Product entity.</returns>
        DeclarationProduct GetProductBySystemName(string systemName);

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        void InsertProduct(DeclarationProduct product);

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
		void UpdateProduct(DeclarationProduct product);

        /// <summary>
        /// Update product review totals
        /// </summary>
        /// <param name="product">Product</param>
        void UpdateProductReviewTotals(DeclarationProduct product);

        /// <summary>
        /// Get low stock products
        /// </summary>
        /// <returns>Result</returns>
        IList<DeclarationProduct> GetLowStockProducts();

        /// <summary>
        /// Gets a product by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductBySku(string sku);

        /// <summary>
        /// Gets a product by GTIN
        /// </summary>
		/// <param name="gtin">GTIN</param>
        /// <returns>Product</returns>
		DeclarationProduct GetProductByGtin(string gtin);

        /// <summary>
        /// Gets a product by manufacturer part number (MPN)
        /// </summary>
        /// <param name="manufacturerPartNumber">Manufacturer part number</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductByManufacturerPartNumber(string manufacturerPartNumber);

        /// <summary>
        /// Gets a product by name
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductByName(string name);

        /// <summary>
        /// Adjusts inventory
        /// </summary>
        /// <param name="sci">Shopping cart item</param>
        /// <param name="decrease">A value indicating whether to increase or descrease product stock quantity</param>
        /// <returns>Adjust inventory result</returns>
        AdjustInventoryResult AdjustInventory(OrganizedShoppingCartItem sci, bool decrease);

        /// <summary>
        /// Adjusts inventory
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <param name="decrease">A value indicating whether to increase or descrease product stock quantity</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Adjust inventory result</returns>
        AdjustInventoryResult AdjustInventory(OrderItem orderItem, bool decrease, int quantity);

        /// <summary>
        /// Adjusts inventory
        /// </summary>
		/// <param name="product">Product</param>
		/// <param name="decrease">A value indicating whether to increase or descrease product stock quantity</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Attributes in XML format</param>
		/// <returns>Adjust inventory result</returns>
		AdjustInventoryResult AdjustInventory(DeclarationProduct product, bool decrease, int quantity, string attributesXml);

        /// <summary>
        /// Update HasTierPrices property (used for performance optimization)
        /// </summary>
		/// <param name="product">Product</param>
        void UpdateHasTierPricesProperty(DeclarationProduct product);

        /// <summary>
        /// Update LowestAttributeCombinationPrice property (used for performance optimization)
        /// </summary>
        /// <param name="product">Product</param>
        void UpdateLowestAttributeCombinationPriceProperty(DeclarationProduct product);

        /// <summary>
        /// Update HasDiscountsApplied property (used for performance optimization)
        /// </summary>
		/// <param name="product">Product</param>
        void UpdateHasDiscountsApplied(DeclarationProduct product);

        /// <summary>
        /// Get product tags by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <returns>Map of product tags</returns>
        Multimap<int, ProductTag> GetProductTagsByProductIds(int[] productIds);

        /// <summary>
        /// Gets products that are assigned to group products.
        /// </summary>
        /// <param name="productIds">Grouped product identifiers.</param>
        /// <param name="showHidden">A value indicating whether to show hidden records.</param>
        /// <returns>Map of associated products.</returns>
        Multimap<int, DeclarationProduct> GetAssociatedProductsByProductIds(int[] productIds, bool showHidden = false);

        /// <summary>
        /// Get applied discounts by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <returns>Map of applied discounts</returns>
        Multimap<int, Discount> GetAppliedDiscountsByProductIds(int[] productIds);

        #endregion Products

        #region Related products

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        void DeleteRelatedProduct(RelatedProduct relatedProduct);

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        IList<RelatedProduct> GetRelatedProductsByProductId1(int productId1, bool showHidden = false);

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifier</param>
        /// <returns>Related product</returns>
        RelatedProduct GetRelatedProductById(int relatedProductId);

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        void InsertRelatedProduct(RelatedProduct relatedProduct);

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="relatedProduct">Related product</param>
        void UpdateRelatedProduct(RelatedProduct relatedProduct);

        /// <summary>
        /// Ensure existence of all mutually related products
        /// </summary>
        /// <param name="productId1">First product identifier</param>
        /// <returns>Number of inserted related products</returns>
        int EnsureMutuallyRelatedProducts(int productId1);

        #endregion Related products

        #region Cross-sell products

        /// <summary>
        /// Deletes a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell</param>
        void DeleteCrossSellProduct(CrossSellProduct crossSellProduct);

        /// <summary>
        /// Gets a cross-sell product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell product collection</returns>
        IList<CrossSellProduct> GetCrossSellProductsByProductId1(int productId1, bool showHidden = false);

        /// <summary>
        /// Gets a cross-sell product collection by many product identifiers
        /// </summary>
        /// <param name="productIds">A sequence of alpha product identifiers</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell product collection</returns>
        IList<CrossSellProduct> GetCrossSellProductsByProductIds(IEnumerable<int> productIds, bool showHidden = false);

        /// <summary>
        /// Gets a cross-sell product
        /// </summary>
        /// <param name="crossSellProductId">Cross-sell product identifier</param>
        /// <returns>Cross-sell product</returns>
        CrossSellProduct GetCrossSellProductById(int crossSellProductId);

        /// <summary>
        /// Inserts a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell product</param>
        void InsertCrossSellProduct(CrossSellProduct crossSellProduct);

        /// <summary>
        /// Updates a cross-sell product
        /// </summary>
        /// <param name="crossSellProduct">Cross-sell product</param>
        void UpdateCrossSellProduct(CrossSellProduct crossSellProduct);

        /// <summary>
        /// Gets a cross-sells
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="numberOfProducts">Number of products to return</param>
        /// <returns>Cross-sells</returns>
        IList<DeclarationProduct> GetCrosssellProductsByShoppingCart(IList<OrganizedShoppingCartItem> cart, int numberOfProducts);

        /// <summary>
        /// Ensure existence of all mutually cross selling products
        /// </summary>
        /// <param name="productId1">First product identifier</param>
        /// <returns>Number of inserted cross selling products</returns>
        int EnsureMutuallyCrossSellProducts(int productId1);

        #endregion Cross-sell products

        #region Tier prices

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void DeleteTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        TierPrice GetTierPriceById(int tierPriceId);

        /// <summary>
        /// Gets tier prices by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <param name="customer">Filter tier prices by customer</param>
        /// <param name="storeId">Filter tier prices by store</param>
        /// <returns>Map of tier prices</returns>
        Multimap<int, TierPrice> GetTierPricesByProductIds(int[] productIds, Customer customer = null, int storeId = 0);

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void InsertTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void UpdateTierPrice(TierPrice tierPrice);

        #endregion Tier prices

        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void DeleteProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Gets a product pictures by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product pictures</returns>
        IList<ProductPicture> GetProductPicturesByProductId(int productId);

        /// <summary>
        /// Get product pictures by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <param name="onlyFirstPicture">Whether to only load the first picture for each product</param>
        /// <returns>Product pictures</returns>
        Multimap<int, ProductPicture> GetProductPicturesByProductIds(int[] productIds, bool onlyFirstPicture = false);

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="productPictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        ProductPicture GetProductPictureById(int productPictureId);

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void InsertProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void UpdateProductPicture(ProductPicture productPicture);

        #endregion Product pictures

        #region Bundled products

        /// <summary>
        /// Inserts a product bundle item
        /// </summary>
        /// <param name="bundleItem">Product bundle item</param>
        void InsertBundleItem(ProductBundleItem bundleItem);

        /// <summary>
        /// Updates a product bundle item
        /// </summary>
        /// <param name="bundleItem">Product bundle item</param>
        void UpdateBundleItem(ProductBundleItem bundleItem);

        /// <summary>
        /// Deletes a product bundle item
        /// </summary>
        /// <param name="bundleItem">Product bundle item</param>
        void DeleteBundleItem(ProductBundleItem bundleItem);

        /// <summary>
        /// Get a product bundle item by item identifier
        /// </summary>
        /// <param name="bundleItemId">Product bundle item identifier</param>
        /// <returns>Product bundle item</returns>
        ProductBundleItem GetBundleItemById(int bundleItemId);

        /// <summary>
        /// Gets a list of bundle items for a particular product identifier
        /// </summary>
        /// <param name="bundleProductId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>List of bundle items</returns>
        IList<ProductBundleItemData> GetBundleItems(int bundleProductId, bool showHidden = false);

        /// <summary>
        /// Get bundle items by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Map of bundle items</returns>
        Multimap<int, ProductBundleItem> GetBundleItemsByProductIds(int[] productIds, bool showHidden = false);

        /// <summary>
        /// Checks whether a product is a bundle item.
        /// </summary>
        /// <param name="productId">Product identifier.</param>
        /// <returns>True if the product is a bundle item.</returns>
        bool IsBundleItem(int productId);

        #endregion Bundled products
    }
}