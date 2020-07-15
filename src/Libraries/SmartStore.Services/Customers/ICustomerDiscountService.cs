using SmartStore.Collections;
using SmartStore.Core;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartStore.Services.Customers
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerDiscountService
    {
        #region Public Methods
        public void Insert(CustomerDiscount model);
        public CustomerDiscount GetOne(int id);
        public List<CustomerDiscount> Get(int customerid);
        public void Delete(CustomerDiscount model);
        public void Use(CustomerDiscount model);
        #endregion Public Methods
    }
}