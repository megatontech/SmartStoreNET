namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum WithdrawalApplyType
    {
        /// <summary>
        /// �ֽ�
        /// </summary>
        Cash = 10,
        /// <summary>
        /// ����
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
   /// ��������״̬
   /// </summary>
     public enum WithdrawalApplyStatus
    {
        /// <summary>
        /// �ѷ���
        /// </summary>
        Pending = 10,
        /// <summary>
        /// ������
        /// </summary>
        Processing = 20,
        /// <summary>
        /// ���
        /// </summary>
        Complete = 30,
        /// <summary>
        /// ����
        /// </summary>
        Cancelled = 40
    }
}
