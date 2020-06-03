using SmartStore.Core.Domain.Declaration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Declaration
{
    public interface IDeclarationCapRuleService
    {
        public  IList<DeclarationCapRule> GetAllRule();
    }
}
