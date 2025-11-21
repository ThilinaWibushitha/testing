using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Franchise
{
    public Guid FranchiseId { get; set; }

    public string? FranchiseName { get; set; }

    public string? FranchiseAddress { get; set; }

    public string? FranchisePhone { get; set; }

    public string? FranchisesStateCityZip { get; set; }

    public string? FranchiseEmail { get; set; }

    public string? FranchiseLogo { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? Status { get; set; }

    public decimal? DefaultLoyaltyFee { get; set; }

    public decimal? Tax1 { get; set; }

    public decimal? Tax2 { get; set; }

    public string? Processor { get; set; }

    public string? MerchantId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? DeviceIp { get; set; }

    public string? Port { get; set; }

    public string? StripeClientId { get; set; }

    public string? StripeApiKey { get; set; }

    public string? StripeEndPointSecret { get; set; }

    public string? BillUsername { get; set; }

    public string? Billpassword { get; set; }

    public string? BillDevKey { get; set; }

    public string? BillOrgId { get; set; }

    public string? Position { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public bool? Remote { get; set; }
}
