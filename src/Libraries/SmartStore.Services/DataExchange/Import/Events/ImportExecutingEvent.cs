namespace SmartStore.Services.DataExchange.Import.Events
{
    public class ImportExecutingEvent
    {
        #region Public Constructors

        public ImportExecutingEvent(ImportExecuteContext context)
        {
            Guard.NotNull(context, nameof(context));

            Context = context;
        }

        #endregion Public Constructors



        #region Public Properties

        public ImportExecuteContext Context
        {
            get;
            private set;
        }

        #endregion Public Properties
    }
}