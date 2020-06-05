//Contributor:  Nicholas Mayne

using System;
using System.Collections.Generic;

namespace SmartStore.Services.Authentication.External
{
    [Serializable]
    public abstract partial class OpenAuthenticationParameters
    {
        #region Public Properties

        public string ExternalDisplayIdentifier { get; set; }

        public string ExternalIdentifier { get; set; }

        public string OAuthAccessToken { get; set; }

        public string OAuthToken { get; set; }

        public abstract string ProviderSystemName { get; }

        public virtual IList<UserClaims> UserClaims
        {
            get { return new List<UserClaims>(0); }
        }

        #endregion Public Properties
    }
}