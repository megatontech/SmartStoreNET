using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;

namespace SmartStore.Services.Wallet
{
    public interface IDeclarationCalcRuleService
    {
        #region Public Methods

        public DeclarationCalcRule GetDeclarationCalcRule();

        public void Update(DeclarationCalcRule entity);

        public void Add(DeclarationCalcRule entity);

        #endregion Public Methods
    }
}