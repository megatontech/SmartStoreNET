using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Directory;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Security;
using SmartStore.Core.Domain.Seo;
using SmartStore.Core.Domain.Stores;

namespace SmartStore.Core.Domain.Declaration
{
    [DataContract]
    public partial class DeclarationCapRule : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int LineCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal RewardAmount { get; set; }
        
    }
}
