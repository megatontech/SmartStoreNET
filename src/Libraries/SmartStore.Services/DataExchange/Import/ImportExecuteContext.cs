using SmartStore.Core.Domain.DataExchange;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Logging;
using System.Collections.Generic;
using System.Threading;

namespace SmartStore.Services.DataExchange.Import
{
    public class ImportExecuteContext
    {
        #region Private Fields

        private DataExchangeAbortion _abortion;

        private IDataTable _dataTable;

        private string _progressInfo;

        private ProgressValueSetter _progressValueSetter;

        private ImportDataSegmenter _segmenter;

        #endregion Private Fields

        #region Public Constructors

        public ImportExecuteContext(
            CancellationToken cancellation,
            ProgressValueSetter progressValueSetter,
            string progressInfo)
        {
            _progressValueSetter = progressValueSetter;
            _progressInfo = progressInfo;

            CancellationToken = cancellation;
            CustomProperties = new Dictionary<string, object>();
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// Indicates whether and how to abort the import
        /// </summary>
        public DataExchangeAbortion Abort
        {
            get
            {
                if (CancellationToken.IsCancellationRequested || IsMaxFailures)
                    return DataExchangeAbortion.Hard;

                return _abortion;
            }

            set
            {
                _abortion = value;
            }
        }

        /// <summary>
        /// Cancellation token
        /// </summary>
        public CancellationToken CancellationToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Mapping information between database and data source
        /// </summary>
        public ColumnMap ColumnMap
        {
            get;
            internal set;
        }

        /// <summary>
        /// Use this dictionary for any custom data required along the import
        /// </summary>
        public Dictionary<string, object> CustomProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Import settings
        /// </summary>
        public DataExchangeSettings DataExchangeSettings
        {
            get;
            internal set;
        }

        public ImportDataSegmenter DataSegmenter
        {
            get
            {
                if (_segmenter == null)
                {
                    if (this.DataTable == null || this.ColumnMap == null)
                    {
                        throw new SmartException("A DataTable and a ColumnMap must be specified before accessing the DataSegmenter property.");
                    }
                    _segmenter = new ImportDataSegmenter(DataTable, ColumnMap);
                }

                return _segmenter;
            }
        }

        /// <summary>
        /// The data source (CSV, Excel etc.)
        /// </summary>
        public IDataTable DataTable
        {
            get
            {
                return _dataTable;
            }

            internal set
            {
                _dataTable = value;
                _segmenter = null;
            }
        }

        /// <summary>
        /// Extra import configuration data
        /// </summary>
        public ImportExtraData ExtraData
        {
            get;
            internal set;
        }

        /// <summary>
        /// Infos about the import file.
        /// </summary>
        public ImportFile File
        {
            get;
            internal set;
        }

        /// <summary>
        /// The import folder
        /// </summary>
        public string ImportFolder
        {
            get;
            internal set;
        }

        public bool IsMaxFailures
        {
            get
            {
                return Result.Errors > 11;
            }
        }

        /// <summary>
        /// Name of key fields to identify existing records for updating
        /// </summary>
        public string[] KeyFieldNames
        {
            get;
            internal set;
        }

        /// <summary>
        /// All active languages
        /// </summary>
        public IList<Language> Languages
        {
            get;
            internal set;
        }

        /// <summary>
        /// To log information into the import log file
        /// </summary>
        public ILogger Log
        {
            get;
            internal set;
        }

        public DataImportRequest Request
        {
            get;
            internal set;
        }

        /// <summary>
        /// Result of the import
        /// </summary>
        public ImportResult Result
        {
            get;
            set;
        }

        /// <summary>
        /// Common Services
        /// </summary>
        public ICommonServices Services
        {
            get;
            internal set;
        }

        /// <summary>
        /// Whether to only update existing records
        /// </summary>
        public bool UpdateOnly
        {
            get;
            internal set;
        }

        #endregion Public Properties



        #region Public Methods

        /// <summary>
        /// Allows to set a progress message
        /// </summary>
        /// <param name="value">Progress value</param>
        /// /// <param name="maximum">Progress maximum</param>
        public void SetProgress(int value, int maximum)
        {
            try
            {
                _progressValueSetter?.Invoke(value, maximum, _progressInfo.FormatInvariant(value, maximum));
            }
            catch { }
        }

        /// <summary>
        /// Allows to set a message
        /// </summary>
        /// <param name="message">Message to display</param>
        public void SetProgress(string message)
        {
            try
            {
                if (message.HasValue())
                    _progressValueSetter?.Invoke(0, 0, message);
            }
            catch { }
        }

        #endregion Public Methods
    }
}