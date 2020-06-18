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
        public decimal Total { get; set; }
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
        }

        #endregion
    }
}