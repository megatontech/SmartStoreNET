using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Events;
using SmartStore.Core.Localization;
using SmartStore.Services.Messages;
using SmartStore.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Customers
{
    public partial class CustomerDiscountService : ICustomerDiscountService
    {
        #region Private Fields

        private readonly IRepository<CustomerDiscount> _checkRepository;

        public CustomerDiscountService(ICheckInService checkService, IRepository<CustomerDiscount> checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public void Insert(CustomerDiscount model)
        {
            model.GetDateTime = DateTime.Now;
            model.IsUsed = false;
            _checkRepository.Insert(model);
        }
        public void Delete(CustomerDiscount model)
        {
            _checkRepository.Delete(model);
        }
        public void Use(CustomerDiscount model)
        {
            model.UseDateTime = DateTime.Now;
            model.IsUsed = true;
            _checkRepository.Update(model);
        }
        public List<CustomerDiscount> Get(int customerid)
        {
            return _checkRepository.Table.Where(x=>x.Customer==customerid).ToList();
        }

        #endregion Private Fields



    }
}