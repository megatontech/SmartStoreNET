namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// 积分获取类型
    /// </summary>
    public enum PointGetType
    {
        /// <summary>
        /// 提现转积分
        /// </summary>
        Withdraw = 10,
        /// <summary>
        /// 钱包转积分
        /// </summary>
        Convert = 20,
        /// <summary>
        /// 购物得积分
        /// </summary>
        Shop = 30,
        /// <summary>
        /// 系统发放
        /// </summary>
        Reward = 40
    }
    /// <summary>
    /// 积分使用类型
    /// </summary>
    public enum PointUseType
    {
        /// <summary>
        /// 购物
        /// </summary>
        Shop = 10,
        /// <summary>
        /// 回收
        /// </summary>
        Recall = 20,
        /// <summary>
        /// 赠送
        /// </summary>
        Send = 30,
        /// <summary>
        /// 转换
        /// </summary>
        Convert = 40
    }
}
