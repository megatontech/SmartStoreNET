using SmartStore.Core.Domain;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;
using System.Collections.Generic;
using System.IO;

namespace SmartStore.Services.DataExchange.Export.Deployment
{
    public interface IFilePublisher
    {
        #region Public Methods

        void Publish(ExportDeploymentContext context, ExportDeployment deployment);

        #endregion Public Methods
    }

    public class ExportDeploymentContext
    {
        #region Public Properties

        public bool CreateZipArchive { get; set; }

        public string FolderContent { get; set; }

        public ILogger Log { get; set; }

        public DataDeploymentResult Result { get; set; }

        public Localizer T { get; set; }

        public string ZipPath { get; set; }

        #endregion Public Properties



        #region Public Methods

        public IEnumerable<string> GetDeploymentFiles()
        {
            if (!CreateZipArchive)
            {
                return System.IO.Directory.EnumerateFiles(FolderContent, "*", SearchOption.AllDirectories);
            }

            if (File.Exists(ZipPath))
            {
                return new string[] { ZipPath };
            }

            return new string[0];
        }

        #endregion Public Methods
    }
}