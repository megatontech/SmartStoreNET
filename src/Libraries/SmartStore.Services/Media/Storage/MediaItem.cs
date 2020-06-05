using SmartStore.Core.Domain.Media;

namespace SmartStore.Services.Media.Storage
{
    public partial class MediaItem
    {
        #region Public Properties

        /// <summary>
        /// Entity of the media storage item
        /// </summary>
        public IHasMedia Entity { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Mime type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Storage path
        /// </summary>
        public string Path { get; set; }

        #endregion Public Properties
    }
}