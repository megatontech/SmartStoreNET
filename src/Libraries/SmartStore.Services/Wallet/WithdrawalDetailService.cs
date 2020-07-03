﻿using SmartStore.Core.Data;
using SmartStore.Core.Domain.Wallet;
using System;
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
        public int GetCount() { return _WithdrawalDetailRepository.Table.Where(x=> x.Amount != 0M).Count(); }
        public decimal GetTodaySum() 
        {
            var today = DateTime.Now.Date;
            var tomrrow = DateTime.Now.Date.AddDays(1);
             var temp =   _WithdrawalDetailRepository.Table.Where(x => x.isOut == false && x.WithdrawType == 1 && x.WithdrawTime >= today && x.WithdrawTime <= tomrrow).ToList();
            if (temp != null) { return temp.Sum(x => x.Amount); }
            else { return 0M; }
            
        }
        public List<WithdrawalDetail> Get()
        {
           return  _WithdrawalDetailRepository.Table.ToList();
        }
        public List<WithdrawalDetail> Get(int start, int length)
        {
            return _WithdrawalDetailRepository.Table.Where(x=>x.Amount != 0M).OrderByDescending(x=>x.WithdrawTime).Skip(start).Take(length).ToList();
        }
        public List<WithdrawalDetail> GetByCustomId(int id,int count)
        {
            return _WithdrawalDetailRepository.Table.Where(x => x.Customer == id && x.Amount != 0M).OrderByDescending(x => x.WithdrawTime).Take(count).ToList();
        }
        public List<WithdrawalDetail> GetByCustomId(int id, int skip, int count)
        {
            return _WithdrawalDetailRepository.Table.Where(x => x.Customer == id && x.Amount != 0M).OrderByDescending(x => x.WithdrawTime).Skip(skip).Take(count).ToList();
        }
        public List<WithdrawalDetail> GetByOrderid(int orderid)
        { 
            return _WithdrawalDetailRepository.Table.Where(x => x.Order == orderid && x.Amount!=0M).OrderByDescending(x=>x.Amount).ToList();

        }
        public List<WithdrawalDetail> GetByCustomId(int id)
        {
            return _WithdrawalDetailRepository.Table.Where(x => x.Customer == id &&x.Amount!=0M).OrderByDescending(x=>x.WithdrawTime).ToList();
        }

        public WithdrawalDetail GetByid(int id)
        {
            return _WithdrawalDetailRepository.Table.FirstOrDefault(x => x.Id == id);
        }

        #endregion Public Methods
    }
}