using System.Collections.Generic;

namespace SmartStore.Services.Customers
{
    public class PasswordChangeResult
    {
        #region Public Constructors

        public PasswordChangeResult()
        {
            this.Errors = new List<string>();
        }

        #endregion Public Constructors



        #region Public Properties

        public IList<string> Errors { get; set; }

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