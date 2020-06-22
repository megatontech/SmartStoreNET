using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Linq;

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
        public DailyCustomerContributionDetail Get(Customer customer) 
        {
            return null;
        }
        /// <summary>
        /// zuotiangongxianzhi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public DailyCustomerContributionDetail Get(int id,Guid guid)
        {
            var today = DateTime.Now.Date;
            var yestoday = DateTime.Now.Date.AddDays(-1);
            if (!_DailyCustomerContributionDetailRepository.Table.Any(x => x.CreateTime <= today&&x.CreateTime>=yestoday&&x.Customer==id))
            {
                DailyCustomerContributionDetail dailyTotalContribution = new DailyCustomerContributionDetail()
                {
                    CreateTime = yestoday,
                    ContributionTime = yestoday,
                     ActiveLine = 0,
                    CountTotalValue = 0M, 
                    Customer = id,
                    TotalLine = 0,
                    TotalPoint=0, 
                    TotalPointValue=0M,
                    CustomerID= guid,
                    TotalValue = 0M,
                    UpdateTime = DateTime.Now
                };
                _DailyCustomerContributionDetailRepository.Insert(dailyTotalContribution);
                return _DailyCustomerContributionDetailRepository.Table.FirstOrDefault(x => x.CreateTime <= today && x.CreateTime >= yestoday && x.Customer == id);
            }
            else
            {
                return _DailyCustomerContributionDetailRepository.Table.FirstOrDefault(x => x.CreateTime <= today && x.CreateTime >= yestoday && x.Customer == id);
            }
        }

        public void Update(DailyCustomerContributionDetail entity)
        {
            _DailyCustomerContributionDetailRepository.Update(entity);
        }

        #endregion Public Methods
    }
}