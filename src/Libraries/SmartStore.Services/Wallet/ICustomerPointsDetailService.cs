using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface ICustomerPointsDetailService
    {
        public void CreateDetail(CustomerPointsDetail entity);
        public List<CustomerPointsDetail> GetDetailByCustomer(int id);
        public CustomerPointsDetail GetDetailByID(int id);
        public void UpdateDetail(CustomerPointsDetail entity);
    }
}