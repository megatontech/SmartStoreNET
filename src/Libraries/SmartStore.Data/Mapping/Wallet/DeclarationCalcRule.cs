
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class DeclarationCalcRuleMap : EntityTypeConfiguration<DeclarationCalcRule>
	{
		public DeclarationCalcRuleMap()
		{
			this.ToTable("DeclarationCalcRule");
			this.HasKey(p => p.Id);
		}
	}
}