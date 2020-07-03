using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmartStore.Core.Domain.Orders;
using SmartStore.Collections;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Forums;
using System.Runtime.Serialization;
using System.Collections.Generic;
using SmartStore.Core.Domain.Discounts;

namespace SmartStore.Core.Domain.Customers
{
	/// <summary>
	/// Represents a digital wallet history entry.
	/// </summary>
	[DataContract]
	public class CustomerDiscount : BaseEntity
	{

[DataMember] public int Customer{ get; set; }
[DataMember] public int Discount { get; set; }
[DataMember] public bool IsUsed{ get; set; }
[DataMember] public DateTime GetDateTime { get; set; }
[DataMember] public DateTime UseDateTime{ get; set; }
[DataMember] public int Order { get; set; }
		[DataMember] public string Comment { get; set; }
	
		[NotMapped]public Discount discount { get; set; }

	}
}
