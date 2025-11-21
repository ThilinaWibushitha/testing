using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class TblTempBillList
{
    public string Id { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public string ItemDeptId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public decimal ItemPrice { get; set; }

    public decimal Qty { get; set; }

    public decimal Tax1Rate { get; set; }

    public decimal Amount { get; set; }
}
