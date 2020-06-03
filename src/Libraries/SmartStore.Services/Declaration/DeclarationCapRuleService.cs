using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using SmartStore.Collections;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Declaration;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Events;
using SmartStore.Data.Caching;
using SmartStore.Services.Messages;
using SmartStore.Services.Orders;

namespace SmartStore.Services.Declaration
{
    public class DeclarationCapRuleService:IDeclarationCapRuleService
    {
        private readonly IRepository<DeclarationCapRule> _Repository;
        public DeclarationCapRuleService(IRepository<DeclarationCapRule> services)
        {
            _Repository = services;

        }
        public virtual IList<DeclarationCapRule> GetAllRule()
        {
            var query =
                from p in _Repository.Table
                orderby p.LineCount
                select p;

            var rule = query.ToList();
            return rule;
        }
    }
}
