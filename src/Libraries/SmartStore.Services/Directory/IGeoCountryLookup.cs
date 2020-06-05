using System.Net;

namespace SmartStore.Services.Directory
{
    /// <summary>
    /// Country lookup helper for IPv4/6 addresses
    /// </summary>
    public partial interface IGeoCountryLookup
    {
        #region Public Methods

        LookupCountryResponse LookupCountry(string addr);

        LookupCountryResponse LookupCountry(IPAddress addr);

        #endregion Public Methods
    }

    public sealed class LookupCountryResponse
    {
        #region Public Properties

        /// <summary>
        /// The GeoName ID for the country.
        /// </summary>
        public int? GeoNameId { get; set; }

        /// <summary>
        /// This is true if the country is a member state of the European Union.
        /// </summary>
        public bool IsInEu { get; set; }

        /// <summary>
        /// The two-letter ISO 3166-1 alpha code for the country
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// The english name of the country.
        /// </summary>
        public string Name { get; set; }

        #endregion Public Properties
    }
}