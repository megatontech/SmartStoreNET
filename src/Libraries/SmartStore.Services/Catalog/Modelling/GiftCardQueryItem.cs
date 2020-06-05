namespace SmartStore.Services.Catalog.Modelling
{
    public class GiftCardQueryItem
    {
        #region Public Constructors

        public GiftCardQueryItem(string name, string value)
        {
            Guard.NotEmpty(name, nameof(name));

            Name = name.ToLower();
            Value = value ?? string.Empty;

            if (Name.StartsWith("."))
            {
                Name = Name.Substring(1);
            }
        }

        #endregion Public Constructors



        #region Public Properties

        public int BundleItemId { get; set; }

        public string Name { get; private set; }

        public int ProductId { get; set; }

        public string Value { get; private set; }

        #endregion Public Properties



        #region Public Methods

        public static string CreateKey(int productId, int bundleItemId, string name)
        {
            if (name.HasValue())
            {
                return $"giftcard{productId}-{bundleItemId}-.{name.EmptyNull().ToLower()}";
            }

            // Just return field prefix for partial views.
            return $"giftcard{productId}-{bundleItemId}-";
        }

        public override string ToString()
        {
            return CreateKey(ProductId, BundleItemId, Name);
        }

        #endregion Public Methods
    }
}