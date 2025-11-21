using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class FrApplication
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? PrefPhoneNum { get; set; }

    public string? NameOfCompany { get; set; }

    public DateTime? Dob { get; set; }

    public string? Citizenship { get; set; }

    public bool? MartialStatus { get; set; }

    public string? NumberofDependents { get; set; }

    public string? HowHeardUs { get; set; }

    public string? WhenforMeeting { get; set; }

    public string? WhenforMeetingOth { get; set; }

    public string? FirstRestOpening { get; set; }

    public string? WhereOpenRest { get; set; }

    public string? Area1 { get; set; }

    public string? Area2 { get; set; }

    public string? Area3 { get; set; }

    public string? WhyShahs { get; set; }

    public bool? EmploymentStatus { get; set; }

    public string? CurrentEmployer { get; set; }

    public string? CurrentPosition { get; set; }

    public string? EmployerLocation { get; set; }

    public string? RecentEmployer { get; set; }

    public string? RecentPosition { get; set; }

    public string? RecentEmplocation { get; set; }

    public string? HighEducLevel { get; set; }

    public bool? WillOtherParnter { get; set; }

    public string? PartnerNames { get; set; }

    public decimal? Cash { get; set; }

    public decimal? MartableSec { get; set; }

    public decimal? HomeValue { get; set; }

    public decimal? OtherValue { get; set; }

    public decimal? OtherAsset { get; set; }

    public decimal? LiqCapital { get; set; }

    public string? FinComment { get; set; }

    public decimal? AmofCrDb { get; set; }

    public string? SoruceofDbCr { get; set; }

    public string? FinancingtheBalance { get; set; }

    public bool? ApprovedforFinancing { get; set; }

    public string? AssetComment { get; set; }

    public decimal? MonthlySalery { get; set; }

    public decimal? InSource1 { get; set; }

    public decimal? InSource2 { get; set; }

    public decimal? InSource3 { get; set; }

    public string? IncmComment { get; set; }

    public decimal? SecuredNtspayble { get; set; }

    public decimal? UnSecNtpayble { get; set; }

    public decimal? AccntsPble { get; set; }

    public decimal? TaxPayble { get; set; }

    public decimal? MortageDbt { get; set; }

    public decimal? OtherLible { get; set; }

    public string? LibComnts { get; set; }

    public bool? RufrancOwner { get; set; }

    public string? FranchiseHowNtype { get; set; }

    public string? Experiance { get; set; }

    public string? Questions { get; set; }
}
