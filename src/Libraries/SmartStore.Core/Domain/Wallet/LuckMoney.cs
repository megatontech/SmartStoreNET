
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
    /// 红包分配表
    /// </summary>
    [DataContract]
    public partial class LuckMoney : BaseEntity, ILocalizedEntity, ISlugSupported
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
        /// 钱数
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// 总钱数
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        [DataMember]
        public int CustomerAmount { get; set; }
        /// <summary>
        /// 是否已领取
        /// </summary>
        [DataMember]
        public bool isOut { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// 发放时间
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发放开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 发放结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }
    }
}

