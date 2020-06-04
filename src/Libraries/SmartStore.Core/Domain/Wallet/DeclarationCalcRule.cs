
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
    /// 计算参数表
    /// </summary>
    [DataContract]
    public partial class DeclarationCalcRule : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        #region 直推奖励

        /// <summary>
        /// 直接上级奖励百分比
        /// </summary>
        [DataMember] 
        public int CalcRewardOneL1Percent { get; set; }
        /// <summary>
        /// 2级奖励百分比
        /// </summary>
        [DataMember] 
        public int CalcRewardOneL2Percent { get; set; }
        /// <summary>
        /// 3级奖励百分比
        /// </summary>
        [DataMember] 
        public int CalcRewardOneL3Percent { get; set; }
        /// <summary>
        /// 2级层数
        /// </summary>
        [DataMember] 
        public int CalcRewardOneL2Count { get; set; }
        /// <summary>
        /// 3级层数
        /// </summary>
        [DataMember]
        public int CalcRewardOneL3Count { get; set; }

        #endregion
        #region 商城分红
        /// <summary>
        /// 商城分红百分比
        /// </summary>
        [DataMember]
        public int CalcRewardThreePercent { get; set; }
        /// <summary>
        /// 贡献值点数分母
        /// </summary>
        [DataMember]
        public int CalcRewardTwoPointPercent { get; set; }
        #endregion
        #region 业绩分红
        /// <summary>
        /// 销售总额分红百分比
        /// </summary>
        [DataMember]
        public int CalcRewardTwoPercent { get; set; }
        #endregion
        #region 红包
        /// <summary>
        /// 是否平均分红包
        /// </summary>
        [DataMember]
        public bool CalcRewardFourEqual { get; set; }
        /// <summary>
        /// 红包占商城利润百分比
        /// </summary>
        [DataMember]
        public int CalcRewardFourPercent { get; set; }
        #endregion
        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public bool isUse { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }
       
    }
}

