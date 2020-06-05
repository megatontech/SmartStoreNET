using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Directory;
using System.Collections.Generic;

namespace SmartStore.Services.Directory
{
    /// <summary>
    /// QuantityUnit service
    /// </summary>
    public partial interface IQuantityUnitService
    {
        #region Public Methods

        /// <summary>
        /// Deletes measure unit
        /// </summary>
        /// <param name="quantityUnit">QuantityUnit</param>
        void DeleteQuantityUnit(QuantityUnit quantityUnit);

        /// <summary>
        /// Gets all measure units
        /// </summary>
        /// <returns>measure unit collection</returns>
        IList<QuantityUnit> GetAllQuantityUnits();

        /// <summary>
        /// Gets the measure unit for a product
        /// </summary>
        /// <param name="product">The product</param>
        /// <returns>measure unit</returns>
        QuantityUnit GetQuantityUnit(Product product);

        /// <summary>
        /// Gets a measure unit
        /// </summary>
        /// <param name="quantityUnitId">measure unit identifier</param>
        /// <returns>QuantityUnit</returns>
        QuantityUnit GetQuantityUnitById(int? quantityUnitId);

        /// <summary>
        /// Inserts a measure unit
        /// </summary>
        /// <param name="quantityUnit">QuantityUnit</param>
        void InsertQuantityUnit(QuantityUnit quantityUnit);

        /// <summary>
        /// Checks if the delivery time is associated with
        /// at least one dependant entity
        /// </summary>
        bool IsAssociated(int quantityUnitId);

        /// <summary>
        /// Updates a measure unit
        /// </summary>
        /// <param name="quantityUnit">QuantityUnit</param>
        void UpdateQuantityUnit(QuantityUnit quantityUnit);

        #endregion Public Methods
    }
}