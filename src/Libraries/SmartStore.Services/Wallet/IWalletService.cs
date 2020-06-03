using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public interface IWalletService
    {
        bool SendRewardToWalletOne(List<Customer> customers, decimal amount, DeclarationOrder order);
        bool SendRewardToWalletTwo(List<Customer> customers);
        bool SendRewardToWalletThree(List<Customer> customers);
        bool SendRewardToWalletFour(List<Customer> customers);
    }
}
