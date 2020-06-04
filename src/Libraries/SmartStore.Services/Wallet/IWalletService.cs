using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public interface IWalletService
    {
       public bool SendRewardToWalletOne(List<Customer> customers, decimal amount, DeclarationOrder order);
       public bool SendRewardToWalletTwo(List<Customer> customers);
       public bool SendRewardToWalletThree(List<Customer> customers);
        public bool SendRewardToWalletFour(List<Customer> customers);
        public bool GetRewardFromWallet(LuckMoney luck, Customer customer);
    }
}
