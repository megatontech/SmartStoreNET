using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Localization;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;

namespace SmartStore.Web.Models.Customer
{
	[Validator(typeof(ChangePasswordValidator))]
    public partial class ChangePasswordModel : ModelBase
    {
        [DataType(DataType.Password)]
        [SmartResourceDisplayName("Account.ChangePassword.Fields.OldPassword")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [SmartResourceDisplayName("Account.ChangePassword.Fields.NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [SmartResourceDisplayName("Account.ChangePassword.Fields.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }
    }
    public partial class WithDrawApplyModel : ModelBase
    {
        [SmartResourceDisplayName("金额")]
        public decimal Amount { get; set; }
        [SmartResourceDisplayName("可用余额")]
        public decimal TotalAmount { get; set; }
        public SmartStore.Core.Domain.Customers.Customer customer { get; set; }
    }
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator(Localizer T, CustomerSettings customerSettings)
        {
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.ConfirmNewPassword).NotEmpty();
            RuleFor(x => x.NewPassword).Length(customerSettings.PasswordMinLength, 15);

            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(T("Account.ChangePassword.Fields.NewPassword.EnteredPasswordsDoNotMatch"));
        }
    }
}