using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalDetailService
    {
        #region Public Methods

        public void Add(WithdrawalDetail entity);

        #endregion Public Methods
    }
}