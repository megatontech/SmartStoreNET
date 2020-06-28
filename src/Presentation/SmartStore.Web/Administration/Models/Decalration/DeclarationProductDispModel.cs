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
	public partial class DeclarationProductDispModel : EntityModelBase
	{
		[SmartResourceDisplayName("产品名")]
		[AllowHtml]
		public string Name { get; set; }
		[SmartResourceDisplayName("单价")]
		[AllowHtml]
		public decimal Price { get; set; }

		[SmartResourceDisplayName("销售量")]
		[AllowHtml]
		public int Count { get; set; }

		[SmartResourceDisplayName("销售额")]
		public decimal Amount { get; set; }

		[SmartResourceDisplayName("产品等级")]
		[AllowHtml]
		public string Level { get; set; }

		
    }
}