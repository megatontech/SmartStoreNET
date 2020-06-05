using SmartStore.Core.Logging;
using System.Collections.Generic;
using System.Threading;

namespace SmartStore.Services.DataExchange.Import.Internal
{
    internal class DataImporterContext
    {
        #region Public Constructors

        public DataImporterContext(
            DataImportRequest request,
            CancellationToken cancellationToken,
            string progressInfo)
        {
            Request = request;
            CancellationToken = cancellationToken;

            ExecuteContext = new ImportExecuteContext(CancellationToken, Request.ProgressValueSetter, progressInfo)
            {
                Request = request
            };

            ColumnMap = new ColumnMapConverter().ConvertFrom<ColumnMap>(Request.Profile.ColumnMapping) ?? new ColumnMap();
            Results = new Dictionary<string, ImportResult>();
        }

        #endregion Public Constructors



        #region Public Properties

        public CancellationToken CancellationToken { get; private set; }

        public ColumnMap ColumnMap { get; private set; }

        public ImportExecuteContext ExecuteContext { get; set; }

        public IEntityImporter Importer { get; set; }

        public TraceLogger Log { get; set; }

        public DataImportRequest Request { get; private set; }

        public Dictionary<string, ImportResult> Results { get; private set; }

        #endregion Public Properties
    }
}