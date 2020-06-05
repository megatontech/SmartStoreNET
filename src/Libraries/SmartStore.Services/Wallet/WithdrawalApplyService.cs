using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalApplyService : IWithdrawalApplyService
    {
        #region Private Fields

        private readonly IRepository<WithdrawalApply> _WithdrawalApplyRepository;

        #endregion Private Fields

        #region Public Constructors

        public WithdrawalApplyService(IRepository<WithdrawalApply> withdrawalApplyRepository)
        {
            _WithdrawalApplyRepository = withdrawalApplyRepository;
        }

        public void Delete(WithdrawalApply model)
        {
            throw new System.NotImplementedException();
        }

        public WithdrawalApply GetByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<WithdrawalApply> GetList()
        {
            throw new System.NotImplementedException();
        }

        public List<WithdrawalApply> GetListByID()
        {
            throw new System.NotImplementedException();
        }

        public void Insert(WithdrawalApply model)
        {
            throw new System.NotImplementedException();
        }

        public void Update(WithdrawalApply model)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Constructors



        #region Public Methods



        #endregion Public Methods
    }
}