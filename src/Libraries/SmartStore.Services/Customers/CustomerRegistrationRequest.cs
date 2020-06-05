using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Customers
{
    public class CustomerRegistrationRequest
    {
        #region Public Constructors

        public CustomerRegistrationRequest(Customer customer, string email, string username,
            string password, string Mobile, string ParentMobile,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.Customer = customer;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            this.Mobile = Mobile;
            this.ParentMobile = ParentMobile;
        }

        #endregion Public Constructors



        #region Public Properties

        public Customer Customer { get; set; }

        public string Email { get; set; }

        public bool IsApproved { get; set; }

        public string Mobile { get; set; }

        public string ParentMobile { get; set; }

        public string Password { get; set; }

        public PasswordFormat PasswordFormat { get; set; }

        public string Username { get; set; }

        #endregion Public Properties

        //public bool IsValid
        //{
        //    get
        //    {
        //        return (!CommonHelper.AreNullOrEmpty(
        //                    this.Email,
        //                    this.Password
        //                    ));
        //    }
        //}
    }
}