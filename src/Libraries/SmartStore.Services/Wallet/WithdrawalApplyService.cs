using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

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

        #endregion Public Constructors



        #region Public Methods

        public void Add(WithdrawalApply entity)
        {
            _WithdrawalApplyRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}