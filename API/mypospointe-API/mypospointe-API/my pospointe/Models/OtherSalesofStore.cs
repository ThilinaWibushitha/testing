using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class OtherSalesofStore
{
    public Guid Id { get; set; }

    public string? Period { get; set; }

    public Guid? StoreId { get; set; }

    public decimal? UberEats { get; set; }

    public decimal? DoorDash { get; set; }

    public decimal? Grabhub { get; set; }

    public decimal? Others { get; set; }

    public DateTime? LastUpdated { get; set; }
}
