using SmartStore.Services.Tasks;

namespace SmartStore.Services.DataExchange.Import
{
    // note: namespace persisted in ScheduleTask.Type
    public partial class DataImportTask : ITask
    {
        #region Private Fields

        private readonly IDataImporter _importer;

        private readonly IImportProfileService _importProfileService;

        #endregion Private Fields

        #region Public Constructors

        public DataImportTask(
            IDataImporter importer,
            IImportProfileService importProfileService)
        {
            _importer = importer;
            _importProfileService = importProfileService;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Execute(TaskExecutionContext ctx)
        {
            var profileId = ctx.ScheduleTaskHistory.ScheduleTask.Alias.ToInt();
            var profile = _importProfileService.GetImportProfileById(profileId);

            var request = new DataImportRequest(profile);

            request.ProgressValueSetter = delegate (int val, int max, string msg)
            {
                ctx.SetProgress(val, max, msg, true);
            };

            _importer.Import(request, ctx.CancellationToken);
        }

        #endregion Public Methods
    }
}