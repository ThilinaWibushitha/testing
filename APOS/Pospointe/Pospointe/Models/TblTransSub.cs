using System;
using System.Collections.Generic;

namespace Pospointe.Models;

public partial class TblTransSub
{
    public string Idkey { get; set; } = null!;

    public string? Id { get; set; }

    public int? TransMainId { get; set; }

    public string? ItemId { get; set; }

    public string? ItemType { get; set; }

    public string? ItemName { get; set; }

    public double? ItemPrice { get; set; }

    public double? Qty { get; set; }

    public double? Tax1 { get; set; }

    public double? Amount { get; set; }

    public DateTime? SaleDateTime { get; set; }

    public double? Credits { get; set; }

    public double? Discount { get; set; }

    public double? ActualPrice { get; set; }

    public int? OrderId { get; set; }

    public string? Tax1Status { get; set; }
}
