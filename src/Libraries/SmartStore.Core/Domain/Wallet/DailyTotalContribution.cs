﻿using System;
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
    /// 当天贡献总表
    /// </summary>
    [DataContract]
    public partial class DailyTotalContribution : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        /// <summary>
        /// 总业绩
        /// </summary>
        [DataMember]
        public decimal TotalValue { get; set; }
        /// <summary>
        /// 贡献值
        /// </summary>
        [DataMember]
        public decimal ContributionValue { get; set; }
        /// <summary>
        /// 分红金额
        /// </summary>
        [DataMember]
        public decimal DecValue { get; set; }
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
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}
