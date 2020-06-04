using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public class LuckMoneyService : ILuckMoneyService
    {
        private readonly IRepository<LuckMoney> _LuckMoneyRepository;

        public LuckMoneyService(IRepository<LuckMoney> luckMoneyRepository)
        {
            _LuckMoneyRepository = luckMoneyRepository;
        }
        public void AddLuckMoney(LuckMoney luck) 
        {
            _LuckMoneyRepository.Insert(luck);
        }
    }
}
