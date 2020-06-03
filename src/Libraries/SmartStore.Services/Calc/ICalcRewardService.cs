using SmartStore.Collections;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Calc
{
    public partial  interface ICalcRewardService
    {
        void CalcRewardOne(List<Customer> treeNode, Customer customer, DeclarationOrder order);
        void CalcRewardTwo(TreeNode<Customer> treeNode);
        void CalcRewardThree();
        void CalcRewardFour();
        Customer recursiveFindNode(List<Customer> result, List<Customer> treeNode, Customer customer, int start, int end, int current);
    }
}
