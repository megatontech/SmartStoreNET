using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class CustomerPointsDetailService : ICustomerPointsDetailService
    {
        #region Private Fields

        private readonly IRepository<CustomerPointsDetail> _CustomerPointsDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public CustomerPointsDetailService(IRepository<CustomerPointsDetail> CustomerPointsDetailRepository)
        {
            _CustomerPointsDetailRepository = CustomerPointsDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(CustomerPointsDetail entity)
        {
            _CustomerPointsDetailRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}