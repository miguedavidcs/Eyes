using System.Security.Principal;
using Back.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =======================
        // DbSets (Tablas) 
        // =======================
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;

        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        // (Marketing para después)
        /*
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignLead> CampaignLeads { get; set; }
        */
        // =======================
        // Contabilidad / Empresa
        // =======================
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<AccountingPeriod> AccountingPeriods { get; set; } = null!;
        public DbSet<Voucher> Vouchers { get; set; } = null!;
        public DbSet<VoucherLine> VoucherLines { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =======================
            // User ↔ Role (UserRole)
            // =======================
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            // =======================
            // Role ↔ Permission
            // =======================
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasIndex(p => p.Code).IsUnique();

                entity.Property(p => p.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Description)
                    .HasMaxLength(255);
            });

            // =======================
            // Filtros globales (Soft Delete)
            // =======================
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDeleted);

            modelBuilder.Entity<Permission>()
                .HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<UserRole>()
                .HasQueryFilter(ur => !ur.IsDeleted);
            modelBuilder.Entity<RolePermission>()
                .HasQueryFilter(rp => !rp.Permission.IsDeleted);
            // =======================
            // Configuariones 2 Entidad Company
            // =======================
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasIndex(c => c.Nit).IsUnique();
            });
            // =======================
            // Configuariones 2 Entidad ComanyUser
            // =======================
            modelBuilder.Entity<CompanyUser>(entity =>
            {
                entity.HasIndex(cu => new { cu.CompanyId, cu.UserId }).IsUnique();

                entity.HasOne(cu => cu.Company)
                    .WithMany(c => c.CompanyUsers)
                    .HasForeignKey(cu => cu.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // =======================
            // Configuariones 2 Entidad Account
            // =======================
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(a => new { a.CompanyId, a.Code }).IsUnique();

                entity.HasOne(a => a.Company)
                    .WithMany(c => c.Accounts)
                    .HasForeignKey(a => a.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Parent)
                    .WithMany()
                    .HasForeignKey(a => a.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // =======================
            // Configuariones 2 Entidad AccountingPeriod
            // =======================
            modelBuilder.Entity<AccountingPeriod>(entity =>
            {
                entity.HasOne(p => p.Company)
                    .WithMany(c => c.Periods)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // =======================
            // Configuariones 2 Entidad Voucher
            // =======================
            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasIndex(v => new { v.CompanyId, v.VoucherType, v.Number }).IsUnique();

                entity.HasOne(v => v.Company)
                    .WithMany()
                    .HasForeignKey(v => v.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.AccountingPeriod)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(v => v.AccountingPeriodId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // =======================
            // Configuariones 2 Entidad VoucherLine
            // =======================
            modelBuilder.Entity<VoucherLine>(entity =>
            {
                entity.Property(vl => vl.DebitAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(vl => vl.CreditAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(vl => vl.Voucher)
                    .WithMany(v => v.Lines)
                    .HasForeignKey(vl => vl.VoucherId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(vl => vl.Account)
                    .WithMany()
                    .HasForeignKey(vl => vl.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
