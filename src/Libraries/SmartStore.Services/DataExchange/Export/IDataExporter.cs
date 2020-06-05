using SmartStore.Core.Domain;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.DataExchange;
using SmartStore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SmartStore.Services.DataExchange.Export
{
    public interface IDataExporter
    {
        #region Public Methods

        /// <summary>
        /// Creates a product export context for fast retrieval (eager loading) of product navigation properties
        /// </summary>
        /// <param name="products">Products. <c>null</c> to lazy load data if required.</param>
        /// <param name="customer">Customer, <c>null</c> to use current customer.</param>
        /// <param name="storeId">Store identifier, <c>null</c> to use current store.</param>
        /// <param name="maxPicturesPerProduct">Pictures per product, <c>null</c> to load all pictures per product.</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product export context</returns>
        ProductExportContext CreateProductExportContext(
            IEnumerable<Product> products = null,
            Customer customer = null,
            int? storeId = null,
            int? maxPicturesPerProduct = null,
            bool showHidden = true);

        DataExportResult Export(DataExportRequest request, CancellationToken cancellationToken);

        DataExportPreviewResult Preview(DataExportRequest request, int pageIndex);

        #endregion Public Methods
    }

    public class DataExportRequest
    {
        #region Private Fields

        private readonly static ProgressValueSetter _voidProgressValueSetter = DataExportRequest.SetProgress;

        #endregion Private Fields

        #region Public Constructors

        public DataExportRequest(ExportProfile profile, Provider<IExportProvider> provider)
        {
            Guard.NotNull(profile, nameof(profile));
            Guard.NotNull(provider, nameof(provider));

            Profile = profile;
            Provider = provider;
            ProgressValueSetter = _voidProgressValueSetter;

            EntitiesToExport = new List<int>();
            CustomData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion Public Constructors



        #region Public Properties

        public string ActionOrigin { get; set; }

        public IDictionary<string, object> CustomData { get; private set; }

        public IList<int> EntitiesToExport { get; set; }

        public bool HasPermission { get; set; }

        public IQueryable<Product> ProductQuery { get; set; }

        public ExportProfile Profile { get; private set; }

        public ProgressValueSetter ProgressValueSetter { get; set; }

        public Provider<IExportProvider> Provider { get; private set; }

        #endregion Public Properties



        #region Private Methods

        private static void SetProgress(int val, int max, string msg)
        {
            // do nothing
        }

        #endregion Private Methods
    }
}