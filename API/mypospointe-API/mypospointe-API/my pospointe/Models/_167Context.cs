using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace my_pospointe.Models;

public partial class _167Context : DbContext
{
    public _167Context()
    {
    }

    //public _167Context(DbContextOptions<_167Context> options)
    //    : base(options)
    //{
    //}
    public _167Context(string databaseConnection)
       : base()
    {
        ConnectionString = databaseConnection;
    }

    public string ConnectionString { get; set; }

    public virtual DbSet<ScopeInfo> ScopeInfos { get; set; }

    public virtual DbSet<TbStation> TbStations { get; set; }

    public virtual DbSet<TbStationTracking> TbStationTrackings { get; set; }

    public virtual DbSet<TblBulkInfo> TblBulkInfos { get; set; }

    public virtual DbSet<TblBulkInfoTracking> TblBulkInfoTrackings { get; set; }

    public virtual DbSet<TblBusinessInfo> TblBusinessInfos { get; set; }

    public virtual DbSet<TblBusinessInfoTracking> TblBusinessInfoTrackings { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblCustomersTracking> TblCustomersTrackings { get; set; }

    public virtual DbSet<TblDayOpen> TblDayOpens { get; set; }

    public virtual DbSet<TblDayOpenCashCollection> TblDayOpenCashCollections { get; set; }

    public virtual DbSet<TblDayOpenCashCollectionTracking> TblDayOpenCashCollectionTrackings { get; set; }

    public virtual DbSet<TblDayOpenTracking> TblDayOpenTrackings { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblDepartmentsTracking> TblDepartmentsTrackings { get; set; }

    public virtual DbSet<TblItem> TblItems { get; set; }

    public virtual DbSet<TblItemsTracking> TblItemsTrackings { get; set; }

    public virtual DbSet<TblLoyalityPlan> TblLoyalityPlans { get; set; }

    public virtual DbSet<TblLoyalityPlanMember> TblLoyalityPlanMembers { get; set; }

    public virtual DbSet<TblLoyalityPlanMembersTracking> TblLoyalityPlanMembersTrackings { get; set; }

    public virtual DbSet<TblLoyalityPlanTracking> TblLoyalityPlanTrackings { get; set; }

    public virtual DbSet<TblMixnMatchGrp> TblMixnMatchGrps { get; set; }

    public virtual DbSet<TblMixnMatchGrpsTracking> TblMixnMatchGrpsTrackings { get; set; }

    public virtual DbSet<TblMnMitem> TblMnMitems { get; set; }

    public virtual DbSet<TblMnMitemsTracking> TblMnMitemsTrackings { get; set; }

    public virtual DbSet<TblModiferGroup> TblModiferGroups { get; set; }

    public virtual DbSet<TblModiferGroupsTracking> TblModiferGroupsTrackings { get; set; }

    public virtual DbSet<TblModifersofItem> TblModifersofItems { get; set; }

    public virtual DbSet<TblModifersofItemsTracking> TblModifersofItemsTrackings { get; set; }

    public virtual DbSet<TblProcessing> TblProcessings { get; set; }

    public virtual DbSet<TblProcessingTracking> TblProcessingTrackings { get; set; }

    public virtual DbSet<TblTaxRate> TblTaxRates { get; set; }

    public virtual DbSet<TblTaxRatesTracking> TblTaxRatesTrackings { get; set; }

    public virtual DbSet<TblTempBillList> TblTempBillLists { get; set; }

    public virtual DbSet<TblTempBillListTracking> TblTempBillListTrackings { get; set; }

    public virtual DbSet<TblTransMain> TblTransMains { get; set; }

    public virtual DbSet<TblTransMainTracking> TblTransMainTrackings { get; set; }

    public virtual DbSet<TblTransSub> TblTransSubs { get; set; }

    public virtual DbSet<TblTransSubTracking> TblTransSubTrackings { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUsersTracking> TblUsersTrackings { get; set; }

   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
       
    {
        modelBuilder.Entity<ScopeInfo>(entity =>
        {
            entity.HasKey(e => e.SyncScopeId);

            entity.ToTable("scope_info");

            entity.Property(e => e.SyncScopeId)
                .ValueGeneratedNever()
                .HasColumnName("sync_scope_id");
            entity.Property(e => e.ScopeLastServerSyncTimestamp).HasColumnName("scope_last_server_sync_timestamp");
            entity.Property(e => e.ScopeLastSync)
                .HasColumnType("datetime")
                .HasColumnName("scope_last_sync");
            entity.Property(e => e.ScopeLastSyncDuration).HasColumnName("scope_last_sync_duration");
            entity.Property(e => e.ScopeLastSyncTimestamp).HasColumnName("scope_last_sync_timestamp");
            entity.Property(e => e.SyncScopeName)
                .HasMaxLength(100)
                .HasColumnName("sync_scope_name");
            entity.Property(e => e.SyncScopeSchema).HasColumnName("sync_scope_schema");
            entity.Property(e => e.SyncScopeSetup).HasColumnName("sync_scope_setup");
            entity.Property(e => e.SyncScopeVersion)
                .HasMaxLength(10)
                .HasColumnName("sync_scope_version");
        });

        modelBuilder.Entity<TbStation>(entity =>
        {
            entity.ToTable("Tb_Station", tb =>
                {
                    tb.HasTrigger("Tb_Station_delete_trigger");
                    tb.HasTrigger("Tb_Station_insert_trigger");
                    tb.HasTrigger("Tb_Station_update_trigger");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Kotprinter1)
                .HasMaxLength(50)
                .HasColumnName("KOTPrinter1");
            entity.Property(e => e.Kotprinter2)
                .HasMaxLength(50)
                .HasColumnName("KOTPrinter2");
            entity.Property(e => e.StationName).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<TbStationTracking>(entity =>
        {
            entity.ToTable("Tb_Station_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tb_Station_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblBulkInfo>(entity =>
        {
            entity.ToTable("Tbl_Bulk_Info", tb =>
                {
                    tb.HasTrigger("Tbl_Bulk_Info_delete_trigger");
                    tb.HasTrigger("Tbl_Bulk_Info_insert_trigger");
                    tb.HasTrigger("Tbl_Bulk_Info_update_trigger");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BulkPrice)
                .HasColumnType("money")
                .HasColumnName("Bulk_Price");
            entity.Property(e => e.Description).HasMaxLength(30);
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.Quantity).HasColumnType("money");
        });

        modelBuilder.Entity<TblBulkInfoTracking>(entity =>
        {
            entity.ToTable("Tbl_Bulk_Info_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_Bulk_Info_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblBusinessInfo>(entity =>
        {
            entity.HasKey(e => e.StoreId);

            entity.ToTable("Tbl_Business_Info", tb =>
                {
                    tb.HasTrigger("Tbl_Business_Info_delete_trigger");
                    tb.HasTrigger("Tbl_Business_Info_insert_trigger");
                    tb.HasTrigger("Tbl_Business_Info_update_trigger");
                });

            entity.Property(e => e.StoreId)
                .HasMaxLength(50)
                .HasColumnName("StoreID");
            entity.Property(e => e.AllowNegativeQty).HasColumnName("AllowNegativeQTY");
            entity.Property(e => e.BusinessAddress).HasMaxLength(50);
            entity.Property(e => e.BusinessEmail)
                .HasMaxLength(50)
                .HasColumnName("Business_Email");
            entity.Property(e => e.BusinessName).HasMaxLength(50);
            entity.Property(e => e.BusinessPhone)
                .HasMaxLength(50)
                .HasColumnName("Business_Phone");
            entity.Property(e => e.CityStatezip).HasMaxLength(50);
            entity.Property(e => e.EncryptionKey)
                .HasMaxLength(150)
                .HasColumnName("encryptionKey");
            entity.Property(e => e.Footer1)
                .HasMaxLength(50)
                .HasColumnName("footer1");
            entity.Property(e => e.Footer2)
                .HasMaxLength(50)
                .HasColumnName("footer2");
            entity.Property(e => e.Footer3)
                .HasMaxLength(50)
                .HasColumnName("footer3");
            entity.Property(e => e.Footer4)
                .HasMaxLength(50)
                .HasColumnName("footer4");
            entity.Property(e => e.LogoPath).HasColumnName("Logo_Path");
            entity.Property(e => e.MixNmatch).HasColumnName("MixNMatch");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(50)
                .HasColumnName("Owner_Name");
            entity.Property(e => e.OwnerPhone)
                .HasMaxLength(50)
                .HasColumnName("Owner_Phone");
        });

        modelBuilder.Entity<TblBusinessInfoTracking>(entity =>
        {
            entity.HasKey(e => e.StoreId);

            entity.ToTable("Tbl_Business_Info_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.StoreId }, "Tbl_Business_Info_tracking_timestamp_index");

            entity.Property(e => e.StoreId)
                .HasMaxLength(50)
                .HasColumnName("StoreID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Tbl_Customers", tb =>
                {
                    tb.HasTrigger("Tbl_Customers_delete_trigger");
                    tb.HasTrigger("Tbl_Customers_insert_trigger");
                    tb.HasTrigger("Tbl_Customers_update_trigger");
                });

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.BusinessName)
                .HasMaxLength(50)
                .HasColumnName("Business_Name");
            entity.Property(e => e.CardId).HasColumnName("CardID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("Created_Date");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .HasColumnName("Customer_Name");
            entity.Property(e => e.DiscountRate).HasColumnType("money");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.ZipCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCustomersTracking>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Tbl_Customers_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.CustomerId }, "Tbl_Customers_tracking_timestamp_index");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblDayOpen>(entity =>
        {
            entity.HasKey(e => e.DayOpenId);

            entity.ToTable("Tbl_DayOpen", tb =>
                {
                    tb.HasTrigger("Tbl_DayOpen_delete_trigger");
                    tb.HasTrigger("Tbl_DayOpen_insert_trigger");
                    tb.HasTrigger("Tbl_DayOpen_update_trigger");
                });

            entity.Property(e => e.DayOpenId).HasColumnName("dayOpenID");
            entity.Property(e => e.CashierId).HasColumnName("cashier_id");
            entity.Property(e => e.ClosedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ClosingBalance)
                .HasColumnType("money")
                .HasColumnName("closing_balance");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.DayClosingTime).HasColumnName("day_closing_time");
            entity.Property(e => e.DayOpeningTime).HasColumnName("day_opening_time");
            entity.Property(e => e.Deference)
                .HasColumnType("money")
                .HasColumnName("deference");
            entity.Property(e => e.OpenedDateTime).HasColumnType("datetime");
            entity.Property(e => e.OpeningBalance)
                .HasColumnType("money")
                .HasColumnName("opening_balance");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TblDayOpenCashCollection>(entity =>
        {
            entity.ToTable("Tbl_DayOpenCashCollection", tb =>
                {
                    tb.HasTrigger("Tbl_DayOpenCashCollection_delete_trigger");
                    tb.HasTrigger("Tbl_DayOpenCashCollection_insert_trigger");
                    tb.HasTrigger("Tbl_DayOpenCashCollection_update_trigger");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coin1).HasColumnName("coin1");
            entity.Property(e => e.Coin10).HasColumnName("coin10");
            entity.Property(e => e.Coin25).HasColumnName("coin25");
            entity.Property(e => e.Coin5).HasColumnName("coin5");
            entity.Property(e => e.Coin50).HasColumnName("coin50");
            entity.Property(e => e.DayOpenId).HasColumnName("dayOpenID");
            entity.Property(e => e.Note1).HasColumnName("note1");
            entity.Property(e => e.Note10).HasColumnName("note10");
            entity.Property(e => e.Note100).HasColumnName("note100");
            entity.Property(e => e.Note20).HasColumnName("note20");
            entity.Property(e => e.Note5).HasColumnName("note5");
            entity.Property(e => e.Note50).HasColumnName("note50");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        modelBuilder.Entity<TblDayOpenCashCollectionTracking>(entity =>
        {
            entity.ToTable("Tbl_DayOpenCashCollection_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_DayOpenCashCollection_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblDayOpenTracking>(entity =>
        {
            entity.HasKey(e => e.DayOpenId);

            entity.ToTable("Tbl_DayOpen_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.DayOpenId }, "Tbl_DayOpen_tracking_timestamp_index");

            entity.Property(e => e.DayOpenId)
                .ValueGeneratedNever()
                .HasColumnName("dayOpenID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("Tbl_Departments", tb =>
                {
                    tb.HasTrigger("Tbl_Departments_delete_trigger");
                    tb.HasTrigger("Tbl_Departments_insert_trigger");
                    tb.HasTrigger("Tbl_Departments_update_trigger");
                });

            entity.Property(e => e.DeptId)
                .HasMaxLength(15)
                .HasColumnName("DeptID");
            entity.Property(e => e.BtnColor).HasColumnName("BTN_Color");
            entity.Property(e => e.DeptName)
                .HasMaxLength(15)
                .HasColumnName("Dept_Name");
            entity.Property(e => e.NameVisible).HasMaxLength(2);
            entity.Property(e => e.PictureVisible).HasMaxLength(2);
            entity.Property(e => e.ShowinOrderTablet).HasColumnName("showinOrderTablet");
            entity.Property(e => e.Visible).HasMaxLength(2);
        });

        modelBuilder.Entity<TblDepartmentsTracking>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("Tbl_Departments_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.DeptId }, "Tbl_Departments_tracking_timestamp_index");

            entity.Property(e => e.DeptId)
                .HasMaxLength(15)
                .HasColumnName("DeptID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.ToTable("Tbl_Items", tb =>
            {
                tb.HasTrigger("Tbl_Items_delete_trigger");
                tb.HasTrigger("Tbl_Items_insert_trigger");
                tb.HasTrigger("Tbl_Items_update_trigger");
            });

            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.Brand).HasMaxLength(30);
            entity.Property(e => e.CasePrice).HasColumnType("money");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EnableName).HasMaxLength(2);
            entity.Property(e => e.EnablePicture).HasMaxLength(2);
            entity.Property(e => e.Idcheck).HasColumnName("IDCheck");
            entity.Property(e => e.IsKot).HasColumnName("isKOT");
            entity.Property(e => e.IsKot2).HasColumnName("isKOT2");
            entity.Property(e => e.ItemDeptId)
                .HasMaxLength(15)
                .HasColumnName("ItemDeptID");
            entity.Property(e => e.ItemName).HasMaxLength(30);
            entity.Property(e => e.ItemPrice).HasColumnType("money");
            entity.Property(e => e.IteminCase).HasColumnType("money");
            entity.Property(e => e.LastSold).HasColumnType("datetime");
            entity.Property(e => e.Lastimported)
                .HasColumnType("datetime")
                .HasColumnName("lastimported");
            entity.Property(e => e.LoyalityCredit).HasColumnType("money");
            entity.Property(e => e.OnlinePrice).HasColumnType("money");
            entity.Property(e => e.PricePrompt)
                .HasMaxLength(2)
                .HasColumnName("Price_Prompt");
            entity.Property(e => e.PromptDescription).HasColumnName("Prompt_Description");
            entity.Property(e => e.Qty)
                .HasColumnType("money")
                .HasColumnName("QTY");
            entity.Property(e => e.Soldby).HasMaxLength(10);
            entity.Property(e => e.Tax1Status)
                .HasMaxLength(2)
                .HasColumnName("Tax1_Status");
            entity.Property(e => e.Visible).HasMaxLength(2);
        });

        modelBuilder.Entity<TblItemsTracking>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.ToTable("Tbl_Items_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.ItemId }, "Tbl_Items_tracking_timestamp_index");

            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblLoyalityPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId);

            entity.ToTable("Tbl_LoyalityPlan", tb =>
                {
                    tb.HasTrigger("Tbl_LoyalityPlan_delete_trigger");
                    tb.HasTrigger("Tbl_LoyalityPlan_insert_trigger");
                    tb.HasTrigger("Tbl_LoyalityPlan_update_trigger");
                });

            entity.Property(e => e.PlanId)
                .ValueGeneratedNever()
                .HasColumnName("PlanID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreditAmount).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.DiscountMethod).HasColumnName("Discount_Method");
            entity.Property(e => e.DollersperCredit).HasColumnType("money");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.MaxCreditReach).HasColumnType("money");
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblLoyalityPlanMember>(entity =>
        {
            entity.ToTable("Tbl_LoyalityPlanMembers", tb =>
                {
                    tb.HasTrigger("Tbl_LoyalityPlanMembers_delete_trigger");
                    tb.HasTrigger("Tbl_LoyalityPlanMembers_insert_trigger");
                    tb.HasTrigger("Tbl_LoyalityPlanMembers_update_trigger");
                });

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CustomerCredit)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.JoinedDate)
                .HasColumnType("datetime")
                .HasColumnName("Joined_Date");
            entity.Property(e => e.LastupdatedDate).HasColumnType("datetime");
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
        });

        modelBuilder.Entity<TblLoyalityPlanMembersTracking>(entity =>
        {
            entity.ToTable("Tbl_LoyalityPlanMembers_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_LoyalityPlanMembers_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblLoyalityPlanTracking>(entity =>
        {
            entity.HasKey(e => e.PlanId);

            entity.ToTable("Tbl_LoyalityPlan_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.PlanId }, "Tbl_LoyalityPlan_tracking_timestamp_index");

            entity.Property(e => e.PlanId)
                .ValueGeneratedNever()
                .HasColumnName("PlanID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblMixnMatchGrp>(entity =>
        {
            entity.ToTable("Tbl_MixnMatchGrps", tb =>
                {
                    tb.HasTrigger("Tbl_MixnMatchGrps_delete_trigger");
                    tb.HasTrigger("Tbl_MixnMatchGrps_insert_trigger");
                    tb.HasTrigger("Tbl_MixnMatchGrps_update_trigger");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.RequiredQty)
                .HasColumnType("money")
                .HasColumnName("RequiredQTY");
        });

        modelBuilder.Entity<TblMixnMatchGrpsTracking>(entity =>
        {
            entity.ToTable("Tbl_MixnMatchGrps_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_MixnMatchGrps_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblMnMitem>(entity =>
        {
            entity.ToTable("Tbl_MnMItems", tb =>
                {
                    tb.HasTrigger("Tbl_MnMItems_delete_trigger");
                    tb.HasTrigger("Tbl_MnMItems_insert_trigger");
                    tb.HasTrigger("Tbl_MnMItems_update_trigger");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GroupId)
                .HasMaxLength(10)
                .HasColumnName("GroupID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
        });

        modelBuilder.Entity<TblMnMitemsTracking>(entity =>
        {
            entity.ToTable("Tbl_MnMItems_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_MnMItems_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblModiferGroup>(entity =>
        {
            entity.HasKey(e => e.ModiferGroupId);

            entity.ToTable("Tbl_ModiferGroups", tb =>
                {
                    tb.HasTrigger("Tbl_ModiferGroups_delete_trigger");
                    tb.HasTrigger("Tbl_ModiferGroups_insert_trigger");
                    tb.HasTrigger("Tbl_ModiferGroups_update_trigger");
                });

            entity.Property(e => e.ModiferGroupId)
                .ValueGeneratedNever()
                .HasColumnName("ModiferGroupID");
            entity.Property(e => e.BtnColor).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.GroupName).HasMaxLength(50);
            entity.Property(e => e.PhromptName)
                .HasMaxLength(50)
                .HasColumnName("Phrompt_Name");
        });

        modelBuilder.Entity<TblModiferGroupsTracking>(entity =>
        {
            entity.HasKey(e => e.ModiferGroupId);

            entity.ToTable("Tbl_ModiferGroups_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.ModiferGroupId }, "Tbl_ModiferGroups_tracking_timestamp_index");

            entity.Property(e => e.ModiferGroupId)
                .ValueGeneratedNever()
                .HasColumnName("ModiferGroupID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblModifersofItem>(entity =>
        {
            entity.ToTable("Tbl_ModifersofItems", tb =>
                {
                    tb.HasTrigger("Tbl_ModifersofItems_delete_trigger");
                    tb.HasTrigger("Tbl_ModifersofItems_insert_trigger");
                    tb.HasTrigger("Tbl_ModifersofItems_update_trigger");
                });

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.ModiferGroupId).HasColumnName("ModiferGroupID");
        });

        modelBuilder.Entity<TblModifersofItemsTracking>(entity =>
        {
            entity.ToTable("Tbl_ModifersofItems_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_ModifersofItems_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblProcessing>(entity =>
        {
            entity.HasKey(e => e.StationId);

            entity.ToTable("Tbl_Processing", tb =>
                {
                    tb.HasTrigger("Tbl_Processing_delete_trigger");
                    tb.HasTrigger("Tbl_Processing_insert_trigger");
                    tb.HasTrigger("Tbl_Processing_update_trigger");
                });

            entity.Property(e => e.StationId)
                .ValueGeneratedNever()
                .HasColumnName("StationID");
            entity.Property(e => e.BusinessName).HasColumnName("Business_Name");
            entity.Property(e => e.MerchantId).HasColumnName("Merchant_ID");
            entity.Property(e => e.PaxIp).HasColumnName("Pax_IP");
            entity.Property(e => e.PaxPort).HasColumnName("Pax_Port");
            entity.Property(e => e.ProcessingMethod).HasColumnName("Processing_Method");
            entity.Property(e => e.TipRequest).HasColumnName("Tip_Request");
        });

        modelBuilder.Entity<TblProcessingTracking>(entity =>
        {
            entity.HasKey(e => e.StationId);

            entity.ToTable("Tbl_Processing_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.StationId }, "Tbl_Processing_tracking_timestamp_index");

            entity.Property(e => e.StationId)
                .ValueGeneratedNever()
                .HasColumnName("StationID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblTaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxNo);

            entity.ToTable("Tbl_TaxRates", tb =>
                {
                    tb.HasTrigger("Tbl_TaxRates_delete_trigger");
                    tb.HasTrigger("Tbl_TaxRates_insert_trigger");
                    tb.HasTrigger("Tbl_TaxRates_update_trigger");
                });

            entity.Property(e => e.TaxNo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TaxRate).HasColumnType("money");
        });

        modelBuilder.Entity<TblTaxRatesTracking>(entity =>
        {
            entity.HasKey(e => e.TaxNo);

            entity.ToTable("Tbl_TaxRates_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.TaxNo }, "Tbl_TaxRates_tracking_timestamp_index");

            entity.Property(e => e.TaxNo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblTempBillList>(entity =>
        {
            entity.ToTable("Tbl_Temp_BillList", tb =>
                {
                    tb.HasTrigger("Tbl_Temp_BillList_delete_trigger");
                    tb.HasTrigger("Tbl_Temp_BillList_insert_trigger");
                    tb.HasTrigger("Tbl_Temp_BillList_update_trigger");
                });

            entity.Property(e => e.Id)
                .HasMaxLength(30)
                .HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.ItemDeptId)
                .HasMaxLength(15)
                .HasColumnName("ItemDeptID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.ItemName).HasMaxLength(30);
            entity.Property(e => e.ItemPrice).HasColumnType("money");
            entity.Property(e => e.Qty).HasColumnType("money");
            entity.Property(e => e.Tax1Rate)
                .HasColumnType("money")
                .HasColumnName("Tax1_Rate");
        });

        modelBuilder.Entity<TblTempBillListTracking>(entity =>
        {
            entity.ToTable("Tbl_Temp_BillList_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Id }, "Tbl_Temp_BillList_tracking_timestamp_index");

            entity.Property(e => e.Id)
                .HasMaxLength(30)
                .HasColumnName("ID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblTransMain>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.ToTable("Tbl_Trans_Main", tb =>
                {
                    tb.HasTrigger("Tbl_Trans_Main_delete_trigger");
                    tb.HasTrigger("Tbl_Trans_Main_insert_trigger");
                    tb.HasTrigger("Tbl_Trans_Main_update_trigger");
                });

            entity.Property(e => e.InvoiceId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("InvoiceID");
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .HasColumnName("Account_Type");
            entity.Property(e => e.Aid).HasColumnName("AID");
            entity.Property(e => e.CardAmount).HasColumnType("money");
            entity.Property(e => e.CardHolder).HasMaxLength(50);
            entity.Property(e => e.CardNumber).HasMaxLength(50);
            entity.Property(e => e.CardType).HasMaxLength(50);
            entity.Property(e => e.CashAmount).HasColumnType("money");
            entity.Property(e => e.CashChangeAmount).HasColumnType("money");
            entity.Property(e => e.CashierId)
                .HasMaxLength(50)
                .HasColumnName("CashierID");
            entity.Property(e => e.CheckNumber).HasMaxLength(50);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(40)
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(40);
            entity.Property(e => e.DeviceOrgRefNum).HasColumnName("Device_Org_Ref_Num");
            entity.Property(e => e.EntryMethod)
                .HasMaxLength(50)
                .HasColumnName("Entry_Method");
            entity.Property(e => e.GiftCardNumber).HasMaxLength(50);
            entity.Property(e => e.GrandTotal).HasColumnType("money");
            entity.Property(e => e.HoldName).HasMaxLength(15);
            entity.Property(e => e.HostRefNum).HasColumnName("Host_Ref_Num");
            entity.Property(e => e.Href).HasColumnName("HRef");
            entity.Property(e => e.InvoiceDiscount)
                .HasDefaultValueSql("((0))")
                .HasColumnType("money")
                .HasColumnName("invoiceDiscount");
            entity.Property(e => e.LoyaltyDiscount).HasColumnType("money");
            entity.Property(e => e.Paidby).HasMaxLength(50);
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(50)
                .HasColumnName("phoneNo");
            entity.Property(e => e.Retref)
                .HasMaxLength(50)
                .HasColumnName("retref");
            entity.Property(e => e.SaleDate).HasColumnType("date");
            entity.Property(e => e.SaleDateTime).HasColumnType("datetime");
            entity.Property(e => e.StationId)
                .HasMaxLength(50)
                .HasColumnName("StationID");
            entity.Property(e => e.Subtotal).HasColumnType("money");
            entity.Property(e => e.Tax1).HasColumnType("money");
            entity.Property(e => e.Tcarqc).HasColumnName("TCARQC");
            entity.Property(e => e.TipAmount).HasColumnType("money");
            entity.Property(e => e.TotalCredit).HasColumnType("money");
            entity.Property(e => e.TransType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTransMainTracking>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.ToTable("Tbl_Trans_Main_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.InvoiceId }, "Tbl_Trans_Main_tracking_timestamp_index");

            entity.Property(e => e.InvoiceId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("InvoiceID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblTransSub>(entity =>
        {
            entity.HasKey(e => e.Idkey);

            entity.ToTable("Tbl_Trans_Sub", tb =>
                {
                    tb.HasTrigger("Tbl_Trans_Sub_delete_trigger");
                    tb.HasTrigger("Tbl_Trans_Sub_insert_trigger");
                    tb.HasTrigger("Tbl_Trans_Sub_update_trigger");
                });

            entity.Property(e => e.Idkey)
                .HasMaxLength(80)
                .HasColumnName("IDkey");
            entity.Property(e => e.ActualPrice).HasColumnType("money");
            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Credits).HasColumnType("money");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("ID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.ItemName).HasMaxLength(50);
            entity.Property(e => e.ItemPrice).HasColumnType("money");
            entity.Property(e => e.ItemType).HasMaxLength(50);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Qty).HasColumnType("money");
            entity.Property(e => e.SaleDateTime).HasColumnType("datetime");
            entity.Property(e => e.Tax1).HasColumnType("money");
            entity.Property(e => e.Tax1Status)
                .HasMaxLength(2)
                .HasColumnName("Tax1_Status");
            entity.Property(e => e.TransMainId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("TransMainID");
        });

        modelBuilder.Entity<TblTransSubTracking>(entity =>
        {
            entity.HasKey(e => e.Idkey);

            entity.ToTable("Tbl_Trans_Sub_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.Idkey }, "Tbl_Trans_Sub_tracking_timestamp_index");

            entity.Property(e => e.Idkey)
                .HasMaxLength(80)
                .HasColumnName("IDkey");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_Users", tb =>
                {
                    tb.HasTrigger("Tbl_Users_delete_trigger");
                    tb.HasTrigger("Tbl_Users_insert_trigger");
                    tb.HasTrigger("Tbl_Users_update_trigger");
                });

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .HasColumnName("UserID");
            entity.Property(e => e.UserBackEnd).HasMaxLength(50);
            entity.Property(e => e.UserDashBoard).HasMaxLength(50);
            entity.Property(e => e.UserEndDayPeform).HasMaxLength(50);
            entity.Property(e => e.UserPin).HasColumnName("UserPIN");
            entity.Property(e => e.UserPos)
                .HasMaxLength(50)
                .HasColumnName("UserPOS");
            entity.Property(e => e.UserStatus).HasMaxLength(2);
        });

        modelBuilder.Entity<TblUsersTracking>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_Users_tracking");

            entity.HasIndex(e => new { e.TimestampBigint, e.UpdateScopeId, e.SyncRowIsTombstone, e.UserId }, "Tbl_Users_tracking_timestamp_index");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .HasColumnName("UserID");
            entity.Property(e => e.LastChangeDatetime)
                .HasColumnType("datetime")
                .HasColumnName("last_change_datetime");
            entity.Property(e => e.SyncRowIsTombstone).HasColumnName("sync_row_is_tombstone");
            entity.Property(e => e.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("timestamp");
            entity.Property(e => e.TimestampBigint)
                .HasComputedColumnSql("(CONVERT([bigint],[timestamp]))", true)
                .HasColumnName("timestamp_bigint");
            entity.Property(e => e.UpdateScopeId).HasColumnName("update_scope_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
