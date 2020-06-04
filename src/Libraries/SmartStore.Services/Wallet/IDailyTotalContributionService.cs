using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public interface IDailyTotalContributionService
    {
        public DailyTotalContribution Get();
            public void Update(DailyTotalContribution entity);
            public void Add(DailyTotalContribution entity);
    }
}