using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalApplyService: IWithdrawalApplyService
    {
        private readonly IRepository<WithdrawalApply> _WithdrawalApplyRepository;

        public WithdrawalApplyService(IRepository<WithdrawalApply> withdrawalApplyRepository)
        {
            _WithdrawalApplyRepository = withdrawalApplyRepository;
        }
        public void Add(WithdrawalApply entity) 
        {
            _WithdrawalApplyRepository.Insert(entity);


        }
    }
}
