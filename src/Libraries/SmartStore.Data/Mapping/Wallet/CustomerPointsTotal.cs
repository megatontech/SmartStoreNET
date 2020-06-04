
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class CustomerPointsTotalMap : EntityTypeConfiguration<CustomerPointsTotal>
	{
		public CustomerPointsTotalMap()
		{
			this.ToTable("DeclarationCustomerPointsTotal");
			this.HasKey(p => p.Id);
		}
	}
}