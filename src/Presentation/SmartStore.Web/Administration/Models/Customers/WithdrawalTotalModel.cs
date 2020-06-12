using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Customers
{
    public class WithdrawalTotalModel : ModelBase
    {
        #region Public Properties

        /// <summary>
        /// 用户guid
        /// </summary>
        
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        
        public int CustomerId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 是否参与统计
        /// </summary>
        
        public bool IsCount { get; set; }

        /// <summary>
        /// 总额
        /// </summary>
        
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 佣金分红
        /// </summary>
        
        public decimal TotalDecShareAmount { get; set; }

        /// <summary>
        /// 冻结的钱
        /// </summary>
        
        public decimal TotalFreezeAmount { get; set; }

        /// <summary>
        /// 红包
        /// </summary>
        
        public decimal TotalLuckyAmount { get; set; }

        /// <summary>
        /// 直推分红
        /// </summary>
        
        public decimal TotalPushAmount { get; set; }

        /// <summary>
        /// 商城分红
        /// </summary>
        
        public decimal TotalStoreShareAmount { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        
        public DateTime UpdateTime { get; set; }

        #endregion Public Properties
    }
}