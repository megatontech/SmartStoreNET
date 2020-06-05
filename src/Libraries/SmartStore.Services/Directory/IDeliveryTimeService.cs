using System.Collections.Generic;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Directory;

namespace SmartStore.Services.Directory
{
    /// <summary>
    /// DeliveryTime service
    /// </summary>
    public partial interface IDeliveryTimeService
    {
        #region Public Methods

        /// <summary>
        /// Deletes delivery time
        /// </summary>
        /// <param name="deliveryTime">DeliveryTime</param>
        void DeleteDeliveryTime(DeliveryTime deliveryTime);

        /// <summary>
        /// Gets all delivery times
        /// </summary>
        /// <returns>delivery time collection</returns>
        IList<DeliveryTime> GetAllDeliveryTimes();

        /// <summary>
        /// Gets the default delivery time
        /// </summary>
        /// <param name="product">The product</param>
        /// <returns>Delivery time</returns>
        DeliveryTime GetDefaultDeliveryTime();

        /// <summary>
        /// Gets the delivery time for a product
        /// </summary>
        /// <param name="product">The product</param>
        /// <returns>Delivery time</returns>
        DeliveryTime GetDeliveryTime(Product product);

        /// <summary>
        /// Gets a delivery time
        /// </summary>
        /// <param name="deliveryTimeId">delivery time identifier</param>
        /// <returns>DeliveryTime</returns>
        DeliveryTime GetDeliveryTimeById(int deliveryTimeId);

        /// <summary>
        /// Inserts a delivery time
        /// </summary>
        /// <param name="deliveryTime">DeliveryTime</param>
        void InsertDeliveryTime(DeliveryTime deliveryTime);

        /// <summary>
        /// Checks if the delivery time is associated with
        /// at least one dependant entity
        /// </summary>
        bool IsAssociated(int deliveryTimeId);

        /// <summary>
        /// Updates a delivery time
        /// </summary>
        /// <param name="deliveryTime">DeliveryTime</param>
        void SetToDefault(DeliveryTime deliveryTime);

        /// <summary>
        /// Updates a delivery time
        /// </summary>
        /// <param name="deliveryTime">DeliveryTime</param>
		void UpdateDeliveryTime(DeliveryTime deliveryTime);

        #endregion Public Methods
    }
}