
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class WithdrawalApplyMap : EntityTypeConfiguration<WithdrawalApply>
	{
		public WithdrawalApplyMap()
		{
			this.ToTable("DeclarationWithdrawalApply");
			this.HasKey(p => p.Id);
		}
	}
}