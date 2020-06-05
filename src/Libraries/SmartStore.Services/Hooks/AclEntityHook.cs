using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Security;
using SmartStore.Services.Security;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Common
{
    public class AclEntityHook : DbSaveHook<IAclSupported>
    {
        #region Private Fields

        private readonly Lazy<IAclService> _aclService;

        private readonly HashSet<AclRecord> _toDelete = new HashSet<AclRecord>();

        #endregion Private Fields

        #region Public Constructors

        public AclEntityHook(Lazy<IAclService> aclService)
        {
            _aclService = aclService;
        }

        #endregion Public Constructors



        #region Public Methods

        public override void OnAfterSaveCompleted()
        {
            if (_toDelete.Count == 0)
                return;

            using (var scope = new DbContextScope(autoCommit: false))
            {
                _toDelete.Each(x => _aclService.Value.DeleteAclRecord(x));
                scope.Commit();
            }

            _toDelete.Clear();
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDeleted(IAclSupported entity, IHookedEntity entry)
        {
            var entityType = entry.EntityType;

            var records = _aclService.Value.GetAclRecordsFor(entityType.Name, entry.Entity.Id);
            _toDelete.AddRange(records);
        }

        #endregion Protected Methods
    }
}