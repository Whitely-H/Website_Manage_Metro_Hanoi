using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Models;

public partial class HanoiMetroDbContext : DbContext
{
    public HanoiMetroDbContext()
    {
    }

    public HanoiMetroDbContext(DbContextOptions<HanoiMetroDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<Line> Lines { get; set; }

    public virtual DbSet<LineStation> LineStations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SmartCard> SmartCards { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-UO1B46CE\\SQLEXPRESS;Database=HanoiMetroDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586F38F26B3");

            entity.HasIndex(e => e.Username, "UQ__Accounts__536C85E439CA08DE").IsUnique();

            entity.Property(e => e.AccountId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("AccountID");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.StationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("StationID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Station).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__Accounts__Statio__5FB337D6");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("PK__Incident__3D8053926CFC8DE2");

            entity.Property(e => e.IncidentId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IncidentID");
            entity.Property(e => e.LineId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LineID");
            entity.Property(e => e.ReportedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Severity).HasMaxLength(50);
            entity.Property(e => e.StationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("StationID");

            entity.HasOne(d => d.Line).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.LineId)
                .HasConstraintName("FK__Incidents__LineI__48CFD27E");

            entity.HasOne(d => d.Station).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK__Incidents__Stati__49C3F6B7");
        });

        modelBuilder.Entity<Line>(entity =>
        {
            entity.HasKey(e => e.LineID).HasName("PK__Lines__2EAE64C9D495FAB8");

            entity.Property(e => e.LineID)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LineID");
            entity.Property(e => e.ColorCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LineName).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Hoạt động");
        });

        modelBuilder.Entity<LineStation>(entity =>
        {
            entity.HasKey(e => new { e.LineId, e.StationId }).HasName("PK__LineStat__E0A3EEA4F89E9B59");

            entity.Property(e => e.LineId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LineID");
            entity.Property(e => e.StationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("StationID");
            entity.Property(e => e.DistanceToNext).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Line).WithMany(p => p.LineStations)
                .HasForeignKey(d => d.LineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LineStati__LineI__3C69FB99");

            entity.HasOne(d => d.Station).WithMany(p => p.LineStations)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LineStati__Stati__3D5E1FD2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.Property(e => e.OrderId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("OrderID");

            entity.Property(e => e.PassengerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PassengerID");

            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.TicketTypeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Passenger)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.PassengerId);

            entity.HasOne(d => d.TicketType)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.TicketTypeId);
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.PassengerId);

            entity.Property(e => e.PassengerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PassengerID");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.FullName).HasMaxLength(100);

            entity.Property(e => e.IdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PassengerType)
                .HasMaxLength(50)
                .HasDefaultValue("Bình thường");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69C97AD244");

            entity.Property(e => e.ScheduleId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ScheduleID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.Direction).HasMaxLength(50);
            entity.Property(e => e.TrainId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TrainID");

            entity.HasOne(d => d.Train).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Train__44FF419A");
        });

        modelBuilder.Entity<SmartCard>(entity =>
        {
            entity.HasKey(e => e.CardId);

            entity.HasIndex(e => e.NfcCode).IsUnique();

            entity.Property(e => e.CardId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CardID");

            entity.Property(e => e.PassengerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PassengerID");

            entity.Property(e => e.Balance)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.NfcCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NFC_Code");

            entity.HasOne(d => d.Passenger)
                .WithMany(p => p.SmartCards)
                .HasForeignKey(d => d.PassengerId);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Stations__E0D8A6DD8BEAD903");

            entity.Property(e => e.StationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("StationID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.StationName).HasMaxLength(100);
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__TicketTy__6CD6845111E034FD");

            entity.Property(e => e.TicketTypeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TicketTypeID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.TrainId).HasName("PK__Trains__8ED2725A69CC3E2B");

            entity.HasIndex(e => e.TrainCode, "UQ__Trains__2AED9B98C21FF1D9").IsUnique();

            entity.Property(e => e.TrainId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TrainID");
            entity.Property(e => e.LineId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LineID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Hoạt động");
            entity.Property(e => e.TrainCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Line).WithMany(p => p.Trains)
                .HasForeignKey(d => d.LineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trains__LineID__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
