using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public Guid? FranchiseId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? TaskId { get; set; }

    public string? TaskName { get; set; }

    public int? StepNo { get; set; }

    public bool? Status { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public Guid? TemplateId { get; set; }

    public Guid? PipelineId { get; set; }
}
