using SmartStore.Core.Domain.Customers;
using System;

namespace SmartStore.Services.Customers
{
    public class ChangePasswordRequest
    {
        #region Public Constructors

        public ChangePasswordRequest(string email, bool validateRequest,
            PasswordFormat newPasswordFormat, string newPassword, string oldPassword = "")
        {
            this.Email = email;
            this.ValidateRequest = validateRequest;
            this.NewPasswordFormat = newPasswordFormat;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
        }

        #endregion Public Constructors



        #region Public Properties
        
        public Guid CustomerGuid { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public PasswordFormat NewPasswordFormat { get; set; }

        public string OldPassword { get; set; }

        public bool ValidateRequest { get; set; }

        #endregion Public Properties
    }
}