using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Linq;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalTotalService : IWithdrawalTotalService
    {
        #region Private Fields

        private readonly IRepository<WithdrawalTotal> _WithdrawalTotalRepository;

        #endregion Private Fields

        #region Public Constructors

        public WithdrawalTotalService(IRepository<WithdrawalTotal> WithdrawalTotalRepository)
        {
            _WithdrawalTotalRepository = WithdrawalTotalRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(WithdrawalTotal entity)
        {
            _WithdrawalTotalRepository.Insert(entity);
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

        public void Update(WithdrawalTotal entity)
        {
            _WithdrawalTotalRepository.Update(entity);
        }

        #endregion Public Methods
    }
}