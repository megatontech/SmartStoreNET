using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;

namespace SmartStore.Services.Wallet
{
    public interface IDailyCustomerContributionDetailService
    {
        public DailyCustomerContributionDetail Get(int id, Guid guid);
        public void Update(DailyCustomerContributionDetail entity);
        public void Add(DailyCustomerContributionDetail entity);
        
        public DailyCustomerContributionDetail Get(Customer customer);

    }
}