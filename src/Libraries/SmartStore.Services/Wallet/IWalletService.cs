using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWalletService
    {
        #region Public Methods

        public bool GetRewardFromWallet(LuckMoney luck, Customer customer);

        public bool SendRewardToWalletFour(List<Customer> customers);

        public bool SendRewardToWalletOne(List<Customer> customers, decimal amount, DeclarationOrder order);

        public bool SendRewardToWalletThree(List<Customer> customers);

        public bool SendRewardToWalletTwo(List<Customer> customers);

        #endregion Public Methods
    }
}