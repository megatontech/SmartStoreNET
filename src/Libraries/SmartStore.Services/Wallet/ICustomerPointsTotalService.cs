using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public interface ICustomerPointsTotalService
    {
        public void AddPointsToCustomer(int points,int customerid);
        public void RemovePointsFromCustomer(int points,int customerid);
        public CustomerPointsTotal GetPoints(int customerid);
        public void UpdatePoints(CustomerPointsTotal entity);
        public void CreatePoints(CustomerPointsTotal entity);
    }
}