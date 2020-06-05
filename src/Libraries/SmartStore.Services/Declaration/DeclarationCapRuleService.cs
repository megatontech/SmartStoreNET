using SmartStore.Core.Data;
using SmartStore.Core.Domain.Declaration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SmartStore.Services.Declaration
{
    public class DeclarationCapRuleService : IDeclarationCapRuleService
    {
        #region Private Fields

        private readonly IRepository<DeclarationCapRule> _Repository;

        #endregion Private Fields

        #region Public Constructors

        public DeclarationCapRuleService(IRepository<DeclarationCapRule> services)
        {
            _Repository = services;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual IList<DeclarationCapRule> GetAllRule()
        {
            var query =
                from p in _Repository.Table
                orderby p.LineCount
                select p;

            var rule = query.ToList();
            return rule;
        }

        #endregion Public Methods
    }
}