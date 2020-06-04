using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class DailyTotalContributionService: IDailyTotalContributionService
    {
        private readonly IRepository<DailyTotalContribution> _DailyTotalContributionRepository;

        public DailyTotalContributionService(IRepository<DailyTotalContribution> DailyTotalContributionRepository)
        {
            _DailyTotalContributionRepository = DailyTotalContributionRepository;
        }
        public void Add(DailyTotalContribution entity)
        {
            _DailyTotalContributionRepository.Insert(entity);


        }
    }
}
