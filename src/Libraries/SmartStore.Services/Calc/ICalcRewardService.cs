using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartStore.Services.Calc
{
    public partial interface ICalcRewardService
    {
        #region Public Methods

        /// <summary>
        /// 发红包
        /// </summary>
        /// <param name="StoreTotal"></param>
        /// <param name="isEqual"></param>
        void CalcRewardFour(decimal StoreTotal);

        /// <summary>
        /// 直推奖
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="order"></param>
        void CalcRewardOne(Customer customer, DeclarationOrder order);

        /// <summary>
        /// 每日商城利润分红
        /// </summary>
        /// <param name="StoreTotal"></param>
        void CalcRewardThree(decimal StoreTotal);

        /// <summary>
        /// 每日业绩分红
        /// </summary>
        /// <param name="CompanyTotal"></param>
        void CalcRewardTwo(decimal CompanyTotal);
         Task<int> CalcRewardTwoAsync(decimal CompanyTotal);
         Task<int> CalcRewardThreeAsync(decimal StoreTotal);
         Task<int> CalcRewardFourAsync(decimal StoreTotal);
        /// <summary>
        /// 查节点
        /// </summary>
        /// <param name="result"></param>
        /// <param name="treeNode"></param>
        /// <param name="customer"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        Customer recursiveFindNode(List<Customer> result, List<Customer> treeNode, Customer customer, int start, int end, int current);

        /// <summary>
        /// 实时更新
        /// </summary>
        public DailyTotalContribution UpdateRealtimeData();

        #endregion Public Methods
    }
}