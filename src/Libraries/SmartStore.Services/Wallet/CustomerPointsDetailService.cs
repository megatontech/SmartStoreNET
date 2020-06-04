using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class CustomerPointsDetailService: ICustomerPointsDetailService
    {
        private readonly IRepository<CustomerPointsDetail> _CustomerPointsDetailRepository;

        public CustomerPointsDetailService(IRepository<CustomerPointsDetail> CustomerPointsDetailRepository)
        {
            _CustomerPointsDetailRepository = CustomerPointsDetailRepository;
        }
        public void Add(CustomerPointsDetail entity)
        {
            _CustomerPointsDetailRepository.Insert(entity);


        }
    }
}
