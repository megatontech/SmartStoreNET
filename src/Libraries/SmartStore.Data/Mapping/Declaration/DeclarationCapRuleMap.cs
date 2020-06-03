using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Declaration;

namespace SmartStore.Data.Mapping.Catalog
{
	public partial class DeclarationCapRuleMap : EntityTypeConfiguration<DeclarationCapRule>
	{
		public DeclarationCapRuleMap()
		{
			this.ToTable("DeclarationCapRule");
			this.HasKey(p => p.Id);
			this.Property(p => p.LineCount);
			this.Property(p => p.RewardAmount).HasPrecision(18, 4);
			this.Property(p => p.IsCount);
		}
	}
}