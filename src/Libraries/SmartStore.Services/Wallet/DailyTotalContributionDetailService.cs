using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class DailyTotalContributionDetailService : IDailyTotalContributionDetailService
    {
        #region Private Fields

        private readonly IRepository<DailyTotalContributionDetail> _DailyTotalContributionDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public DailyTotalContributionDetailService(IRepository<DailyTotalContributionDetail> DailyTotalContributionDetailRepository)
        {
            _DailyTotalContributionDetailRepository = DailyTotalContributionDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(DailyTotalContributionDetail entity)
        {
            _DailyTotalContributionDetailRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}