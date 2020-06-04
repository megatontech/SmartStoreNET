using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System.Linq;
using System.Linq.Dynamic;

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

       

        #endregion Public Methods
    }
}