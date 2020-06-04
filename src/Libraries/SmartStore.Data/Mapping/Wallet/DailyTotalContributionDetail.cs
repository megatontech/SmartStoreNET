
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class DailyTotalContributionDetailMap : EntityTypeConfiguration<DailyTotalContributionDetail>
	{
		public DailyTotalContributionDetailMap()
		{
			this.ToTable("DeclarationDailyTotalContributionDetail");
			this.HasKey(p => p.Id);
		}
	}
}