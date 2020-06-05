using SmartStore.Core.Domain.Customers;

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

        public string Email { get; set; }

        public string NewPassword { get; set; }

        public PasswordFormat NewPasswordFormat { get; set; }

        public string OldPassword { get; set; }

        public bool ValidateRequest { get; set; }

        #endregion Public Properties
    }
}