using SmartStore.Admin.Models.Wallet;
using SmartStore.Core.Domain.Wallet;
using SmartStore.Web.Framework.Controllers;
using System.Web.Mvc;
using SmartStore.Admin.Models.Stores;
using SmartStore.Core.Domain.Stores;
using SmartStore.Services.Directory;
using SmartStore.Services.Media;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using System;
using System.Linq;
using Telerik.Web.Mvc;
using SmartStore.Services.Wallet;
using SmartStore.Services.Customers;
using SmartStore.Core;

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单审核/财务提现审核
    /// </summary>
    public class DeclarationFinwithdraladuitController : AdminControllerBase
    {
        private IWithdrawalApplyService _withdrawalApplyService;
        private ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        public DeclarationFinwithdraladuitController(ICustomerService customerService, IWithdrawalApplyService withdrawalApplyService, IWorkContext workContext)
        {
            _withdrawalApplyService = withdrawalApplyService;
            _customerService = customerService; _workContext = workContext;
        }
        #region Public Methods

        // GET: DeclarationFinwithdraladuit/Create
        public ActionResult Audit(int id)
        {
            var model = _withdrawalApplyService.GetByTableID(id);
            return View(model);
        }

        // POST: DeclarationFinwithdraladuit/Create
        [HttpPost]
        public ActionResult Audit(int id,FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var model = _withdrawalApplyService.GetByTableID(id);
               var customer = _customerService.GetCustomerById(model.Customer);
               var apply = _workContext.CurrentCustomer;
                model.WithdrawStatus = WithdrawalApplyStatus.Complete;
                _withdrawalApplyService.WithdrawalApplyAudit(model, customer, apply);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeclarationFinwithdraladuit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeclarationFinwithdraladuit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeclarationFinwithdraladuit/Details/5
        public ActionResult Details(int id)
        {
            var storeModels = _withdrawalApplyService.GetByID(id);
            return View(storeModels);
        }

        // GET: DeclarationFinwithdraladuit/Edit/5
        public ActionResult Edit(int id)
        {
            var storeModels = _withdrawalApplyService.GetByID(id);
            return View(storeModels);
        }

        // POST: DeclarationFinwithdraladuit/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var storeModels = _withdrawalApplyService.GetByID(id);
                _withdrawalApplyService.Update(storeModels);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var gridModel = new GridModel<DeclarationFinwithdraladuitModel>();
            var customer = _customerService.BuildAllTreeWithoutOrder();
            if (Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
            {
                var storeModels = _withdrawalApplyService.GetList()
                    .Select(x =>
                    {
                        var model = x.ToModel();
                        //PrepareStoreModel(model, x);
                        model.WithdrawTypeStr = "转账";
                        var custom = customer.FirstOrDefault(y=>y.Id==x.Customer);
                        model.CustomerLoginName = custom.Username;
                        model.CustomerName = custom.FullName;
                        model.WithdrawStatusStr = model.WithdrawStatus==10?"申请中":"已完成";
                        return model;
                    })
                    .ToList();

                gridModel.Data = storeModels;
                gridModel.Total = storeModels.Count();
            }
            else
            {
                gridModel.Data = Enumerable.Empty<DeclarationFinwithdraladuitModel>();
                NotifyAccessDenied();
            }

            return new JsonResult
            {
                Data = gridModel
            };
        }
        // GET: DeclarationFinwithdraladuit
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            return View();
        }

        #endregion Public Methods
        private void PrepareStoreModel(DeclarationFinwithdraladuitModel model, WithdrawalApply apply)
        {
            AutoMapper.Mapper.Instance.Map(apply, model);
        }
    }
}