﻿using SmartStore.Admin.Models.Customers;
using SmartStore.Core.Domain.Tax;
using SmartStore.Core.Logging;
using SmartStore.Services.Customers;
using SmartStore.Services.Localization;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class CustomerRoleController : AdminControllerBase
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly TaxSettings _taxSettings;

        #endregion Fields

        #region Constructors

        public CustomerRoleController(ICustomerService customerService,
            ILocalizationService localizationService, ICustomerActivityService customerActivityService,
            IPermissionService permissionService, TaxSettings taxSettings)
        {
            this._customerService = customerService;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._taxSettings = taxSettings;
        }

        #endregion Constructors

        #region Utilities

        [NonAction]
        protected List<SelectListItem> GetTaxDisplayTypesList(CustomerRoleModel model)
        {
            var list = new List<SelectListItem>();

            if (model.TaxDisplayType.HasValue)
            {
                list.Insert(0, new SelectListItem()
                {
                    Text = _localizationService.GetResource("Enums.Smartstore.Core.Domain.Tax.TaxDisplayType.IncludingTax"),
                    Value = "0",
                    Selected = (TaxDisplayType)model.TaxDisplayType.Value == TaxDisplayType.IncludingTax
                });
                list.Insert(1, new SelectListItem()
                {
                    Text = _localizationService.GetResource("Enums.Smartstore.Core.Domain.Tax.TaxDisplayType.ExcludingTax"),
                    Value = "10",
                    Selected = (TaxDisplayType)model.TaxDisplayType.Value == TaxDisplayType.ExcludingTax
                });
            }
            else
            {
                list.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Enums.Smartstore.Core.Domain.Tax.TaxDisplayType.IncludingTax"), Value = "0" });
                list.Insert(1, new SelectListItem() { Text = _localizationService.GetResource("Enums.Smartstore.Core.Domain.Tax.TaxDisplayType.ExcludingTax"), Value = "10" });
            }

            return list;
        }

        #endregion Utilities

        #region Customer roles

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var model = new CustomerRoleModel();
            model.TaxDisplayTypes = GetTaxDisplayTypesList(model);
            //default values
            model.Active = true;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(CustomerRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var customerRole = model.ToEntity();
                _customerService.InsertCustomerRole(customerRole);

                //activity log
                _customerActivityService.InsertActivity("AddNewCustomerRole", _localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name);

                NotifySuccess(_localizationService.GetResource("Admin.Customers.CustomerRoles.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                _customerService.DeleteCustomerRole(customerRole);

                //activity log
                _customerActivityService.InsertActivity("DeleteCustomerRole", _localizationService.GetResource("ActivityLog.DeleteCustomerRole"), customerRole.Name);

                NotifySuccess(_localizationService.GetResource("Admin.Customers.CustomerRoles.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                NotifyError(exc.Message);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = customerRole.ToModel();
            model.TaxDisplayTypes = GetTaxDisplayTypesList(model);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(CustomerRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var customerRole = _customerService.GetCustomerRoleById(model.Id);
            if (customerRole == null)
                // No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (customerRole.IsSystemRole && !model.Active)
                        throw new SmartException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Active.CantEditSystem"));

                    if (customerRole.IsSystemRole && !customerRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new SmartException(_localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.SystemName.CantEditSystem"));

                    customerRole = model.ToEntity(customerRole);
                    _customerService.UpdateCustomerRole(customerRole);

                    _customerActivityService.InsertActivity("EditCustomerRole", _localizationService.GetResource("ActivityLog.EditCustomerRole"), customerRole.Name);

                    NotifySuccess(_localizationService.GetResource("Admin.Customers.CustomerRoles.Updated"));
                    return continueEditing ? RedirectToAction("Edit", customerRole.Id) : RedirectToAction("List");
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                NotifyError(exc);
                return RedirectToAction("Edit", new { id = customerRole.Id });
            }
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
                return AccessDeniedView();

            var customerRoles = _customerService.GetAllCustomerRoles(true);
            var gridModel = new GridModel<CustomerRoleModel>
            {
                Data = customerRoles.Select(x => x.ToModel()),
                Total = customerRoles.Count()
            };
            return View(gridModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var model = new GridModel<CustomerRoleModel>();

            if (_permissionService.Authorize(StandardPermissionProvider.ManageCustomerRoles))
            {
                var customerRoles = _customerService.GetAllCustomerRoles(true);

                model.Data = customerRoles.Select(x => x.ToModel());
                model.Total = customerRoles.Count();
            }
            else
            {
                model.Data = Enumerable.Empty<CustomerRoleModel>();

                NotifyAccessDenied();
            }

            return new JsonResult
            {
                Data = model
            };
        }

        #endregion Customer roles
    }
}