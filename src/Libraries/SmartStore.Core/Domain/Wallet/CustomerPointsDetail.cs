using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Seo;
using System;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// 积分明细表
    /// </summary>
    [DataContract]
    public partial class CustomerPointsDetail : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        #region Public Properties

        /// <summary>
        /// 积分数
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// 积分备注
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

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
        /// 是否参与统计
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }

        /// <summary>
        /// 是否转出
        /// </summary>
        [DataMember]
        public bool isOut { get; set; }

        /// <summary>
        /// 积分使用类型
        /// </summary>
        [DataMember]
        public int PointGetType { get; set; }

        /// <summary>
        /// 积分获取类型
        /// </summary>
        [DataMember]
        public int PointUseType { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }

        #endregion Public Properties
    }
}