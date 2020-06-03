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
    /// 当前提现额度总表
    /// </summary>
    [DataContract]
    public partial class WithdrawalTotal : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        //冻结的钱 分红 直推 红包 总额
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

