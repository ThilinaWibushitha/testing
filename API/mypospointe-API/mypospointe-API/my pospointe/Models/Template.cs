using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Template
{
    public Guid Id { get; set; }

    public Guid? FracnhiseId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Datatext { get; set; }

    public bool Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastDate { get; set; }

    public bool? EveryOwnersSign { get; set; }

    public bool? Mysignreq { get; set; }
}
