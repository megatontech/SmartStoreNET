using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Services.Wallet
{
    public class LuckMoneyService : ILuckMoneyService
    {
        #region Private Fields

        private readonly IRepository<LuckMoney> _LuckMoneyRepository;

        #endregion Private Fields

        #region Public Constructors

        public LuckMoneyService(IRepository<LuckMoney> luckMoneyRepository)
        {
            _LuckMoneyRepository = luckMoneyRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void AddLuckMoney(LuckMoney luck)
        {
            _LuckMoneyRepository.Insert(luck);
        }

        #endregion Public Methods
    }
}