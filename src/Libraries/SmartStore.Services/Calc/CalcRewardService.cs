﻿using SmartStore.Collections;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Declaration;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Orders;
using SmartStore.Services.Customers;
using SmartStore.Services.Declaration;
using SmartStore.Services.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartStore.Services.Calc
{
    public class CalcRewardService : ICalcRewardService
    {
        private readonly WalletService _walletService;
        private readonly CustomerService _CustomerService;
        private readonly DeclarationCapRuleService _DeclarationCapRuleService;
        private readonly List<DeclarationCapRule> _rule;
        public CalcRewardService(WalletService walletService, CustomerService customerService, DeclarationCapRuleService declarationCapRuleService)
        {
            _walletService = walletService;
            _CustomerService = customerService;
            _DeclarationCapRuleService = declarationCapRuleService;
            _rule = _DeclarationCapRuleService.GetAllRule().ToList();
        }
        /// <summary>
        /// 佣金（可提现额度的增长值）来源一：直推一代（表示下级录单）奖金，报单金额15%，
        /// 同时上找5层每人再返15个点的10个点，例如单额100，直推15，往上5层每人再给1.5，
        /// 同理6层到13层每人给15个点的5个点，例如单额100，直推15，往上6层到13层每人再给7.5元
        /// </summary>
        /// <param name="treeNode">下单人所属的树</param>
        /// <param name="customer">下单人</param>
        /// <param name="order">订单</param>
        public void CalcRewardOne(List<Customer> treeNode, Customer customer, DeclarationOrder order)
        {
            //直接上级orderAmount*15%
            var customer1 = treeNode.Where(x => x.CustomerGuid == customer.ParentCustomerGuid).FirstOrDefault();
            var reward01 = Math.Round(order.OrderTotal * (decimal)0.15, 2);
            //向上5层orderAmount*15%*10%
            List<Customer> customers2 = new List<Customer>();
            var reward02 = Math.Round(order.OrderTotal * (decimal)0.15 * (decimal)0.10, 2);
            recursiveFindNode(customers2, treeNode, customer, 2, 6, 0);
            customers2 = customers2.Distinct().ToList();
            //再向上5层orderAmount*15%*5%
            List<Customer> customers3 = new List<Customer>();
            var reward03 = Math.Round(order.OrderTotal * (decimal)0.15 * (decimal)0.05, 2);
            recursiveFindNode(customers3, treeNode, customer, 7, 11, 0);
            customers3 = customers3.Distinct().ToList();
            //保存佣金计算结果，分配钱到每个人的钱包
            _walletService.SendRewardToWalletOne(new List<Customer> { customer1 }, reward01, order);
            _walletService.SendRewardToWalletOne(customers2, reward01, order);
            _walletService.SendRewardToWalletOne(customers3, reward01, order);
        }

        /// <summary>
        /// 传入当前节点，查询上级N层节点
        /// </summary>
        /// <param name="result"></param>
        /// <param name="treeNode"></param>
        /// <param name="customer"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public Customer recursiveFindNode(List<Customer> result, List<Customer> treeNode, Customer customer, int start, int end, int current)
        {
            var customer1 = treeNode.Where(x => x.CustomerGuid == customer.ParentCustomerGuid).FirstOrDefault();
            ++current;
            if (current >= start && current <= end && customer1 != null)
            {
                if (current == start) { result.Add(customer1); }

                result.Add(recursiveFindNode(result, treeNode, customer1, start, end, current));
            }
            else if (current < start)
            {
                result.Add(recursiveFindNode(result, treeNode, customer1, start, end, current));
                customer1 = null;
            }
            else { return null; }
            result.Remove(x => x == null);
            return customer1;
        }
        /// <summary>
        /// 佣金来源二：公司当日总体业绩（销售总额）25%商城利润分红，分红按个人贡献值的个数加权分红，贡献值个数的算法：
        ///1.公司当日总体业绩2.8万；
        ///2.去掉一个业绩最大的市场，其他市场业绩总和比如1.8万，则该点位当天总贡献值个数为：1.8万/100=180个；
        ///3.当日新增总分红额为2.8*25%=7000元；
        ///4.每个贡献值的价值为7000/180=38.8元；
        ///5.封顶限制，该点位两个区（需为活跃区，活跃区的意思就是 当日有见点，以下同理），每日分红封顶值为2000，三个区每日封顶值为5000，四个区为10000，以后每新增一个区封顶值加10000，例如，该点位有三条直推线，那么该点位每日分红最高可得2000+5000=7000（多区封顶累加），有四条直推线则为17000元，当该点位按权重个数获得的分红不超过总封顶额，分红按计算值获得，超过则按封顶值获得；
        /// </summary>
        /// <param name="treeNode">所有报单人</param>
        /// <param name="CompanyTotal">公司当日总体业绩</param>
        public void CalcRewardTwo(decimal CompanyTotal)
        {
            //分红总数
            var reward = Math.Round(CompanyTotal * (decimal)0.25, 2);
            //贡献值总数
            var totalPoint = (decimal)0;
            //计算并填充贡献点数计算业绩计算活跃线数
            var allCustomer = GenetateeNode();
            //计算每个人点数
            List<Customer> list = CalcCustomersPoints(allCustomer);
            totalPoint = (decimal)list.Sum(x => x.TotalPoints);
            //每个贡献值的价值
            var pointValue = (decimal)0;
            pointValue = Math.Round(reward / totalPoint);
            //每个人的贡献值
            foreach (var item in list)
            {
                item.TotalPointsValue2 = pointValue * (decimal)item.TotalPoints;
            }
            //按每个人的贡献值发放分红*
            _walletService.SendRewardToWalletTwo(list);
        }
        /// <summary>
        /// 商城利润分红
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="StoreTotal"></param>
        public void CalcRewardThree( decimal StoreTotal)
        {
            //商城利润分红
            var storeReward = Math.Round(StoreTotal * (decimal)0.5, 2);
            //贡献值总数
            var totalPoint = (decimal)0;
            //计算并填充贡献点数计算业绩计算活跃线数
            var allCustomer = GenetateeNode();
            //计算每个人点数
            List<Customer> list = CalcCustomersPoints(allCustomer);
            totalPoint = (decimal)list.Sum(x => x.TotalPoints);
            //每个贡献值的价值
            var pointValue = (decimal)0;
            pointValue = Math.Round(storeReward / totalPoint);
            //每个人的贡献值
            foreach (var item in list)
            {
                item.TotalPointsValue3 = pointValue * (decimal)item.TotalPoints;
            }
            //按每个人的贡献值发放分红*
            _walletService.SendRewardToWalletThree(list);
        }
        /// <summary>
        /// 商城红包
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="StoreTotal"></param>
        public void CalcRewardFour(TreeNode<Customer> treeNode, decimal StoreTotal)
        {
            //商城利润红包
            var storeReward = Math.Round(StoreTotal * (decimal)0.1, 2);
            _walletService.SendRewardToWalletFour();
        }
        /// <summary>
        /// 计算每个人点数
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public List<Customer> CalcCustomersPoints(List<Customer> customers)
        {
            //List<Customer> customers = new List<Customer>();
            foreach (var item in customers)
            {
                var pair = item.LineTotalpairs;
                pair.Remove(item.LineTotalpairs.Max().Key);
                item.TotalPoints = (float)(pair.Sum(x => x.Value) / 100);
                //活跃线数
                item.ActiveLines = pair.Count(x => x.Value != 0);
                //封顶线数
                item.CapLines = item.ActiveLines;
                //封顶钱数
                for (int i = 1; i < item.CapLines; i++)
                {
                    item.CapLinesTotal += GetActiveLineCapValue(1);
                }
            }
            return customers;

        }
        /// <summary>
        /// 查询每一级封顶值
        /// </summary>
        /// <param name="line">当前级别</param>
        /// <returns></returns>
        public decimal GetActiveLineCapValue(int line)
        {
            decimal result = 0.00M;
            result = _rule.Any(x=>x.LineCount==line)? _rule.First(x=> x.LineCount == line).RewardAmount:0M;
            return result;
        }
        /// <summary>
        /// 填充客户。计算每人每个线中业绩
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public List<Customer> GenetateeNode()
        {
            List<Customer> customers = _CustomerService.BuildTree();
            foreach (var item in customers)
            {
                item.SubLines = customers.Count(x => x.ParentCustomerGuid == item.CustomerGuid);
                item.HasChild = customers.Any(x => x.ParentCustomerGuid == item.CustomerGuid);
                Dictionary<Guid, decimal> keyValuePairsTotal = new Dictionary<Guid, decimal>();
                Dictionary<Guid, decimal> keyValuePairsDirect = new Dictionary<Guid, decimal>();
                //每条线业绩累加到最下级
                item.LineTotalpairs = keyValuePairsTotal;


                TreeNode<Customer> tree = new TreeNode<Customer>(item);
                tree.AppendRange(customers.Where(x => x.ParentCustomerGuid == item.CustomerGuid));
                item.ChildNode = tree;
            }
            return customers;
        }
    }
}