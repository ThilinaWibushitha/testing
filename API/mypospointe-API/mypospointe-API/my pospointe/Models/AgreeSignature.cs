using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class AgreeSignature
{
    public Guid? Id { get; set; }

    public Guid? AgreementId { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public string? SignatureData { get; set; }

    public bool? Completed { get; set; }

    public string? Ipaddress { get; set; }

    public string? Location { get; set; }

    public DateTime? SignedDate { get; set; }
}
