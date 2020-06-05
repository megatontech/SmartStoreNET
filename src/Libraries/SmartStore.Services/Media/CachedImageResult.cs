using SmartStore.Core.IO;
using System;

namespace SmartStore.Services.Media
{
    /// <summary>
    /// Contains information about a cached image
    /// </summary>
    /// <remarks>
    /// An instance of this object is always returned, even when
    /// the requested image does not physically exists in the storage.
    /// </remarks>
    public class CachedImageResult
    {
        #region Private Fields

        private bool? _exists;

        private string _mimeType;

        #endregion Private Fields

        #region Public Constructors

        public CachedImageResult(IFile file)
        {
            Guard.NotNull(file, nameof(file));

            File = file;
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// <c>true</c> when the image exists in the cache, <c>false</c> otherwise.
        /// </summary>
        public bool Exists
        {
            get
            {
                return _exists ?? (_exists = File.Exists).Value;
            }

            // For internal use
            set
            {
                _exists = value;
            }
        }

        /// <summary>
        /// The file extension (without 'dot')
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The abstracted file object
        /// </summary>
        public IFile File { get; internal set; }

        /// <summary>
        /// The name of the file (without path)
        /// </summary>
        public string FileName
        {
            get { return System.IO.Path.GetFileName(this.Path); }
        }

        public long FileSize
        {
            get { return !Exists ? 0 : File.Size; }
        }

        /// <summary>
        /// Checks whether the file is remote (outside the application's physical root)
        /// </summary>
        public bool IsRemote { get; set; }

        /// <summary>
        /// The last modified date or <c>null</c> if the file does not exist
        /// </summary>
        public DateTime? LastModifiedUtc
        {
            get { return Exists ? File.LastUpdated : (DateTime?)null; }
        }

        /// <summary>
        /// The filemime type
        /// </summary>
        public string MimeType
        {
            get => _mimeType ?? (_mimeType = MimeTypes.MapNameToMimeType(FileName));
        }

        /// <summary>
        /// The path relative to the cache root folder
        /// </summary>
        public string Path { get; set; }

        #endregion Public Properties
    }
}