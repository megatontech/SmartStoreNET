using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
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
        /// <summary>
        /// 提现审核
        /// </summary>
        /// <param name="customid"></param>
        /// <param name="amount"></param>
        public void WithdrawalApplyAudit(WithdrawalApply withdrawal, Customer customer, Customer applier);
        public WithdrawalApply GetByID(int id);
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