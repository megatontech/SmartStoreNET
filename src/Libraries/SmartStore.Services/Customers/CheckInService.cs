using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Events;
using SmartStore.Core.Localization;
using SmartStore.Services.Messages;
using SmartStore.Services.Security;
using System;
using System.Linq;

namespace SmartStore.Services.Customers
{
    public partial class CheckInService : ICheckInService
    {
        #region Private Fields

        private readonly IRepository<CheckIn> _checkRepository;

        public CheckInService(IRepository<CheckIn> checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public void Insert(CheckIn model)
        {
            _checkRepository.Insert(model);
        }
        public CheckIn Get(int customerid)
        {
            var date = DateTime.Now.Date;
            return _checkRepository.Table.Where(x => x.Customer == customerid && x.CheckDate.Date == date).FirstOrDefault();
        }

        #endregion Private Fields



    }
}