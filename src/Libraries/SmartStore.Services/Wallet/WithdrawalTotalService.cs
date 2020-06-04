using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
   public class WithdrawalTotalService:IWithdrawalTotalService
    {
        private readonly IRepository<WithdrawalTotal> _WithdrawalTotalRepository;

        public WithdrawalTotalService(IRepository<WithdrawalTotal> WithdrawalTotalRepository)
        {
            _WithdrawalTotalRepository = WithdrawalTotalRepository;
        }
        public void Add(WithdrawalTotal entity)
        {
            _WithdrawalTotalRepository.Insert(entity);
        }
        public void Update(WithdrawalTotal entity)
        {
            _WithdrawalTotalRepository.Update(entity);
        }
        public WithdrawalTotal Get(Customer customer)
        {
            if (_WithdrawalTotalRepository.Table.Any(x => x.CustomerId == customer.Id)) 
            {
               return _WithdrawalTotalRepository.GetFirst(x => x.CustomerId == customer.Id); 
            }
            else
            {
                WithdrawalTotal total = new WithdrawalTotal();
                total.CustomerId = customer.Id;
                total.CustomerGuid = customer.CustomerGuid;
                total.UpdateTime = DateTime.Now;
                _WithdrawalTotalRepository.Insert(total);
                return _WithdrawalTotalRepository.GetFirst(x => x.CustomerId == customer.Id);
            }
            
            
        }
    }
}
