using SmartStore.Admin.Models.Stores;
using SmartStore.Core.Domain.Stores;
using SmartStore.Core.Plugins;
using SmartStore.Services.Configuration;
using SmartStore.Services.Directory;
using SmartStore.Services.Media;
using SmartStore.Services.Media.Storage;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public partial class StoreController : AdminControllerBase
    {
        #region Private Fields

        private readonly ISettingService _settingService;

        private readonly ICurrencyService _currencyService;
        private readonly Provider<IMediaStorageProvider> _storageProvider;
        private readonly IPictureService _pictureService;
        #endregion Private Fields

        #region Public Constructors

        public StoreController(ICurrencyService currencyService, IProviderManager providerManager, ISettingService settingService, IPictureService pictureService)
        {
            _currencyService = currencyService;
            _pictureService = pictureService;
            _storageProvider = providerManager.GetProvider<IMediaStorageProvider>(FileSystemMediaStorageProvider.SystemName);

        }

        #endregion Public Constructors



        #region Public Methods

        [HttpPost]
        public ActionResult UploadAvatar01()
        {
            var success = false;
            string avatarUrl = null;
            var uploadedFile = Request.Files["uploadedFile01-file"].ToPostedFileResult();
            if (uploadedFile != null && uploadedFile.FileName.HasValue())
            {
                avatarUrl = "~/Content/Images/01.jpg";
                string path = VirtualPathUtility.ToAbsolute(avatarUrl);
                _storageProvider.Value.Save(path, uploadedFile.Buffer);
                success =true;
            }
            return Json(new { success, avatarUrl });
        }
        [HttpPost]
        public ActionResult UploadAvatar02()
        {
            var success = false;
            string avatarUrl = null;
            var uploadedFile = Request.Files["uploadedFile02-file"].ToPostedFileResult();
            if (uploadedFile != null && uploadedFile.FileName.HasValue())
            {
                avatarUrl = "~/Content/Images/02.jpg";
                string path = VirtualPathUtility.ToAbsolute(avatarUrl);
                _storageProvider.Value.Save(path, uploadedFile.Buffer);
                success = true;
            }
            return Json(new { success, avatarUrl });
        }
        [HttpPost]
        public ActionResult UploadAvatar03()
        {
            var success = false;
            string avatarUrl = null;
            var uploadedFile = Request.Files["uploadedFile03-file"].ToPostedFileResult();
            if (uploadedFile != null && uploadedFile.FileName.HasValue())
            {
                avatarUrl = "~/Content/Images/03.jpg";
                string path = VirtualPathUtility.ToAbsolute(avatarUrl);
                _storageProvider.Value.Save(path, uploadedFile.Buffer);
                success = true;
            }
            return Json(new { success, avatarUrl });
        }
        [HttpPost]
        public ActionResult UploadAvatar04()
        {
            var success = false;
            string avatarUrl = null;
            var uploadedFile = Request.Files["uploadedFile04-file"].ToPostedFileResult();
            if (uploadedFile != null && uploadedFile.FileName.HasValue())
            {
                avatarUrl = "~/Content/Images/04.jpg";
                string path = VirtualPathUtility.ToAbsolute(avatarUrl);
                _storageProvider.Value.Save(path, uploadedFile.Buffer);
                success = true;
            }
            return Json(new { success, avatarUrl });
        }
        [HttpPost]
        public ActionResult UploadAvatar05()
        {
            var success = false;
            string avatarUrl = null;
            var uploadedFile = Request.Files["uploadedFile05-file"].ToPostedFileResult();
            if (uploadedFile != null && uploadedFile.FileName.HasValue())
            {
                avatarUrl = "~/Content/Images/05.jpg";
                string path = VirtualPathUtility.ToAbsolute(avatarUrl);
                _storageProvider.Value.Save(path, uploadedFile.Buffer);
                success = true;
            }
            return Json(new { success, avatarUrl });
        }

        public ActionResult AllStores(string label, int selectedId = 0)
        {
            var stores = Services.StoreService.GetAllStores();

            if (label.HasValue())
            {
                stores.Insert(0, new Store { Name = label, Id = 0 });
            }

            var list =
                from m in stores
                select new
                {
                    id = m.Id.ToString(),
                    text = m.Name,
                    selected = m.Id == selectedId
                };

            return new JsonResult { Data = list.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Create()
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            var model = new StoreModel();
            PrepareStoreModel(model, null);

            return View(model);
        }
        public ActionResult CreateImage()
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            var model = new StoreModel();
            PrepareStoreModel(model, null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(StoreModel model, bool continueEditing)
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var store = model.ToEntity();
                MediaHelper.UpdatePictureTransientStateFor(store, s => s.LogoPictureId);
                //ensure we have "/" at the end
                store.Url = store.Url.EnsureEndsWith("/");
                Services.StoreService.InsertStore(store);

                NotifySuccess(T("Admin.Configuration.Stores.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = store.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareStoreModel(model, null);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            var store = Services.StoreService.GetStoreById(id);
            if (store == null)
                return RedirectToAction("List");

            try
            {
                Services.StoreService.DeleteStore(store);

                //when we delete a store we should also ensure that all "per store" settings will also be deleted
                var settingsToDelete = Services.Settings
                    .GetAllSettings()
                    .Where(s => s.StoreId == id)
                    .ToList();

                settingsToDelete.ForEach(x => Services.Settings.DeleteSetting(x));

                //when we had two stores and now have only one store, we also should delete all "per store" settings
                var allStores = Services.StoreService.GetAllStores();

                if (allStores.Count == 1)
                {
                    settingsToDelete = Services.Settings
                        .GetAllSettings()
                        .Where(s => s.StoreId == allStores[0].Id)
                        .ToList();

                    settingsToDelete.ForEach(x => Services.Settings.DeleteSetting(x));
                }

                NotifySuccess(T("Admin.Configuration.Stores.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                NotifyError(exc);
            }
            return RedirectToAction("Edit", new { id = store.Id });
        }

        public ActionResult Edit(int id)
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            var store = Services.StoreService.GetStoreById(id);
            if (store == null)
                return RedirectToAction("List");

            var model = store.ToModel();
            PrepareStoreModel(model, store);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(StoreModel model, bool continueEditing)
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            var store = Services.StoreService.GetStoreById(model.Id);
            if (store == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                store = model.ToEntity(store);

                MediaHelper.UpdatePictureTransientStateFor(store, s => s.LogoPictureId);

                //ensure we have "/" at the end
                store.Url = store.Url.EnsureEndsWith("/");
                Services.StoreService.UpdateStore(store);

                NotifySuccess(T("Admin.Configuration.Stores.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = store.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            PrepareStoreModel(model, store);
            return View(model);
        }

        public ActionResult List()
        {
            if (!Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            return View();
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var gridModel = new GridModel<StoreModel>();

            if (Services.Permissions.Authorize(StandardPermissionProvider.ManageStores))
            {
                var storeModels = Services.StoreService.GetAllStores()
                    .Select(x =>
                    {
                        var model = x.ToModel();

                        PrepareStoreModel(model, x);

                        model.Hosts = model.Hosts.EmptyNull().Replace(",", "<br />");

                        return model;
                    })
                    .ToList();

                gridModel.Data = storeModels;
                gridModel.Total = storeModels.Count();
            }
            else
            {
                gridModel.Data = Enumerable.Empty<StoreModel>();

                NotifyAccessDenied();
            }

            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion Public Methods



        #region Private Methods

        private void PrepareStoreModel(StoreModel model, Store store)
        {
            model.AvailableCurrencies = _currencyService.GetAllCurrencies(false, store == null ? 0 : store.Id)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                .ToList();
        }

        #endregion Private Methods
    }
}