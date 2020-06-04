using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalDetailService : IWithdrawalDetailService
    {
        private readonly IRepository<WithdrawalDetail> _WithdrawalDetailRepository;

        public WithdrawalDetailService(IRepository<WithdrawalDetail> WithdrawalDetailRepository)
        {
            _WithdrawalDetailRepository = WithdrawalDetailRepository;
        }
        public void Add(WithdrawalDetail entity)
        {
            _WithdrawalDetailRepository.Insert(entity);
        }
    }
}
