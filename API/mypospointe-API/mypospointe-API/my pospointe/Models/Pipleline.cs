using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Pipleline
{
    public Guid Id { get; set; }

    public Guid? FranchiseId { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool Status { get; set; }

    public int? StepNo { get; set; }
}
