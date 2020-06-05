//Contributor:  Nicholas Mayne

using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartStore.Services.Authentication.External
{
    public partial class AuthorizeState
    {
        #region Private Fields

        private readonly string _returnUrl;

        #endregion Private Fields

        #region Public Constructors

        public AuthorizeState(string returnUrl, OpenAuthenticationStatus openAuthenticationStatus)
        {
            Errors = new List<string>();
            _returnUrl = returnUrl;
            AuthenticationStatus = openAuthenticationStatus;

            // in a way SEO friendly language URLs will be persisted
            if (AuthenticationStatus == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(_returnUrl) ? _returnUrl : "~/");
        }

        public AuthorizeState(string returnUrl, AuthorizationResult authorizationResult)
            : this(returnUrl, authorizationResult.Status)
        {
            Errors = authorizationResult.Errors;
        }

        #endregion Public Constructors



        #region Public Properties

        public OpenAuthenticationStatus AuthenticationStatus { get; private set; }

        public IList<string> Errors { get; set; }

        public ActionResult Result { get; set; }

        public bool Success
        {
            get { return (this.Errors.Count == 0); }
        }

        #endregion Public Properties



        #region Public Methods

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        #endregion Public Methods
    }
}