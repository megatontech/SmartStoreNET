using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalApplyService
    {
        public WithdrawalApply GetByID(int id);
        public List<WithdrawalApply> GetList();
        public List<WithdrawalApply> GetListByID();
        public void Insert(WithdrawalApply model);
        public void Update(WithdrawalApply model);
        public void Delete(WithdrawalApply model);
    }
}