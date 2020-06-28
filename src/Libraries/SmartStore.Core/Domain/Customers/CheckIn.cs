using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmartStore.Core.Domain.Orders;
using SmartStore.Collections;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Orders;
using System.Runtime.Serialization;

namespace SmartStore.Core.Domain.Customers
{
	/// <summary>
	/// Represents a digital wallet history entry.
	/// </summary>
	public class CheckIn : BaseEntity
	{


		/// <summary>
		/// Gets or sets the customer identifier.
		/// </summary>
		[DataMember] public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		[DataMember] public DateTime Date { get; set; }

	}
}
