
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class WithdrawalTotalMap : EntityTypeConfiguration<WithdrawalTotal>
	{
		public WithdrawalTotalMap()
		{
			this.ToTable("DeclarationWithdrawalTotal");
			this.HasKey(p => p.Id);
		}
	}
}