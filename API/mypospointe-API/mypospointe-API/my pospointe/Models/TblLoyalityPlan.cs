using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblLoyalityPlan
{
    public int PlanId { get; set; }

    public string? PlanName { get; set; }

    public bool? Status { get; set; }

    public string? Description { get; set; }

    public bool? DiscountMethod { get; set; }

    public decimal? DollersperCredit { get; set; }

    public decimal? CreditAmount { get; set; }

    public decimal? MaxCreditReach { get; set; }

    public DateTime? ExpireDate { get; set; }

    public DateTime? CreatedDate { get; set; }
}
