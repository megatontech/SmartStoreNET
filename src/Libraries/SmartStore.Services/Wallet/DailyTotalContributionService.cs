using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Linq;

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
       
        public DailyTotalContribution Get()
        {
            var today = DateTime.UtcNow.Date;
            if (!_DailyTotalContributionRepository.Table.Any(x=>x.CreateTime>= today))
            {
                DailyTotalContribution dailyTotalContribution = new DailyTotalContribution()
                {
                    CreateTime = today,
                    ContributionTime = today,
                    ContributionValue = 0M,
                    DecValue = 0M,
                    IsCount = false,
                    TotalValue = 0M,
                    UpdateTime = DateTime.UtcNow
                };
                _DailyTotalContributionRepository.Insert(dailyTotalContribution);
                return _DailyTotalContributionRepository.Table.FirstOrDefault(x => x.CreateTime >= today);
            }
            else
            {
                return _DailyTotalContributionRepository.Table.FirstOrDefault(x => x.CreateTime >= today);
            }

        }
        public void Update(DailyTotalContribution entity)
        {
            _DailyTotalContributionRepository.Update(entity);
        }
        public void Add(DailyTotalContribution entity)
        {
            _DailyTotalContributionRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}