using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class Agreement
{
    public Guid? Id { get; set; }

    public Guid? TemplateId { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? Data { get; set; }

    public bool? MultiSign { get; set; }

    public bool? Mysign { get; set; }

    public bool? OtherParty { get; set; }

    public Guid? AuthKey { get; set; }

    public bool? Completed { get; set; }

    public string? SentEmails { get; set; }

    public Guid? FranchiseId { get; set; }

    public string? DownloadLink { get; set; }
}
