using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogicLayer;

public partial class VaccineScheduleDbContext : DbContext
{
    public VaccineScheduleDbContext()
    {
    }

    public VaccineScheduleDbContext(DbContextOptions<VaccineScheduleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ChildrenProfile> ChildrenProfiles { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS05; Database=VaccineScheduleDB; Uid=sa; Pwd=1234567890");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accounts__3214EC07DD2578D5");

            entity.HasIndex(e => e.Username, "UQ__Accounts__536C85E49E4F336D").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D105342FE23CB0").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LastUpdatedTime).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<ChildrenProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Children__3214EC070D510281");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ParentId).HasMaxLength(50);

            entity.HasOne(d => d.Parent).WithMany(p => p.ChildrenProfiles)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChildrenP__Paren__3F466844");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07BAC3B7E7");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ScheduleId).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(50);

            entity.HasOne(d => d.Schedule).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__Sched__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedbacks__UserI__4F7CD00D");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07676D9127");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.ScheduleId).HasMaxLength(50);

            entity.HasOne(d => d.Schedule).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Schedu__4AB81AF0");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC0723014FBA");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.ChildId).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Scheduled");
            entity.Property(e => e.VaccineId).HasMaxLength(50);

            entity.HasOne(d => d.Child).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ChildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__ChildI__44FF419A");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Vaccin__45F365D3");
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vaccines__3214EC074F688216");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.AgeGroup).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
