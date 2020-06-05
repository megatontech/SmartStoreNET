using MaxMind.GeoIP2;
using SmartStore.Utilities;
using System.Net;

namespace SmartStore.Services.Directory
{
    public partial class GeoCountryLookup : DisposableObject, IGeoCountryLookup
    {
        #region Private Fields

        private readonly object _lock = new object();

        private readonly DatabaseReader _reader;

        #endregion Private Fields

        #region Public Constructors

        public GeoCountryLookup()
        {
            _reader = new DatabaseReader(CommonHelper.MapPath("~/App_Data/GeoLite2/GeoLite2-Country.mmdb"));
        }

        #endregion Public Constructors



        #region Public Methods

        public LookupCountryResponse LookupCountry(string addr)
        {
            if (addr.HasValue() && IPAddress.TryParse(addr, out var ipAddress))
            {
                return LookupCountry(ipAddress);
            }

            return null;
        }

        public LookupCountryResponse LookupCountry(IPAddress addr)
        {
            Guard.NotNull(addr, nameof(addr));

            if (_reader.TryCountry(addr, out var response) && response.Country != null)
            {
                var country = response.Country;
                return new LookupCountryResponse
                {
                    GeoNameId = country.GeoNameId,
                    IsoCode = country.IsoCode,
                    Name = country.Name,
                    IsInEu = country.IsInEuropeanUnion
                };
            }

            return null;
        }

        #endregion Public Methods



        #region Protected Methods

        protected override void OnDispose(bool disposing)
        {
            if (disposing && _reader != null)
            {
                _reader.Dispose();
            }
        }

        #endregion Protected Methods
    }
}