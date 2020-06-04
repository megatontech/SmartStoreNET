
using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Wallet;

namespace SmartStore.Data.Mapping.Wallet
{
	public partial class WithdrawalDetailMap : EntityTypeConfiguration<WithdrawalDetail>
	{
		public WithdrawalDetailMap()
		{
			this.ToTable("DeclarationWithdrawalDetail");
			this.HasKey(p => p.Id);
		}
	}
}