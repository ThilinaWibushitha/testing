using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblTransSub
{
    public string Idkey { get; set; } = null!;

    public string? Id { get; set; }

    public decimal? TransMainId { get; set; }

    public string? ItemId { get; set; }

    public string? ItemType { get; set; }

    public string? ItemName { get; set; }

    public decimal? ItemPrice { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Tax1 { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? SaleDateTime { get; set; }

    public decimal? Credits { get; set; }

    public decimal? Discount { get; set; }

    public decimal? ActualPrice { get; set; }

    public int? OrderId { get; set; }

    public string? Tax1Status { get; set; }
}
