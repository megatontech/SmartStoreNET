

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Security;
using SmartStore.Core.Domain.Seo;
using SmartStore.Core.Domain.Stores;

namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// 积分表
    /// </summary>
    [DataContract]
    public partial class CustomerPointsTotal : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        /// <summary>
        /// 客户
        /// </summary>
        [DataMember]
        public int Customer { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        [DataMember]
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 积分数
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

