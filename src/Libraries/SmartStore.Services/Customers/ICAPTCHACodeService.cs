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
    public partial interface ICAPTCHACodeService
    {
        #region Public Methods
        public void Insert(CAPTCHACode model);
        public bool Send(string mobile);
        public bool ValidCode(string mobile, string code);
        public void Update(CAPTCHACode model);
        public List<CAPTCHACode> Get(string mobile);
        
        #endregion Public Methods
    }
}