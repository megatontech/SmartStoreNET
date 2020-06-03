using SmartStore.Admin.Models.Customers;
using SmartStore.Admin.Models.Security;
using SmartStore.Core;
using SmartStore.Core.Logging;
using SmartStore.Services.Customers;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class SecurityController : AdminControllerBase
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Constructors

        public SecurityController(IWorkContext workContext,
            IPermissionService permissionService,
            ICustomerService customerService)
        {
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._customerService = customerService;
        }

        #endregion Constructors

        #region Methods

        public ActionResult AccessDenied(string pageUrl)
        {
            var currentCustomer = _workContext.CurrentCustomer;

            if (currentCustomer == null || currentCustomer.IsGuest())
            {
                Logger.Info(T("Admin.System.Warnings.AccessDeniedToAnonymousRequest", pageUrl.NaIfEmpty()));
                return View();
            }

            Logger.Info(T("Admin.System.Warnings.AccessDeniedToUser",
                currentCustomer.Email.NaIfEmpty(), currentCustomer.Email.NaIfEmpty(), pageUrl.NaIfEmpty()));

            return View();
        }

        // Ajax.
        public ActionResult AllAccessPermissions(string selected)
        {
            var permissions = _permissionService.GetAllPermissionRecords();
            var selectedArr = selected.SplitSafe(",");

            var data = permissions
                .Select(x => new
                {
                    id = x.SystemName,
                    text = x.Name,  // TODO: localization.
                    selected = selectedArr.Contains(x.SystemName)
                })
                .ToList();

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Permissions()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var model = new PermissionMappingModel();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);

            foreach (var pr in permissionRecords)
            {
                model.AvailablePermissions.Add(new PermissionRecordModel
                {
                    Name = pr.Name,
                    SystemName = pr.SystemName,
                    Category = pr.Category
                });
            }

            foreach (var cr in customerRoles)
            {
                model.AvailableCustomerRoles.Add(new CustomerRoleModel
                {
                    Id = cr.Id,
                    Name = cr.Name
                });
            }

            foreach (var pr in permissionRecords)
            {
                foreach (var cr in customerRoles)
                {
                    var allowed = pr.CustomerRoles.Any(x => x.Id == cr.Id);

                    if (!model.Allowed.ContainsKey(pr.SystemName))
                        model.Allowed[pr.SystemName] = new Dictionary<int, bool>();

                    model.Allowed[pr.SystemName][cr.Id] = allowed;
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("Permissions")]
        public ActionResult PermissionsSave(FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);

            foreach (var cr in customerRoles)
            {
                var restrictedSystemNames = form["allow_" + cr.Id.ToString()].SplitSafe(",").ToList();

                foreach (var permission in permissionRecords)
                {
                    bool allow = restrictedSystemNames.Contains(permission.SystemName);

                    if (allow)
                    {
                        if (!permission.CustomerRoles.Any(x => x.Id == cr.Id))
                        {
                            permission.CustomerRoles.Add(cr);
                            _permissionService.UpdatePermissionRecord(permission);
                        }
                    }
                    else
                    {
                        if (permission.CustomerRoles.Any(x => x.Id == cr.Id))
                        {
                            permission.CustomerRoles.Remove(cr);
                            _permissionService.UpdatePermissionRecord(permission);
                        }
                    }
                }
            }

            NotifySuccess(T("Admin.Configuration.ACL.Updated"));

            return RedirectToAction("Permissions");
        }

        #endregion Methods
    }
}