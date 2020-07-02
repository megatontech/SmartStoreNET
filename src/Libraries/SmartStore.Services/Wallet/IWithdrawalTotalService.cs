using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalTotalService
    {
        #region Public Methods

        public void Add(WithdrawalTotal entity);

        public WithdrawalTotal Get(Customer customer);
        public decimal GetAllSum();
        public List<WithdrawalTotal> GetAll();
        public List<WithdrawalTotal> GetAll(int start, int length);
        public int GetAllCount();
        public void Update(WithdrawalTotal entity);

        #endregion Public Methods
    }
}