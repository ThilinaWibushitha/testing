using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pospointe.Models;

public partial class PosDb1Context : DbContext
{
    public PosDb1Context()
    {
    }

    public PosDb1Context(DbContextOptions<PosDb1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TbStation> TbStations { get; set; }

    public virtual DbSet<TblBulkInfo> TblBulkInfos { get; set; }

    public virtual DbSet<TblBusinessInfo> TblBusinessInfos { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblDayOpen> TblDayOpens { get; set; }

    public virtual DbSet<TblDayOpenCashCollection> TblDayOpenCashCollections { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblItem> TblItems { get; set; }

    public virtual DbSet<TblLoyalityPlan> TblLoyalityPlans { get; set; }

    public virtual DbSet<TblLoyalityPlanMember> TblLoyalityPlanMembers { get; set; }

    public virtual DbSet<TblMixnMatchGrp> TblMixnMatchGrps { get; set; }

    public virtual DbSet<TblMnMitem> TblMnMitems { get; set; }

    public virtual DbSet<TblModiferGroup> TblModiferGroups { get; set; }

    public virtual DbSet<TblModifersofItem> TblModifersofItems { get; set; }

    public virtual DbSet<TblProcessing> TblProcessings { get; set; }

    public virtual DbSet<TblSignature> TblSignatures { get; set; }

    public virtual DbSet<TblTaxRate> TblTaxRates { get; set; }

    public virtual DbSet<TblTempBillList> TblTempBillLists { get; set; }

    public virtual DbSet<TblTransMain> TblTransMains { get; set; }

    public virtual DbSet<TblTransSub> TblTransSubs { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(@"Server=LOCALHOST\POSPOINTE;Initial Catalog=PosDB;Persist Security Info=False;User ID=sa;Password=POS@573184;Trust Server Certificate=true;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {
        modelBuilder.Entity<TbStation>(entity =>
        {
            entity.ToTable("Tb_Station");

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

        modelBuilder.Entity<TblBulkInfo>(entity =>
        {
            entity.ToTable("Tbl_Bulk_Info");

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

        modelBuilder.Entity<TblBusinessInfo>(entity =>
        {
            entity.HasKey(e => e.StoreId);

            entity.ToTable("Tbl_Business_Info");

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

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("Tbl_Customers");

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

        modelBuilder.Entity<TblDayOpen>(entity =>
        {
            entity.HasKey(e => e.DayOpenId);

            entity.ToTable("Tbl_DayOpen");

            entity.Property(e => e.DayOpenId).HasColumnName("dayOpenID");
            entity.Property(e => e.CashierId)
                .HasMaxLength(10)
                .HasColumnName("cashier_id");
            entity.Property(e => e.ClosedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ClosingBalance)
                .HasColumnType("money")
                .HasColumnName("closing_balance");
            entity.Property(e => e.Date).HasColumnName("date");
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
            entity.ToTable("Tbl_DayOpenCashCollection");

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

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("Tbl_Departments");

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

        modelBuilder.Entity<TblItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.ToTable("Tbl_Items");

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

        modelBuilder.Entity<TblLoyalityPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId);

            entity.ToTable("Tbl_LoyalityPlan");

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
            entity.ToTable("Tbl_LoyalityPlanMembers");

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

        modelBuilder.Entity<TblMixnMatchGrp>(entity =>
        {
            entity.ToTable("Tbl_MixnMatchGrps");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.RequiredQty)
                .HasColumnType("money")
                .HasColumnName("RequiredQTY");
        });

        modelBuilder.Entity<TblMnMitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TblMnMItems");

            entity.ToTable("Tbl_MnMItems");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GroupId)
                .HasMaxLength(10)
                .HasColumnName("GroupID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
        });

        modelBuilder.Entity<TblModiferGroup>(entity =>
        {
            entity.HasKey(e => e.ModiferGroupId);

            entity.ToTable("Tbl_ModiferGroups");

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

        modelBuilder.Entity<TblModifersofItem>(entity =>
        {
            entity.ToTable("Tbl_ModifersofItems");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(30)
                .HasColumnName("ItemID");
            entity.Property(e => e.ModiferGroupId).HasColumnName("ModiferGroupID");
        });

        modelBuilder.Entity<TblProcessing>(entity =>
        {
            entity.HasKey(e => e.StationId);

            entity.ToTable("Tbl_Processing");

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

        modelBuilder.Entity<TblSignature>(entity =>
        {
            entity.HasKey(e => e.TransMainId);

            entity.ToTable("Tbl_Signature");

            entity.Property(e => e.TransMainId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("TransMainID");
            entity.Property(e => e.Signature).HasColumnType("image");
        });

        modelBuilder.Entity<TblTaxRate>(entity =>
        {
            entity.HasKey(e => e.TaxNo);

            entity.ToTable("Tbl_TaxRates");

            entity.Property(e => e.TaxNo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TaxRate).HasColumnType("money");
        });

        modelBuilder.Entity<TblTempBillList>(entity =>
        {
            entity.ToTable("Tbl_Temp_BillList");

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

        modelBuilder.Entity<TblTransMain>(entity =>
        {
            entity.HasKey(e => e.InvoiceId);

            entity.ToTable("Tbl_Trans_Main");

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
                .HasDefaultValue(0m)
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

        modelBuilder.Entity<TblTransSub>(entity =>
        {
            entity.HasKey(e => e.Idkey);

            entity.ToTable("Tbl_Trans_Sub");

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

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Tbl_Users");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
