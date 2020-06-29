using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmartStore.Core.Domain.Orders;
using SmartStore.Collections;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Forums;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Customers
{
	/// <summary>
	/// Represents a digital wallet history entry.
	/// </summary>
	[DataContract]
	public class CAPTCHACode : BaseEntity
	{
		[DataMember] public string Mobile { get; set; }
		[DataMember] public string Code { get; set; }
		[DataMember] public bool IsUsed { get; set; }
		[DataMember] public string Content { get; set; }
		[DataMember] public DateTime SendDate { get; set; }

	}
}
