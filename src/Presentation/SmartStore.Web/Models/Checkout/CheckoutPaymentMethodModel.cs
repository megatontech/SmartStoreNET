using System.Collections.Generic;
using SmartStore.Web.Framework.Modelling;
using SmartStore.Services.Localization;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Web.Models.Checkout
{
    public partial class CheckoutPaymentMethodModel : ModelBase
    {
        public CheckoutPaymentMethodModel()
        {
            PaymentMethods = new List<PaymentMethodModel>();
            WithdrawalTotal = new WithdrawalTotal();
        }
        public SmartStore.Core.Domain.Customers.Customer customer { get; set; }
        public WithdrawalTotal WithdrawalTotal { get; set; }
        public decimal Total { get; set; }
        public string Mobile { get; set; }
        public string CustomerComment { get; set; }
        public List<PaymentMethodModel> PaymentMethods { get; set; }

        public bool SkippedSelectShipping { get; set; }

        #region Nested classes

        public partial class PaymentMethodModel : ModelBase
        {
            public string PaymentMethodSystemName { get; set; }
            public string Name { get; set; }
			public string Description { get; set; }
			public LocalizedValue<string> FullDescription { get; set; }
            public string BrandUrl { get; set; }
            public string Fee { get; set; }
            public bool Selected { get; set; }
			public RouteInfo PaymentInfoRoute { get; set; }
			public bool RequiresInteraction { get; set; }
        }

        #endregion
    }
}