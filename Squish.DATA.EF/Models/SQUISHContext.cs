using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Squish.DATA.EF.Models
{
    public partial class SQUISHContext : DbContext
    {
        public SQUISHContext()
        {
        }

        public SQUISHContext(DbContextOptions<SQUISHContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole>? AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim>? AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser>? AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim>? AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin>? AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken>? AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Order>? Orders { get; set; } = null!;
        public virtual DbSet<ShippingInformation>? ShippingInformations { get; set; } = null!;
        public virtual DbSet<SquishInformation>? SquishInformations { get; set; } = null!;
        public virtual DbSet<SquishSpecy>? SquishSpecies { get; set; } = null!;
        public virtual DbSet<Status>? Statuses { get; set; } = null!;
        public virtual DbSet<UserAccountInfo>? UserAccountInfo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=SQUISH;Trusted_Connection=true;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderID");

                entity.Property(e => e.SquishId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SquishID");

                entity.HasOne(d => d.Squish)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SquishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Squish1");
            });

            modelBuilder.Entity<ShippingInformation>(entity =>
            {
                entity.HasKey(e => e.ShippingId);

                entity.ToTable("ShippingInformation");

                entity.Property(e => e.ShippingId)
                    .ValueGeneratedNever()
                    .HasColumnName("ShippingID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OrderID");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(250)
                    .HasColumnName("UserID");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ShippingInformations)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShippingInformation_Orders");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShippingInformation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShippingInformation_UserAccountInfo");
            });

            modelBuilder.Entity<SquishInformation>(entity =>
            {
                entity.HasKey(e => e.SquishId)
                    .HasName("PK_Squish");

                entity.ToTable("SquishInformation");

                entity.Property(e => e.SquishId)
                    .ValueGeneratedNever()
                    .HasColumnName("SquishID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.SpeciesId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SpeciesID");

                entity.Property(e => e.SquishColor).HasMaxLength(50);

                entity.Property(e => e.SquishSize).HasMaxLength(50);

                entity.Property(e => e.Squishname).HasMaxLength(150);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Species)
                    .WithMany(p => p.SquishInformation)
                    .HasForeignKey(d => d.SpeciesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Squish_SquishSpecies1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SquishInformations)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_SquishInformation_Status");
            });

            modelBuilder.Entity<SquishSpecy>(entity =>
            {
                entity.HasKey(e => e.SpeciesId);

                entity.Property(e => e.SpeciesId)
                    .ValueGeneratedNever()
                    .HasColumnName("SpeciesID");

                entity.Property(e => e.SpeciesDescription).HasMaxLength(50);

                entity.Property(e => e.SpeciesName).HasMaxLength(120);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("StatusID");
            });

            modelBuilder.Entity<UserAccountInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserAccountInfo");

                entity.Property(e => e.UserId)
                    .HasMaxLength(250)
                    .HasColumnName("UserID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
