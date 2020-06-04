
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class DailyCustomerContributionDetailMap : EntityTypeConfiguration<DailyCustomerContributionDetail>
	{
		public DailyCustomerContributionDetailMap()
		{
			this.ToTable("DeclarationDailyCustomerContributionDetail");
			this.HasKey(p => p.Id);
		}
	}
}