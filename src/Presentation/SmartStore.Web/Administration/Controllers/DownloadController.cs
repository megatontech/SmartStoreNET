using SmartStore.Core.Domain.Media;
using SmartStore.Services.Media;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using System;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class DownloadController : AdminControllerBase
    {
        #region Private Fields

        private const string DOWNLOAD_TEMPLATE = "~/Administration/Views/Shared/EditorTemplates/Download.cshtml";

        private readonly IDownloadService _downloadService;

        #endregion Private Fields

        #region Public Constructors

        public DownloadController(IDownloadService downloadService)
        {
            this._downloadService = downloadService;
        }

        #endregion Public Constructors



        #region Public Methods

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddChangelog(int downloadId, string changelogText)
        {
            var success = false;

            var download = _downloadService.GetDownloadById(downloadId);
            if (download != null)
            {
                download.Changelog = changelogText;
                _downloadService.UpdateDownload(download);
                success = true;
            }

            return Json(new
            {
                success = success
            });
        }

        [HttpPost]
        public ActionResult AsyncUpload(bool minimalMode = false, string fieldName = null, int entityId = 0, string entityName = "")
        {
            var postedFile = Request.ToPostedFileResult();
            if (postedFile == null)
            {
                throw new ArgumentException(T("Common.NoFileUploaded"));
            }

            var download = new Download
            {
                EntityId = entityId,
                EntityName = entityName,
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = false,
                DownloadUrl = "",
                ContentType = postedFile.ContentType,
                // we store filename without extension for downloads
                Filename = postedFile.FileTitle,
                Extension = postedFile.FileExtension,
                IsNew = true,
                IsTransient = true,
                UpdatedOnUtc = DateTime.Now
            };

            _downloadService.InsertDownload(download, postedFile.Buffer);

            return Json(new
            {
                success = true,
                downloadId = download.Id,
                html = this.RenderPartialViewToString(DOWNLOAD_TEMPLATE, download.Id, new { minimalMode = minimalMode, fieldName = fieldName, entityId = entityId, entityName = entityName })
            });
        }

        [HttpPost]
        public ActionResult DeleteDownload(bool minimalMode = false, string fieldName = null)
        {
            // We don't actually delete here. We just return the editor in it's init state
            // so the download entity can be set to transient state and deleted later by a scheduled task.
            return Json(new
            {
                success = true,
                html = this.RenderPartialViewToString(DOWNLOAD_TEMPLATE, null, new { minimalMode = minimalMode, fieldName = fieldName }),
            });
        }

        public ActionResult DownloadFile(int downloadId)
        {
            var download = _downloadService.GetDownloadById(downloadId);
            if (download == null)
                return Content(T("Common.Download.NoDataAvailable"));

            if (download.UseDownloadUrl)
            {
                return new RedirectResult(download.DownloadUrl);
            }
            else
            {
                //use stored data
                var data = _downloadService.LoadDownloadBinary(download);

                if (data == null || data.LongLength == 0)
                    return Content(T("Common.Download.NoDataAvailable"));

                var fileName = (download.Filename.HasValue() ? download.Filename : downloadId.ToString());
                var contentType = (download.ContentType.HasValue() ? download.ContentType : "application/octet-stream");

                return new FileContentResult(data, contentType)
                {
                    FileDownloadName = fileName + download.Extension
                };
            }
        }

        [HttpPost]
        public ActionResult GetChangelogText(int downloadId)
        {
            var success = false;
            var changeLogText = String.Empty;

            var download = _downloadService.GetDownloadById(downloadId);
            if (download != null)
            {
                changeLogText = download.Changelog;
                success = true;
            }

            return Json(new
            {
                success = success,
                changelog = changeLogText
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveDownloadUrl(string downloadUrl, bool minimalMode = false, string fieldName = null, int entityId = 0, string entityName = "")
        {
            var download = new Download
            {
                EntityId = entityId,
                EntityName = entityName,
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = true,
                DownloadUrl = downloadUrl,
                IsNew = true,
                IsTransient = true,
                UpdatedOnUtc = DateTime.Now
            };

            _downloadService.InsertDownload(download, null);

            return Json(new
            {
                success = true,
                downloadId = download.Id,
                html = this.RenderPartialViewToString(DOWNLOAD_TEMPLATE, download.Id, new { minimalMode = minimalMode, fieldName = fieldName })
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion Public Methods
    }
}