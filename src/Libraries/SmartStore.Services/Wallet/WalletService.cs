using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class WalletService : IWalletService
    {
        public WalletService()
        {
        }

        /// <summary>
        /// 向列表中每个客户打钱，并且记录此次交易细节
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="amount"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool SendRewardToWalletOne(List<Customer> customers, decimal amount, DeclarationOrder order)
        {
            foreach (var customer in customers)
            {
                //log

                //send reward
            }
            return true;
        }
        /// <summary>
        /// 按每个用户的贡献值发放分红
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool SendRewardToWalletTwo(List<Customer> customers)
        {
            throw new NotImplementedException();
        }
        public bool SendRewardToWalletThree(List<Customer> customers)
        {
            throw new NotImplementedException();
        }

       
        public bool SendRewardToWalletFour()
        {
            throw new NotImplementedException();
        }
    }
}
