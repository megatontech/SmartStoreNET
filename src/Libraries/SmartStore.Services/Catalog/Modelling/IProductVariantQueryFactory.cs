namespace SmartStore.Services.Catalog.Modelling
{
    public interface IProductVariantQueryFactory
    {
        #region Public Properties

        /// <summary>
        /// The last created product variant query. The MVC model binder uses this property to avoid repeated binding.
        /// </summary>
        ProductVariantQuery Current { get; }

        #endregion Public Properties



        #region Public Methods

        /// <summary>
        /// Creates a name value collection with product variants from the current <see cref="HttpContextBase"/>
        /// by looking up corresponding keys in posted form and/or query string
        /// </summary>
        /// <returns>Product variant query</returns>
        ProductVariantQuery CreateFromQuery();

        #endregion Public Methods
    }
}