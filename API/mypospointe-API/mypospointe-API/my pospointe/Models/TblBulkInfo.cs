using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblBulkInfo
{
    public int Id { get; set; }

    public string? ItemId { get; set; }

    public decimal? BulkPrice { get; set; }

    public decimal? Quantity { get; set; }

    public string? Description { get; set; }
}
