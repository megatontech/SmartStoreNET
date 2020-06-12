using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Customers
{
    public class WithdrawalDetailModel : ModelBase
    {
        //类别 直推 红包 佣金 分红 提现 代报单 额度支付 转积分
        /// <summary>
        /// 客户
        /// </summary>
        
        public int Customer { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        
        public int Operater { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        
        public Guid OperaterID { get; set; }
        /// <summary>
        /// 钱数
        /// </summary>
        
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否转出
        /// </summary>
        
        public bool isOut { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        
        public string Comment { get; set; }
        /// <summary>
        /// 转账类型
        /// </summary>
        
        public int WithdrawType { get; set; }
        /// <summary>
        /// 转账时间
        /// </summary>
        
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        
        public bool IsCount { get; set; }
    }
}