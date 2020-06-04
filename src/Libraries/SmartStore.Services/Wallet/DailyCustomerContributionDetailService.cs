using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class DailyCustomerContributionDetailService: IDailyCustomerContributionDetailService
    {
        private readonly IRepository<DailyCustomerContributionDetail> _DailyCustomerContributionDetailRepository;

        public DailyCustomerContributionDetailService(IRepository<DailyCustomerContributionDetail> DailyCustomerContributionDetailRepository)
        {
            _DailyCustomerContributionDetailRepository = DailyCustomerContributionDetailRepository;
        }
        public void Add(DailyCustomerContributionDetail entity)
        {
            _DailyCustomerContributionDetailRepository.Insert(entity);


        }
    }
}
