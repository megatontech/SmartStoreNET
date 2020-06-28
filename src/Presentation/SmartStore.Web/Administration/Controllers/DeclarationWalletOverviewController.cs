using SmartStore.Admin.Models.Orders;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Payments;
using SmartStore.Core.Domain.Shipping;
using SmartStore.Core.Domain.Tax;
using SmartStore.Core.Events;
using SmartStore.Core.Html;
using SmartStore.Core.Logging;
using SmartStore.Core.Plugins;
using SmartStore.Core.Search;
using SmartStore.Services.Affiliates;
using SmartStore.Services.Catalog;
using SmartStore.Services.Catalog.Extensions;
using SmartStore.Services.Catalog.Modelling;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using SmartStore.Services.Helpers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Services.Pdf;
using SmartStore.Services.Search;
using SmartStore.Services.Security;
using SmartStore.Services.Shipping;
using SmartStore.Services.Tax;
using SmartStore.Utilities;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Pdf;
using SmartStore.Web.Framework.Plugins;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.Web.Mvc;
using SmartStore.Services.Calc;
using SmartStore.Core.Domain.Wallet;
using SmartStore.Services.Wallet;
using SmartStore.Admin.Models.Catalog;
using SmartStore.Admin.Models.Customers;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单系统管理/钱包流水
    /// </summary>
    [AdminAuthorize]
    public class DeclarationWalletOverviewController : AdminControllerBase
    {
        private readonly IDeclarationCalcRuleService _calcruleService;
        private readonly DeclarationCalcRule _calcrule;
        private readonly IWithdrawalDetailService _detailrule;
        private readonly IWithdrawalTotalService _total;
        private readonly ICustomerService _CustomerService;
        public DeclarationWalletOverviewController(IDeclarationCalcRuleService calcruleService, ICustomerService customerService, IWithdrawalDetailService detailService, IWithdrawalTotalService totalService)
        {
            _calcruleService = calcruleService;
            _calcrule = _calcruleService.GetDeclarationCalcRule();
            _detailrule = detailService;
            _total = totalService;
            _CustomerService = customerService;
        }
        #region Public Methods
       
        // GET: DeclarationParamControl
        public ActionResult List()
        {
            //var detailList = _detailrule.Get();
            return View();
        }
        public ActionResult MainList()
        {
            //var detailList = _detailrule.Get();
            return View();
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Wallet(GridCommand command, DeclarationProductListModel model = null)
        {
            var gridModel = new GridModel<WithdrawalTotalModel>();
            var count = _total.GetAllCount();
                var detail = _total.GetAll((command.Page - 1) * command.PageSize, command.PageSize);
                var customer = _CustomerService.BuildAllTreeWithoutOrder();
                List<WithdrawalTotalModel> modelList = new List<WithdrawalTotalModel>();
                foreach (var item in detail)
                {
                    var name = customer.FirstOrDefault(x => x.Id == item.CustomerId) == null ? "" : customer.FirstOrDefault(x => x.Id == item.CustomerId).Username;
                    item.CustomerName = name;
                    modelList.Add(item.ToModel());
                }
                var products = new PagedList<WithdrawalTotalModel>(modelList.AsEnumerable(), command.Page - 1, command.PageSize, detail.Count());
                gridModel.Data = products;
                gridModel.Total = count;
            return new JsonResult
            {
                Data = gridModel
            };
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult WalletList(GridCommand command, DeclarationProductListModel model=null)
        {
            var gridModel = new GridModel<WithdrawalDetailModel>();
            {
                var count = _detailrule.GetCount();
                var detail = _detailrule.Get((command.Page - 1) * command.PageSize, command.PageSize);
                var customer = _CustomerService.BuildAllTreeWithoutOrder();
                List<WithdrawalDetailModel> modelList = new List<WithdrawalDetailModel>();
                foreach (var item in detail)
                {
                    var name = customer.FirstOrDefault(x => x.Id == item.Customer) == null ? "" : customer.FirstOrDefault(x => x.Id == item.Customer).Username;
                    item.CustomerName = name;
                    var WithdrawTypeStr = ConvertEnum(item);
                    var mod = item.ToModel();
                    mod.WithdrawTypeStr = WithdrawTypeStr;
                    if (item.Amount != 0M) { modelList.Add(mod); }
                }
               // modelList.Select(x => x.WithdrawTypeStr = ConvertEnum(x));
                var products = new PagedList<WithdrawalDetailModel>(modelList.AsEnumerable(), command.Page - 1, command.PageSize, detail.Count());
                gridModel.Data = products;
                gridModel.Total = count;
            }
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        public string ConvertEnum(WithdrawalDetail detail) 
        {
            string result = "";
            if (detail.isOut) { result = "提现"; }
            else
            {
                if (detail.WithdrawType == 1) { result = "直推佣金"; }
                else if (detail.WithdrawType == 2) { result = "业绩分红"; }
                else if (detail.WithdrawType == 3) { result = "商城分红"; }
                else if (detail.WithdrawType == 4) { result = "红包"; }
            }
            return result;
        }
        #endregion Public Methods
    }
}