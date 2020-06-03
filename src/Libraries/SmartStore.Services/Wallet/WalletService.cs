using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public class WalletService : IWalletService
    {
        #region Public Constructors

        public WalletService()
        {
        }

        #endregion Public Constructors



        #region Public Methods

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public bool SendRewardToWalletFour(List<Customer> customers)
        {
            throw new NotImplementedException();
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
        /// 按每个用户的贡献值发放商城利润分红
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public bool SendRewardToWalletThree(List<Customer> customers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按每个用户的贡献值发放营业额度分红
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool SendRewardToWalletTwo(List<Customer> customers)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}