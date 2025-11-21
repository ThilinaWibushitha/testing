using System;
using System.Collections.Generic;

namespace my_pospointe.Models;

public partial class InvoiceSub
{
    public Guid Id { get; set; }

    public Guid? InvoiceMainId { get; set; }

    public Guid? ItemId { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? Qty { get; set; }

    public int? OrderList { get; set; }

   
}
