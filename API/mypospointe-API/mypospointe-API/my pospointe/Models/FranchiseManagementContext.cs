using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace my_pospointe.Models;

public partial class FranchiseManagementContext : DbContext
{
    public FranchiseManagementContext()
    {
    }

    public FranchiseManagementContext(DbContextOptions<FranchiseManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgreeSignature> AgreeSignatures { get; set; }

    public virtual DbSet<Agreement> Agreements { get; set; }

    public virtual DbSet<Cctranscation> Cctranscations { get; set; }

    public virtual DbSet<FrApplication> FrApplications { get; set; }

    public virtual DbSet<FrLeadForm> FrLeadForms { get; set; }

    public virtual DbSet<Franchise> Franchises { get; set; }

    public virtual DbSet<Fstore> Fstores { get; set; }

    public virtual DbSet<InvoiceMain> InvoiceMains { get; set; }

    public virtual DbSet<InvoiceSub> InvoiceSubs { get; set; }

    public virtual DbSet<OtherSalesofStore> OtherSalesofStores { get; set; }

    public virtual DbSet<Pipleline> Piplelines { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskTemplate> TaskTemplates { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<ValueBinding> ValueBindings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgreeSignature>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AgreementId).HasColumnName("AgreementID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.SignedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Agreement>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
        });

        modelBuilder.Entity<Cctranscation>(entity =>
        {
            entity.ToTable("CCTranscations");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.Amount).HasMaxLength(13);
            entity.Property(e => e.AuthCode).HasMaxLength(10);
            entity.Property(e => e.Avsresp)
                .HasMaxLength(5)
                .HasColumnName("AVSresp");
            entity.Property(e => e.Bin)
                .HasMaxLength(30)
                .HasColumnName("BIN");
            entity.Property(e => e.Cvvresp)
                .HasMaxLength(3)
                .HasColumnName("CVVResp");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Emv).HasColumnName("EMV");
            entity.Property(e => e.Expiry).HasMaxLength(5);
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.MerchId)
                .HasMaxLength(15)
                .HasColumnName("MerchID");
            entity.Property(e => e.MerchantService).HasMaxLength(2);
            entity.Property(e => e.PaidByAch).HasColumnName("PaidByACH");
            entity.Property(e => e.Respproc).HasMaxLength(5);
            entity.Property(e => e.Respstat).HasMaxLength(2);
            entity.Property(e => e.Retref).HasMaxLength(15);
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Token).HasMaxLength(20);
        });

        modelBuilder.Entity<FrApplication>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AccntsPble).HasColumnType("money");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AmofCrDb).HasColumnType("money");
            entity.Property(e => e.Area1).HasMaxLength(50);
            entity.Property(e => e.Area2).HasMaxLength(50);
            entity.Property(e => e.Area3).HasMaxLength(50);
            entity.Property(e => e.AssetComment)
                .HasMaxLength(100)
                .HasColumnName("Asset_Comment");
            entity.Property(e => e.Cash).HasColumnType("money");
            entity.Property(e => e.Citizenship).HasMaxLength(30);
            entity.Property(e => e.CurrentEmployer).HasMaxLength(50);
            entity.Property(e => e.CurrentPosition).HasMaxLength(50);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmployerLocation).HasMaxLength(70);
            entity.Property(e => e.FinComment)
                .HasMaxLength(200)
                .HasColumnName("Fin_Comment");
            entity.Property(e => e.FinancingtheBalance).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FirstRestOpening).HasMaxLength(50);
            entity.Property(e => e.FranchiseHowNtype).HasColumnName("FranchiseHowNType");
            entity.Property(e => e.HighEducLevel).HasMaxLength(50);
            entity.Property(e => e.HomeValue).HasColumnType("money");
            entity.Property(e => e.HowHeardUs).HasMaxLength(50);
            entity.Property(e => e.InSource1).HasColumnType("money");
            entity.Property(e => e.InSource2).HasColumnType("money");
            entity.Property(e => e.InSource3).HasColumnType("money");
            entity.Property(e => e.IncmComment)
                .HasMaxLength(50)
                .HasColumnName("Incm_Comment");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LibComnts).HasMaxLength(50);
            entity.Property(e => e.LiqCapital).HasColumnType("money");
            entity.Property(e => e.MartableSec).HasColumnType("money");
            entity.Property(e => e.MartialStatus).HasColumnName("Martial_Status");
            entity.Property(e => e.MonthlySalery).HasColumnType("money");
            entity.Property(e => e.MortageDbt).HasColumnType("money");
            entity.Property(e => e.NameOfCompany).HasMaxLength(50);
            entity.Property(e => e.NumberofDependents).HasMaxLength(50);
            entity.Property(e => e.OtherAsset).HasColumnType("money");
            entity.Property(e => e.OtherLible).HasColumnType("money");
            entity.Property(e => e.OtherValue).HasColumnType("money");
            entity.Property(e => e.PrefPhoneNum)
                .HasMaxLength(15)
                .HasColumnName("Pref_PhoneNum");
            entity.Property(e => e.RecentEmplocation)
                .HasMaxLength(70)
                .HasColumnName("RecentEMPLocation");
            entity.Property(e => e.RecentEmployer).HasMaxLength(50);
            entity.Property(e => e.RecentPosition)
                .HasMaxLength(50)
                .HasColumnName("REcentPosition");
            entity.Property(e => e.RufrancOwner).HasColumnName("RUFrancOwner");
            entity.Property(e => e.SecuredNtspayble).HasColumnType("money");
            entity.Property(e => e.SoruceofDbCr).HasMaxLength(50);
            entity.Property(e => e.TaxPayble).HasColumnType("money");
            entity.Property(e => e.UnSecNtpayble).HasColumnType("money");
            entity.Property(e => e.WhenforMeeting).HasMaxLength(50);
            entity.Property(e => e.WhenforMeetingOth)
                .HasMaxLength(50)
                .HasColumnName("WhenforMeeting_Oth");
            entity.Property(e => e.WhereOpenRest).HasMaxLength(50);
            entity.Property(e => e.WhyShahs).HasMaxLength(150);
        });

        modelBuilder.Entity<FrLeadForm>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.BackNexp)
                .HasMaxLength(200)
                .HasColumnName("BackNExp");
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CurrentOccup).HasMaxLength(50);
            entity.Property(e => e.CurrentStatus).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .HasColumnName("FName");
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.InComments)
                .HasMaxLength(50)
                .HasColumnName("In_Comments");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .HasColumnName("LName");
            entity.Property(e => e.NetWorth).HasMaxLength(50);
            entity.Property(e => e.OpenTime).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PreferedCityState).HasMaxLength(50);
        });

        modelBuilder.Entity<Franchise>(entity =>
        {
            entity.HasKey(e => e.FranchiseId).HasName("PK_TblFranchises");

            entity.Property(e => e.FranchiseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("FranchiseID");
            entity.Property(e => e.BillOrgId).HasColumnName("BillOrgID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DefaultLoyaltyFee).HasColumnType("money");
            entity.Property(e => e.DeviceIp)
                .HasMaxLength(50)
                .HasColumnName("DeviceIP");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FranchiseAddress).HasMaxLength(50);
            entity.Property(e => e.FranchiseEmail).HasMaxLength(50);
            entity.Property(e => e.FranchiseName).HasMaxLength(30);
            entity.Property(e => e.FranchisePhone).HasMaxLength(15);
            entity.Property(e => e.FranchisesStateCityZip).HasMaxLength(70);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MerchantId)
                .HasMaxLength(25)
                .HasColumnName("MerchantID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Port).HasMaxLength(10);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.Processor).HasMaxLength(15);
            entity.Property(e => e.StripeClientId).HasColumnName("StripeClientID");
            entity.Property(e => e.Tax1).HasColumnType("money");
            entity.Property(e => e.Tax2).HasColumnType("money");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Fstore>(entity =>
        {
            entity.ToTable("FStores");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.BillCustomerId)
                .HasMaxLength(50)
                .HasColumnName("BillCustomerID");
            entity.Property(e => e.BillDefaultPayment).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.LegalBusiness)
                .HasMaxLength(70)
                .HasColumnName("Legal_Business");
            entity.Property(e => e.LocalMarketing).HasColumnType("money");
            entity.Property(e => e.LoyaltyFee).HasColumnType("money");
            entity.Property(e => e.Marketing).HasColumnType("money");
            entity.Property(e => e.PaymentProfileId)
                .HasMaxLength(100)
                .HasColumnName("PaymentProfileID");
            entity.Property(e => e.State).HasMaxLength(3);
            entity.Property(e => e.StoreCity).HasMaxLength(50);
            entity.Property(e => e.StoreDb)
                .HasMaxLength(50)
                .HasColumnName("StoreDB");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.TaxId)
                .HasMaxLength(50)
                .HasColumnName("Tax_ID");
        });

        modelBuilder.Entity<InvoiceMain>(entity =>
        {
            entity.ToTable("InvoiceMain");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.BillBankId)
                .HasMaxLength(50)
                .HasColumnName("BillBankID");
            entity.Property(e => e.BillCustomerId)
                .HasMaxLength(50)
                .HasColumnName("BillCustomerID");
            entity.Property(e => e.BillInvoiceId)
                .HasMaxLength(50)
                .HasColumnName("BillInvoiceID");
            entity.Property(e => e.BillPaymentId)
                .HasMaxLength(50)
                .HasColumnName("BillPaymentID");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(30);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("money");
            entity.Property(e => e.DiscountDecription).HasMaxLength(50);
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.GrandTotal).HasColumnType("money");
            entity.Property(e => e.InvoiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("InvoiceID");
            entity.Property(e => e.InvoicePeriod).HasMaxLength(6);
            entity.Property(e => e.ManualPaymentType).HasMaxLength(50);
            entity.Property(e => e.PaidAmount).HasMaxLength(15);
            entity.Property(e => e.Status).HasMaxLength(15);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.SubTotal).HasColumnType("money");
            entity.Property(e => e.Tax1).HasColumnType("money");
            entity.Property(e => e.Tax2).HasColumnType("money");
            entity.Property(e => e.TransType).HasMaxLength(15);
        });

        modelBuilder.Entity<InvoiceSub>(entity =>
        {
            entity.ToTable("InvoiceSub");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.InvoiceMainId).HasColumnName("InvoiceMainID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Qty)
                .HasColumnType("money")
                .HasColumnName("QTY");
            entity.Property(e => e.TaxAmount).HasColumnType("money");
        });

        modelBuilder.Entity<OtherSalesofStore>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.DoorDash).HasColumnType("money");
            entity.Property(e => e.Grabhub).HasColumnType("money");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.Others).HasColumnType("money");
            entity.Property(e => e.Period).HasMaxLength(6);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.UberEats).HasColumnType("money");
        });

        modelBuilder.Entity<Pipleline>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.PipelineId).HasColumnName("PipelineID");
            entity.Property(e => e.TaskDescription).HasMaxLength(50);
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.TaskName).HasMaxLength(50);
            entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
        });

        modelBuilder.Entity<TaskTemplate>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.FranchiseId).HasColumnName("FranchiseID");
            entity.Property(e => e.Name).HasMaxLength(60);
            entity.Property(e => e.PipelineId).HasColumnName("PipelineID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FracnhiseId).HasColumnName("FracnhiseID");
            entity.Property(e => e.LastDate).HasColumnType("datetime");
            entity.Property(e => e.Mysignreq).HasColumnName("mysignreq");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(15);
        });

        modelBuilder.Entity<ValueBinding>(entity =>
        {
            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Data).HasMaxLength(50);
            entity.Property(e => e.Text).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
