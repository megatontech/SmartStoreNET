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
    public partial interface ICheckInService
    {
        #region Public Methods
        public void Insert(CheckIn model);
        public CheckIn Get(int customerid);
        #endregion Public Methods
    }
}