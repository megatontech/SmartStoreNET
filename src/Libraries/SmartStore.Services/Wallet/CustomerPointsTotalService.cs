using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class CustomerPointsTotalService: ICustomerPointsTotalService
    {
        private readonly IRepository<CustomerPointsTotal> _CustomerPointsTotalRepository;

        public CustomerPointsTotalService(IRepository<CustomerPointsTotal> CustomerPointsTotalRepository)
        {
            _CustomerPointsTotalRepository = CustomerPointsTotalRepository;
        }
        public void Add(CustomerPointsTotal entity)
        {
            _CustomerPointsTotalRepository.Insert(entity);
        }
    }
}
