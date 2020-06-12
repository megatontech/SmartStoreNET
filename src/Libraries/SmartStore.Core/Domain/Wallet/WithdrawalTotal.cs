using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Seo;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// 当前提现额度总表
    /// </summary>
    [DataContract]
    public partial class WithdrawalTotal : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        #region Public Properties

        /// <summary>
        /// 用户guid
        /// </summary>
        [DataMember] 
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember] 
        public int CustomerId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [NotMapped]
        public string CustomerName { get; set; }
        
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        [DataMember] 
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 营业额度分红
        /// </summary>
        [DataMember] 
        public decimal TotalDecShareAmount { get; set; }

        /// <summary>
        /// 冻结的钱
        /// </summary>
        [DataMember] 
        public decimal TotalFreezeAmount { get; set; }

        /// <summary>
        /// 红包
        /// </summary>
        [DataMember] 
        public decimal TotalLuckyAmount { get; set; }

        /// <summary>
        /// 直推佣金、推广佣金
        /// </summary>
        [DataMember] 
        public decimal TotalPushAmount { get; set; }

        /// <summary>
        /// 商城分红
        /// </summary>
        [DataMember] 
        public decimal TotalStoreShareAmount { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        [DataMember] 
        public DateTime UpdateTime { get; set; }

        #endregion Public Properties
    }
}