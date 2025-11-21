using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class PoItem
{
    public Guid Id { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? Poid { get; set; }

    public string? ItemId { get; set; }

    public string? ItemName { get; set; }

    public decimal? CostPerItem { get; set; }

    public decimal? ItemInCase { get; set; }

    public decimal? CostPerCase { get; set; }

    public decimal? Qtyordered { get; set; }

    public decimal? Qtyreceived { get; set; }

    public decimal? Qtydamaged { get; set; }

    public string? Notes { get; set; }
}
