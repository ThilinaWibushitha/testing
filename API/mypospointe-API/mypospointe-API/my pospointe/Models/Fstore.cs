using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Fstore
{
    public Guid Id { get; set; }

    public Guid? StoreId { get; set; }

    public string? StoreDb { get; set; }

    public Guid? FranchiseId { get; set; }

    public bool? Status { get; set; }

    public string? StoreCity { get; set; }

    public string? LegalBusiness { get; set; }

    public string? TaxId { get; set; }

    public string? State { get; set; }

    public bool? LoyaltyFeeinFlatRate { get; set; }

    public decimal? LoyaltyFee { get; set; }

    public decimal? LocalMarketing { get; set; }

    public decimal? Marketing { get; set; }

    public string? PaymentProfileId { get; set; }

    public string? Email { get; set; }

    public bool? OfflineStores { get; set; }

    public string? BillCustomerId { get; set; }

    public bool? AutoCharge { get; set; }

    public string? BillDefaultPayment { get; set; }

    public bool? Remote { get; set; }
}
