using FluentValidation;
using FluentValidation.Attributes;
using SmartStore.Core.Domain.Wallet;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Wallet
{
    public class DailyTotalContributionModel : EntityModelBase
    {
        public DailyTotalContribution model { get; set; }
    }
}