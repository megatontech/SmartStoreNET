using SmartStore.Core.Domain.DataExchange;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmartStore.Services.DataExchange.Export
{
    public class DataExportPreviewResult
    {
        #region Public Constructors

        public DataExportPreviewResult()
        {
            Data = new List<dynamic>();
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// Preview data.
        /// </summary>
        public List<dynamic> Data { get; set; }

        /// <summary>
        /// Number of total records.
        /// </summary>
        public int TotalRecords { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public class DataExportResult
    {
        #region Public Constructors

        public DataExportResult()
        {
            Files = new List<ExportFileInfo>();
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// The path of the folder with the export files
        /// </summary>
        [XmlIgnore]
        public string FileFolder { get; set; }

        /// <summary>
        /// Files created by last export
        /// </summary>
        public List<ExportFileInfo> Files { get; set; }

        /// <summary>
        /// Last error
        /// </summary>
        [XmlIgnore]
        public string LastError { get; set; }

        /// <summary>
        /// Whether the export succeeded
        /// </summary>
        public bool Succeeded
        {
            get { return LastError.IsEmpty(); }
        }

        #endregion Public Properties



        #region Public Classes

        [Serializable]
        public class ExportFileInfo
        {
            #region Public Properties

            /// <summary>
            /// Name of file.
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// Short optional text that describes the content of the file.
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// The related entity type.
            /// </summary>
            public RelatedEntityType? RelatedType { get; set; }

            /// <summary>
            /// Store identifier, can be 0.
            /// </summary>
            public int StoreId { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}