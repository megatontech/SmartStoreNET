using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class DailyCustomerContributionDetailService : IDailyCustomerContributionDetailService
    {
        #region Private Fields

        private readonly IRepository<DailyCustomerContributionDetail> _DailyCustomerContributionDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public DailyCustomerContributionDetailService(IRepository<DailyCustomerContributionDetail> DailyCustomerContributionDetailRepository)
        {
            _DailyCustomerContributionDetailRepository = DailyCustomerContributionDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(DailyCustomerContributionDetail entity)
        {
            _DailyCustomerContributionDetailRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}