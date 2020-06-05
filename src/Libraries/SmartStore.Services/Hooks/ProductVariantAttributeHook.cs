using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Services.Catalog;
using System;
using System.Collections.Generic;

namespace SmartStore.Services.Hooks
{
    public class ProductVariantAttributeHook : DbSaveHook<ProductVariantAttribute>
    {
        #region Private Fields

        private readonly Lazy<IProductAttributeService> _productAttributeService;

        private readonly HashSet<int> _toDelete = new HashSet<int>();

        #endregion Private Fields

        #region Public Constructors

        public ProductVariantAttributeHook(Lazy<IProductAttributeService> productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        #endregion Public Constructors



        #region Public Methods

        public override void OnAfterSaveCompleted()
        {
            if (_toDelete.Count == 0)
                return;

            using (var scope = new DbContextScope(autoCommit: false))
            {
                _toDelete.Each(x => _productAttributeService.Value.DeleteProductBundleItemAttributeFilter(x));
                scope.Commit();
            }

            _toDelete.Clear();
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDeleted(ProductVariantAttribute entity, IHookedEntity entry)
        {
            _toDelete.Add(entity.Id);
        }

        #endregion Protected Methods
    }
}