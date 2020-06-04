
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class DailyTotalContributionMap : EntityTypeConfiguration<DailyTotalContribution>
	{
		public DailyTotalContributionMap()
		{
			this.ToTable("DeclarationDailyTotalContribution");
			this.HasKey(p => p.Id);
		}
	}
}