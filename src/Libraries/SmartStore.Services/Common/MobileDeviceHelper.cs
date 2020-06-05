namespace SmartStore.Services.Common
{
    public partial class MobileDeviceHelper : IMobileDeviceHelper
    {
        #region Private Fields

        private readonly IUserAgent _userAgent;

        #endregion Private Fields

        #region Public Constructors

        public MobileDeviceHelper(IUserAgent userAgent)
        {
            _userAgent = userAgent;
        }

        #endregion Public Constructors



        #region Public Methods

        public virtual bool IsMobileDevice()
        {
            return _userAgent.IsMobileDevice && !_userAgent.IsTablet;
        }

        #endregion Public Methods
    }
}