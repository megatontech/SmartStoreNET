using SmartStore.Collections;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Orders;
using System.Collections.Generic;

namespace SmartStore.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial interface IDeclarationProductService
    {
        #region Public Methods

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
        /// Delete a product
        /// </summary>
        /// <param name="product">Product</param>
        void DeleteProduct(DeclarationProduct product);

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void DeleteProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void DeleteTierPrice(TierPrice tierPrice);

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        IList<DeclarationProduct> GetAllProductsDisplayedOnHomePage();

        /// <summary>
        /// Get applied discounts by product identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <returns>Map of applied discounts</returns>
        Multimap<int, Discount> GetAppliedDiscountsByProductIds(int[] productIds);

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductById(int productId);

        /// <summary>
        /// Gets a product by name
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns>Product</returns>
        DeclarationProduct GetProductByName(string name);

        /// <summary>
        /// Get product by system name.
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Product entity.</returns>
        DeclarationProduct GetProductBySystemName(string systemName);

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="productPictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        ProductPicture GetProductPictureById(int productPictureId);

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
        /// Gets products by identifier
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
		/// <param name="flags">Which navigation properties to eager load</param>
        /// <returns>Products</returns>
        IList<DeclarationProduct> GetProductsByIds(int[] productIds, ProductLoadFlags flags = ProductLoadFlags.None);

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
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        void InsertProduct(DeclarationProduct product);

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void InsertProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void InsertTierPrice(TierPrice tierPrice);

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
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
		void UpdateProduct(DeclarationProduct product);

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void UpdateProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPrice">Tier price</param>
        void UpdateTierPrice(TierPrice tierPrice);

        #endregion Public Methods
    }
}