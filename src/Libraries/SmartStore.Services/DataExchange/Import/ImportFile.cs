using SmartStore.Core.Domain.DataExchange;
using System;
using System.Linq;

namespace SmartStore.Services.DataExchange.Import
{
    public partial class ImportFile
    {
        #region Public Constructors

        public ImportFile(string path)
        {
            Guard.NotEmpty(path, nameof(path));

            Path = path;

            var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (fileName.HasValue())
            {
                foreach (RelatedEntityType type in Enum.GetValues(typeof(RelatedEntityType)))
                {
                    if (fileName.EndsWith(type.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        RelatedType = type;
                    }
                }
            }
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// File extension of <see cref="Path"/>.
        /// </summary>
        public string Extension => System.IO.Path.GetExtension(Path);

        /// <summary>
        /// File label text.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// File name of <see cref="Path"/>.
        /// </summary>
        public string Name => System.IO.Path.GetFileName(Path);

        /// <summary>
        /// Path of the import file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Related entity type.
        /// </summary>
        public RelatedEntityType? RelatedType { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// Indicates whether the file has an CSV file extension.
        /// </summary>
        internal bool IsCsv
        {
            get
            {
                return (new string[] { ".csv", ".txt", ".tab" }).Contains(Extension, StringComparer.OrdinalIgnoreCase);
            }
        }

        #endregion Internal Properties
    }
}