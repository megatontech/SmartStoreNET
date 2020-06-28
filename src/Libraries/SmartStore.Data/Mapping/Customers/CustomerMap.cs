using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;

namespace SmartStore.Data.Mapping.Customers
{
    public partial class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.HasKey(c => c.Id);
            this.Property(u => u.Username).HasMaxLength(500);
            this.Property(u => u.Email).HasMaxLength(500);
			this.Property(u => u.SystemName).HasMaxLength(500);
			this.Property(u => u.Password).HasMaxLength(500);
			this.Property(u => u.PasswordSalt).HasMaxLength(500);
			this.Property(u => u.LastIpAddress).HasMaxLength(100);

			this.Property(u => u.Title).HasMaxLength(100);
			this.Property(u => u.Salutation).HasMaxLength(50);
			this.Property(u => u.FirstName).HasMaxLength(225);
			this.Property(u => u.LastName).HasMaxLength(225);
			this.Property(u => u.FullName).HasMaxLength(450);
			this.Property(u => u.Company).HasMaxLength(255);
			this.Property(u => u.CustomerNumber).HasMaxLength(100);

			this.Ignore(u => u.PasswordFormat);

            this.HasMany(c => c.CustomerRoles)
                .WithMany()
                .Map(m => m.ToTable("Customer_CustomerRole_Mapping"));

            this.HasMany<Address>(c => c.Addresses)
                .WithMany()
                .Map(m => m.ToTable("CustomerAddresses"));
            this.HasOptional<Address>(c => c.BillingAddress);
            this.HasOptional<Address>(c => c.ShippingAddress);
        }
    }
    public partial class CheckInMap : EntityTypeConfiguration<CheckIn>
    {
        public CheckInMap()
        {
            this.ToTable("CheckIn");
            this.HasKey(c => c.Id);
        }
    }
}