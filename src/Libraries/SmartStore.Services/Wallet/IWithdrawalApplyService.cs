using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalApplyService
    {
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="customid"></param>
        /// <param name="amount"></param>
        public void WithdrawalApplyMethod(Customer customer, decimal amount);
        public void WithdrawalPointMethod(Customer customer, decimal amount);
        public void WithdrawalPayMethod(Customer customer, decimal amount,int order, Guid orderguid);
        /// <summary>
        /// 提现审核
        /// </summary>
        /// <param name="customid"></param>
        /// <param name="amount"></param>
        public void WithdrawalApplyAudit(WithdrawalApply withdrawal, Customer customer, Customer applier);
        public void WithdrawalApplyDeny(WithdrawalApply withdrawal, Customer customer, Customer applier);
        public void WithdrawalPayAudit(WithdrawalApply withdrawal, Customer customer);
        public WithdrawalApply GetByID(int id);
        public WithdrawalApply GetByTableID(int id);
        public WithdrawalApply GetByOrderID(int id,Guid orerid);
        

        public List<WithdrawalApply> GetList();
        public List<WithdrawalApply> GetListByID(int id);
        public void Insert(WithdrawalApply model);
        public void Update(WithdrawalApply model);
        public void Delete(WithdrawalApply model);
        /// <summary>
        /// 转积分
        /// </summary>
        /// <param name="model"></param>
        public void ConvertToPoints(WithdrawalApply model, Customer customer);
    }
}