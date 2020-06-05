using SmartStore.Core.Data;
using SmartStore.Core.Data.Hooks;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Data.Utilities;
using System.Collections.Generic;

namespace SmartStore.Services.Common
{
    public class FixProductMainPictureHook : DbSaveHook<ProductPicture>
    {
        #region Private Fields

        private readonly HashSet<Product> _products = new HashSet<Product>();

        private readonly IRepository<Product> _rsProduct;

        #endregion Private Fields

        #region Public Constructors

        public FixProductMainPictureHook(IRepository<Product> rsProduct)
        {
            _rsProduct = rsProduct;
        }

        #endregion Public Constructors



        #region Public Methods

        public override void OnBeforeSaveCompleted()
        {
            foreach (var product in _products)
            {
                DataMigrator.FixProductMainPictureId(_rsProduct.Context, product);
            }

            _products.Clear();
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDeleting(ProductPicture entity, IHookedEntity entry)
        {
            Fix(entity);
        }

        protected override void OnInserting(ProductPicture entity, IHookedEntity entry)
        {
            Fix(entity);
        }

        protected override void OnUpdating(ProductPicture entity, IHookedEntity entry)
        {
            Fix(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Fix(ProductPicture entity)
        {
            var product = entity.Product ?? _rsProduct.GetById(entity.ProductId);
            if (product != null)
            {
                _products.Add(product);
            }
        }

        #endregion Private Methods
    }
}