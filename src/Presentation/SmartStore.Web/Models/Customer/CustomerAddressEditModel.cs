using SmartStore.Web.Framework.Modelling;
using SmartStore.Web.Models.Common;
using System;

namespace SmartStore.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : ModelBase
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        public AddressModel Address { get; set; }
    }
    public partial class CheckInModel : ModelBase
    {
        public CheckInModel()
        {
        }
        /// <summary>
		/// Gets or sets the customer identifier.
		/// </summary>
		public int CustomerId { get; set; }
		public string Result { get; set; }
        
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        public DateTime Date { get; set; }
    }
}