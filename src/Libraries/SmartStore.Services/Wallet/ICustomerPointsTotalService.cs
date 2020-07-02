using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface ICustomerPointsTotalService
    {
        public void AddPointsToCustomer(int points,int customerid);
        public void RemovePointsFromCustomer(int points,int customerid);
        public List<CustomerPointsTotal> GetAll();
        public decimal GetAllSum();
        
        public CustomerPointsTotal GetPoints(int customerid);
        public void UpdatePoints(CustomerPointsTotal entity);
        public void CreatePoints(CustomerPointsTotal entity);
    }
}