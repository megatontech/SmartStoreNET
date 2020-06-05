using SmartStore.Core.Domain.Catalog;
using System.Collections.Generic;

namespace SmartStore.Services.Catalog
{
    /// <summary>
    /// Product template interface
    /// </summary>
    public partial interface IProductTemplateService
    {
        #region Public Methods

        /// <summary>
        /// Delete product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        void DeleteProductTemplate(ProductTemplate productTemplate);

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product templates</returns>
        IList<ProductTemplate> GetAllProductTemplates();

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        ProductTemplate GetProductTemplateById(int productTemplateId);

        /// <summary>
        /// Inserts product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        void InsertProductTemplate(ProductTemplate productTemplate);

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        void UpdateProductTemplate(ProductTemplate productTemplate);

        #endregion Public Methods
    }
}