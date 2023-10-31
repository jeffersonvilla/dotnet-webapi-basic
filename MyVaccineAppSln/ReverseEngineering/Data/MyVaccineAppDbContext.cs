using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReverseEngineering.Models;

namespace ReverseEngineering.Data;

public partial class MyVaccineAppDbContext : DbContext
{
    public MyVaccineAppDbContext()
    {
    }

    public MyVaccineAppDbContext(DbContextOptions<MyVaccineAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Dependent> Dependents { get; set; }

    public virtual DbSet<FamilyGroup> FamilyGroups { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

    public virtual DbSet<VaccineCategory> VaccineCategories { get; set; }

    public virtual DbSet<VaccineRecord> VaccineRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost,1433;Initial Catalog=MyVaccineAppDb;Integrated Security=False;User Id=sa;Password=Abc.123456;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Allergies_UserId");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.Allergies).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Discriminator).HasDefaultValueSql("(N'')");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Dependent>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Dependents_UserId");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Dependents).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<FamilyGroup>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasMany(d => d.UsersUsers).WithMany(p => p.FamilyGroupsFamilyGroups)
                .UsingEntity<Dictionary<string, object>>(
                    "FamilyGroupUser",
                    r => r.HasOne<User>().WithMany().HasForeignKey("UsersUserId"),
                    l => l.HasOne<FamilyGroup>().WithMany().HasForeignKey("FamilyGroupsFamilyGroupId"),
                    j =>
                    {
                        j.HasKey("FamilyGroupsFamilyGroupId", "UsersUserId");
                        j.ToTable("FamilyGroupUser");
                        j.HasIndex(new[] { "UsersUserId" }, "IX_FamilyGroupUser_UsersUserId");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.AspNetUserId, "IX_Users_AspNetUserId");

            entity.Property(e => e.AspNetUserId).HasDefaultValueSql("(N'')");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.Users).HasForeignKey(d => d.AspNetUserId);
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<VaccineCategory>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.VaccinesVaccines).WithMany(p => p.CategoriesVaccineCategories)
                .UsingEntity<Dictionary<string, object>>(
                    "VaccineCategoryVaccine",
                    r => r.HasOne<Vaccine>().WithMany().HasForeignKey("VaccinesVaccineId"),
                    l => l.HasOne<VaccineCategory>().WithMany().HasForeignKey("CategoriesVaccineCategoryId"),
                    j =>
                    {
                        j.HasKey("CategoriesVaccineCategoryId", "VaccinesVaccineId");
                        j.ToTable("VaccineCategoryVaccines");
                        j.HasIndex(new[] { "VaccinesVaccineId" }, "IX_VaccineCategoryVaccines_VaccinesVaccineId");
                    });
        });

        modelBuilder.Entity<VaccineRecord>(entity =>
        {
            entity.HasIndex(e => e.DependentId, "IX_VaccineRecords_DependentId");

            entity.HasIndex(e => e.UserId, "IX_VaccineRecords_UserId");

            entity.HasIndex(e => e.VaccineId, "IX_VaccineRecords_VaccineId");

            entity.Property(e => e.AdministeredBy).HasMaxLength(255);
            entity.Property(e => e.AdministeredLocation).HasMaxLength(255);

            entity.HasOne(d => d.Dependent).WithMany(p => p.VaccineRecords).HasForeignKey(d => d.DependentId);

            entity.HasOne(d => d.User).WithMany(p => p.VaccineRecords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccineRecords).HasForeignKey(d => d.VaccineId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
