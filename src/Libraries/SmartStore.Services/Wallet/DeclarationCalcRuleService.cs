using SmartStore.Core.Data;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Wallet
{
    public class DeclarationCalcRuleService : IDeclarationCalcRuleService
    {
        #region Public Fields


        #endregion Public Fields



        #region Private Fields


        #endregion Private Fields

        #region Public Constructors

        private readonly IRepository<DeclarationCalcRule> _DeclarationCalcRule;


        public DeclarationCalcRuleService(IRepository<DeclarationCalcRule> declarationCalcRule)
        {
            _DeclarationCalcRule = declarationCalcRule;
        }

        public void Add(DeclarationCalcRule entity)
        {
            _DeclarationCalcRule.Insert(entity);

        }

        public DeclarationCalcRule GetDeclarationCalcRule()
        {
            if (_DeclarationCalcRule.Table.Any(x => x.isUse == true))
            { return _DeclarationCalcRule.Table.FirstOrDefault(x => x.isUse == true); } 
            else {
                DeclarationCalcRule entity = new DeclarationCalcRule()
                {
                    UpdateTime = DateTime.Now,
                    CalcRewardFourEqual = false,
                    CalcRewardFourPercent = 10,
                    CalcRewardOneL1Percent = 15,
                    CalcRewardOneL2Count = 5,
                    CalcRewardOneL2Percent = 10,
                    CalcRewardOneL3Count = 5,
                    CalcRewardOneL3Percent = 5,
                    CalcRewardThreePercent = 50,
                    CalcRewardTwoPercent = 25,
                    CalcRewardTwoPointPercent = 100,
                    Comment = "",
                    CreateTime = DateTime.Now,
                    WithDrawToPointPercent =1,
                    isUse = true
                };
                Add(entity);
                return _DeclarationCalcRule.Table.FirstOrDefault(x => x.isUse == true); }
          
        }

        public void Update(DeclarationCalcRule entity)
        {
            _DeclarationCalcRule.Update(entity);
        }

        #endregion Public Constructors



        #region Public Methods



        #endregion Public Methods
    }
}