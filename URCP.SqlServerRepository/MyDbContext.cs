using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.Lookups;
using URCP.Core.Security;

namespace URCP.SqlServerRepository
{
    public class MyDbContext : DbContext
    {
        public static MyDbContext Create()
        {
            return new MyDbContext();
        }

        #region Identity
        public DbSet<IdentityUserClaim> UserClaims { get; set; }

        public DbSet<IdentityUserLogin> UserLogins { get; set; }

        public DbSet<IdentityRole> Roles { get; set; }

        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }

        public DbSet<IdentityUser> Users { get; set; }

        public DbSet<UserMorgageMoneyType> UserMorgageMoneyTypes { get; set; }
        #endregion

        #region Core Entities
        public DbSet<Request> Requests { get; set; }

        public DbSet<ReverseMortgage> ReverseMortgages { get; set; }

        public DbSet<ReverseMortgageAirplaneDetail> ReverseMortgageAirplaneDetails { get; set; }

        public DbSet<ReverseMortgageShipDetail> ReverseMortgageShipDetails { get; set; }

        public DbSet<ReverseMortgageTrademarkDetail> ReverseMortgageTrademarkDetails { get; set; }

        public DbSet<Mortgage> Mortgages { get; set; }

        public DbSet<MortgageFinancialSharesDetail> MortgageFinancialSharesDetails { get; set; }

        public DbSet<MortgageBankDepositDetail> MortgageBankDepositDetails { get; set; }

        public DbSet<MortgageOtherMoneyDetail> MortgageOtherMoneyDetails { get; set; }

        public DbSet<MortgageBankAccountDetail> MortgageBankAccountDetails { get; set; }

        public DbSet<MortgageImplementationAgent> MortgageImplementationAgents { get; set; }

        public DbSet<MortgageObjection> MortgageObjections { get; set; }

        public DbSet<MortgageInquiry> MortgagedInquiries { get; set; }

        public DbSet<MortgageInquiryDetail> MortgageInquiryDetails { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Lookup> Lookups { get; set; }

        public DbSet<LookupType> LookupTypes { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Country> Countries { get; set; }

        #endregion

        #region Util Entities
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<SmsNotification> SmsNotifications { get; set; }
        public DbSet<EmailAttachment> EmailAttachments { get; set; }
        #endregion

        public MyDbContext() : base("CnnStr")
        {
            base.Configuration.ProxyCreationEnabled = false;
            base.Configuration.ValidateOnSaveEnabled = false;

            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif

            Database.SetInitializer<MyDbContext>(new CreateDatabaseIfNotExists<MyDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            BaseEntityMap(modelBuilder);

            #region Identity
            modelBuilder.Entity<IdentityUserClaim>()
                .HasKey(uc => uc.Id)
                .ToTable("UserClaims");

            modelBuilder.Entity<IdentityRoleClaim>()
                .HasKey(rc => rc.Id)
                .ToTable("RoleClaims");

            modelBuilder.Entity<IdentityUserLogin>()
             .HasKey(l => new { l.LoginProvider, l.ProviderKey })
                .ToTable("UserLogins");

            modelBuilder.Entity<IdentityRole>()
           .Property(e => e.Id)
           .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Setting>()
           .Property(e => e.Id)
           .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles")
                .Property(r => r.ConcurrencyStamp).IsRowVersion();
            modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasMaxLength(256);
            modelBuilder.Entity<IdentityRole>().HasMany(r => r.Claims).WithRequired().HasForeignKey(r => r.RoleId);

            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users")
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(x =>
                {
                    x.MapLeftKey("UserID");
                    x.MapRightKey("RoleID");
                    x.ToTable("UsersRoles");
                });
            modelBuilder.Entity<IdentityUser>().Property(u => u.ConcurrencyStamp).IsRowVersion();
            modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<IdentityUser>().HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        private void BaseEntityMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedByID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedByID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                .HasRequired(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                .HasRequired(x => x.MortgageOwner)
                .WithMany()
                .HasForeignKey(x => x.MortgageOwnerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                .HasRequired(x => x.MortgagedCategory)
                .WithMany()
                .HasForeignKey(x => x.MortgagedCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                .HasRequired(x => x.MortgageOwnerCategory)
                .WithMany()
                .HasForeignKey(x => x.MortgageOwnerCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                 .HasRequired(x => x.MoneyStatus)
                 .WithMany()
                 .HasForeignKey(x => x.MoneyStatusId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
                .HasRequired(x => x.MoneyType)
                .WithMany()
                .HasForeignKey(x => x.MoneyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mortgage>()
               .HasRequired(x => x.MortgageImplementationMethod)
               .WithMany()
               .HasForeignKey(x => x.MortgageImplementationMethodId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<MortgageBankAccountDetail>()
                .HasRequired(x => x.BankAccountType)
                .WithMany()
                .HasForeignKey(x => x.BankAccountTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MortgageBankAccountDetail>()
                .HasRequired(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MortgageBankDepositDetail>()
                .HasRequired(x => x.Bank)
                .WithMany()
                .HasForeignKey(x => x.BankId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MortgageBankDepositDetail>()
                .HasRequired(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Mortgage>().Property(x => x.Debt).HasPrecision(18, 2);
            modelBuilder.Entity<Mortgage>().Property(x => x.MortgageMoneyApproximately).HasPrecision(18, 2);

            modelBuilder.Entity<MortgageObjection>()
                .HasRequired(x => x.Mortgage)
                .WithMany()
                .HasForeignKey(x => x.MortgageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MortgageInquiry>()
                .HasMany(m => m.MortgageInquiryDetails)
                .WithRequired(m => m.MortgageInquiry)
                .HasForeignKey(m => m.MortgageInquiryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Setting>()
                .HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .WillCascadeOnDelete(false);
        }
    }
}