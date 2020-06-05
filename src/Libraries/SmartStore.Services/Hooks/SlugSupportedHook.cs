using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Seo;
using SmartStore.Services.Seo;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Common
{
    public class SlugSupportedHook : DbSaveHook<ISlugSupported>
    {
        #region Private Fields

        private readonly HashSet<UrlRecord> _toDelete = new HashSet<UrlRecord>();

        private readonly Lazy<IUrlRecordService> _urlRecordService;

        #endregion Private Fields

        #region Public Constructors

        public SlugSupportedHook(Lazy<IUrlRecordService> urlRecordService)
        {
            _urlRecordService = urlRecordService;
        }

        #endregion Public Constructors



        #region Public Methods

        public override void OnAfterSaveCompleted()
        {
            if (_toDelete.Count == 0)
                return;

            using (var scope = new DbContextScope(autoCommit: false))
            {
                using (_urlRecordService.Value.BeginScope())
                {
                    _toDelete.Each(x => _urlRecordService.Value.DeleteUrlRecord(x));
                }

                scope.Commit();
                _toDelete.Clear();
            }
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDeleted(ISlugSupported entity, IHookedEntity entry)
        {
            var entityType = entry.EntityType;
            var records = _urlRecordService.Value.GetUrlRecordsFor(entityType.Name, entry.Entity.Id);
            _toDelete.AddRange(records);
        }

        #endregion Protected Methods
    }
}