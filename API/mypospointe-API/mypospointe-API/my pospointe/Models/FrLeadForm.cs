using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class FrLeadForm
{
    public Guid Id { get; set; }

    public Guid? FranchiseId { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? NetWorth { get; set; }

    public string? CurrentOccup { get; set; }

    public string? BackNexp { get; set; }

    public bool? CurrentFranchise { get; set; }

    public string? PreferedCityState { get; set; }

    public string? OpenTime { get; set; }

    public string? Comment { get; set; }

    public string? CurrentStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? InComments { get; set; }
}
