using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalDetailService
    {
        #region Public Methods

        public void Add(WithdrawalDetail entity);
        public WithdrawalDetail GetByid(int id);
        public List<WithdrawalDetail> GetByCustomId(int id);
        public List<WithdrawalDetail> GetByCustomId(int id, int count);
        public List<WithdrawalDetail> GetByCustomId(int id,int skip, int count);
        public List<WithdrawalDetail> Get();

        #endregion Public Methods
    }
}