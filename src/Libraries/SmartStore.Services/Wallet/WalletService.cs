using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Wallet
{
    public class WalletService : IWalletService
    {
        #region Public Fields

        public readonly DateTime edateTime;
        public readonly DateTime sdateTime;

        #endregion Public Fields



        #region Private Fields
        private readonly IDeclarationCalcRuleService _calcruleService;
        private readonly DeclarationCalcRule _calcrule;

        private readonly ILuckMoneyService _ILuckMoneyService;
        private readonly IWithdrawalDetailService _IWithdrawalDetailService;
        private readonly IWithdrawalTotalService _IWithdrawalTotalService;
        #endregion Private Fields

        #region Public Constructors

        public WalletService(ILuckMoneyService iLuckMoneyService,
            IWithdrawalTotalService iWithdrawalTotalService, IDeclarationCalcRuleService calcruleService,
            IWithdrawalDetailService iWithdrawalDetailService
            )
        {
            
            _ILuckMoneyService = iLuckMoneyService;
            _IWithdrawalTotalService = iWithdrawalTotalService;
            _IWithdrawalDetailService = iWithdrawalDetailService;
            _calcruleService = calcruleService;
            _calcrule = _calcruleService.GetDeclarationCalcRule();
            sdateTime = DateTime.Now;
            edateTime = DateTime.Now.AddHours(_calcrule.CalcRewardFourAliveHours);
        }

        #endregion Public Constructors



        #region Public Methods

        /// <summary>
        /// 领红包
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public bool GetRewardFromWallet(LuckMoney luck, Customer customer)
        {
            var total = _IWithdrawalTotalService.Get(customer);
            total.TotalAmount += luck.Amount;
            total.TotalLuckyAmount += luck.Amount;
            total.UpdateTime = DateTime.Now;
            _IWithdrawalTotalService.Update(total);
            luck.isOut = true;
            _ILuckMoneyService.Update(luck);
            _IWithdrawalDetailService.Add(new WithdrawalDetail
            {
                Amount = luck.Amount,
                Comment = "领取红包入账" + luck.Amount.ToString("F2"),
                Customer = customer.Id,
                isOut = false,
                WithdrawTime = DateTime.Now,
                WithdrawType = 4,
                CustomerID = customer.CustomerGuid
            });
            return true;
        }

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public bool SendRewardToWalletFour(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                if (customer.EverHadOrder)
                {
                    //log
                    LuckMoney luck = new LuckMoney();
                    luck.Amount = customer.TotalPointsValue4;
                    luck.StartTime = sdateTime;
                    luck.SendTime = sdateTime;
                    luck.EndTime = edateTime;
                    luck.Comment = sdateTime.ToString() + "2小时内有效";
                    luck.Customer = customer.Id;
                    luck.CustomerAmount = customers.Count;
                    luck.CustomerID = customer.CustomerGuid;
                    luck.IsCount = false;
                    luck.isOut = false;
                    luck.TotalAmount = customers.Sum(x => x.TotalPointsValue4);
                    //send reward
                    _ILuckMoneyService.AddLuckMoney(luck);
                }
            }
            return true;
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
                if (customer.EverHadOrder)
                {
                    //log
                    var total = _IWithdrawalTotalService.Get(customer);
                    total.TotalAmount += amount;
                    total.TotalPushAmount += amount;
                    total.UpdateTime = DateTime.Now;
                    _IWithdrawalTotalService.Update(total);
                    _IWithdrawalDetailService.Add(new WithdrawalDetail
                    {
                        Amount = amount,
                        Comment = "直推佣金入账" + amount.ToString("F2"),
                        Customer = customer.Id,
                        isOut = false,
                        WithdrawTime = DateTime.Now,
                        WithdrawType = 1,
                        CustomerID = customer.CustomerGuid,
                        Order = order.Id
                    });
                    //send reward
                }
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
            foreach (var customer in customers)
            {
                if (customer.EverHadOrder)
                {
                    //log
                    var total = _IWithdrawalTotalService.Get(customer);
                    total.TotalAmount += customer.TotalPointsValue3;
                    total.TotalPushAmount += customer.TotalPointsValue3;
                    total.UpdateTime = DateTime.Now;
                    _IWithdrawalTotalService.Update(total);
                    _IWithdrawalDetailService.Add(new WithdrawalDetail
                    {
                        Amount = customer.TotalPointsValue3,
                        Comment = "商城利润分红入账" + customer.TotalPointsValue3.ToString("F2"),
                        Customer = customer.Id,
                        isOut = false,
                        WithdrawTime = DateTime.Now,
                        WithdrawType = 3,
                        CustomerID = customer.CustomerGuid
                    });
                    //send reward
                }
            }
            return true;
        }

        /// <summary>
        /// 按每个用户的贡献值发放营业额度分红
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool SendRewardToWalletTwo(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                if (customer.EverHadOrder)
                {
                    //log
                    var total = _IWithdrawalTotalService.Get(customer);
                    var finalAmount = customer.TotalPointsValue2 > customer.CapLinesTotal ? customer.CapLinesTotal : customer.TotalPointsValue2;
                    total.TotalAmount += finalAmount;
                    total.TotalDecShareAmount += finalAmount;
                    total.UpdateTime = DateTime.Now;
                    _IWithdrawalTotalService.Update(total);
                    _IWithdrawalDetailService.Add(new WithdrawalDetail
                    {
                        Amount = finalAmount,
                        Comment = "营业额度分红入账：" + finalAmount.ToString("F2") + "封顶值：" + customer.CapLinesTotal.ToString("F2") + "贡献值预计分红：" + customer.TotalPointsValue2.ToString("F2"),
                        Customer = customer.Id,
                        isOut = false,
                        WithdrawTime = DateTime.Now,
                        WithdrawType = 2,
                        CustomerID = customer.CustomerGuid
                    });
                    //send reward
                }
            }
            return true;
        }

        #endregion Public Methods
    }
}