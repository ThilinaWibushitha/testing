using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TaskTemplate
{
    public Guid Id { get; set; }

    public Guid? FranchiseId { get; set; }

    public Guid? PipelineId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public bool? Required { get; set; }

    public bool? Enabled { get; set; }

    public string? Type { get; set; }

    public bool? Automate { get; set; }

    public int? AutomatedDate { get; set; }

    public int? StepNo { get; set; }

    public Guid? TemplateId { get; set; }
}
