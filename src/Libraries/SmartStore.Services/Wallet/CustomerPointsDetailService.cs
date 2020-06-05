using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
namespace SmartStore.Services.Wallet
{
    public class CustomerPointsDetailService : ICustomerPointsDetailService
    {
        #region Private Fields

        private readonly IRepository<CustomerPointsDetail> _CustomerPointsDetailRepository;

        #endregion Private Fields

        #region Public Constructors

        public CustomerPointsDetailService(IRepository<CustomerPointsDetail> CustomerPointsDetailRepository)
        {
            _CustomerPointsDetailRepository = CustomerPointsDetailRepository;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(CustomerPointsDetail entity)
        {
            _CustomerPointsDetailRepository.Insert(entity);
        }

        public void CreateDetail(CustomerPointsDetail entity)
        {
            _CustomerPointsDetailRepository.Insert(entity);
        }

        public List<CustomerPointsDetail> GetDetailByCustomer(int id)
        {
            return _CustomerPointsDetailRepository.Table.Where(x => x.Customer == id).ToList();
        }

        public CustomerPointsDetail GetDetailByID(int id)
        {
            return _CustomerPointsDetailRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateDetail(CustomerPointsDetail entity)
        {
            _CustomerPointsDetailRepository.Update(entity);
        }

        #endregion Public Methods
    }
}