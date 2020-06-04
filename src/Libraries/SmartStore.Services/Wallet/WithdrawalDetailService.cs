using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalDetailService : IWithdrawalDetailService
    {
        #region Private Fields

        private readonly IRepository<WithdrawalDetail> _WithdrawalDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public WithdrawalDetailService(IRepository<WithdrawalDetail> WithdrawalDetailRepository)
        {
            _WithdrawalDetailRepository = WithdrawalDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(WithdrawalDetail entity)
        {
            _WithdrawalDetailRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}