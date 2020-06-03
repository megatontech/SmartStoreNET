
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
    /// 提现明细表包括进出及来源
    /// </summary>
    [DataContract]
    public partial class WithdrawalDetail : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        //类别 直推 红包 佣金 分红 提现 代报单 额度支付 转积分

        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

