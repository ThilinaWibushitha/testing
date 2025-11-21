using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblModiferGroup
{
    public int ModiferGroupId { get; set; }

    public string? GroupName { get; set; }

    public string? Description { get; set; }

    public string? PhromptName { get; set; }

    public int? MaximumSelect { get; set; }

    public bool? Status { get; set; }

    public bool? Hide { get; set; }

    public string? BtnColor { get; set; }
}
