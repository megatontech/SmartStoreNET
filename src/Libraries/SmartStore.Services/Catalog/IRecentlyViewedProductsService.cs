using SmartStore.Core.Domain.Catalog;
using System.Collections.Generic;

namespace SmartStore.Services.Catalog
{
    /// <summary>
    /// Recently viewed products service
    /// </summary>
    public partial interface IRecentlyViewedProductsService
    {
        #region Public Methods

        /// <summary>
        /// Adds a product to a recently viewed products list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        void AddProductToRecentlyViewedList(int productId);

        /// <summary>
        /// Gets a "recently viewed products" list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
		IList<Product> GetRecentlyViewedProducts(int number);

        #endregion Public Methods
    }
}