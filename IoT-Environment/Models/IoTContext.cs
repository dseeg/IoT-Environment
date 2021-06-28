using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IoT_Environment.Models
{
    public partial class IoTContext : DbContext
    {
        public IoTContext()
        {
        }

        public IoTContext(DbContextOptions<IoTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataType> DataTypes { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Relay> Relays { get; set; }
        public virtual DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DataType>(entity =>
            {
                entity.ToTable("DataType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasMaxLength(20)
                    .IsUnicode();
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Active).HasDefaultValueSql("('true')");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ConnectionType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Unknown')");

                entity.Property(e => e.DateRegistered).HasPrecision(2);

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.RelayNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.Relay)
                    .HasConstraintName("FK__Device__Relay__20C1E124");
            });

            modelBuilder.Entity<Relay>(entity =>
            {
                entity.ToTable("Relay");

                entity.HasIndex(e => e.PhysicalAddress, "UC_Relay_PhysicalAddress")
                    .IsUnique();

                entity.Property(e => e.DateRegistered).HasPrecision(2);

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetworkAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhysicalAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stale).HasDefaultValueSql("('false')");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Posted).HasPrecision(2);

                entity.Property(e => e.Value).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.DataTypeNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.DataType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__DataType__276EDEB3");

                entity.HasOne(d => d.DeviceNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.Device)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__Device__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
