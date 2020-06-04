
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class LuckMoneyMap : EntityTypeConfiguration<LuckMoney>
	{
		public LuckMoneyMap()
		{
			this.ToTable("DeclarationLuckMoney");
			this.HasKey(p => p.Id);
		}
	}
}