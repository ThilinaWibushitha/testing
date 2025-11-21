using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace my_pospointe.Models;

public partial class PosavanceInventoryContext : DbContext
{
    public PosavanceInventoryContext()
    {
    }

    public PosavanceInventoryContext(DbContextOptions<PosavanceInventoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PoItem> PoItems { get; set; }

    public virtual DbSet<PosSummery> PosSummeries { get; set; }

    public virtual DbSet<my_pospointe_api.Models.TblSyncTransaction> TblSyncTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PoItem>(entity =>
        {
            entity.ToTable("PO_Items");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CostPerCase).HasColumnType("money");
            entity.Property(e => e.CostPerItem).HasColumnType("money");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.ItemInCase).HasColumnType("money");
            entity.Property(e => e.ItemName).HasMaxLength(30);
            entity.Property(e => e.Notes).HasMaxLength(30);
            entity.Property(e => e.Poid).HasColumnName("POID");
            entity.Property(e => e.Qtydamaged)
                .HasColumnType("money")
                .HasColumnName("QTYDamaged");
            entity.Property(e => e.Qtyordered)
                .HasColumnType("money")
                .HasColumnName("QTYOrdered");
            entity.Property(e => e.Qtyreceived)
                .HasColumnType("money")
                .HasColumnName("QTYReceived");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
        });

        modelBuilder.Entity<PosSummery>(entity =>
        {
            entity.ToTable("POS_Summery");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedFrom).HasMaxLength(30);
            entity.Property(e => e.CreatedPerson).HasMaxLength(30);
            entity.Property(e => e.ExpDate).HasColumnType("datetime");
            entity.Property(e => e.IdforStore)
                .ValueGeneratedOnAdd()
                .HasColumnName("IDforStore");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.ShippingCost).HasColumnType("money");
            entity.Property(e => e.Status).HasMaxLength(15);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.SubCost).HasColumnType("money");
            entity.Property(e => e.TotalCost).HasColumnType("money");
            entity.Property(e => e.VenderName).HasMaxLength(30);
            entity.Property(e => e.VendorId)
                .HasMaxLength(40)
                .HasColumnName("VendorID");
        });

        modelBuilder.Entity<my_pospointe_api.Models.TblSyncTransaction>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
