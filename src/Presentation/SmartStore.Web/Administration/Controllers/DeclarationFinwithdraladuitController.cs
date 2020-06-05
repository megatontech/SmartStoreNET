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

namespace SmartStore.Admin.Controllers
{
    /// <summary>
    /// 报单审核/财务提现审核
    /// </summary>
    public class DeclarationFinwithdraladuitController : AdminControllerBase
    {
        private IWithdrawalApplyService _withdrawalApplyService;

        public DeclarationFinwithdraladuitController(IWithdrawalApplyService withdrawalApplyService)
        {
            _withdrawalApplyService = withdrawalApplyService;
        }
        #region Public Methods

        // GET: DeclarationFinwithdraladuit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeclarationFinwithdraladuit/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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

            if (Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
            {
                var storeModels = _withdrawalApplyService.GetList()
                    .Select(x =>
                    {
                        var model = x.ToModel();
                        //PrepareStoreModel(model, x);
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