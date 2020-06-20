using SmartStore.Core.Domain.Media;
using SmartStore.Data.Utilities;
using SmartStore.Services.Media;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class PictureController : AdminControllerBase
    {
        #region Private Fields

        private readonly MediaSettings _mediaSettings;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;

        #endregion Private Fields

        #region Public Constructors

        public PictureController(
            IPictureService pictureService,
            IPermissionService permissionService,
            MediaSettings mediaSettings)
        {
            _pictureService = pictureService;
            _permissionService = permissionService;
            _mediaSettings = mediaSettings;
        }

        #endregion Public Constructors



        #region Public Methods

        [HttpPost]
        public ActionResult AsyncUpload(bool isTransient = false, bool validate = true)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = T("Admin.AccessDenied.Description") });

            var postedFile = Request.ToPostedFileResult();
            if (postedFile == null)
            {
                return Json(new { success = false });
            }

            var picture = _pictureService.InsertPicture(postedFile.Buffer, postedFile.ContentType, null, true, isTransient, validate);

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetUrl(picture, _mediaSettings.ProductThumbPictureSize, host: "")
            });
        }

        public ActionResult MoveFsMedia()
        {
            var count = DataMigrator.MoveFsMedia(Services.DbContext);
            return Content("Moved and reorganized {0} media files.".FormatInvariant(count));
        }

        #endregion Public Methods
    }
}