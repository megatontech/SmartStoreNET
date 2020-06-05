using SmartStore.Core.Domain;
using SmartStore.Core.Domain.DataExchange;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SmartStore.Services.DataExchange.Import
{
    public interface IDataImporter
    {
        #region Public Methods

        void Import(DataImportRequest request, CancellationToken cancellationToken);

        #endregion Public Methods
    }

    public class DataImportRequest
    {
        #region Private Fields

        private readonly static ProgressValueSetter _voidProgressValueSetter = DataImportRequest.SetProgress;

        #endregion Private Fields

        #region Public Constructors

        public DataImportRequest(ImportProfile profile)
        {
            Guard.NotNull(profile, nameof(profile));

            Profile = profile;
            ProgressValueSetter = _voidProgressValueSetter;

            EntitiesToImport = new List<int>();
            CustomData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion Public Constructors



        #region Public Properties

        public IDictionary<string, object> CustomData { get; private set; }

        public IList<int> EntitiesToImport { get; set; }

        public bool HasPermission { get; set; }

        public ImportProfile Profile { get; private set; }

        public ProgressValueSetter ProgressValueSetter { get; set; }

        #endregion Public Properties



        #region Private Methods

        private static void SetProgress(int val, int max, string msg)
        {
            // do nothing
        }

        #endregion Private Methods
    }
}