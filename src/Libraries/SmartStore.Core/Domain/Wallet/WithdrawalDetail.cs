
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
    /// 钱包明细表包括进出及来源
    /// </summary>
    [DataContract]
    public partial class WithdrawalDetail : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        //类别 直推 红包 佣金 分红 提现 代报单 额度支付 转积分
        /// <summary>
        /// 客户
        /// </summary>
        [DataMember] 
        public int Customer { get; set; }
        [DataMember]
        public int Order { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [DataMember] 
        public int Operater { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        [DataMember] 
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [NotMapped]
        public string CustomerName { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        [DataMember] 
        public Guid OperaterID { get; set; }
        /// <summary>
        /// 钱数
        /// </summary>
        [DataMember] 
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否转出
        /// </summary>
        [DataMember] 
        public bool isOut { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember] 
        public string Comment { get; set; }
        /// <summary>
        /// 转账类型
        /// </summary>
        [DataMember] 
        public int WithdrawType { get; set; }
        /// <summary>
        /// 转账时间
        /// </summary>
        [DataMember] 
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
        [NotMapped]
        public string customerName { get; set; }
        [NotMapped]
        public string customerMobile { get; set; }
    }
}

