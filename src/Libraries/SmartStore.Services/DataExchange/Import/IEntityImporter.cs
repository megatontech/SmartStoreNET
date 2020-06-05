namespace SmartStore.Services.DataExchange.Import
{
    public partial interface IEntityImporter
    {
        #region Public Methods

        void Execute(ImportExecuteContext context);

        #endregion Public Methods
    }
}