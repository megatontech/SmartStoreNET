using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class DailyTotalContributionService : IDailyTotalContributionService
    {
        #region Private Fields

        private readonly IRepository<DailyTotalContribution> _DailyTotalContributionRepository;

        #endregion Private Fields

        #region Public Constructors

        public DailyTotalContributionService(IRepository<DailyTotalContribution> DailyTotalContributionRepository)
        {
            _DailyTotalContributionRepository = DailyTotalContributionRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(DailyTotalContribution entity)
        {
            _DailyTotalContributionRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}