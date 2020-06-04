using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class CustomerPointsTotalService : ICustomerPointsTotalService
    {
        #region Private Fields

        private readonly IRepository<CustomerPointsTotal> _CustomerPointsTotalRepository;

        #endregion Private Fields

        #region Public Constructors

        public CustomerPointsTotalService(IRepository<CustomerPointsTotal> CustomerPointsTotalRepository)
        {
            _CustomerPointsTotalRepository = CustomerPointsTotalRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(CustomerPointsTotal entity)
        {
            _CustomerPointsTotalRepository.Insert(entity);
        }

        #endregion Public Methods
    }
}