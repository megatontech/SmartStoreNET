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
    /// 当天会员贡献明细
    /// </summary>
    [DataContract]
    public partial class DailyCustomerContributionDetail : BaseEntity, ILocalizedEntity, ISlugSupported
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
        /// 日期（每天00：00）
        /// </summary>
        [DataMember]
        public DateTime ContributionTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 活跃线数
        /// </summary>
        [DataMember]
        public int ActiveLine { get; set; }
        /// <summary>
        /// 总线数
        /// </summary>
        [DataMember]
        public int TotalLine { get; set; }
        /// <summary>
        /// 总点数价值
        /// </summary>
        [DataMember]
        public decimal TotalPointValue { get; set; }
        /// <summary>
        /// 总点数
        /// </summary>
        [DataMember]
        public int TotalPoint { get; set; }
        /// <summary>
        /// 总业绩
        /// </summary>
        [DataMember]
        public decimal TotalValue { get; set; }
        /// <summary>
        /// 去掉最大区的总业绩
        /// </summary>
        [DataMember]
        public decimal CountTotalValue { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

