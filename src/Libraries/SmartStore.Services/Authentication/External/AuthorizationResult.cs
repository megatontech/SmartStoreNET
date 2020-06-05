//Contributor:  Nicholas Mayne

using System.Collections.Generic;

namespace SmartStore.Services.Authentication.External
{
    public partial class AuthorizationResult
    {
        #region Public Constructors

        public AuthorizationResult(OpenAuthenticationStatus status)
        {
            this.Errors = new List<string>();
            Status = status;
        }

        #endregion Public Constructors



        #region Public Properties

        public IList<string> Errors { get; set; }

        public OpenAuthenticationStatus Status { get; private set; }

        public bool Success
        {
            get { return this.Errors.Count == 0; }
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