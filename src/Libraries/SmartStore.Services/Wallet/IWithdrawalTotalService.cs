using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalTotalService
    {
        #region Public Methods

        public void Add(WithdrawalTotal entity);

        public WithdrawalTotal Get(Customer customer);

        public void Update(WithdrawalTotal entity);

        #endregion Public Methods
    }
}