using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalApplyService : IWithdrawalApplyService
    {
        #region Private Fields

        private readonly IRepository<WithdrawalApply> _WithdrawalApplyRepository;
        private readonly IDeclarationCalcRuleService _calcruleService;
        private readonly DeclarationCalcRule _calcrule;
        private readonly IWithdrawalTotalService _IWithdrawalTotalService;
        private readonly IWithdrawalDetailService _IWithdrawalDetailService;
        private readonly ICustomerPointsTotalService _ICustomerPointsTotalService;
        private readonly ICustomerPointsDetailService _ICustomerPointsDetailService;
        
            
        #endregion Private Fields

        #region Public Constructors

        public WithdrawalApplyService(IRepository<WithdrawalApply> withdrawalApplyRepository,
            IWithdrawalTotalService iWithdrawalTotalService, 
            IDeclarationCalcRuleService calcruleService,
            ICustomerPointsTotalService iCustomerPointsTotalService,
            ICustomerPointsDetailService iCustomerPointsDetailService,
            IWithdrawalDetailService iWithdrawalDetailService
            )
        {
            _ICustomerPointsTotalService = iCustomerPointsTotalService;
            _ICustomerPointsDetailService = iCustomerPointsDetailService;
            _IWithdrawalDetailService = iWithdrawalDetailService;
            _WithdrawalApplyRepository = withdrawalApplyRepository;
            _IWithdrawalTotalService = iWithdrawalTotalService;
            _calcruleService = calcruleService;
            _calcrule = _calcruleService.GetDeclarationCalcRule();
        }
        /// <summary>
        /// 转换钱数为积分
        /// </summary>
        /// <param name="model"></param>
        public void ConvertToPoints(WithdrawalApply model, Customer customer)
        {
            //减余额，加积分，记日志
            var total = _IWithdrawalTotalService.Get(customer);
            total.TotalAmount -= model.Amount;
            total.UpdateTime = DateTime.Now;
            _IWithdrawalTotalService.Update(total);
            var pointVal = 0;
            pointVal = (int)model.Amount * (int)_calcrule.WithDrawToPointPercent;
            _ICustomerPointsTotalService.AddPointsToCustomer(pointVal, customer.Id);
            _ICustomerPointsDetailService.CreateDetail(new CustomerPointsDetail()
            {
                Amount = pointVal,
                Comment = "",
                Customer = customer.Id,
                CustomerID = customer.CustomerGuid,
                IsCount = false,
                isOut = false,
                PointGetType = -1,
                PointUseType = 1,
                UpdateTime = DateTime.Now
            }) ;
            _IWithdrawalDetailService.Add(new WithdrawalDetail()
            {
                Amount = model.Amount,
                Customer = customer.Id,
                Comment = "",
                CustomerID = customer.CustomerGuid,
                IsCount = false,
                isOut = true,
                Operater = customer.Id,
                OperaterID = customer.CustomerGuid,
                WithdrawTime = DateTime.Now,
                WithdrawType = 2
            }) ;
        }

        public void Delete(WithdrawalApply model)
        {
            model.IsCount = false;
            _WithdrawalApplyRepository.Update(model);
        }

        public WithdrawalApply GetByID(int id)
        {
            return _WithdrawalApplyRepository.Table.FirstOrDefault(x => x.Customer == id);
        }

        public List<WithdrawalApply> GetList()
        {
           return _WithdrawalApplyRepository.Table.ToList();
        }

        public List<WithdrawalApply> GetListByID(int id)
        {
            return _WithdrawalApplyRepository.Table.Where(x => x.Customer == id).ToList();
        }

        public void Insert(WithdrawalApply model)
        {
            model.IsCount = true;
            _WithdrawalApplyRepository.Insert(model);
        }

        public void Update(WithdrawalApply model)
        {
             _WithdrawalApplyRepository.Update(model);
        }
        /// <summary>
        /// 提现审核
        /// </summary>
        /// <param name="customid"></param>
        /// <param name="amount"></param>
        public void WithdrawalApplyAudit(WithdrawalApply withdrawal,Customer customer,Customer applier)
        {
            //修改WithdrawalApply记录 减冻钱 扣手续费 加积分 写入日志详细
            this._WithdrawalApplyRepository.Update(withdrawal);
            var total = _IWithdrawalTotalService.Get(customer);
            total.TotalFreezeAmount -= withdrawal.Amount;
            total.UpdateTime = DateTime.Now;
            _IWithdrawalTotalService.Update(total);
            var pointVal = 0f;
            pointVal = (float)withdrawal.Amount * (float)(_calcrule.WithDrawApplyToPointPercent / 100) * (float)_calcrule.WithDrawToPointPercent;
            _ICustomerPointsTotalService.AddPointsToCustomer((int)pointVal, customer.Id);
            _ICustomerPointsDetailService.CreateDetail(new CustomerPointsDetail()
            {
                Amount = (int)pointVal,
                Comment = "提现金额" + withdrawal.Amount +  "转积分金额"+ withdrawal.ToPointAmount + "获得了商城积分" + (int)pointVal,
                Customer = customer.Id,
                CustomerID = customer.CustomerGuid,
                IsCount = false,
                isOut = false,
                PointGetType = -1,
                PointUseType = 1,
                UpdateTime = DateTime.Now
            });
            _IWithdrawalDetailService.Add(new WithdrawalDetail()
            {
                Amount = withdrawal.Amount,
                Customer = customer.Id,
                Comment = "提现金额" + withdrawal.Amount + "其中手续费" + withdrawal.ToFeeAmount + "转积分" + withdrawal.ToPointAmount + "获得了商城积分" + (int)pointVal,
                CustomerID = customer.CustomerGuid,
                IsCount = false,
                isOut = true,
                Operater = customer.Id,
                OperaterID = customer.CustomerGuid,
                WithdrawTime = DateTime.Now,
                WithdrawType = 2
            });
        }
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="customid"></param>
        /// <param name="amount"></param>
        public void WithdrawalApplyMethod(Customer customer, decimal amount)
        {
            // 减total余额 冻钱 不写日志  创建WithdrawalApply纪录
            var total = _IWithdrawalTotalService.Get(customer);
            total.TotalAmount -= amount;
            total.TotalFreezeAmount += amount;
            total.UpdateTime = DateTime.Now;
            _IWithdrawalTotalService.Update(total);
            this._WithdrawalApplyRepository.Insert(new WithdrawalApply()
            {
                Amount = amount,
                ExpectAmount = amount * ((100 - _calcrule.WithDrawApplyFeePercent - _calcrule.WithDrawApplyToPointPercent) / 100),
                 ToFeeAmount= amount * (_calcrule.WithDrawApplyFeePercent / 100), 
                  ToPointAmount = amount * (_calcrule.WithDrawApplyToPointPercent / 100),
                Comment = "",
                Customer = customer.Id,
                CustomerID = customer.CustomerGuid,
                IsCount = false,
                isOut = true,
                Operater = customer.Id,
                OperaterID = customer.CustomerGuid,
                UpdateTime = DateTime.Now,
                WithdrawStatus = 1,
                WithdrawTime = DateTime.Now,
                WithdrawType = 1

            }) ;
            
        }

        #endregion Public Constructors



        #region Public Methods



        #endregion Public Methods
    }
}