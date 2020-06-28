using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IWithdrawalDetailService
    {
        #region Public Methods

        public void Add(WithdrawalDetail entity);
        public WithdrawalDetail GetByid(int id);
        public List<WithdrawalDetail> GetByOrderid(int orderid);
        public List<WithdrawalDetail> GetByCustomId(int id);
        public List<WithdrawalDetail> GetByCustomId(int id, int count);
        public List<WithdrawalDetail> GetByCustomId(int id,int skip, int count);
        public List<WithdrawalDetail> Get();
        public List<WithdrawalDetail> Get(int start,int length);
        public int GetCount();

        #endregion Public Methods
    }
}