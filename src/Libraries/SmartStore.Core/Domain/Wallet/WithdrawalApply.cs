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
    /// 提现申请表
    /// </summary>
    [DataContract]
    public partial class WithdrawalApply : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        /// <summary>
        /// 客户
        /// </summary>
        [DataMember]
        public int Customer { get; set; }
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
        /// 操作人ID
        /// </summary>
        [DataMember]
        public Guid OperaterID { get; set; }
        /// <summary>
        /// 提现钱数
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// 实际应到账钱数
        /// </summary>
        [DataMember]
        public decimal ExpectAmount { get; set; }
        /// <summary>
        /// 手续费钱数
        /// </summary>
        [DataMember]
        public decimal ToFeeAmount { get; set; }
        /// <summary>
        /// 转积分钱数
        /// </summary>
        [DataMember]
        public decimal ToPointAmount { get; set; }
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
        /// 转账申请时间
        /// </summary>
        [DataMember]
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 转账审核时间
        /// </summary>
        [DataMember]
        public DateTime WithdrawAuditTime { get; set; }
        /// <summary>
        /// 转账审核人
        /// </summary>
        [DataMember]
        public int WithdrawAuditCustomer { get; set; }
        /// <summary>
        /// 转账审核备注
        /// </summary>
        [DataMember]
        public string WithdrawAuditComment { get; set; }
        /// <summary>
        /// 转账类型
        /// </summary>
        [DataMember]
        public int WithdrawType { get; set; }
        /// <summary>
        /// 转账状态
        /// </summary>
        [DataMember]
        public int WithdrawStatus { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

