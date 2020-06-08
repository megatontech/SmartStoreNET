namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum WithdrawalApplyType
    {
        /// <summary>
        /// 现金
        /// </summary>
        Cash = 10,
        /// <summary>
        /// 积分
        /// </summary>
        Point = 20,
        /// <summary>
        /// Coupon
        /// </summary>
        Coupon = 30,
        /// <summary>
        /// Present
        /// </summary>
        Present = 40
    }
   /// <summary>
   /// 提现申请状态
   /// </summary>
     public enum WithdrawalApplyStatus
    {
        /// <summary>
        /// 已发起
        /// </summary>
        Pending = 10,
        /// <summary>
        /// 审批中
        /// </summary>
        Processing = 20,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 30,
        /// <summary>
        /// 被拒
        /// </summary>
        Cancelled = 40
    }
}
