using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Linq;
using System.Linq.Dynamic;

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

        public LuckMoney GetLuckMoneyByCustomer(int id)
        {
            return _LuckMoneyRepository.Table.FirstOrDefault(x => x.Customer == id && x.StartTime >= DateTime.UtcNow && x.EndTime <= DateTime.UtcNow);
        }

        public LuckMoney GetLuckMoneyById(int id)
        {
            return _LuckMoneyRepository.Table.FirstOrDefault(x => x.Id == id );
        }

        public void Update(LuckMoney luck) 
        {
            _LuckMoneyRepository.Update(luck);
        }
        #endregion Public Methods
    }
}