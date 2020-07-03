using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SmartStore.Services.Wallet
{
    public class CustomerPointsTotalService : ICustomerPointsTotalService
    {
        #region Private Fields

        private readonly IRepository<CustomerPointsTotal> _CustomerPointsTotalRepository;
        private readonly ICustomerPointsDetailService _ICustomerPointsDetailService;
        
        #endregion Private Fields

        #region Public Constructors

        public CustomerPointsTotalService(IRepository<CustomerPointsTotal> CustomerPointsTotalRepository
            , ICustomerPointsDetailService iCustomerPointsDetailService
            )
        {
            _CustomerPointsTotalRepository = CustomerPointsTotalRepository;
            _ICustomerPointsDetailService = iCustomerPointsDetailService;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Add(CustomerPointsTotal entity)
        {
            _CustomerPointsTotalRepository.Insert(entity);
        }

        public void AddPointsToCustomer(int points, int customerid)
        {
            var entity = GetPoints(customerid);
            entity.Amount += points;
            entity.UpdateTime = DateTime.Now;
            _CustomerPointsTotalRepository.Update(entity);
        }

        public void CreatePoints(CustomerPointsTotal entity)
        {
            _CustomerPointsTotalRepository.Insert(entity);

        }
        public List<CustomerPointsTotal> GetAll() 
        {
            return _CustomerPointsTotalRepository.Table.ToList();
        }
        public decimal GetAllSum()
        {
            if (_CustomerPointsTotalRepository.Table.Any()) { return _CustomerPointsTotalRepository.Table.Sum(x => x.Amount); }
            else { return 0M; }
           
        }
        public CustomerPointsTotal GetPoints(int customerid)
        {
            if (!_CustomerPointsTotalRepository.Table.Any(x=>x.Customer==customerid))
            {
                CustomerPointsTotal pointsTotal = new CustomerPointsTotal() { };
                pointsTotal.Customer = customerid;
                _CustomerPointsTotalRepository.Insert(pointsTotal);
                return _CustomerPointsTotalRepository.Table.FirstOrDefault(x => x.Customer == customerid);
            }
            else 
            {
                return _CustomerPointsTotalRepository.Table.FirstOrDefault(x => x.Customer == customerid);
            }
        }

        public void RemovePointsFromCustomer(int points, int customerid)
        {
            var entity = GetPoints(customerid);
            entity.Amount -= points;
            entity.UpdateTime = DateTime.Now;
            _CustomerPointsTotalRepository.Update(entity);
        }

        public void UpdatePoints(CustomerPointsTotal entity)
        {
            _CustomerPointsTotalRepository.Update(entity); 
        }

        #endregion Public Methods
    }
}