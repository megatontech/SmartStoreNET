using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class DailyTotalContributionDetailService : IDailyTotalContributionDetailService
    {
        private readonly IRepository<DailyTotalContributionDetail> _DailyTotalContributionDetailRepository;

        public DailyTotalContributionDetailService(IRepository<DailyTotalContributionDetail> DailyTotalContributionDetailRepository)
        {
            _DailyTotalContributionDetailRepository = DailyTotalContributionDetailRepository;
        }
        public void Add(DailyTotalContributionDetail entity)
        {
            _DailyTotalContributionDetailRepository.Insert(entity);


        }
    }
}
