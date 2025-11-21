using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class ValueBinding
{
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public string? Data { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool Status { get; set; }
}
