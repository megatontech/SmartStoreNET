﻿using SmartStore.Core.Domain.Media;
using SmartStore.Core.IO;
using SmartStore.Core.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStore.Services.Media
{
    public class ImageCache : IImageCache
    {
        #region Public Fields

        public const string IdFormatString = "0000000";

        #endregion Public Fields

        #region Internal Fields

        internal const int MaxDirLength = 4;

        #endregion Internal Fields



        #region Private Fields

        private readonly IMediaFileSystem _fileSystem;

        private readonly IImageProcessor _imageProcessor;

        private readonly MediaSettings _mediaSettings;

        private readonly string _thumbsRootDir;

        #endregion Private Fields

        #region Public Constructors

        public ImageCache(MediaSettings mediaSettings, IMediaFileSystem fileSystem, IImageProcessor imageProcessor)
        {
            _mediaSettings = mediaSettings;
            _fileSystem = fileSystem;
            _imageProcessor = imageProcessor;

            _thumbsRootDir = "Thumbs/";

            Logger = NullLogger.Instance;
        }

        #endregion Public Constructors



        #region Public Properties

        public ILogger Logger
        {
            get;
            set;
        }

        #endregion Public Properties



        #region Public Methods

        public void CacheStatistics(out long fileCount, out long totalSize)
        {
            fileCount = 0;
            totalSize = 0;

            if (!_fileSystem.FolderExists(_thumbsRootDir))
            {
                return;
            }

            fileCount = _fileSystem.SearchFiles(_thumbsRootDir, "*.*").Count();
            totalSize = _fileSystem.ListFolders(_thumbsRootDir).Sum(x => x.Size);
        }

        public virtual void Clear()
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    foreach (var file in _fileSystem.ListFiles(_thumbsRootDir))
                    {
                        if (!file.Name.IsCaseInsensitiveEqual("placeholder") && !file.Name.IsCaseInsensitiveEqual("placeholder.txt"))
                        {
                            _fileSystem.DeleteFile(file.Path);
                        }
                    }
                    foreach (var dir in _fileSystem.ListFolders(_thumbsRootDir))
                    {
                        _fileSystem.DeleteFolder(dir.Path);
                    }

                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        public virtual void Delete(Picture picture)
        {
            var filter = string.Format("{0}*.*", picture.Id.ToString(IdFormatString));

            var files = _fileSystem.SearchFiles(_thumbsRootDir, filter);
            foreach (var file in files)
            {
                _fileSystem.DeleteFile(file);
            }
        }

        public virtual void Delete(IFile file)
        {
            // TODO: (mc) this could lead to more thumbs getting deleted as desired. But who cares? :-)
            var filter = string.Format("{0}*.*", file.Title);

            var files = _fileSystem.SearchFiles(BuildPath(file.Directory), filter);
            foreach (var f in files)
            {
                _fileSystem.DeleteFile(f);
            }
        }

        public virtual CachedImageResult Get(int? pictureId, string seoFileName, string extension, ProcessImageQuery query = null)
        {
            Guard.NotEmpty(extension, nameof(extension));

            extension = query?.GetResultExtension() ?? extension.TrimStart('.').ToLower();
            var imagePath = GetCachedImagePath(pictureId, seoFileName, extension, query);

            var file = _fileSystem.GetFile(BuildPath(imagePath));

            var result = new CachedImageResult(file)
            {
                Path = imagePath,
                Extension = extension,
                IsRemote = _fileSystem.IsCloudStorage
            };

            if (file.Exists && file.Size <= 0)
            {
                result.Exists = false;
            }

            return result;
        }

        public virtual CachedImageResult Get(IFile file, ProcessImageQuery query)
        {
            Guard.NotNull(file, nameof(file));
            Guard.NotNull(query, nameof(query));

            var imagePath = GetCachedImagePath(file, query);
            var thumbFile = _fileSystem.GetFile(BuildPath(imagePath));

            var result = new CachedImageResult(thumbFile)
            {
                Path = imagePath,
                Extension = file.Extension.TrimStart('.'),
                IsRemote = _fileSystem.IsCloudStorage
            };

            if (file.Exists && file.Size <= 0)
            {
                result.Exists = false;
            }

            return result;
        }

        public virtual string GetPublicUrl(string imagePath)
        {
            if (imagePath.IsEmpty())
                return null;

            return _fileSystem.GetPublicUrl(BuildPath(imagePath), true).EmptyNull();
        }

        public virtual Stream Open(CachedImageResult cachedImage)
        {
            Guard.NotNull(cachedImage, nameof(cachedImage));

            return _fileSystem.GetFile(BuildPath(cachedImage.Path)).OpenRead();
        }

        public void Put(CachedImageResult cachedImage, byte[] buffer)
        {
            if (PreparePut(cachedImage, buffer))
            {
                var path = BuildPath(cachedImage.Path);

                _fileSystem.WriteAllBytes(path, buffer);

                cachedImage.Exists = true;
                cachedImage.File = _fileSystem.GetFile(path);
            }
        }

        public async Task PutAsync(CachedImageResult cachedImage, byte[] buffer)
        {
            if (PreparePut(cachedImage, buffer))
            {
                var path = BuildPath(cachedImage.Path);

                // save file
                await _fileSystem.WriteAllBytesAsync(path, buffer);

                // Refresh info
                cachedImage.Exists = true;
                cachedImage.File = _fileSystem.GetFile(path);
            }
        }

        public virtual void RefreshInfo(CachedImageResult cachedImage)
        {
            Guard.NotNull(cachedImage, nameof(cachedImage));

            var file = _fileSystem.GetFile(cachedImage.File.Path);
            cachedImage.File = file;
            cachedImage.Exists = file.Exists && file.Size > 0;
        }

        #endregion Public Methods



        #region Private Methods

        private string BuildPath(string imagePath)
        {
            if (imagePath.IsEmpty())
                return null;

            return String.Concat(_thumbsRootDir, imagePath);
        }

        /// <summary>
        /// Returns the file name with the subfolder (when multidirs are enabled)
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="seoFileName">File name without extension</param>
        /// <param name="extension">Dot-less file extension</param>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetCachedImagePath(int? pictureId, string seoFileName, string extension, ProcessImageQuery query = null)
        {
            string imageFileName = null;

            string firstPart = "";
            if (pictureId.GetValueOrDefault() > 0)
            {
                firstPart = pictureId.Value.ToString(IdFormatString) + (seoFileName.IsEmpty() ? "" : "-");
            }

            if (firstPart.IsEmpty() && seoFileName.IsEmpty())
            {
                // files without name? No way!
                return null;
            }

            seoFileName = seoFileName.EmptyNull();

            if (query == null || !query.NeedsProcessing())
            {
                imageFileName = String.Concat(firstPart, seoFileName);
            }
            else
            {
                imageFileName = String.Concat(firstPart, seoFileName, query.CreateHash());
            }

            if (_mediaSettings.MultipleThumbDirectories && imageFileName != null && imageFileName.Length > MaxDirLength)
            {
                // Get the first four letters of the file name
                var subDirectoryName = imageFileName.Substring(0, MaxDirLength);
                imageFileName = String.Concat(subDirectoryName, "/", imageFileName);
            }

            return String.Concat(imageFileName, ".", extension);
        }

        /// <summary>
        /// Returns the images thumb path as is plus query (required for uploaded images)
        /// </summary>
        /// <param name="file">Image file to get thumbnail for</param>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetCachedImagePath(IFile file, ProcessImageQuery query)
        {
            if (!_imageProcessor.IsSupportedImage(file.Name))
            {
                throw new InvalidOperationException("Thumbnails for '{0}' files are not supported".FormatInvariant(file.Extension));
            }

            // TODO: (mc) prevent creating thumbs for thumbs AND check equality of source and target

            var imageFileName = String.Concat(file.Title, query.CreateHash());
            var extension = (query.GetResultExtension() ?? file.Extension).EnsureStartsWith(".").ToLower();
            var path = _fileSystem.Combine(file.Directory, imageFileName + extension);

            return path.TrimStart('/', '\\');
        }

        private bool PreparePut(CachedImageResult cachedImage, byte[] buffer)
        {
            Guard.NotNull(cachedImage, nameof(cachedImage));

            if (buffer == null || buffer.Length == 0)
            {
                return false;
            }

            if (cachedImage.Exists)
            {
                _fileSystem.DeleteFile(BuildPath(cachedImage.Path));
            }

            // create folder if needed
            string imageDir = System.IO.Path.GetDirectoryName(cachedImage.Path);
            if (imageDir.HasValue())
            {
                _fileSystem.TryCreateFolder(BuildPath(imageDir));
            }

            return true;
        }

        #endregion Private Methods
    }
}