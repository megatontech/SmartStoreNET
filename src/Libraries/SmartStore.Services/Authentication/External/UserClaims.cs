//Contributor:  Nicholas Mayne

using System;

namespace SmartStore.Services.Authentication.External
{
    [Serializable]
    public partial class AddressClaims
    {
        #region Public Properties

        public string City { get; set; }

        public string Country { get; set; }

        public string DisplayName { get; set; }

        public string Host { get; set; }

        public string PostalCode { get; set; }

        public string SingleLineAddress { get; set; }

        public string State { get; set; }

        public string StreetAddressLine1 { get; set; }

        public string StreetAddressLine2 { get; set; }

        public string User { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class BirthDateClaims
    {
        #region Public Properties

        public int DayOfMonth { get; set; }

        public DateTime GeneratedBirthDate { get { return new DateTime(Year, Month, DayOfMonth); } }

        public int Month { get; set; }

        public string Raw { get; set; }

        public DateTime? WholeBirthDate { get; set; }

        public int Year { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class CompanyClaims
    {
        #region Public Properties

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class ContactClaims
    {
        #region Public Properties

        public AddressClaims Address { get; set; }

        public string Email { get; set; }

        public InstantMessagingClaims IM { get; set; }

        public AddressClaims MailAddress { get; set; }

        public TelephoneClaims Phone { get; set; }

        public WebClaims Web { get; set; }

        public AddressClaims WorkAddress { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class ImageClaims
    {
        #region Public Properties

        public string Aspect11 { get; set; }

        public string Aspect34 { get; set; }

        public string Aspect43 { get; set; }

        public string Default { get; set; }

        public string FavIcon { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class InstantMessagingClaims
    {
        #region Public Properties

        public string AOL { get; set; }

        public string ICQ { get; set; }

        public string Jabber { get; set; }

        public string MSN { get; set; }

        public string Skype { get; set; }

        public string Yahoo { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class MediaClaims
    {
        #region Public Properties

        public string AudioGreeting { get; set; }

        public ImageClaims Images { get; set; }

        public string SpokenName { get; set; }

        public string VideoGreeting { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class NameClaims
    {
        #region Public Properties

        public string Alias { get; set; }

        public string First { get; set; }

        public string FullName { get; set; }

        public string Last { get; set; }

        public string Middle { get; set; }

        public string Nickname { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class PersonClaims
    {
        #region Public Properties

        public string Biography { get; set; }

        public string Gender { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class PreferenceClaims
    {
        #region Public Properties

        public string Language { get; set; }

        public string PrimaryLanguage { get; set; }

        public string TimeZone { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class TelephoneClaims
    {
        #region Public Properties

        public string Fax { get; set; }

        public string Home { get; set; }

        public string Mobile { get; set; }

        public string Preferred { get; set; }

        public string Work { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class UserClaims
    {
        #region Public Properties

        public BirthDateClaims BirthDate { get; set; }

        public CompanyClaims Company { get; set; }

        public ContactClaims Contact { get; set; }

        public bool IsSignedByProvider { get; set; }

        public MediaClaims Media { get; set; }

        public NameClaims Name { get; set; }

        public PersonClaims Person { get; set; }

        public PreferenceClaims Preferences { get; set; }

        public Version Version { get; set; }

        #endregion Public Properties
    }

    [Serializable]
    public partial class WebClaims
    {
        #region Public Properties

        public string Amazon { get; set; }

        public string Blog { get; set; }

        public string Delicious { get; set; }

        public string Flickr { get; set; }

        public string Homepage { get; set; }

        public string LinkedIn { get; set; }

        #endregion Public Properties
    }
}