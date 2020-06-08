namespace SmartStore.Core.Domain.Wallet
{
    /// <summary>
    /// ���ֻ�ȡ����
    /// </summary>
    public enum PointGetType
    {
        /// <summary>
        /// ����ת����
        /// </summary>
        Withdraw = 10,
        /// <summary>
        /// Ǯ��ת����
        /// </summary>
        Convert = 20,
        /// <summary>
        /// ����û���
        /// </summary>
        Shop = 30,
        /// <summary>
        /// ϵͳ����
        /// </summary>
        Reward = 40
    }
    /// <summary>
    /// ����ʹ������
    /// </summary>
    public enum PointUseType
    {
        /// <summary>
        /// ����
        /// </summary>
        Shop = 10,
        /// <summary>
        /// ����
        /// </summary>
        Recall = 20,
        /// <summary>
        /// ����
        /// </summary>
        Send = 30,
        /// <summary>
        /// ת��
        /// </summary>
        Convert = 40
    }
}
