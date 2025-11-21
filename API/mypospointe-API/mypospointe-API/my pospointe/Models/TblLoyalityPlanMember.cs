using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblLoyalityPlanMember
{
    public int Id { get; set; }

    public int? PlanId { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerCredit { get; set; }

    public DateTime? LastupdatedDate { get; set; }

    public DateTime? JoinedDate { get; set; }
}
