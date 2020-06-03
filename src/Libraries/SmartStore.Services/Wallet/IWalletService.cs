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
        bool SendRewardToWalletOne(List<Customer> customers,decimal amount, DeclarationOrder order);
        bool SendRewardToWalletTwo(Dictionary<Customer, float> customers, decimal amount, int point);
        bool SendRewardToWalletThree();
        bool SendRewardToWalletFour();
    }
}
