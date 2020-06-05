using SmartStore.ComponentModel;
using System.Collections.Generic;

namespace SmartStore.Services.DataExchange.Export.Internal
{
    internal class DynamicEntity : HybridExpando
    {
        #region Public Constructors

        public DynamicEntity(DynamicEntity dynamicEntity)
            : this(dynamicEntity.WrappedObject)
        {
            MergeRange(dynamicEntity);
        }

        public DynamicEntity(object entity)
            : base(entity)
        {
            base.Properties["Entity"] = entity;
        }

        #endregion Public Constructors



        #region Public Methods

        public void Merge(string name, object value)
        {
            Properties[name] = value;
        }

        public void MergeRange(IDictionary<string, object> other)
        {
            foreach (var kvp in other)
            {
                Properties[kvp.Key] = kvp.Value;
            }
        }

        #endregion Public Methods



        #region Protected Methods

        protected override bool TrySetMemberCore(string name, object value)
        {
            Properties[name] = value;
            return true;
        }

        #endregion Protected Methods
    }
}