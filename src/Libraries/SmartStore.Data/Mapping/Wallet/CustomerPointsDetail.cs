
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class CustomerPointsDetailMap : EntityTypeConfiguration<CustomerPointsDetail>
	{
		public CustomerPointsDetailMap()
		{
			this.ToTable("DeclarationCustomerPointsDetail");
			this.HasKey(p => p.Id);
		}
	}
}