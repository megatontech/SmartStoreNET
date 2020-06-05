using System;

namespace SmartStore.Services.Catalog.Modelling
{
    public class CheckoutAttributeQueryItem
    {
        #region Public Constructors

        public CheckoutAttributeQueryItem(int attributeId, string value)
        {
            Value = value ?? string.Empty;
            AttributeId = attributeId;
        }

        #endregion Public Constructors



        #region Public Properties

        public int AttributeId { get; private set; }

        public DateTime? Date { get; set; }

        public bool IsFile { get; set; }

        public bool IsText { get; set; }

        public string Value { get; private set; }

        #endregion Public Properties



        #region Public Methods

        /// <summary>
        /// Key used for form names.
        /// </summary>
        /// <param name="attributeId">Checkout attribute identifier</param>
        /// <returns>Key</returns>
        public static string CreateKey(int attributeId)
        {
            return $"cattr{attributeId}";
        }

        public override string ToString()
        {
            var key = CreateKey(AttributeId);

            if (Date.HasValue)
            {
                return key + "-date";
            }
            else if (IsFile)
            {
                return key + "-file";
            }
            else if (IsText)
            {
                return key + "-text";
            }

            return key;
        }

        #endregion Public Methods
    }
}