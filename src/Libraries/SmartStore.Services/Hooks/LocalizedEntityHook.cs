using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Hooks
{
    public class LocalizedEntityHook : DbSaveHook<ILocalizedEntity>
    {
        #region Private Fields

        private readonly Lazy<ILocalizedEntityService> _localizedEntityService;

        private readonly HashSet<LocalizedProperty> _toDelete = new HashSet<LocalizedProperty>();

        #endregion Private Fields

        #region Public Constructors

        public LocalizedEntityHook(Lazy<ILocalizedEntityService> localizedEntityService)
        {
            _localizedEntityService = localizedEntityService;
        }

        #endregion Public Constructors



        #region Public Methods

        public override void OnAfterSaveCompleted()
        {
            if (_toDelete.Count == 0)
                return;

            using (var scope = new DbContextScope(autoCommit: false))
            {
                using (_localizedEntityService.Value.BeginScope())
                {
                    _toDelete.Each(x => _localizedEntityService.Value.DeleteLocalizedProperty(x));
                }

                scope.Commit();
                _toDelete.Clear();
            }
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDeleted(ILocalizedEntity entity, IHookedEntity entry)
        {
            var entityType = entry.EntityType;
            var localizedEntities = _localizedEntityService.Value.GetLocalizedProperties(entry.Entity.Id, entityType.Name);
            _toDelete.AddRange(localizedEntities);
        }

        #endregion Protected Methods
    }
}