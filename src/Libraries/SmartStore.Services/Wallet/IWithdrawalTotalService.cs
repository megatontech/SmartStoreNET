using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalTotalService
    {
        public void Add(WithdrawalTotal entity);
        public WithdrawalTotal Get(Customer customer);
        public void Update(WithdrawalTotal entity);
    }
}
