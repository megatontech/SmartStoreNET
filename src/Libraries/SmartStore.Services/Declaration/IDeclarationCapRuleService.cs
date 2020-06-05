using SmartStore.Core.Domain.Declaration;
using System.Collections.Generic;

namespace SmartStore.Services.Declaration
{
    public interface IDeclarationCapRuleService
    {
        #region Public Methods

        public IList<DeclarationCapRule> GetAllRule();

        #endregion Public Methods
    }
}