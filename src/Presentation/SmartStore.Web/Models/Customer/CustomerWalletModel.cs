using System;
using System.Collections.Generic;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;

namespace SmartStore.Web.Models.Customer
{
    public partial class CustomerWalletModel : ModelBase
    {
        public CustomerWalletModel()
        {
            RewardPoints = new List<RewardPointsHistoryModel>();
        }

        public IList<RewardPointsHistoryModel> RewardPoints { get; set; }
        public int TotalPoints { get; set; }
        public decimal Total { get; set; }
        public decimal TotalShopPoints { get; set; }
        public decimal Freeze { get; set; }
        public decimal DecShare { get; set; }
        public decimal Luck { get; set; }
        public decimal StoreShare { get; set; }
        public decimal Push { get; set; }
        public DateTime Update { get; set; }
        #region Nested classes
        public partial class RewardPointsHistoryModel : EntityModelBase
        {
            [SmartResourceDisplayName("金额")]
            public decimal Points { get; set; }

            [SmartResourceDisplayName("详情")]
            public string Message { get; set; }

			[SmartResourceDisplayName("时间")]
            public DateTime CreatedOn { get; set; }
            [SmartResourceDisplayName("时间")]
            public string CreatedOnStr { get; set; }
        }

        #endregion
    }
    public partial class CustomerTeamModel : ModelBase
    {
        public CustomerTeamModel()
        {
            
        }

        public int Total { get; set; }
        public List<SmartStore.Core.Domain.Customers.Customer> Team { get; set; }
        public SmartStore.Core.Domain.Customers.Customer Self { get; set; }
    }
}