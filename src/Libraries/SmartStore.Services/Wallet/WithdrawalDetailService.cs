using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Wallet
{
    public class WithdrawalDetailService : IWithdrawalDetailService
    {
        #region Private Fields

        private readonly IRepository<WithdrawalDetail> _WithdrawalDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public WithdrawalDetailService(IRepository<WithdrawalDetail> WithdrawalDetailRepository)
        {
            _WithdrawalDetailRepository = WithdrawalDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(WithdrawalDetail entity)
        {
            _WithdrawalDetailRepository.Insert(entity);
        }

        public List<WithdrawalDetail> Get()
        {
           return  _WithdrawalDetailRepository.Table.ToList();
        }

        public List<WithdrawalDetail> Get3ByCustomId(int id)
        {
            return _WithdrawalDetailRepository.Table.Where(x => x.Customer == id).Take(3).ToList();
        }

        public List<WithdrawalDetail> GetByCustomId(int id)
        {
            return _WithdrawalDetailRepository.Table.Where(x => x.Customer == id).ToList();
        }

        public WithdrawalDetail GetByid(int id)
        {
            return _WithdrawalDetailRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        #endregion Public Methods
    }
}