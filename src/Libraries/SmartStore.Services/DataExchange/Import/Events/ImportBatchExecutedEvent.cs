using SmartStore.Core;
using System.Collections.Generic;

namespace SmartStore.Services.DataExchange.Import.Events
{
    /// <summary>
    /// An event that is fired after an import of a data batch.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be imported.</typeparam>
    public class ImportBatchExecutedEvent<TEntity> where TEntity : BaseEntity
    {
        #region Public Constructors

        public ImportBatchExecutedEvent(ImportExecuteContext context, IEnumerable<ImportRow<TEntity>> batch)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(batch, nameof(batch));

            Context = context;
            Batch = batch;
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// Current batch of import data.
        /// </summary>
        public IEnumerable<ImportRow<TEntity>> Batch { get; private set; }

        /// <summary>
        /// Context of the import.
        /// </summary>
        public ImportExecuteContext Context { get; private set; }

        #endregion Public Properties
    }
}