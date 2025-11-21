using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblModifersofItem
{
    public int Id { get; set; }

    public string? ItemId { get; set; }

    public int? ModiferGroupId { get; set; }

    public int? MaximumSelect { get; set; }

    public bool? Forced { get; set; }
}
