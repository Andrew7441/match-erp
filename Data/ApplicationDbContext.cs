using System;
using System.Collections.Generic;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //Map tables using EF Core
    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27DA083E6D");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105347879693F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        // Stores money-like values as decimal(18,2) in SQL Server.
        modelBuilder.Entity<Staff>()
            .Property(s => s.income)
            .HasPrecision(18, 2);

        // Stores item price as decimal(18,2) in SQL Server.
        modelBuilder.Entity<InventoryItem>()
            .Property(i => i.UnitPrice)
            .HasPrecision(18, 2);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
