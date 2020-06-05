using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public interface ILuckMoneyService
    {
        #region Public Methods
        public void Update(LuckMoney luck);
        public LuckMoney GetLuckMoneyById(int id);
        public LuckMoney GetLuckMoneyByCustomer(int id);
        public void AddLuckMoney(LuckMoney luck);


        #endregion Public Methods
    }
}