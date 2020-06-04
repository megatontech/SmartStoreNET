using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public interface ILuckMoneyService
    {
        void AddLuckMoney(LuckMoney luck)
    }
}
