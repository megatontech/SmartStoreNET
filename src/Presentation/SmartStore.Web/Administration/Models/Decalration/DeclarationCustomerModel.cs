using FluentValidation;
using FluentValidation.Attributes;
using SmartStore.Core.Localization;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Stores
{
	public partial class DeclarationCustomerModel : EntityModelBase
	{
		[SmartResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
		[AllowHtml]
		public string Name { get; set; }

		[SmartResourceDisplayName("Admin.Configuration.Stores.Fields.Url")]
		[AllowHtml]
		public string Mobile { get; set; }

		[SmartResourceDisplayName("Admin.Configuration.Stores.Fields.SslEnabled")]
		public decimal  Total { get; set; }

		[SmartResourceDisplayName("Admin.Configuration.Stores.Fields.SecureUrl")]
		[AllowHtml]
		public  int OrderCount { get; set; }

	}

    
}