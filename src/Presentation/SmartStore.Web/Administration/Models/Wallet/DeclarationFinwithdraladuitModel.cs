﻿using FluentValidation;
using FluentValidation.Attributes;
using SmartStore.Core.Localization;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Wallet
{
    public class DeclarationFinwithdraladuitModel : EntityModelBase
    {
        /// <summary>
        /// 客户
        /// </summary>
        [SmartResourceDisplayName("客户")]
        [AllowHtml]
        public int Customer { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [SmartResourceDisplayName("操作人")]
        [AllowHtml]
        public int Operater { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        [SmartResourceDisplayName("客户id")]
        [AllowHtml]
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        [SmartResourceDisplayName("操作人ID")]
        [AllowHtml]
        public Guid OperaterID { get; set; }
        /// <summary>
        /// 钱数
        /// </summary>
        [SmartResourceDisplayName("钱数")]
        [AllowHtml]
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否转出
        /// </summary>
        [SmartResourceDisplayName("是否转出")]
        [AllowHtml]
        public bool isOut { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SmartResourceDisplayName("备注")]
        [AllowHtml]
        public string Comment { get; set; }
        /// <summary>
        /// 转账申请时间
        /// </summary>
        [SmartResourceDisplayName("转账申请时间")]
        [AllowHtml]
        public DateTime WithdrawTime { get; set; }
        /// <summary>
        /// 转账审核时间
        /// </summary>
        [SmartResourceDisplayName("转账审核时间")]
        [AllowHtml]
        public DateTime WithdrawAuditTime { get; set; }
        /// <summary>
        /// 转账审核人
        /// </summary>
        [SmartResourceDisplayName("转账审核人")]
        [AllowHtml]
        public int WithdrawAuditCustomer { get; set; }
        /// <summary>
        /// 转账审核备注
        /// </summary>
        [SmartResourceDisplayName("转账审核备注")]
        [AllowHtml]
        public string WithdrawAuditComment { get; set; }
        /// <summary>
        /// 转账类型
        /// </summary>
        [SmartResourceDisplayName("转账类型")]
        [AllowHtml]
        public int WithdrawType { get; set; }
        /// <summary>
        /// 转账状态
        /// </summary>
        [SmartResourceDisplayName("转账状态")]
        [AllowHtml]
        public int WithdrawStatus { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        [SmartResourceDisplayName("最后更新日期")]
        [AllowHtml]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否参与统计
        /// </summary>
        [SmartResourceDisplayName("是否参与统计")]
        [AllowHtml]
        public bool IsCount { get; set; }
    }
}